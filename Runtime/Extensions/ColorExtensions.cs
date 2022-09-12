using System;
using UnityEngine;

namespace Phobebase.Extensions 
{
    public static class ColorExtensions
    {
        // Make other Complementary Colours
        public static Color GetComplementaryColor(this Color color)
        {
            float H, S, V;

            Color.RGBToHSV(color, out H, out S, out V);

            H = (H + 0.5f) % 1;

            return Color.HSVToRGB(H,S,V);
        }

        // Make other Complementary Colours with alterable Saturation and Luminance
        public static Color GetComplementaryColor(this Color color, float SMulti = 1, float VMulti = 1)
        {
            float H, S, V;

            Color.RGBToHSV(color, out H, out S, out V);

            H = (H + 0.5f) % 1;

            return Color.HSVToRGB(H,S * SMulti,V * VMulti);
        }

        // Make other Triadic Colours
        public static Color[] GetTriadicColor(this Color color)
        {
            float H, H2, S, V;

            Color.RGBToHSV(color, out H, out S, out V);

            H = (H + (1/3)) % 1;
            H2 = (H + (1/3)) % 1;

            return new Color[]{color, Color.HSVToRGB(H,S,V), Color.HSVToRGB(H2,S,V)};
        }

        // Make other Triadic Colours with alterable Saturation and Luminance
        public static Color[] GetTriadicColor(this Color color, float[] SMultis = null, float[] VMultis = null)
        {
            if (SMultis == null) SMultis = new float[] { 1, 1 };
            if (VMultis == null) VMultis = new float[] { 1, 1 };

            float H, H2, S, V;

            Color.RGBToHSV(color, out H, out S, out V);

            H = (H + (1/3)) % 1;
            H2 = (H + (1/3)) % 1;

            return new Color[]{color, Color.HSVToRGB(H,S*SMultis[0],V*VMultis[0]), Color.HSVToRGB(H2,S*SMultis[1],V*VMultis[1])};
        }
    }
}