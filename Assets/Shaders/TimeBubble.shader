// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-6974-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32070,y:32821,ptovrint:False,ptlb:Color Outer,ptin:_ColorOuter,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.9558823,c3:0.3602941,c4:1;n:type:ShaderForge.SFN_Fresnel,id:988,x:31930,y:32984,varname:node_988,prsc:2|NRM-2303-OUT,EXP-835-OUT;n:type:ShaderForge.SFN_Multiply,id:6974,x:32512,y:32809,varname:node_6974,prsc:2|A-158-OUT,B-9954-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9954,x:32283,y:33002,ptovrint:False,ptlb:Glow Intensity,ptin:_GlowIntensity,varname:node_9954,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1.5;n:type:ShaderForge.SFN_Lerp,id:158,x:32283,y:32809,varname:node_158,prsc:2|A-2957-OUT,B-7241-RGB,T-3490-OUT;n:type:ShaderForge.SFN_ValueProperty,id:835,x:31756,y:33048,ptovrint:False,ptlb:node_835,ptin:_node_835,varname:node_835,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3;n:type:ShaderForge.SFN_NormalVector,id:2303,x:31756,y:32886,prsc:2,pt:True;n:type:ShaderForge.SFN_Clamp01,id:2957,x:32094,y:32627,varname:node_2957,prsc:2|IN-4189-RGB;n:type:ShaderForge.SFN_Clamp01,id:3490,x:32088,y:32984,varname:node_3490,prsc:2|IN-988-OUT;n:type:ShaderForge.SFN_SceneColor,id:4189,x:31917,y:32627,varname:node_4189,prsc:2;proporder:7241-9954-835;pass:END;sub:END;*/

Shader "Shader Forge/TimeBubble" {
    Properties {
        _ColorOuter ("Color Outer", Color) = (1,0.9558823,0.3602941,1)
        _GlowIntensity ("Glow Intensity", Float ) = 1.5
        _node_835 ("node_835", Float ) = 3
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform float4 _ColorOuter;
            uniform float _GlowIntensity;
            uniform float _node_835;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float4 projPos : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
////// Emissive:
                float3 emissive = (lerp(saturate(sceneColor.rgb),_ColorOuter.rgb,saturate(pow(1.0-max(0,dot(normalDirection, viewDirection)),_node_835)))*_GlowIntensity);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
