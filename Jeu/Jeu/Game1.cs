using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;


namespace Jeu
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //fenêtre - à modifier
        public const int FENETRE_HAUTEUR = 0;
        public const int FENETRE_LARGEUR = 0;

        //map
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        private TiledMapTileLayer mapLayer;

        //personnage élève
        private Vector2 _elevePosition;
        private AnimatedSprite _eleve;
        private int _eleveVitesse;

        //personnage prof
        private Vector2 _profPosition;
        private AnimatedSprite _prof;
        private int _profVitesse;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _eleveVitesse = 100;
            _profVitesse = 100;
            _graphics.PreferredBackBufferWidth = FENETRE_LARGEUR;
            _graphics.PreferredBackBufferHeight = FENETRE_HAUTEUR;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //map
            _tiledMap = Content.Load<TiledMap>(""); //faudra ajouter le nom de la map
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float walkSpeed = deltaSeconds * _eleveVitesse;
            string animation = "idle";

            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                if (_elevePosition.X >= _eleve.TextureRegion.Width / 2)
                {
                    animation = "walkWest";
                    _elevePosition.X -= walkSpeed;
                }
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                if (_elevePosition.X <= FENETRE_HAUTEUR - _eleve.TextureRegion.Width / 2)
                {
                    animation = "walkEast";
                    _elevePosition.X += walkSpeed;
                }
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                if (_elevePosition.Y >= _eleve.TextureRegion.Height / 2)
                {
                    animation = "walkNorth";
                    _elevePosition.Y -= walkSpeed;
                }
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                if (_elevePosition.Y <= FENETRE_HAUTEUR - _eleve.TextureRegion.Height / 2)
                {
                    animation = "walkSouth";
                    _elevePosition.Y += walkSpeed;
                }
            }

            _eleve.Play(animation);
            _eleve.Update(deltaSeconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Red);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
