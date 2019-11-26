using Game1;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace duel
{
    class Laser : SpriteGeneric
    {
        //Défini une vitesse pour le laser
        public Vector2 vitesse = new Vector2(20, 0);
        

        public KeyboardState clavierActuel;
        public KeyboardState clavierPrecedent;

        public Laser(Game1 game) : base(game)
        {

        }

        public void Update(GameTime gameTime)
        {
            //Sauver l'état précédent du clavier pour déterminer les pressions des touches
            clavierPrecedent = clavierActuel;

            //Lire l'état actuel du clavier et le stocker
            clavierActuel = Keyboard.GetState();

            _position.X += vitesse.X;
        }
    }
}
