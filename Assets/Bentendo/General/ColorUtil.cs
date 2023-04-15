using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo
{
	public class ColorUtil
    {
        public static string ColorToHexString(Color color)
        {
            int r = (int)(color.r * 255);
            int g = (int)(color.g * 255);
            int b = (int)(color.b * 255);
            int a = (int)(color.a * 255);

            string hex = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, a);
            return hex;
        }
    }
}
