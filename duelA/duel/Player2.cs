using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace duel
{
    class Player2 : SpriteGeneric
    {
        public Vector2 vitesse = new Vector2(15, 15);

        public float _vie = 5;
        public string _nom = "JOUEUR 2";

        KeyboardState etatClavierActuel;
        KeyboardState etatClavierPrecedent;

        public Player2(Game1 game) : base(game)
        {

        }

        public void Update(GameTime gameTime)
        {
            //Sauve l'état précédent du clavier pour déterminer les pressions des touches
            etatClavierPrecedent = etatClavierActuel;

            //Lis l'état actuel du clavier et le stock
            etatClavierActuel = Keyboard.GetState();

            if (etatClavierActuel.IsKeyDown(Keys.Up))
            {
                _position.Y -= vitesse.Y;
            }
            if (etatClavierActuel.IsKeyDown(Keys.Down))
            {
                _position.Y += vitesse.Y;
            }
            //Contrôle si le joueur 1 n'est pas hors-champ
            _position.Y = MathHelper.Clamp(_position.Y, _texture.Height/2, _game.GraphicsDevice.Viewport.Height - _texture.Height*3/2 );
        }
    }
}
