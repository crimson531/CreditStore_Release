using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using GameDataEditor;
using I2.Loc;
using DarkTonic.MasterAudio;
using ChronoArkMod;
using ChronoArkMod.Plugin;
using ChronoArkMod.Template;
using Debug = UnityEngine.Debug;
using ChronoArkMod.ModData;
using HarmonyLib;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Text;
using ue = UnityEngine;

namespace CreditStore_Release
{
    public static class ColorOps
    {
        public static double RedVal => System.Drawing.Color.FromArgb(1, 0, 0, 0).ToArgb();
        public static double GreVal => System.Drawing.Color.FromArgb(0, 1, 0, 0).ToArgb();
        public static double BluVal => System.Drawing.Color.FromArgb(0, 0, 1, 0).ToArgb();
        public static double ContrastRatio(ue.Color color1, ue.Color color2)
        {
            if (color1.ToNumber() > color2.ToNumber())
            {
                return (CalculateLuminosity(color1) + 0.05) / (CalculateLuminosity(color2) + 0.05);
            }
            else
            {
                return (CalculateLuminosity(color2) + 0.05) / (CalculateLuminosity(color1) + 0.05);
            }
        }
        public static double CalculateLuminosity(ue.Color color)
        {
            double result = 0d;

            if (color != null)
            {
                double r = AdjustValue(color.r);
                double g = AdjustValue(color.g);
                double b = AdjustValue(color.b);

                result = 0.2126d * r + 0.7152d * g + 0.0722 * b;

            }

            return result;
        }
        public static double AdjustValue(double value)
        {
            if (value <= 0.03928d)
            {
                return value / 12.92d;
            }
            else
            {
                return Math.Pow(((value + 0.055d) / 1.055d), 2.4d);
            }
        }
        public static double ToNumber(this ue.Color color)
        {
            return (color.r * 255) + (color.g * 255) + (color.b * 255);
        }
        public static ue.Color GetColorForContrast(this ue.Color color1, double desiredRatio = 4.5d)
        {
            double L2 = CalculateLuminosity(color1);
            double L1 = (desiredRatio * (L2 + 0.05d)) - 0.05d;
            double r = LuminosityToRGB(L1, CTarg.Red);
            double g = LuminosityToRGB(L1, CTarg.Green);
            double b = LuminosityToRGB(L1, CTarg.Blue);
            return new ue.Color((float)r, (float)g, (float)b, 0.5f);
        }
        public static double LuminosityToRGB(this double L1, CTarg targ)
        {

            switch (targ)
            {
                case CTarg.Red:
                    return (L1 * 0.2125d).Clamp(0, 1);
                case CTarg.Green:
                    return (L1 * 0.7152d).Clamp(0, 1);
                case CTarg.Blue:
                    return (L1 * .0722d).Clamp(0, 1);
                default: return 0;
            }
        }
        public static double Clamp(this double value, double min, double max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
        public enum CTarg
        {
            Red,
            Green,
            Blue
        }
        public static ue.UI.ColorBlock Clone(this ue.UI.ColorBlock obj, params Tuple<string, object>[] setFields)
        {
            float cm = (float)setFields.Where(t => t.Item1 == nameof(ColorBlock.colorMultiplier)).Select(t => t.Item2).ToDefaultIfNull(obj.colorMultiplier);
            float fade = (float)setFields.Where(t => t.Item1 == nameof(ColorBlock.fadeDuration)).Select(t => t.Item2).ToDefaultIfNull(obj.fadeDuration);
            Color disable = (Color)setFields.Where(t => t.Item1 == nameof(ColorBlock.disabledColor)).Select(t => t.Item2).ToDefaultIfNull(obj.disabledColor); 
            Color normal = (Color)setFields.Where(t => t.Item1 == nameof(ColorBlock.normalColor)).Select(t => t.Item2).ToDefaultIfNull(obj.normalColor); 
            Color highlight = (Color)setFields.Where(t => t.Item1 == nameof(ColorBlock.highlightedColor)).Select(t => t.Item2).ToDefaultIfNull(obj.highlightedColor);
            Color pressed = (Color)setFields.Where(t => t.Item1 == nameof(ColorBlock.pressedColor)).Select(t => t.Item2).ToDefaultIfNull(obj.pressedColor);
            ColorBlock cb = new ColorBlock()
            {
                colorMultiplier = cm,
                disabledColor = disable,
                normalColor = normal,
                highlightedColor = highlight,
                fadeDuration = fade,
                pressedColor = pressed
            };
            return cb;
        }
        public enum ColorBlockTarget
        {
            ColorMultiplier = 0x00000000,
            DisabledColor = 0x00000001,
            NormalColor = 0x00000010,
            HighlightedColor = 0x00000100,
            FadeDuration = 0x00001000,
            PressedColor = 0x00010000,
            None = 0x00100000
        }
        // R = (L1 + 0.05) / (L2 + 0.05)
        // R * (L2 + 0.05) = (L1 + 0.05)
        // (R * (L2 + 0.05)) - 0.05 = L1

    }
}
