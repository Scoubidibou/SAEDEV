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
using MonoGame.Extended.Screens.Transitions;
using Microsoft.Xna.Framework.Media;


namespace Jeu
{
    public class ScreenMapSalle3 : GameScreen
    {
        private new Game1 Game => (Game1)base.Game; // pour récupérer le jeu en cours

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

        //random prof
        private Random tete = new Random();
        private int temps;

        //gestionnaire de scènes
        private readonly ScreenManager _screenManager;

        //son
        private Song _sonJeu;

        private Ecran _ecranEncours;

        public ScreenMapSalle3(Game game) : base(game)
        {
        }

        public override void LoadContent()
        {

            base.LoadContent();
            //map
            _tiledMap = Content.Load<TiledMap>("SalleDeCour6"); //faudra ajouter le nom de la map
            _tiledMapRendu = new TiledMapRenderer(GraphicsDevice, _tiledMap);

            //collisions
            _tiledMapObstacles = _tiledMap.GetLayer<TiledMapTileLayer>("Mur");
            _tilesMapCoffre = _tiledMap.GetLayer<TiledMapTileLayer>("Coffre");

            //spritesheet élève
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("motw.sf", new JsonContentLoader());
            _eleve = new AnimatedSprite(spriteSheet);
            _elevePosition = new Vector2(320,190);

            //spritesheet prof
            SpriteSheet spriteSheet2 = Content.Load<SpriteSheet>("motw2.sf", new JsonContentLoader());
            _prof = new AnimatedSprite(spriteSheet2);
            _profPosition = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            SpriteSheet spriteSheet3 = Content.Load<SpriteSheet>("motw_coeurR.sf", new JsonContentLoader());
            _CoeurRouge = new AnimatedSprite(spriteSheet3);
            _CoeurPosition = new Vector2(580, 10);
            _CoeurPosition1 = new Vector2(560, 10);
            _CoeurPosition2 = new Vector2(540, 10);
            _eleveVitesse = 100;
            _sonJeu = Content.Load<Song>("sonJeu");
            MediaPlayer.Play(_sonJeu);

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

            //ia prof
            if ((keyboardState.IsKeyDown(Keys.Left)) || (keyboardState.IsKeyDown(Keys.Right)) || (keyboardState.IsKeyDown(Keys.Up)) || keyboardState.IsKeyDown(Keys.Down))
            {
                if (animation2 == "tetehaut")
                {
                    _CoeurPosition = new Vector2(999, 999);
                }
            }
        }

        private void Exit()
        {
            throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime)
        {
            Game.SpriteBatch.Begin();
            /*_spriteBatch.Draw(_eleve, _elevePosition);
            _spriteBatch.Draw(_prof, _profPosition);

            _spriteBatch.Draw(_CoeurRouge, _CoeurPosition);
            _spriteBatch.Draw(_CoeurRouge, _CoeurPosition1);
            _spriteBatch.Draw(_CoeurRouge, _CoeurPosition2);*/

            _tiledMapRendu.Draw();

            //personnages
            Game.SpriteBatch.Draw(_eleve, _elevePosition);
            Game.SpriteBatch.Draw(_prof, _profPosition);

            Game.SpriteBatch.Draw(_CoeurRouge, _CoeurPosition);
            Game.SpriteBatch.Draw(_CoeurRouge, _CoeurPosition1);
            Game.SpriteBatch.Draw(_CoeurRouge, _CoeurPosition2);
            Game.SpriteBatch.End();
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
