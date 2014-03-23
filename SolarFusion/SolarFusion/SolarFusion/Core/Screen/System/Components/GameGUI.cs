using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SolarFusion.Core.Screen
{
    public struct GUIElement
    {
        public Texture2D Texture;
        public Vector2 Position;
        public Vector2 Origin;
        public string Text;
        public Vector2 TextOrigin;
        public string Value;
        public float Scale;
    }

    public class GameGUI
    {
        protected GraphicsDevice _obj_graphics = null;
        protected List<GUIElement> _obj_elements = null;
        protected Dictionary<string, int> _obj_reference = null;
        protected SpriteFont mDefaultFont;

        #region "Properties"
        public int Ammo
        {
            set { this.UpdateElement("Ammo", value.ToString()); }
        }

        public float Health
        {
            set { this.UpdateElement("Health", value.ToString()); }
        }

        public float Points
        {
            set { this.UpdateElement("Points", value.ToString()); }
        }

        public bool Crystal
        {
            set { if (value) { this.UpdateElement("Crystal", "COLLECTED"); } else { this.UpdateElement("Crystal", "MISSING"); } }
        }
        #endregion

        public GameGUI(GraphicsDevice _graphics, SpriteFont _font)
        {
            this._obj_graphics = _graphics;
            this._obj_elements = new List<GUIElement>();
            this._obj_reference = new Dictionary<string, int>();
            this.mDefaultFont = _font;
        }

        public void Load(ContentManager _content)
        {
            this.AddElement(_content.Load<Texture2D>("Sprites/Misc/UI/GUI/life"), new Vector2(30, 30), "Health", 0.7f);
            this.AddElement(_content.Load<Texture2D>("Sprites/Objects/Static/energy_ball/sprite"), new Vector2(30, 60), "Points", 1f);
            this.AddElement(_content.Load<Texture2D>("Sprites/Objects/Static/crystal/sprite"), new Vector2(30, 90), "Crystal", 1f);
            this.AddElement(_content.Load<Texture2D>("Sprites/Objects/Static/blast/sprite"), new Vector2(30, 120), "Ammo", 1f);
        }

        public void AddElement(Texture2D _texture, Vector2 _position, string _text, float _scale)
        {
            GUIElement tmpElement = new GUIElement();
            tmpElement.Texture = _texture;
            tmpElement.Position = _position;
            tmpElement.Origin = new Vector2(_texture.Width / 2f, _texture.Height / 2f);
            tmpElement.Text = _text;
            tmpElement.Value = "";
            tmpElement.Scale = _scale;
            this._obj_elements.Add(tmpElement);
            this._obj_reference.Add(_text, this._obj_elements.Count - 1);
        }

        public void UpdateElement(string _reference, string _newValue)
        {
            if (this._obj_reference.ContainsKey(_reference))
            {
                GUIElement tmpElement = this._obj_elements[this._obj_reference[_reference]];
                tmpElement.Value = _newValue;
                this._obj_elements[this._obj_reference[_reference]] = tmpElement;
            }
        }

        public void Unload()
        {
            this._obj_graphics = null;
        }

        public void Update(GameTime _elapsedTime, Camera2D _camera)
        {
            for (int i = 0; i < this._obj_elements.Count; i++)
            {
                GUIElement tmpElement = this._obj_elements[i];
                Vector2 tmpMeasure = this.mDefaultFont.MeasureString("- " + tmpElement.Value);
                tmpElement.TextOrigin = new Vector2(0, tmpMeasure.Y / 2f);
                tmpElement.Position = new Vector2(30 + (_camera.Position.X - (this._obj_graphics.Viewport.Width / 2f)), tmpElement.Position.Y);
                this._obj_elements[i] = tmpElement;
            }
        }

        public void Draw(SpriteBatch _sb)
        {
            for (int i = 0; i < this._obj_elements.Count; i++)
            {
                _sb.Draw(this._obj_elements[i].Texture, this._obj_elements[i].Position, null, Color.White, 0f, this._obj_elements[i].Origin, this._obj_elements[i].Scale, SpriteEffects.None, 0f);
                _sb.DrawString(this.mDefaultFont, "- " + this._obj_elements[i].Value, new Vector2(this._obj_elements[i].Position.X + 30, this._obj_elements[i].Position.Y), Color.White, 0f, this._obj_elements[i].TextOrigin, 0.5f, SpriteEffects.None, 0f);
            }
        }
    }
}
