global using Exfal.Extensions;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Exfal.Drawing;
using Exfal.InputHandling;
using Exfal;

namespace mnswpr
{
    public class Main : Game
    {
        public static Point VirtualResoulution { get; } = new(128, 128);
        public static Point WindowResolution { get; } = new(384, 384);

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Renderer _renderer; 

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = WindowResolution.X,
                PreferredBackBufferHeight = WindowResolution.Y,
            };

            Window.ClientSizeChanged += (s, a) =>
            {
                _renderer.WindowBounds = GraphicsDevice.Viewport.Bounds;
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Window.AllowUserResizing = true;
            Window.AllowAltF4 = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _renderer = new(_spriteBatch, _graphics, VirtualResoulution)
            {
                BackgroundColor = Color.Black,
                Options = new() { SamplerState = SamplerState.PointClamp },
                ScaleFunc = Renderer.OutputScaler.Fit
            };
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();

            if (Input.IsKeyDown(Key.F1))
                Exit();

            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _renderer.Draw();
            base.Draw(gameTime);
        }
    }
}
