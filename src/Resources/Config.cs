using Microsoft.Xna.Framework;

namespace mnswpr.Resources
{
    public class Config
    {
        public const string SpecialSymbols = "ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛ";

        public Point VirtualResoulution { get; set; } = new(1024, 1024);
        public Point WindowResolution { get; set; } = new(640, 640);

        public int MineCount { get; set; } = 99;

        public int FieldWidth { get; set; } = 20;
        public int FieldHeight { get; set; } = 20;

        public int Flag { get; set; } = 7;
        public int Mine { get; set; } = 6;

        public int GridThickness { get; set; } = 2;

        public Color GridColor { get; set; } = new(82, 82, 82);
        public Color RevealedColor { get; set; } = new(42, 42, 46);
        public Color UnrevealedColor { get; set; } = new(62, 64, 70);
        public Color SelectedColor { get; set; } = new(92, 96, 108);
        public Color FlagColor { get; set; } = new(235, 72, 72);

        public Color[] NumberColors { get; set; } =
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
    }
}