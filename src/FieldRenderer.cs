using System;
using Exfal.Drawing;
using Microsoft.Xna.Framework;
using mnswpr.Core;

namespace mnswpr
{
    public class FieldRenderer
    {
        public static Color RevealedColor { get; } = new(160, 160, 160);
        public static Color UnrevealedColor { get; } = new(190, 190, 190);
        public static Color HoverColor { get; } = new(210, 210, 210);
        public static Color[] NumberColors { get; } =
        [
            RevealedColor * 0.95f,  // 0
            Color.Blue,             // 1
            Color.Green,            // 2
            Color.Red,              // 3
            Color.DarkBlue,         // 4
            Color.DarkRed,          // 5
            Color.DarkCyan,         // 6
            Color.Black,            // 7
            Color.LightGray,        // 8
        ];

        public Field Field { get; set; }

        public void Draw(DrawContext context)
        {
            
        }
    }
}