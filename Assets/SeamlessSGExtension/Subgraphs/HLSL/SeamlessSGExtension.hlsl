/*
   Copyright (c) 2023 Léo Chaumartin
   All rights reserved.
*/

#include "Common.hlsl"

void Voronoise_float(
    float2 UV,
    float Voronoi,
    float Blur,
    float2 Pan,
    float2 Scale,
    out float Value)
{

    float lPattern = voronoise(float2((UV.x + (Pan.x * (0.5 / Scale.x)) + 1.0) * Scale.x, (UV.y + (Pan.y * (0.5 / Scale.y))) * Scale.y), clamp(Voronoi, 0.0, 1.0), clamp(Blur, 0.0, 1.0), Scale);
    Value = lPattern;
}

void Voronoise_half(
    half2 UV,
    half Voronoi,
    half Blur,
    half2 Pan,
    half2 Scale,
    out half Value)
{
    float lPattern = voronoise(float2((UV.x + (Pan.x * (0.5 / Scale.x)) + 1.0) * Scale.x, (UV.y + (Pan.y * (0.5 / Scale.y))) * Scale.y), clamp(Voronoi, 0.0, 1.0), clamp(Blur, 0.0, 1.0), Scale);
    Value = lPattern;
}


// FRACTAL NOISE

float voronoise_fractal(float2 x, float u, float v, float2 Scale, float Lacunarity, int iter)
{
    float va = 0;
    float wt = 0;
    for (float j = -2; j <= 2; j+=1)
        for (float i = -2; i <= 2; i+=1)
        {
            float3 o = hash3(mod((floor(x) + float2(i, j)), Scale * pow(abs(Lacunarity), float(iter)))) * float3(u, u, 1);
            float2 r = float2(i, j) - frac(x) + float2(o.xy);
            float ww = pow(abs(1 - smoothstep(.0f, 1.414f, sqrt(dot(r, r)))), 1 + 63 * pow(abs(1 - v), 4));
            va += o.z * ww;
            wt += ww;
        }
    if (wt == 0.0)
        return 0.0;
    return va / wt;
}


float fbm(float2 coord, uint Octaves, float2 Scale, float Voronoi, float Blur, float Lacunarity, float Gain) {
    float value = 0.0;
    float amplitude = 1;
    Voronoi = clamp(Voronoi, 0.0, 1.0);
    Blur = clamp(Blur, 0.0, 1.0);
    Octaves = min(Octaves, 16);
    for (uint i = 0; i < Octaves; i++) {
        value += amplitude * abs(voronoise_fractal(coord, Voronoi, Blur, Scale, Lacunarity, i) * 2.0 - 1.0);
        coord *= Lacunarity;
        amplitude *= Gain;
    }
    return value;
}


void FractalNoise_float(float2 UV, uint Octaves, float2 Pan, float2 Scale, float Voronoi, float Blur, float Lacunarity, float Gain, out float Value)
{
    float2 p = float2((UV.x + (Pan.x * (0.5 / Scale.x))) * Scale.x, (UV.y + (Pan.y * (0.5 / Scale.y))) * Scale.y);
    float n = fbm(p, Octaves, Scale, Voronoi, Blur, Lacunarity, Gain);
    Value = n;
}
void FractalNoise_half(float2 UV, uint Octaves, float2 Pan, float2 Scale, float Voronoi, float Blur, float Lacunarity, float Gain, out float Value)
{
    float2 p = float2((UV.x + (Pan.x * (0.5 / Scale.x))) * Scale.x, (UV.y + (Pan.y * (0.5 / Scale.y))) * Scale.y);
    float n = fbm(p, Octaves, Scale, Voronoi, Blur, Lacunarity, Gain);
    Value = n;
}

// FRACTAL WARP


void FractalWarp_float(float2 UV, uint Octaves, float2 Pan, float2 Scale, float Voronoi, float Blur, float Lacunarity, float Gain, out float Value)
{
    float2 p = float2((UV.x + (Pan.x * (0.5 / Scale.x))) * Scale.x, (UV.y + (Pan.y * (0.5 / Scale.y))) * Scale.y);
    float n = fbm(p + float2(fbm(p, Octaves, Scale, Voronoi, Blur, Lacunarity, Gain), fbm(p - 1.0, Octaves, Scale, Voronoi, Blur, Lacunarity, Gain)), Octaves, Scale, Voronoi, Blur, Lacunarity, Gain);
    Value = n;
}
void FractalWarp_half(float2 UV, uint Octaves, float2 Pan, float2 Scale, float Voronoi, float Blur, float Lacunarity, float Gain, out float Value)
{
    float2 p = float2((UV.x + (Pan.x * (0.5 / Scale.x))) * Scale.x, (UV.y + (Pan.y * (0.5 / Scale.y))) * Scale.y);
    float n = fbm(p + float2(fbm(p, Octaves, Scale, Voronoi, Blur, Lacunarity, Gain), fbm(p - 1.0, Octaves, Scale, Voronoi, Blur, Lacunarity, Gain)), Octaves, Scale, Voronoi, Blur, Lacunarity, Gain);
    Value = n;
}

// VORONOI ULTIMATE


