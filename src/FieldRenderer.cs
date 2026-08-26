using Exfal.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mnswpr.Core;
using mnswpr.Resources;
using System;
using System.Collections.Generic;
using mnswpr.Extensions;

namespace mnswpr
{
    public class FieldRenderer(Field field, Rectangle fieldArea)
    {
        private const float ReferenceCellSize = 7f;

        public static Color RevealedColor { get; } = new(160, 160, 160);
        public static Color UnrevealedColor { get; } = new(190, 190, 190);
        public static Color SelectedColor { get; } = new(210, 210, 210);
        public static Color FlagColor { get; } = Color.Red;

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

        public Field Field { get; set; } = field;
        public Rectangle FieldArea { get; set; } = fieldArea;

        public void Draw(DrawContext draw)
        {
            Vector2 start = FieldArea.Location.ToVector2();

            float cellWidth = FieldArea.Width / (float)Field.Width;
            float cellHeight = FieldArea.Height / (float)Field.Height;

            Vector2 cellSize = new(cellWidth, cellHeight);

            for (int y = 0; y < Field.Height; y++)
            {
                for (int x = 0; x < Field.Width; x++)
                {
                    Vector2 pos = new(
                        start.X + x * cellWidth,
                        start.Y + y * cellHeight);

                    string text = " ";
                    Cell cell = Field[x, y];

                    Color textColor = Color.White;
                    Color cellColor = UnrevealedColor;

                    switch (cell.State)
                    {
                        case CellState.Revealed:
                            if (cell.IsMine)
                            {
                                text = "\u00C6";
                                textColor = Color.Black;
                            }
                            else
                            {
                                text = $"{cell.AdjacentMines}";
                                textColor = NumberColors[cell.AdjacentMines];
                                textColor.A = 255;
                            }
                            cellColor = RevealedColor;
                            break;

                        case CellState.Flagged:
                            text = "P";
                            textColor = FlagColor;
                            break;
                    }

                    if (Field.Cursor.X == x && Field.Cursor.Y == y)
                        cellColor = SelectedColor;

                    draw.Rectangle(pos, cellSize, cellColor);

                    if (text != " ")
                    {
                        int scaleFactor = Math.Max(1, (int)Math.Min(cellWidth / ReferenceCellSize, cellHeight / ReferenceCellSize));
                        Vector2 fontScale = new(scaleFactor, scaleFactor);
                        
                        draw.CenteredChar(text[0], Fonts.Pico_8, pos + cellSize / 2, textColor, fontScale, 0);
                    }
                }
            }
        }
    }
}