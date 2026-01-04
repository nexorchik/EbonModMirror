sampler2D tex : register(s0);
float speed = -0.2;
float uTime;
float colMult = 10;
float4 colOverride = float4(0.12, 0.56, 1, 1);

float4 MainPS(float2 uv : TEXCOORD0) : COLOR0
{
    float gradient = lerp(0, 1, pow(uv.y, 6));
    float4 c = (tex2D(tex, uv + float2(uTime * 0.052, -uTime * 0.01) * speed)
    * tex2D(tex, 1.5 * uv + float2(uTime * 0.1, -uTime * 0.023) * speed)
    * (tex2D(tex, uv + float2(uTime * 0.2, 0) * speed) + 0.1)
    * colMult) * colOverride;
    return float4(c.rgb * gradient, gradient);
}

Technique technique1
{
    pass Main
    {
        PixelShader = compile ps_2_0 MainPS();
    }
}