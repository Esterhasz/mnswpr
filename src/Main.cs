global using Exfal.Extensions;
using Exfal;
using Exfal.Drawing;
using Exfal.InputHandling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mnswpr.Core;
using mnswpr.Resources;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

namespace mnswpr
{
    public class Main : Game
    {
        public static Config Config { get; private set; } = new();
        public const string ConfigPath = "config.txt";


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Renderer _renderer;

        private Field _field;
        private FieldRenderer _fieldRenderer;

        private Minesweeper _minesweeper;
        private InputSystem _inputSystem;

        private Camera _camera;    

        public Main()
        {
            if (!File.Exists(ConfigPath))
            {
                File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(Config, Formatting.Indented));
            }

            string json = File.ReadAllText(ConfigPath);
            Config = JsonConvert.DeserializeObject<Config>(json);

            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Config.WindowResolution.X,
                PreferredBackBufferHeight = Config.WindowResolution.Y,
            };

            Window.ClientSizeChanged += (s, a) =>
            {
                _renderer.WindowBounds = GraphicsDevice.Viewport.Bounds;
            };

            Content.RootDirectory = "Content";
            Asset.Content = Content;

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
            _renderer = new(_spriteBatch, _graphics, Config.VirtualResoulution)
            {
                BackgroundColor = Color.Black,
                Options = new() { SamplerState = SamplerState.PointClamp },
                ScaleFunc = Renderer.OutputScaler.Fit
            };
            
            _camera = _renderer.Cameras[0] = _renderer.CreateCamera();
            _camera.Options = new() { SamplerState = SamplerState.PointClamp };
            _camera.Layers = [[]];

            _field = new();
            _field.New(Config.FieldWidth, Config.FieldHeight);
            _fieldRenderer = new(_field, new(Point.Zero, Config.VirtualResoulution));

            _inputSystem = new(_camera, _field, _fieldRenderer);
            _minesweeper = new(_field, _inputSystem);

            _camera.Layers[0].Add(_fieldRenderer.Draw);

            Activated += (e, a) =>
            {
                var task = StepTask.Run(() => CursorEnable(1));
                task.Completed += () => 
                _field.Cursor.Enabled = true;
            };
            Deactivated += (e, a) =>
            {
                _field.Cursor.Enabled = false;
            };
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Update(gameTime);
            Input.Update();
            StepTask.Update();

            if (Input.IsKeyDown(Key.F1))
                Exit();

            if (Input.IsKeyDown(Key.F2))
                _minesweeper.New();

            var vp = _renderer.ToViewportPoint(Input.MousePosition);

            _inputSystem.Update(vp);
        }

        protected override void Draw(GameTime gameTime)
        {
            _renderer.Draw();
        }

        private IEnumerator CursorEnable(int frames)
        {
            yield return StepTask.Yields.WaitWhile(() => frames-- > 0);
        }
    }
}