float4 VoronoiUltimate(float2 x, float2 aTiling, float2 aEdges, float aSeed)
{
    x *= aTiling;
    float2 p = floor(x);
    float2  f = frac(x);

    float2 mb = float2(0.0, 0.0);
    float2 mr = float2(0.0, 0.0);
    float i = 0.0, j = 0.0;
    float res = 8.0;
    for (j = -1.0; j <= 1.0; j += 1.0)
        for (i = -1.0; i <= 1.0; i += 1.0)
        {
            float2 b = float2(i, j);
            float2  r = b + Hash2(mod(p + b, aTiling), aSeed) - f;
            float d = dot(r, r);

            if (d < res)
            {
                res = d;
                mr = r;
                mb = b;
            }
        }

    float va = 0;
    float wt = 0;
    float cells = 1.0e10;
    res = 8.0;
    for (j = -2.0; j <= 2.0; j += 1.0)
        for (i = -2.0; i <= 2.0; i += 1.0)
        {
            float2 b = mb + float2(i, j);
            float2  o = Hash2(mod(p + b, aTiling), aSeed);
            float2  r = float2(b)+o - f;
            float d = dot(0.5 * (mr + r), normalize(r - mr));
            float drr = dot(r, r);
            if (d < res)
                res = d;
            if (drr < cells)
                cells = drr;
            float ww = pow(abs(1.0 - smoothstep(0.0, 1.414, sqrt(drr))), 64.0);
            va += o.y * ww;
            wt += ww;
        }

    float border = 1.0 - smoothstep(aEdges.x, aEdges.y, res);
    float eschema = va / wt;
    return float4(res, border, eschema, 1.0 - cells);
}

void Voronoi_float(float2 UV,
    int Seed,
    float Thickness,
    float Hardness,
    float2 Pan,
    float2 Scale,
    out float Gems,
    out float Cracks,
    out float Code,
    out float Cells
)
{
    float4 lPattern = VoronoiUltimate(mod(UV + float2(Pan.x, Pan.y), float2(1.0, 1.0)), float2(Scale.x, Scale.y), float2(Hardness, Thickness), float(Seed));
    Gems = lPattern.x;
    Cracks = lPattern.y;
    Code = lPattern.z;
    Cells = lPattern.w;
}
void Voronoi_half(
    half2 UV,
    int Seed,
    half Thickness,
    half Hardness,
    half2 Pan,
    half2 Scale,
    out half Gems,
    out half Cracks,
    out half Code,
    out half Cells
)
{
    half4 lPattern = VoronoiUltimate(mod(UV + half2(Pan.x, Pan.y), half2(1.0, 1.0)), half2(Scale.x, Scale.y), half2(Hardness, Thickness), half(Seed));
    Gems = lPattern.x;
    Cracks = lPattern.y;
    Code = lPattern.z;
    Cells = lPattern.w;
}

// SQUIRCLE

void Squircle_float(float2 UV, float2 Margins, float Falloff, float Convex, out float Value)
{
    float2 c = (UV - 0.5) * 2.0; //remap from 0->1 to -1 -> 1
    c = abs(c);
    c *= (1.0 + Margins);
    c = abs(pow(abs(c.xy), float2(Convex, Convex)));
    float f = 1.0 - length(c);
    f = smoothstep(0.0, Falloff, f);
    Value = clamp(f, 0.0, 1.0);
}
void Squircle_half(half2 UV, half2 Margins, half Falloff, half Convex, out half Value)
{
    half2 c = (UV - 0.5) * 2.0; //remap from 0->1 to -1 -> 1
    c = abs(c);
    c *= (1.0 + Margins);
    c = abs(pow(abs(c.xy), half2(Convex, Convex)));
    half f = 1.0 - length(c);
    f = smoothstep(0.0, Falloff, f);
    Value = clamp(f, 0.0, 1.0);
}

// POLYGON

void Polygon_half(half2 uv, half2 Size, int Sides, half Falloff, out half Value)
{
    half2 st = uv - half2(0.5, 0.5);
    st.x *= 1.0 - Size.x;
    st.y *= 1.0 - Size.y;
    st.y -= (Sides == 3) ? 0.25 : 0.0;
    half ata = atan2(st.x, -st.y) + M_PI;
    half r = M_TWO_PI / half(Sides);
    half dist = cos(floor(.5f + ata / r) * r - ata) * length(st);
    half polygon = 1.0f - smoothstep(.5f - Falloff, 0.5f, dist);
    Value = polygon;
}
void Polygon_float(float2 uv, float2 Size, int Sides, float Falloff, out float Value)
{
    float2 st = uv - float2(0.5, 0.5);
    st.x *= 2.0 - Size.x;
    st.y *= 2.0 - Size.y;
    st.y -= (Sides == 3) ? 0.25 : 0.0;
    float ata = atan2(st.x, -st.y) + M_PI;
    float r = M_TWO_PI / float(Sides);
    float dist = cos(floor(.5f + ata / r) * r - ata) * length(st);
    float polygon = 1.0f - smoothstep(.5f - Falloff, 0.5f, dist);
    Value = polygon;
}

// ROUNDED RECT

float roundBox(float2 p, float2 b, float r) {
    float2 d = abs(p) - b + float2(r, r);
    return min(max(d.x, d.y), 0.0) + length(max(d, 0.0)) - r;
}

void RoundedRect_float(float2 uv, float2 Size, float Radius, float Falloff, out float Value) {
    float2 p = uv - 0.5;
    float2 halfSize = Size * 0.5;
    float roundedRectDist = roundBox(p, halfSize, min(1.0,Radius/4.0));
    Value = smoothstep(0.0, -Falloff, roundedRectDist);
}


void RoundedRect_half(half2 uv, half2 Size, half Radius, half Falloff, out half Value) {
    half2 p = uv - 0.5;
    half2 halfSize = Size * 0.5;
    half roundedRectDist = roundBox(p, halfSize, min(1.0, Radius / 4.0));
    Value = smoothstep(0.0, -Falloff, roundedRectDist);
}

