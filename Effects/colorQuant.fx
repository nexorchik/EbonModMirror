sampler2D uImage0 : register(s0);

float res = 32;
float4 Main(float2 uv : TEXCOORD) : COLOR
{
    float4 c = tex2D(uImage0, uv);
    float3 _res = float3(res, res, res);
    float3 quant = floor(float3(c.xyz) * _res) / (_res - 1);
    return float4(quant.xyz, c.a);
}

technique Technique1
{
    pass colorQuant
    {
        PixelShader = compile ps_2_0 Main();
    }
}