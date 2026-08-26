using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

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

        private readonly Stack<(int x, int y)> _revealStack = new();

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

            int x = Cursor.X;
            int y = Cursor.Y;

            ref Cell cell = ref _cells[x, y];

            if (cell.State == CellState.Flagged)
                return false;

            if (cell.State == CellState.Unrevealed)
            {
                if (cell.IsMine)
                {
                    cell.State = CellState.Revealed;
                    return true;
                }

                SafeReveal(x, y);
                return false;
            }

            int minX = Math.Max(x - 1, 0);
            int maxX = Math.Min(x + 1, Width - 1);
            int minY = Math.Max(y - 1, 0);
            int maxY = Math.Min(y + 1, Height - 1);

            int flagsCount = 0;

            for (int nx = minX; nx <= maxX; nx++)
            {
                for (int ny = minY; ny <= maxY; ny++)
                {
                    if (nx == x && ny == y)
                        continue;

                    if (_cells[nx, ny].State == CellState.Flagged)
                        flagsCount++;
                }
            }

            if (flagsCount != cell.AdjacentMines)
                return false;

            for (int nx = minX; nx <= maxX; nx++)
            {
                for (int ny = minY; ny <= maxY; ny++)
                {
                    if (nx == x && ny == y)
                        continue;

                    ref Cell neighbor = ref _cells[nx, ny];

                    if (neighbor.State != CellState.Unrevealed)
                        continue;

                    if (neighbor.IsMine)
                    {
                        neighbor.State = CellState.Revealed;
                        return true;
                    }

                    SafeReveal(nx, ny);
                }
            }

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
            _revealStack.Clear();
            _revealStack.Push((x, y));

            while (_revealStack.Count > 0)
            {
                var (cx, cy) = _revealStack.Pop();

                ref Cell cell = ref _cells[cx, cy];

                if (cell.IsMine
                    || cell.State == CellState.Revealed
                    || cell.State == CellState.Flagged)
                    continue;

                cell.State = CellState.Revealed;

                if (cell.AdjacentMines > 0)
                    continue;

                int minX = Math.Max(cx - 1, 0);
                int maxX = Math.Min(cx + 1, Width - 1);
                int minY = Math.Max(cy - 1, 0);
                int maxY = Math.Min(cy + 1, Height - 1);

                for (int nx = minX; nx <= maxX; nx++)
                {
                    for (int ny = minY; ny <= maxY; ny++)
                    {
                        if (nx == cx && ny == cy)
                            continue;

                        _revealStack.Push((nx, ny));
                    }
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