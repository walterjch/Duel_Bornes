using Game1;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace duel
{
    class Explosion
    {
        Animation _animationExplosion;
        Vector2 _position;
        public bool _actif;
        int _tempsAffichage;

        public int Width
        {
            get { return _animationExplosion.FrameWidth; }
        }
        public int Height
        {
            get { return _animationExplosion.FrameHeight; }
        }

        public void Initialize(Animation animation, Vector2 position)
        {
            _animationExplosion = animation;
            _position = position;
            _actif = true;
            _tempsAffichage = 10;
        }
        public void Update(GameTime gameTime)
        {
            _animationExplosion.Update(gameTime);
            _tempsAffichage -= 1;
            if (_tempsAffichage <= 0)
            {
                this._actif = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _animationExplosion.Draw(spriteBatch);
        }
    }
}
