using Microsoft.Xna.Framework;
using System;
using System.Drawing;

namespace mnswpr.Core
{
    public class Field(int width, int height, int mineCount)
    {
        public static Vector2 SafeArea { get; } = new(3, 3);
        
        private Cell[,] _cells = new Cell[width, height];
        public int Width { get; } = width;
        public int Height { get; } = height;

        public Cursor Cursor { get; } = new(width - 1, height - 1);

        public ref Cell this[int x, int y] => ref _cells[x, y];


        public bool Reveal()
        {
            if (!Cursor.Enabled)
                return true;

            if (_cells[Cursor.X, Cursor.Y].IsMine)
                return false;

            SafeReveal(Cursor.X, Cursor.Y);
            return true;
        }

        public void SpawnMines()
        {
            for (int i = 0; i < mineCount; i++)
            {
                int x = Random.Shared.Next(_cells.GetLength(0));
                int y = Random.Shared.Next(_cells.GetLength(1));

                if (new Vector2(x, y).DistanceTo(SafeArea) < 3)
                    continue;

                if (_cells[x, y].IsMine)
                    continue;

                _cells[x, y] = new(true);
            }

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (_cells[x, y].IsMine)
                        return;

                    _cells[x, y].AdjacentMines = GetAdjacentMines(x, y);
                }
            }
        }

        private void SafeReveal(int x, int y)
        {
            ref var cell = ref _cells[x, y];

            if (cell.IsMine 
                || cell.State == CellState.Revealed
                || cell.State == CellState.Flag )
                return;

            cell.State = CellState.Revealed;
            cell.AdjacentMines = GetAdjacentMines(x, y);

            if (cell.AdjacentMines > 0)
                return;

            int minX = Math.Max(x - 1, 0);
            int maxX = Math.Min(x + 1, Width - 1);
            int minY = Math.Max(y - 1, 0);
            int maxY = Math.Min(y + 1, Height - 1);

            for (int nx = minX; nx <= maxX; nx++)
            {
                for (int ny = minY; ny <= maxY; ny++)
                {
                    if (nx == x && ny == y)
                        continue;

                    SafeReveal(nx, ny);
                }
            }

        }
        private int GetAdjacentMines(int x, int y)
        {
            int count = 0;

            int minX = Math.Max(x - 1, 0);
            int maxX = Math.Min(x + 1, Width - 1);

            int minY = Math.Max(y - 1, 0);
            int maxY = Math.Min(y + 1, Height - 1);

            for (int dx = minX; dx <= maxX; dx++)
            {
                for (int dy = minY; dy <= maxY; dy++)
                {
                    if (dx == x && dy == y)
                        continue;

                    if (_cells[dx, dy].IsMine)
                        count++;
                }
            }

            return count;
        }
    }
}