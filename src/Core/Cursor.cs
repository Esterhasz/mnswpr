namespace mnswpr.Core
{
    public class Cursor
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool Enabled { get; set; } = true;

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
