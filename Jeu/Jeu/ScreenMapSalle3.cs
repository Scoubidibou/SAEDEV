﻿using System;
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
    class ScreenMapSalle3 : GameScreen
    {
        private Game1 _myGame; // pour récupérer le jeu en cours
        public ScreenMapSalle3(Game1 game) : base(game)
        {
            _myGame = game;
        }
        public override void LoadContent()
        {
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        { }
        public override void Draw(GameTime gameTime)
        {
            _myGame.GraphicsDevice.Clear(Color.Yellow);
            _myGame.SpriteBatch.Begin();
            _myGame.SpriteBatch.End();
        }
    }
}
