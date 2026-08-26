using mnswpr.Core;
using mnswpr.Resources;

namespace mnswpr
{
    public enum GameState
    {
        Innocent, 
        Playing,
        GameOver,
        Win,
    }

    public class Minesweeper
    {
        public Field Field { get; set; }
        public GameState State { get; set; }

        public Minesweeper(Field field, InputSystem input)
        {
            Field = field;

            input.RevealRequested += OnReveal;
            input.FlagRequested += OnFlag;
        }

        
        public void New()
        {
            Field.New(Config.FieldWidth, Config.FieldHeight);
            State = GameState.Innocent;
        }
        public void OnReveal()
        {
            switch (State)
            {
                case GameState.Innocent:
                    Field.SpawnMines(Config.MineCount);
                    Field.Reveal();
                    State = GameState.Playing;

                    break;

                case GameState.Playing:
                    if (Field.Reveal())
                    {
                        for (int y = 0; y < Field.Height; y++)
                        {
                            for (int x = 0; x < Field.Width; x++)
                            {
                                ref Cell cell = ref Field[x, y];

                                if (cell.IsMine)
                                    cell.State = CellState.Revealed;
                            }
                        }

                        Field.Cursor.Enabled = false;
                        State = GameState.GameOver;
                    }
                    break;

                case GameState.GameOver:
                    break;

                case GameState.Win:
                    break;
            }

        }
        public void OnFlag()
        {
            Field.Flag();
        }
    }
}
