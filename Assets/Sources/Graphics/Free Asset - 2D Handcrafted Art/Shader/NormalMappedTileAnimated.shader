Shader "Custom/NormalMappedTileAnimated_URP" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        [PerRendererData] _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _MyNormalMap("My Normal map", 2D) = "white" {}
        _EmissiveMap("Emissive map", 2D) = "white" {}
        [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
        [Toggle(EMISSIVE_TEXTURE)] _EnabledEmissive ("Use Emissive?", Float) = 0
        [Space]
        _RandOffset("Unique Offset", Range(0,1)) = 1
        [Space]
        _WindDir("Wind Direction", Range(-1,1)) = 1
        _BendScale("Bend Scale", Range(0,1)) = 1
        _SwayFreq("Sway Freq", Range(0,20)) = 1
    }
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "DisableBatching" = "True" "RenderPipeline" = "UniversalPipeline" }
        LOD 200
        Cull Off
        
        Pass {
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _EMISSIVE_TEXTURE

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/BRDF.hlsl"

            sampler2D _MainTex;
            sampler2D _MyNormalMap;
            sampler2D _EmissiveMap;

            float _WindDir;
            float _RandOffset;
            float _BendScale;
            float _SwayFreq;

            struct Attributes {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 color : COLOR;
                float2 uv_MainTex : TEXCOORD0;
            };

            struct Varyings {
                float4 pos : SV_POSITION;
                float2 uv_MainTex : TEXCOORD0;
                float4 color : COLOR;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
                float3 worldTangent : TEXCOORD3;
                float3 worldBinormal : TEXCOORD4;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_INSTANCING_BUFFER_END(Props)

            float4 _Color;
            float4 _RendererColor;
            half4 _Flip;

            // Function for bending simulation
            float4 SineApproximation(float4 x) {
                return abs(frac(x + _RandOffset + 0.5) * 2.0 - 1.0);
            }

            Varyings vert(Attributes v)
            {
                Varyings o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);

                v.vertex.xy *= _Flip.xy;

                 float4 localPosition = float4(v.vertex.xyz, 1.0);
                o.pos = TransformObjectToHClip(localPosition);
                o.uv_MainTex = v.uv_MainTex;
                o.color = v.color * _Color * _RendererColor;
                o.worldPos = TransformObjectToWorld(v.vertex.xyz);
                o.worldNormal = TransformObjectToWorldNormal(v.normal);
                o.worldTangent = TransformObjectToWorldDir(v.tangent.xyz);
                o.worldBinormal = cross(o.worldNormal, o.worldTangent) * v.tangent.w;

                // Wind bending logic
                float3 vPos = v.vertex.xyz;
                float fLength = length(vPos);
                float fBF = vPos.y * (_BendScale) * ((SineApproximation(_Time.y * (_SwayFreq)) + 0.5)*0.5);
                fBF += 1.0;
                fBF *= fBF;
                fBF = fBF * fBF - fBF;

                // Displace position
                float3 vNewPos = vPos;
                vNewPos.x += _WindDir * fBF * saturate(vPos.y);
                vPos.xy = normalize(vNewPos.xy) * fLength;
                v.vertex.xy = vPos.xy;

                return o;
            }

            float4 frag(Varyings i) : SV_Target
            {
                // Sample main texture
                float4 albedo = tex2D(_MainTex, i.uv_MainTex) * _Color;

                // Normal mapping
                float3 normalMap = UnpackNormal(tex2D(_MyNormalMap, i.uv_MainTex));

                // Setup SurfaceOutputStandard
                SurfaceData surfaceData;
                ZERO_INITIALIZE(SurfaceData, surfaceData);
                surfaceData.albedo = albedo.rgb;
                surfaceData.alpha = albedo.a;
                surfaceData.normalTS = normalMap;
                // surfaceData.emission = float3(0, 0, 0); // Ініціалізація поля emission
                // surfaceData.metallic = 0.0; // Ініціалізація metallic
                // surfaceData.smoothness = 0.5; // Ініціалізація smoothness
                surfaceData.occlusion = 1.0; // Ініціалізація occlusion
                // surfaceData.specular = float3(0, 0, 0); // Ініціалізація specular, якщо потрібно

                #if EMISSIVE_TEXTURE
                if (_EnabledEmissive == 1) {
                    surfaceData.emission = tex2D(_EmissiveMap, i.uv_MainTex).rgb;
                }
                #endif

                // Handle outline by making black pixels fully black
                if (length(albedo.rgb) < 0.001) {
                    surfaceData.albedo = float3(0, 0, 0);
                    surfaceData.normalTS = float3(0, 0, -1);
                }

                // Lighting and final color output using URP
                float3 worldNormal = normalize(i.worldNormal);
                float3 worldViewDir = normalize(GetWorldSpaceViewDir(i.worldPos));
                InputData inputData;
                inputData.positionWS = i.worldPos;
                inputData.normalWS = worldNormal;
                inputData.viewDirectionWS = worldViewDir;
                // inputData.shadowCoord = i.shadowCoord;
                inputData.bakedGI = 1.0;
                
                half3 lightResult = UniversalFragmentPBR(inputData, surfaceData);
                return float4(lightResult, 1);
            }

            ENDHLSL
        }
    }
    Fallback "Diffuse"
}
