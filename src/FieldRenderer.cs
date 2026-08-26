using Exfal.Drawing;
using Microsoft.Xna.Framework;
using mnswpr.Core;
using mnswpr.Extensions;
using mnswpr.Resources;
using System;

namespace mnswpr
{
    public class FieldRenderer
    {
        private const float ReferenceCellSize = 7f;

        public char Flag { get; private set; }
        public char Mine { get; private set; }

        public Field Field { get; set; } 
        public Rectangle FieldArea { get; set; }

        public FieldRenderer(Field field, Rectangle fieldArea)
        {
            Field = field;
            FieldArea = fieldArea;

            Config config = Main.Config;

            Flag = Config.SpecialSymbols[config.Flag];
            Mine = Config.SpecialSymbols[config.Mine];
        }

        public void Draw(DrawContext draw)
        {
            Vector2 start = FieldArea.Location.ToVector2();

            float cellWidth = FieldArea.Width / (float)Field.Width;
            float cellHeight = FieldArea.Height / (float)Field.Height;

            Vector2 cellSize = new(cellWidth, cellHeight);

            Config config = Main.Config;

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
                    Color cellColor = config.UnrevealedColor;

                    switch (cell.State)
                    {
                        case CellState.Revealed:
                            if (cell.IsMine)
                            {
                                text = $"{Mine}";
                                textColor = Color.Black;
                            }
                            else
                            {
                                text = $"{cell.AdjacentMines}";
                                textColor = config.NumberColors[cell.AdjacentMines];
                            }
                            cellColor = config.RevealedColor;
                            break;

                        case CellState.Flagged:

                            text = $"{Flag}";
                            textColor = config.FlagColor;
                            break;
                    }

                    Cursor cur = Field.Cursor;

                    if (cur.X == x && cur.Y == y)
                        cellColor = config.SelectedColor;

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
                draw.Line(end.WithY(0), end, Main.Config.GridColor, Main.Config.GridThickness);
            }
            for (int y = 0; y < Field.Height; y++)
            {
                Vector2 end = (cellSize * new Vector2(Field.Width, y + 1));
                draw.Line(end.WithX(0), end, Main.Config.GridColor, Main.Config.GridThickness);
            }
        }
    }
}