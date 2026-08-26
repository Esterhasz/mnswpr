using Exfal.Drawing;
using Microsoft.Xna.Framework;
using mnswpr.Core;
using mnswpr.Extensions;
using mnswpr.Resources;
using System;

namespace mnswpr
{
    public class FieldRenderer(Field field, Rectangle fieldArea)
    {
        private const float ReferenceCellSize = 7f;

        public const char FlagChar = '\u00C7';
        public const char MineChar = '\u00C6';

        public static Color GridColor { get; } = new(82, 82, 82);
        public static Color RevealedColor { get; } = new(42, 42, 46);
        public static Color UnrevealedColor { get; } = new(62, 64, 70);
        public static Color SelectedColor { get; } = new(92, 96, 108);
        public static Color FlagColor { get; } = new(235, 72, 72);

        public static Color[] NumberColors { get; } =
        [
            new(145, 145, 150), 
            new(82, 150, 255),  
            new(85, 200, 130),  
            new(235, 82, 82),   
            new(145, 105, 235), 
            new(235, 145, 75),  
            new(70, 195, 195),  
            new(205, 205, 210), 
            new(120, 120, 125), 
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
                                text = $"{MineChar}";
                                textColor = Color.Black;
                            }
                            else
                            {
                                text = $"{cell.AdjacentMines}";
                                textColor = NumberColors[cell.AdjacentMines];
                            }
                            cellColor = RevealedColor;
                            break;

                        case CellState.Flagged:
                            text = $"{FlagChar}";
                            textColor = FlagColor;
                            break;
                    }

                    Cursor cur = Field.Cursor;

                    if (cur.X == x && cur.Y == y)
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

            for (int x = 0; x < Field.Width; x++)
            {
                Vector2 end = (cellSize * new Vector2(x + 1, Field.Height)).Rounded();
                draw.Line(end.WithY(0), end, GridColor, 2);
            }
            for (int y = 0; y < Field.Height; y++)
            {
                Vector2 end = (cellSize * new Vector2(Field.Width, y + 1));
                draw.Line(end.WithX(0), end, GridColor, 2);
            }
        }
    }
}