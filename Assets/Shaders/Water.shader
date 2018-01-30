// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:7230,x:32998,y:32566,varname:node_7230,prsc:2|diff-1972-OUT,spec-8374-OUT,gloss-8374-OUT,normal-2888-OUT,emission-7934-OUT,alpha-8710-OUT,refract-8488-OUT,voffset-8727-OUT;n:type:ShaderForge.SFN_Color,id:2857,x:31660,y:31847,ptovrint:False,ptlb:Color Deep,ptin:_ColorDeep,varname:node_2857,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.4525357,c3:0.6764706,c4:0.541;n:type:ShaderForge.SFN_Color,id:4410,x:31660,y:32019,ptovrint:False,ptlb:Color Shallow,ptin:_ColorShallow,varname:node_4410,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.6581135,c3:0.9264706,c4:0.566;n:type:ShaderForge.SFN_Lerp,id:1972,x:31937,y:32113,varname:node_1972,prsc:2|A-2857-RGB,B-4410-RGB,T-2229-OUT;n:type:ShaderForge.SFN_Fresnel,id:2229,x:31660,y:32173,varname:node_2229,prsc:2|NRM-9317-OUT;n:type:ShaderForge.SFN_NormalVector,id:9317,x:31452,y:32173,prsc:2,pt:True;n:type:ShaderForge.SFN_ValueProperty,id:8374,x:32574,y:32690,ptovrint:False,ptlb:Glossiness,ptin:_Glossiness,varname:node_8374,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.95;n:type:ShaderForge.SFN_ValueProperty,id:8710,x:32230,y:32728,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:node_8710,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Tex2d,id:963,x:31978,y:32871,varname:node_963,prsc:2,tex:d54b410e9bb54411ca325817acb81486,ntxv:0,isnm:False|UVIN-7789-OUT,TEX-4379-TEX;n:type:ShaderForge.SFN_Tex2d,id:8478,x:31978,y:33023,varname:node_8478,prsc:2,tex:d54b410e9bb54411ca325817acb81486,ntxv:0,isnm:False|UVIN-5267-OUT,TEX-4379-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:4379,x:31741,y:32675,ptovrint:False,ptlb:Normal Map,ptin:_NormalMap,varname:node_4379,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:d54b410e9bb54411ca325817acb81486,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Slider,id:7034,x:31900,y:33190,ptovrint:False,ptlb:Blend Strength,ptin:_BlendStrength,varname:node_7034,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.25,max:1;n:type:ShaderForge.SFN_Lerp,id:2888,x:32261,y:32947,varname:node_2888,prsc:2|A-963-RGB,B-8478-RGB,T-7034-OUT;n:type:ShaderForge.SFN_ComponentMask,id:6717,x:32421,y:33009,varname:node_6717,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-2888-OUT;n:type:ShaderForge.SFN_Multiply,id:8488,x:32571,y:33121,varname:node_8488,prsc:2|A-6717-OUT,B-7126-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7126,x:32348,y:33198,ptovrint:False,ptlb:Refraction,ptin:_Refraction,varname:node_7126,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.15;n:type:ShaderForge.SFN_Color,id:7322,x:31996,y:32369,ptovrint:False,ptlb:Fresnel Deep,ptin:_FresnelDeep,varname:node_7322,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.01516231,c3:0.1691176,c4:1;n:type:ShaderForge.SFN_Fresnel,id:9970,x:31690,y:32460,varname:node_9970,prsc:2|NRM-9317-OUT,EXP-5226-OUT;n:type:ShaderForge.SFN_Lerp,id:7934,x:32329,y:32508,varname:node_7934,prsc:2|A-7322-RGB,B-383-RGB,T-9970-OUT;n:type:ShaderForge.SFN_Color,id:383,x:31996,y:32575,ptovrint:False,ptlb:Fresnel Shallow,ptin:_FresnelShallow,varname:node_383,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.3382353,c2:0.7261662,c3:1,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:5226,x:31356,y:32548,ptovrint:False,ptlb:Fresnel Strength,ptin:_FresnelStrength,varname:node_5226,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1.5;n:type:ShaderForge.SFN_Multiply,id:8727,x:32744,y:32894,varname:node_8727,prsc:2|A-2888-OUT,B-1900-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1900,x:32585,y:33009,ptovrint:False,ptlb:Vertex Offset Strength,ptin:_VertexOffsetStrength,varname:node_1900,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_FragmentPosition,id:9208,x:30365,y:32267,varname:node_9208,prsc:2;n:type:ShaderForge.SFN_Append,id:7504,x:30583,y:32282,varname:node_7504,prsc:2|A-9208-X,B-9208-Z;n:type:ShaderForge.SFN_Divide,id:4406,x:30792,y:32306,varname:node_4406,prsc:2|A-7504-OUT,B-2906-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2906,x:30554,y:32454,ptovrint:False,ptlb:UV Scale,ptin:_UVScale,varname:node_2906,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Set,id:4260,x:30961,y:32306,varname:_worldUV,prsc:2|IN-4406-OUT;n:type:ShaderForge.SFN_Get,id:6489,x:29255,y:32885,varname:node_6489,prsc:2|IN-4260-OUT;n:type:ShaderForge.SFN_Multiply,id:7261,x:29475,y:32762,varname:node_7261,prsc:2|A-6489-OUT,B-4043-OUT;n:type:ShaderForge.SFN_Multiply,id:5343,x:29475,y:32917,varname:node_5343,prsc:2|A-6489-OUT,B-3325-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4043,x:29276,y:32747,ptovrint:False,ptlb:UV1 Tiling,ptin:_UV1Tiling,varname:node_4043,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:3325,x:29276,y:33016,ptovrint:False,ptlb:UV2 Tiling,ptin:_UV2Tiling,varname:node_3325,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Time,id:8640,x:29664,y:32812,varname:node_8640,prsc:2;n:type:ShaderForge.SFN_ComponentMask,id:4725,x:29874,y:32750,varname:node_4725,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-7261-OUT;n:type:ShaderForge.SFN_ComponentMask,id:7690,x:29874,y:32934,varname:node_7690,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-5343-OUT;n:type:ShaderForge.SFN_Multiply,id:7574,x:29879,y:32611,varname:node_7574,prsc:2|A-3361-Y,B-8640-TSL;n:type:ShaderForge.SFN_Multiply,id:9316,x:29879,y:32476,varname:node_9316,prsc:2|A-3361-X,B-8640-TSL;n:type:ShaderForge.SFN_Multiply,id:5836,x:29874,y:33090,varname:node_5836,prsc:2|A-7125-X,B-8640-TSL;n:type:ShaderForge.SFN_Multiply,id:3598,x:29874,y:33220,varname:node_3598,prsc:2|A-7125-Y,B-8640-TSL;n:type:ShaderForge.SFN_Vector4Property,id:3361,x:29574,y:32508,ptovrint:False,ptlb:UV1 Animator,ptin:_UV1Animator,varname:node_3361,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_Vector4Property,id:7125,x:29581,y:33089,ptovrint:False,ptlb:UV2 Animator,ptin:_UV2Animator,varname:node_7125,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_Add,id:6283,x:30117,y:32567,varname:node_6283,prsc:2|A-9316-OUT,B-4725-R;n:type:ShaderForge.SFN_Add,id:7489,x:30119,y:32715,varname:node_7489,prsc:2|A-7574-OUT,B-4725-G;n:type:ShaderForge.SFN_Add,id:1335,x:30119,y:32960,varname:node_1335,prsc:2|A-7690-R,B-5836-OUT;n:type:ShaderForge.SFN_Add,id:2689,x:30119,y:33103,varname:node_2689,prsc:2|A-7690-G,B-3598-OUT;n:type:ShaderForge.SFN_Append,id:649,x:30309,y:32645,varname:node_649,prsc:2|A-6283-OUT,B-7489-OUT;n:type:ShaderForge.SFN_Append,id:5104,x:30312,y:33042,varname:node_5104,prsc:2|A-1335-OUT,B-2689-OUT;n:type:ShaderForge.SFN_Set,id:691,x:30469,y:32645,varname:_UV1,prsc:2|IN-649-OUT;n:type:ShaderForge.SFN_Set,id:8298,x:30476,y:33042,varname:_UV2,prsc:2|IN-5104-OUT;n:type:ShaderForge.SFN_Get,id:7789,x:31482,y:32960,varname:node_7789,prsc:2|IN-691-OUT;n:type:ShaderForge.SFN_Get,id:5267,x:31482,y:33074,varname:node_5267,prsc:2|IN-8298-OUT;proporder:2857-4410-7322-383-5226-8374-8710-4379-7034-7126-1900-2906-4043-3361-3325-7125;pass:END;sub:END;*/

