using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using Microsoft.Xna.Framework.Media;
using System;
using Microsoft.Xna.Framework.Content;

namespace Jeu
{
    public enum Ecran
    {
        Principal, Salle1, Salle2, Salle3, Salle4, Salle5, Salle6,
        SallePrincipale
    }
    public enum TypeAnimation { walkSouth, walkNorth, walkEast, walkWest, idle };

    public class Game1 : Game
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
        private TiledMapTileLayer _tilesMapCoffre;

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


        private Vector2 _CoeurPosition;
        private Vector2 _CoeurPosition1;
        private Vector2 _CoeurPosition2;
        private AnimatedSprite _CoeurRouge;
        private AnimatedSprite _CoeurNoir;

        //random prof
        private Random tete = new Random();
        private int temps;

        //gestionnaire de scènes
        private readonly ScreenManager _screenManager;

        //son
        private Song _sonJeu;

        private Ecran _ecranEncours;
        internal object spriteBatch;

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

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //initialisation du gestionnaire de scènes
            _screenManager = new ScreenManager();
            Components.Add(_screenManager);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //faire une classe pour la map principale
            _elevePosition = new Vector2(300, 300);
            base.Initialize();

            //fenêtre
            _graphics.PreferredBackBufferWidth = FENETRE_LARGEUR;
            _graphics.PreferredBackBufferHeight = FENETRE_HAUTEUR;
            _graphics.ApplyChanges();

            //élève
            _eleveVitesse = 100;

            //chrono prof
            _chrono = 0;
            temps = tete.Next(0, 10);
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
            _tilesMapCoffre = _tiledMap.GetLayer<TiledMapTileLayer>("Coffre");
            //spritesheet élève
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("motw.sf", new JsonContentLoader());
            _eleve = new AnimatedSprite(spriteSheet);
            _elevePosition = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            SpriteSheet spriteSheet3 = Content.Load<SpriteSheet>("motw_coeurR.sf", new JsonContentLoader());
            _CoeurRouge = new AnimatedSprite(spriteSheet3);
            _CoeurPosition = new Vector2(580, 10);
            _CoeurPosition1 = new Vector2(560, 10);
            _CoeurPosition2 = new Vector2(540, 10);

            SpriteSheet spriteSheet4 = Content.Load<SpriteSheet>("motw_coeurN.sf", new JsonContentLoader());
            _CoeurNoir = new AnimatedSprite(spriteSheet3);

            //_screenManager.LoadScreen(_screenMapPrincipale, new FadeTransition(GraphicsDevice, Color.Black));
            _ecranEncours = Ecran.Principal;

            //son
            _sonJeu = Content.Load<Song>("sonJeu");
            MediaPlayer.Play(_sonJeu);

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

            //ia prof
            if ((keyboardState.IsKeyDown(Keys.Left)) || (keyboardState.IsKeyDown(Keys.Right)) || (keyboardState.IsKeyDown(Keys.Up)) || keyboardState.IsKeyDown(Keys.Down))
            {
                if (animation2 == "tetehaut")
                {
                    _CoeurPosition = new Vector2(999, 999);
                }
            }

            Console.WriteLine(PositionEleve);
            //changements de maps



