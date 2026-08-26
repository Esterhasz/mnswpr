namespace mnswpr.Core
{
    public enum CellState
    {
        Unrevealed,
        Revealed,
        Flag,
    }

    public struct Cell(bool mine)
    {
        public bool IsMine { get; set; } = mine;
        public int AdjacentMines { get; set; }
        public CellState State { get; set; } = CellState.Unrevealed;
    }
}