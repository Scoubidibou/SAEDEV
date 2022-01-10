using System;
using System.Collections.Generic;
using System.Text;
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
    class deplacements
    {
        //méthode pour le déplacement
        /*public static Vector2 deplacements(AnimatedSprite _perso, Vector2 _persoPosition, TiledMap _map)
        {

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                ushort tx = (ushort)(_persoPosition.X / _map.TileWidth - 1); //la tuile à gauche en x
                ushort ty = (ushort)(_persoPosition.Y / _map.TileHeight);
                animation = "walkWest";
                if (!IsCollision(tx, ty))
                    _persoPosition.X -= walkSpeed; // _persoPosition vecteur position du sprite

                if (_persoPosition.X <= 0 + _perso.TextureRegion.Width / 2)
                    _persoPosition.X = 0 + _perso.TextureRegion.Width / 2;
            }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                ushort tx = (ushort)(_persoPosition.X / _map.TileWidth + 1); //la tuile à droite en x
                ushort ty = (ushort)(_persoPosition.Y / _map.TileHeight);
                animation = "walkEast";
                if (!IsCollision(tx, ty))
                    _persoPosition.X += walkSpeed; // _persoPosition vecteur position du sprite

                if (_persoPosition.X >= Game1.FENETRE_LARGEUR - _perso.TextureRegion.Width / 2)
                    _persoPosition.X = Game1.FENETRE_LARGEUR - _perso.TextureRegion.Width / 2;
            }

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                ushort tx = (ushort)(_persoPosition.X / _map.TileWidth);
                ushort ty = (ushort)(_persoPosition.Y / _map.TileHeight - 1); //la tuile au dessus en y
                animation = "walkNorth";
                if (!IsCollision(tx, ty))
                    _persoPosition.Y -= walkSpeed; // _persoPosition vecteur position du sprite

                if (_persoPosition.Y <= 0 + _perso.TextureRegion.Height / 2)
                    _persoPosition.Y = 0 + _perso.TextureRegion.Height / 2;
            }

            if (keyboardState.IsKeyDown(Keys.Down))
            {
                ushort tx = (ushort)(_persoPosition.X / _map.TileWidth);
                ushort ty = (ushort)(_persoPosition.Y / _map.TileHeight + 2); //la tuile en dessous en y
                animation = "walkSouth";
                if (!IsCollision(tx, ty))
                    _persoPosition.Y += walkSpeed; // _persoPosition vecteur position du sprite

                if (_persoPosition.Y >= Game1.FENETRE_HAUTEUR - _perso.TextureRegion.Height / 2)
                    _persoPosition.Y = Game1.FENETRE_HAUTEUR - _perso.TextureRegion.Height / 2;
            }
        }*/
    }
}
