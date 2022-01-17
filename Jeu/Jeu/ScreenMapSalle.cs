using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;


namespace Jeu
{
    public class ScreenMapSalle : GameScreen
    {
        private Game1 _myGame; // pour récupérer le jeu en cours
        private SpriteFont _font;
        public ScreenMapSalle(Game1 game) : base(game)
        {
            _myGame = game;
        }
        public override void LoadContent()
        {
            _font = Content.Load<SpriteFont>("Font");
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        { }
        public override void Draw(GameTime gameTime)
        {
            _myGame.GraphicsDevice.Clear(Color.Red);
            _myGame.SpriteBatch.Begin();
            _myGame.SpriteBatch.DrawString(_font, "Scene 1", new Vector2(350, 200), Color.White);
            _myGame.SpriteBatch.End();
        }
    }
}
