﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using System;


namespace Jeu
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        //fenêtre - à modifier
        public const int FENETRE_HAUTEUR = 620;
        public const int FENETRE_LARGEUR = 605;

        //maps
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRendu;
        private TiledMapTileLayer _tiledMapObstacles;

        //Collisions
        private TiledMapTileLayer mapLayer;

        //personnage élève
        private Vector2 _elevePosition;
        private AnimatedSprite _eleve;
        private int _eleveVitesse;

        //personnage prof
        private Vector2 _profPosition;
        private AnimatedSprite _prof;

        //gestionnaire de scènes
        private readonly ScreenManager _screenManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //fenêtre
            _graphics.PreferredBackBufferWidth = FENETRE_LARGEUR;
            _graphics.PreferredBackBufferHeight = FENETRE_HAUTEUR;
            _graphics.ApplyChanges();

            //élève
            _eleveVitesse = 100;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //map
             _tiledMap = Content.Load<TiledMap>("Couloir"); //faudra ajouter le nom de la map
             _tiledMapRendu = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            //collisions
             _tiledMapObstacles = _tiledMap.GetLayer<TiledMapTileLayer>("Mur");

            //spritesheet élève
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("motw.sf", new JsonContentLoader());
            _eleve = new AnimatedSprite(spriteSheet);
            _elevePosition = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            //spritesheet prof
            SpriteSheet spriteSheet2 = Content.Load<SpriteSheet>("motw2.sf", new JsonContentLoader());
            _prof = new AnimatedSprite(spriteSheet2);
            _profPosition = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float walkSpeed = deltaSeconds * _eleveVitesse;
            string animation = "idle";
            string animation2 = "idle2";

            //map
            _tiledMapRendu.Update(gameTime);
            //_tiledMapRenderer.Update(gameTime);
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            //déplacements élève
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Left))
            {

                if (_elevePosition.X >= _eleve.TextureRegion.Width / 2)
                {
                    ushort tx = (ushort)(_elevePosition.X / _tiledMap.TileWidth - 1);
                    ushort ty = (ushort)(_elevePosition.Y / _tiledMap.TileHeight); //la tuile au-dessus en y
                    animation = "walkWest";
                    if (!IsCollision(tx, ty))
                        _elevePosition.X -= walkSpeed; // _persoPosition vecteur position du sprite
                }
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {

                if (_elevePosition.X <= FENETRE_HAUTEUR - _eleve.TextureRegion.Width / 2)
                {
                    ushort tx = (ushort)(_elevePosition.X / _tiledMap.TileWidth + 1);
                    ushort ty = (ushort)(_elevePosition.Y / _tiledMap.TileHeight);
                    animation = "walkEast";
                    if (!IsCollision(tx, ty))
                        _elevePosition.X += walkSpeed; // _persoPosition vecteur position du sprite
                }
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {

                if (_elevePosition.Y >= _eleve.TextureRegion.Height / 2)
                {
                    ushort tx = (ushort)(_elevePosition.X / _tiledMap.TileWidth);
                    ushort ty = (ushort)(_elevePosition.Y / _tiledMap.TileHeight - 1); //la tuile au-dessus en y
                    animation = "walkNorth";
                    if (!IsCollision(tx, ty))
                        _elevePosition.Y -= walkSpeed; // _persoPosition vecteur position du sprite
                }
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {

                if (_elevePosition.Y <= FENETRE_HAUTEUR - _eleve.TextureRegion.Height / 2)
                {
                    ushort tx = (ushort)(_elevePosition.X / _tiledMap.TileWidth);
                    ushort ty = (ushort)(_elevePosition.Y / _tiledMap.TileHeight + 2); //la tuile au-dessus en y
                    animation = "walkSouth";
                    if (!IsCollision(tx, ty))
                        _elevePosition.Y += walkSpeed; // _persoPosition vecteur position du sprite
                }
            }

            _eleve.Play(animation);
            _eleve.Update(deltaSeconds);

            //deplacement prof
            _prof.Play(animation2);
            //_prof.Update(deltaSeconds);

            
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Yellow);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            //personnages
            _spriteBatch.Draw(_eleve, _elevePosition);
            _spriteBatch.Draw(_prof, _profPosition);

            //map
            _tiledMapRendu.Draw();
            //_tiledMapRendu.Draw();

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        //méthode pour les collisions
        private bool IsCollision(ushort x, ushort y)
        {
            TiledMapTile? tile;
            if (_tiledMapObstacles.TryGetTile(x, y, out tile) == false)
                return false;
            if (!tile.Value.IsBlank)
                return true;
            return false;
        }
    }
}
