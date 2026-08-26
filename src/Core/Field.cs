using Microsoft.Xna.Framework;
using System;

namespace mnswpr.Core
{
    public class Field
    {
        public const int SafeArea = 3;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Cursor Cursor { get; private set; } = new();

        public ref Cell this[int x, int y] => ref _cells[x, y];

        private Cell[,] _cells;

        public void New(int width, int height)
        {
            Width = width;
            Height = height;
            Cursor.Enabled = true;

            _cells = new Cell[Width, Height];
        }

        public bool Reveal()
        {
            if (!Cursor.Enabled)
                return false;

            ref Cell cell = ref _cells[Cursor.X, Cursor.Y];

            if (cell.IsMine)
            {
                cell.State = CellState.Revealed;
                return true;
            }

            if (cell.State == CellState.Flagged)
            {
                cell.State = CellState.Revealed;

                return cell.IsMine;
            }

            SafeReveal(Cursor.X, Cursor.Y);
            return false;
        }
        public void Flag()
        {
            if (!Cursor.Enabled)
                return;

            ref Cell cell = ref _cells[Cursor.X, Cursor.Y];

            if (cell.State == CellState.Flagged)
            {
                cell.State = CellState.Unrevealed;
            }
            else if (cell.State == CellState.Unrevealed)
            {
                cell.State = CellState.Flagged;
            }
        }

        public void SpawnMines(int count)
        {
            for (int i = 0; i < count; i++)
            {
                int x = Random.Shared.Next(_cells.GetLength(0));
                int y = Random.Shared.Next(_cells.GetLength(1));

                if (new Vector2(x, y).DistanceTo(new Vector2(Cursor.X, Cursor.Y)) < SafeArea)
                    continue;

                if (_cells[x, y].IsMine)
                    continue;

                _cells[x, y].IsMine = true;
            }

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (_cells[x, y].IsMine)
                        continue;

                    _cells[x, y].AdjacentMines = GetAdjacentMines(x, y);
                }
            }
        }

        private void SafeReveal(int x, int y)
        {
            ref var cell = ref _cells[x, y];

            if (cell.IsMine 
                || cell.State == CellState.Revealed
                || cell.State == CellState.Flagged)
                return;

            cell.State = CellState.Revealed;
            
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