            if (_elevePosition.X >= 340 && _elevePosition.X <= 350 && _elevePosition.Y >= 495 && _elevePosition.Y <= 550)
            {
                LoadScreen1();
                _ecranEncours = Ecran.Salle1;
                _elevePosition = new Vector2(170, 280);
                if (_elevePosition.X >= 165 && _elevePosition.X <= 180 && _elevePosition.Y >= 250 && _elevePosition.Y <= 295 && (keyboardState.IsKeyDown(Keys.Left)))
                {
                    LoadScreen0();
                    _ecranEncours = Ecran.SallePrincipale;
                    _elevePosition = new Vector2(300, 300);
                }
            }
            else if (_elevePosition.X >= 112 && _elevePosition.Y <= 160 && _elevePosition.X <= 125 && _elevePosition.Y >= 205)
            {
                LoadScreen2();
                _ecranEncours = Ecran.Salle2;
                _elevePosition = new Vector2(300, 300);
                if (_elevePosition.X >= 165 && _elevePosition.X <= 180 && _elevePosition.Y >= 250 && _elevePosition.Y <= 295 && (keyboardState.IsKeyDown(Keys.Left)))
                {
                    LoadScreen0();
                    _ecranEncours = Ecran.SallePrincipale;
                    _elevePosition = new Vector2(300, 300);
                }
            }
            else if (_elevePosition.X >= 15 && _elevePosition.Y >= 80 && _elevePosition.X <= 35 && _elevePosition.Y <= 125)
            {
                LoadScreen3();
                _ecranEncours = Ecran.Salle3;
                _elevePosition = new Vector2(320, 190);
                if (_elevePosition.X >= 310 && _elevePosition.X <= 330 && _elevePosition.Y >= 180 && _elevePosition.Y <= 200 && (keyboardState.IsKeyDown(Keys.Left)))
                {
                    LoadScreen0();
                    _ecranEncours = Ecran.SallePrincipale;
                    _elevePosition = new Vector2(300, 300);
                }
            }
            else if (_elevePosition.X >= 110 && _elevePosition.Y >= 80 && _elevePosition.X <= 130 && _elevePosition.Y <= 125)
            {
                LoadScreen4();
                _ecranEncours = Ecran.Salle4;
                _elevePosition = new Vector2(170, 240);
                if (_elevePosition.X >= 160 && _elevePosition.X <= 180 && _elevePosition.Y >= 230 && _elevePosition.Y <= 250 && (keyboardState.IsKeyDown(Keys.Left)))
                {
                    LoadScreen0();
                    _ecranEncours = Ecran.SallePrincipale;
                    _elevePosition = new Vector2(300, 300);
                }
            }
            else if (_elevePosition.X >= 15 && _elevePosition.Y >= 160 && _elevePosition.X <= 35 && _elevePosition.Y <= 208)
            {
                LoadScreen5();
                _ecranEncours = Ecran.Salle5;
                _elevePosition = new Vector2(470, 250);
                if (_elevePosition.X >= 460 && _elevePosition.X <= 480 && _elevePosition.Y >= 240 && _elevePosition.Y <= 260 && (keyboardState.IsKeyDown(Keys.Left)))
                {
                    LoadScreen0();
                    _ecranEncours = Ecran.SallePrincipale;
                    _elevePosition = new Vector2(300, 300);
                }
            }
            else if (_elevePosition.X >= 480 && _elevePosition.Y <= 120)
            {
                LoadScreen6();
                _ecranEncours = Ecran.Salle6;
                _elevePosition = new Vector2(280, 390);
                if (_elevePosition.X >= 270 && _elevePosition.X <= 290 && _elevePosition.Y >= 380 && _elevePosition.Y <= 400 && (keyboardState.IsKeyDown(Keys.Left)))
                {
                    LoadScreen0();
                    _ecranEncours = Ecran.SallePrincipale;
                    _elevePosition = new Vector2(300, 300);
                }
            }

            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Yellow);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            //map
            _tiledMapRendu.Draw();

            //personnages
            _spriteBatch.Draw(_eleve, _elevePosition);

            _spriteBatch.Draw(_CoeurRouge, _CoeurPosition);
            _spriteBatch.Draw(_CoeurRouge, _CoeurPosition1);
            _spriteBatch.Draw(_CoeurRouge, _CoeurPosition2);

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

        //méthodes pour load les différentes map
        //private void LoadScreen0()
        //{
        //    _screenManager.LoadScreen(this, new FadeTransition(GraphicsDevice, Color.Black));

        //}
        private void LoadScreen0()
        {
            _screenManager.LoadScreen(new ScreenMapPrincipale(this), new FadeTransition(GraphicsDevice, Color.Black));

        }
        private void LoadScreen1()
        {
            _screenManager.LoadScreen(new ScreenMapSalle1(this), new FadeTransition(GraphicsDevice, Color.Black));

        }

        private void LoadScreen2()
        {
            _screenManager.LoadScreen(new ScreenMapSalle2(this), new FadeTransition(GraphicsDevice, Color.Black));

        }
        private void LoadScreen3()
        {
            _screenManager.LoadScreen(new ScreenMapSalle3(this), new FadeTransition(GraphicsDevice, Color.Black));

        }
        private void LoadScreen4()
        {
            _screenManager.LoadScreen(new ScreenMapSalle4(this), new FadeTransition(GraphicsDevice, Color.Black));

        }
        private void LoadScreen5()
        {
            _screenManager.LoadScreen(new ScreenMapSalle5(this), new FadeTransition(GraphicsDevice, Color.Black));

        }
        private void LoadScreen6()
        {
            _screenManager.LoadScreen(new ScreenMapSalle6(this), new FadeTransition(GraphicsDevice, Color.Black));

        }

    }
}
