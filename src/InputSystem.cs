using Exfal.Drawing;
using Exfal.InputHandling;
using Microsoft.Xna.Framework;
using mnswpr.Core;
using System;
using System.Collections.Generic;

namespace mnswpr
{
    public class InputSystem
    {
        public Field Field { get; set; }
        public FieldRenderer FieldRenderer { get; set; }
        
        public Camera Camera { get; set; }

        public Action RevealRequested { get; set; }
        public Action FlagRequested { get; set; }

        public InputSystem(Camera camera, Field field, FieldRenderer fieldRenderer)
        {
            Field = field;
            Camera = camera;
            FieldRenderer = fieldRenderer;

        }

        public void Update(in ViewportPoint vp)
        {
            var field = Field;
            var fieldArea = FieldRenderer.FieldArea;
            var worldPos = Camera.ToWorldPoint(vp);

            if (!field.Cursor.Enabled)
                return;

            if (!fieldArea.Contains(worldPos))
            {
                field.Cursor.Select(-1, -1);
                return;
            }

            float cellWidth = fieldArea.Width / (float)field.Width;
            float cellHeight = fieldArea.Height / (float)field.Height;

            int x = (int)((worldPos.X - fieldArea.X) / cellWidth);
            int y = (int)((worldPos.Y - fieldArea.Y) / cellHeight);

            field.Cursor.Select(
                x.Clamp(0, field.Width - 1), 
                y.Clamp(0, field.Height - 1));

            if (Input.JustDown(Key.MouseLeft))
                RevealRequested?.Invoke();
            else if (Input.JustUp(Key.MouseRight))
                FlagRequested?.Invoke();

        }

        public static bool TrySelectCell(Vector2 worldPos, Rectangle fieldArea, Field field)
        {
            
            return true;
        }
    }
}
