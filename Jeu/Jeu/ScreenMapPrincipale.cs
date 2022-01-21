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
    public class ScreenMapPrincipale : GameScreen
    {
        private Game1 _game1; // pour récupérer la fenêtre de jeu principale

        public static int FENETRE_LARGEUR { get; internal set; }
        public static int FENETRE_HAUTEUR { get; internal set; }

        public ScreenMapPrincipale(Game1 game) : base(game)
        {
            _game1 = game;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            
        }

    }
}
