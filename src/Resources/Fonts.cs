using Exfal;
using Microsoft.Xna.Framework.Graphics;

namespace mnswpr.Resources
{
    public static class Fonts
    {
        public static SpriteFont Pico_8 { get; }

        static Fonts()
        {
            Pico_8 = Asset.Load<SpriteFont>("Pico-8");
        }
    }
}
