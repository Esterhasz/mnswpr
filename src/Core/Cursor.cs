using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnswpr.Core
{
    public class Cursor(int maxX, int maxY)
    {
        public int X { get => _x; set => _x = value.Clamp(0, MaxX); }
        public int Y { get => _y; set => _y = value.Clamp(0, MaxY); }

        public bool Enabled { get; set; } = true;

        public int MaxX { get; } = maxX;
        public int MaxY { get; } = maxY;

        private int _x;
        private int _y;

        public void Select(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Right() => X++;
        public void Left() => X--;

        public void Up() => Y--;
        public void Down() => Y++;
    }

}
