#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

sampler2D TextureSampler = sampler_state
{
    Texture = (SpriteTexture);
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float4 ps_main(VertexShaderOutput input) : COLOR0
{
    float4 color = tex2D(TextureSampler, input.TextureCoordinates);
    return color;
}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL ps_main();
	}
};