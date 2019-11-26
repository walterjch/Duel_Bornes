using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace duel
{
    class SpriteGeneric
    {
        protected Game1 _game;

        //Position du sprite
        public Vector2 _position;

        public Texture2D _texture;

        public Rectangle _hitbox
        {
            get
            {
                return new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
            }
        }

        public SpriteGeneric(Game1 game)
        {
            _game = game;
        }

        public void Initialize(Texture2D texture, Vector2 position)
        {
            _position = position;
            _texture = texture;
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White);
        }
    }
}