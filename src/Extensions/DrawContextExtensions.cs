using Exfal.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace mnswpr.Extensions
{
    public static class DrawContextExtensions
    {
        private static Dictionary<SpriteFont, Dictionary<char, SpriteFont.Glyph>> _cachedGlyphs = [];

        public static void CenteredChar(
            this DrawContext draw,
            char c,
            SpriteFont font,
            Vector2 position,
            Color color,
            Vector2 scale,
            float rotation,
            float depth = 0)
        {

            if (!_cachedGlyphs.TryGetValue(font, out var glyphs))
            {
                glyphs = _cachedGlyphs[font] = font.GetGlyphs();
            }

            var glyph = glyphs[c];

            var bounds = glyph.BoundsInTexture;

            Vector2 origin = new(
               bounds.Width / 2f,
               bounds.Height / 2f
            );

            draw.SpriteBatch.Draw(
                font.Texture,
                position,
                bounds,
                color,
                rotation,
                origin,
                scale,
                SpriteEffects.None,
                depth
            );
        }
    }
}
