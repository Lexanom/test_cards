using UnityEngine;
using UnityEngine.UI;

namespace TestApp
{
    public static class Extensions
    {
        public static Color Set(this Color color, float? r = null, float? g = null, float? b = null, float? a = null)
        {
            if (r.HasValue)
                color.r = r.Value;
            if (g.HasValue)
                color.g = g.Value;
            if (b.HasValue)
                color.b = b.Value;
            if (a.HasValue)
                color.a = a.Value;
            return color;
        }

        public static Image SetAlpha(this Image img, float value)
        {
            img.color.Set(a: value);
            return img;
        }
    }
}