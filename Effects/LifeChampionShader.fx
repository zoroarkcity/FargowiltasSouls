sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;

float4 LCWings(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float frameY = (coords.y * 2 * uImageSize0.y - uSourceRect.y) / uSourceRect.w;
    float wave = 1 - sin((frameY.x - uTime * 3) + 1) / 2;
    color.rgb *= (wave * uColor) + ((1 - wave) * uSecondaryColor);
    
    return color * sampleColor;
}

technique Technique1
{
    pass LCWings
    {
        PixelShader = compile ps_2_0 LCWings();
    }
}