Shader "Custom/water2" {
    Properties {
        _ColorDeep ("Color Deep", Color) = (0,0.4525357,0.6764706,0.541)
        _ColorShallow ("Color Shallow", Color) = (0,0.6581135,0.9264706,0.566)
        _FresnelDeep ("Fresnel Deep", Color) = (0,0.01516231,0.1691176,1)
        _FresnelShallow ("Fresnel Shallow", Color) = (0.3382353,0.7261662,1,1)
        _FresnelStrength ("Fresnel Strength", Float ) = 1.5
        _Glossiness ("Glossiness", Float ) = 0.95
        _Opacity ("Opacity", Float ) = 0.5
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _BlendStrength ("Blend Strength", Range(0, 1)) = 0.25
        _Refraction ("Refraction", Float ) = 0.15
        _VertexOffsetStrength ("Vertex Offset Strength", Float ) = 0.1
        _UVScale ("UV Scale", Float ) = 2
        _UV1Tiling ("UV1 Tiling", Float ) = 1
        _UV1Animator ("UV1 Animator", Vector) = (0,0,0,0)
        _UV2Tiling ("UV2 Tiling", Float ) = 1
        _UV2Animator ("UV2 Animator", Vector) = (0,0,0,0)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _GrabTexture;
            uniform float4 _ColorDeep;
            uniform float4 _ColorShallow;
            uniform float _Glossiness;
            uniform float _Opacity;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _BlendStrength;
            uniform float _Refraction;
            uniform float4 _FresnelDeep;
            uniform float4 _FresnelShallow;
            uniform float _FresnelStrength;
            uniform float _VertexOffsetStrength;
            uniform float _UVScale;
            uniform float _UV1Tiling;
            uniform float _UV2Tiling;
            uniform float4 _UV1Animator;
            uniform float4 _UV2Animator;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float3 tangentDir : TEXCOORD2;
                float3 bitangentDir : TEXCOORD3;
                float4 projPos : TEXCOORD4;
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 node_8640 = _Time;
                float2 _worldUV = (float2(mul(unity_ObjectToWorld, v.vertex).r,mul(unity_ObjectToWorld, v.vertex).b)/_UVScale);
                float2 node_6489 = _worldUV;
                float2 node_4725 = (node_6489*_UV1Tiling).rg;
                float2 _UV1 = float2(((_UV1Animator.r*node_8640.r)+node_4725.r),((_UV1Animator.g*node_8640.r)+node_4725.g));
                float2 node_7789 = _UV1;
                float3 node_963 = UnpackNormal(tex2Dlod(_NormalMap,float4(TRANSFORM_TEX(node_7789, _NormalMap),0.0,0)));
                float2 node_7690 = (node_6489*_UV2Tiling).rg;
                float2 _UV2 = float2((node_7690.r+(_UV2Animator.r*node_8640.r)),(node_7690.g+(_UV2Animator.g*node_8640.r)));
                float2 node_5267 = _UV2;
                float3 node_8478 = UnpackNormal(tex2Dlod(_NormalMap,float4(TRANSFORM_TEX(node_5267, _NormalMap),0.0,0)));
                float3 node_2888 = lerp(node_963.rgb,node_8478.rgb,_BlendStrength);
                v.vertex.xyz += (node_2888*_VertexOffsetStrength);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 node_8640 = _Time;
                float2 _worldUV = (float2(i.posWorld.r,i.posWorld.b)/_UVScale);
                float2 node_6489 = _worldUV;
                float2 node_4725 = (node_6489*_UV1Tiling).rg;
                float2 _UV1 = float2(((_UV1Animator.r*node_8640.r)+node_4725.r),((_UV1Animator.g*node_8640.r)+node_4725.g));
                float2 node_7789 = _UV1;
                float3 node_963 = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(node_7789, _NormalMap)));
                float2 node_7690 = (node_6489*_UV2Tiling).rg;
                float2 _UV2 = float2((node_7690.r+(_UV2Animator.r*node_8640.r)),(node_7690.g+(_UV2Animator.g*node_8640.r)));
                float2 node_5267 = _UV2;
                float3 node_8478 = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(node_5267, _NormalMap)));
                float3 node_2888 = lerp(node_963.rgb,node_8478.rgb,_BlendStrength);
                float3 normalLocal = node_2888;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float2 sceneUVs = (i.projPos.xy / i.projPos.w) + (node_2888.rg*_Refraction);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = _Glossiness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float3 specularColor = float3(_Glossiness,_Glossiness,_Glossiness);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float3 diffuseColor = lerp(_ColorDeep.rgb,_ColorShallow.rgb,(1.0-max(0,dot(normalDirection, viewDirection))));
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = lerp(_FresnelDeep.rgb,_FresnelShallow.rgb,pow(1.0-max(0,dot(normalDirection, viewDirection)),_FresnelStrength));
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                fixed4 finalRGBA = fixed4(lerp(sceneColor.rgb, finalColor,_Opacity),1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _GrabTexture;
            uniform float4 _ColorDeep;
            uniform float4 _ColorShallow;
            uniform float _Glossiness;
            uniform float _Opacity;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _BlendStrength;
            uniform float _Refraction;
            uniform float4 _FresnelDeep;
            uniform float4 _FresnelShallow;
            uniform float _FresnelStrength;
            uniform float _VertexOffsetStrength;
            uniform float _UVScale;
            uniform float _UV1Tiling;
            uniform float _UV2Tiling;
            uniform float4 _UV1Animator;
            uniform float4 _UV2Animator;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float3 tangentDir : TEXCOORD2;
                float3 bitangentDir : TEXCOORD3;
                float4 projPos : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 node_8640 = _Time;
                float2 _worldUV = (float2(mul(unity_ObjectToWorld, v.vertex).r,mul(unity_ObjectToWorld, v.vertex).b)/_UVScale);
                float2 node_6489 = _worldUV;
                float2 node_4725 = (node_6489*_UV1Tiling).rg;
                float2 _UV1 = float2(((_UV1Animator.r*node_8640.r)+node_4725.r),((_UV1Animator.g*node_8640.r)+node_4725.g));
                float2 node_7789 = _UV1;
                float3 node_963 = UnpackNormal(tex2Dlod(_NormalMap,float4(TRANSFORM_TEX(node_7789, _NormalMap),0.0,0)));
                float2 node_7690 = (node_6489*_UV2Tiling).rg;
                float2 _UV2 = float2((node_7690.r+(_UV2Animator.r*node_8640.r)),(node_7690.g+(_UV2Animator.g*node_8640.r)));
                float2 node_5267 = _UV2;
                float3 node_8478 = UnpackNormal(tex2Dlod(_NormalMap,float4(TRANSFORM_TEX(node_5267, _NormalMap),0.0,0)));
                float3 node_2888 = lerp(node_963.rgb,node_8478.rgb,_BlendStrength);
                v.vertex.xyz += (node_2888*_VertexOffsetStrength);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 node_8640 = _Time;
                float2 _worldUV = (float2(i.posWorld.r,i.posWorld.b)/_UVScale);
                float2 node_6489 = _worldUV;
                float2 node_4725 = (node_6489*_UV1Tiling).rg;
                float2 _UV1 = float2(((_UV1Animator.r*node_8640.r)+node_4725.r),((_UV1Animator.g*node_8640.r)+node_4725.g));
                float2 node_7789 = _UV1;
                float3 node_963 = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(node_7789, _NormalMap)));
                float2 node_7690 = (node_6489*_UV2Tiling).rg;
                float2 _UV2 = float2((node_7690.r+(_UV2Animator.r*node_8640.r)),(node_7690.g+(_UV2Animator.g*node_8640.r)));
                float2 node_5267 = _UV2;
                float3 node_8478 = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(node_5267, _NormalMap)));
                float3 node_2888 = lerp(node_963.rgb,node_8478.rgb,_BlendStrength);
                float3 normalLocal = node_2888;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float2 sceneUVs = (i.projPos.xy / i.projPos.w) + (node_2888.rg*_Refraction);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = _Glossiness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float3 specularColor = float3(_Glossiness,_Glossiness,_Glossiness);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 diffuseColor = lerp(_ColorDeep.rgb,_ColorShallow.rgb,(1.0-max(0,dot(normalDirection, viewDirection))));
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * _Opacity,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _BlendStrength;
            uniform float _VertexOffsetStrength;
            uniform float _UVScale;
            uniform float _UV1Tiling;
            uniform float _UV2Tiling;
            uniform float4 _UV1Animator;
            uniform float4 _UV2Animator;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float4 posWorld : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                float4 node_8640 = _Time;
                float2 _worldUV = (float2(mul(unity_ObjectToWorld, v.vertex).r,mul(unity_ObjectToWorld, v.vertex).b)/_UVScale);
                float2 node_6489 = _worldUV;
                float2 node_4725 = (node_6489*_UV1Tiling).rg;
                float2 _UV1 = float2(((_UV1Animator.r*node_8640.r)+node_4725.r),((_UV1Animator.g*node_8640.r)+node_4725.g));
                float2 node_7789 = _UV1;
                float3 node_963 = UnpackNormal(tex2Dlod(_NormalMap,float4(TRANSFORM_TEX(node_7789, _NormalMap),0.0,0)));
                float2 node_7690 = (node_6489*_UV2Tiling).rg;
                float2 _UV2 = float2((node_7690.r+(_UV2Animator.r*node_8640.r)),(node_7690.g+(_UV2Animator.g*node_8640.r)));
                float2 node_5267 = _UV2;
                float3 node_8478 = UnpackNormal(tex2Dlod(_NormalMap,float4(TRANSFORM_TEX(node_5267, _NormalMap),0.0,0)));
                float3 node_2888 = lerp(node_963.rgb,node_8478.rgb,_BlendStrength);
                v.vertex.xyz += (node_2888*_VertexOffsetStrength);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
