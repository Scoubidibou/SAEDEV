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
    public class ScreenMapSalle1 : GameScreen
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        //fenêtre
        public const int FENETRE_HAUTEUR = 620;
        public const int FENETRE_LARGEUR = 605;

        //maps
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRendu;
        private TiledMapTileLayer _tiledMapObstacles;

        //Collisions
        //private TiledMapTileLayer mapLayer;

        //personnage élève
        private Vector2 _elevePosition;
        private AnimatedSprite _eleve;
        private int _eleveVitesse;

        //personnage prof
        private Vector2 _profPosition;
        private AnimatedSprite _prof;
        private TypeAnimation _animation;
        private float _chrono;

        //random prof
        private Random tete = new Random();
        private int temps;

        //gestionnaire de scènes
        private readonly ScreenManager _screenManager;

        private Ecran _ecranEncours;
        public SpriteBatch SpriteBatch
        {
            get
            {
                return this._spriteBatch;
            }

            set
            {
                this._spriteBatch = value;
            }
        }

        public AnimatedSprite Eleve
        {
            get
            {
                return this._eleve;
            }

            set
            {
                this._eleve = value;
            }
        }

        public AnimatedSprite Prof
        {
            get
            {
                return this._prof;
            }

            set
            {
                this._prof = value;
            }
        }

        public Vector2 PositionEleve
        {
            get
            {
                return this._elevePosition;
            }

            set
            {
                this._elevePosition = value;
            }
        }
        public Vector2 PositionProf
        {
            get
            {
                return this._profPosition;
            }

            set
            {
                this._profPosition = value;
            }
        }

        public TypeAnimation Animation
        {
            get
            {
                return this._animation;
            }

            set
            {
                this._animation = value;
            }
        }

        public GraphicsDeviceManager Graphics
        {
            get
            {
                return this._graphics;
            }

            set
            {
                this._graphics = value;
            }
        }

        private Game1 _myGame; // pour récupérer le jeu en cours
        public ScreenMapSalle1(Game1 game) : base(game)
        {
            Content.RootDirectory = "Content";

            _myGame = game;
        }

        public override void LoadContent()
        {
            //spritesheet élève
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("motw.sf", new JsonContentLoader());
            _eleve = new AnimatedSprite(spriteSheet);
            _elevePosition = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            //spritesheet prof
            SpriteSheet spriteSheet2 = Content.Load<SpriteSheet>("motw2.sf", new JsonContentLoader());
            _prof = new AnimatedSprite(spriteSheet2);
            _profPosition = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            _tiledMap = Content.Load<TiledMap>("salleDeCour8"); //faudra ajouter le nom de la map
            _tiledMapRendu = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float walkSpeed = deltaSeconds * _eleveVitesse;
            string animation = "idle";
            string animation2 = "idle2";
            _tiledMapRendu.Update(gameTime);
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
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

            if (_chrono >= 5 + temps)
                animation2 = "tetehaut";
            if (_chrono >= temps + 10)
            {
                _chrono = 0;
                temps = tete.Next(0, 10);
            }
            if (_chrono < 5 + temps)
                animation2 = "tetebas";


            _chrono += deltaSeconds;
            _prof.Update(deltaSeconds);
            _prof.Play(animation2);
        }

        private void Exit()
        {
            throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime)
        {
            _myGame.SpriteBatch.Begin();
            _spriteBatch.Draw(_eleve, _elevePosition);
            _spriteBatch.Draw(_prof, _profPosition);

            
            _tiledMapRendu.Draw();
            _myGame.SpriteBatch.End();
        }
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
