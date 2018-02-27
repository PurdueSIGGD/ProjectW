// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-5606-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32120,y:32808,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.1397059,c2:0.04582354,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:5606,x:32542,y:32808,varname:node_5606,prsc:2|A-3258-OUT,B-7851-OUT;n:type:ShaderForge.SFN_Fresnel,id:8133,x:32120,y:32984,varname:node_8133,prsc:2|NRM-1566-OUT,EXP-1805-OUT;n:type:ShaderForge.SFN_NormalVector,id:1566,x:31897,y:32890,prsc:2,pt:True;n:type:ShaderForge.SFN_ValueProperty,id:1805,x:31875,y:33089,ptovrint:False,ptlb:Fresnel Exponent,ptin:_FresnelExponent,varname:node_1805,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Lerp,id:3258,x:32372,y:32808,varname:node_3258,prsc:2|A-9652-OUT,B-7241-RGB,T-8133-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7851,x:32355,y:33001,ptovrint:False,ptlb:Glow Intensity,ptin:_GlowIntensity,varname:node_7851,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:5;n:type:ShaderForge.SFN_Color,id:1875,x:31927,y:32621,ptovrint:False,ptlb:node_1875,ptin:_node_1875,varname:node_1875,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5147059,c2:0.2413793,c3:0.06433824,c4:1;n:type:ShaderForge.SFN_Clamp01,id:9652,x:32120,y:32621,varname:node_9652,prsc:2|IN-1875-RGB;proporder:7241-1805-7851-1875;pass:END;sub:END;*/

Shader "Shader Forge/Fireball" {
    Properties {
        _Color ("Color", Color) = (0.1397059,0.04582354,0,1)
        _FresnelExponent ("Fresnel Exponent", Float ) = 0.5
        _GlowIntensity ("Glow Intensity", Float ) = 5
        _node_1875 ("node_1875", Color) = (0.5147059,0.2413793,0.06433824,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _Color;
            uniform float _FresnelExponent;
            uniform float _GlowIntensity;
            uniform float4 _node_1875;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float3 emissive = (lerp(saturate(_node_1875.rgb),_Color.rgb,pow(1.0-max(0,dot(normalDirection, viewDirection)),_FresnelExponent))*_GlowIntensity);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
