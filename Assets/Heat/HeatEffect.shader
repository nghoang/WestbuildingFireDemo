// Upgrade NOTE: replaced 'samplerRECT' with 'sampler2D'
// Upgrade NOTE: replaced 'texRECTproj' with 'tex2Dproj'

// Per pixel bumped refraction.
// Uses a normal map to distort the image behind

Shader "FX/HeatDistort" {
Properties {
	_BumpAmt  ("Distortion", range (0,128)) = 10
	_BumpMap ("Bumpmap (RGB)", 2D) = "bump" {}
}

Category {

	// We must be transparent, so other objects are drawn before this one.
	Tags { "Queue" = "Transparent"}
	
	// ------------------------------------------------------------------
	//  ARB fragment program
	
	SubShader {

		// This pass grabs the screen behind the object into a texture.
		// We can access the result in the next pass as _GrabTexture
		GrabPass {							
			Name "BASE"
			Tags { "LightMode" = "Always" }
 		}
 		
 		// Main pass: Take the texture grabbed above and use the bumpmap to perturb it
 		// on to the screen
		Pass {
			Name "BASE"
			Tags { "LightMode" = "Always" }
			
CGPROGRAM
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it does not contain a surface program or both vertex and fragment programs.
#pragma exclude_renderers gles
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest 
#pragma fragmentoption ARB_fog_exp2

sampler2D _GrabTexture : register(s0);
sampler2D _BumpMap : register(s1);


struct v2f {
	float4 uv : TEXCOORD0;
	float2 uvbump : TEXCOORD1;
};

uniform float _BumpAmt;

half4 frag( v2f i ) : COLOR
{
	// calculate perturbed coordinates
	half2 bump = tex2D( _BumpMap, i.uvbump ).rg * 2 - 1;
	//i.uv.xy = bump * _BumpAmt * i.uv.w + i.uv.xy;
	float2 offset = bump * _BumpAmt;
	i.uv.xy = offset * i.uv.z + i.uv.xy;
	
	half4 col = tex2Dproj( _GrabTexture, i.uv.xyw );
	
	return col;
}

ENDCG
			// Set up the textures for this pass
			SetTexture [_GrabTexture] {}	// Texture we grabbed in the pass above
			SetTexture [_BumpMap] {}		// Perturbation bumpmap
			SetTexture [_MainTex] {}		// Color tint
		}
	}
	
	// ------------------------------------------------------------------
	// Fallback for older cards and Unity non-Pro
	
	SubShader {
        Lighting Off        ZWrite Off        ColorMask A        Pass {}
	}
}

}

