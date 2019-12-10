using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace duel
{
    class Arrow
    {

        Texture2D _texture;
        Vector2 _position;
        float _velocite;
        float elapsedTime = 0;
        float top;
        float bot;
        float Decalage;
        int nb = 0;

        public float Velocite { get => _velocite; set => _velocite = value; }
        public Vector2 Position { get => _position; set => _position = value; }
        public Texture2D Texture { get => _texture; set => _texture = value; }

        public void Initialize(Texture2D texture, Vector2 position, float decalage)
        {
            Texture = texture;
            Position = position;
            Velocite = -1.25f;
            top = position.Y - 1 * Texture.Height;
            bot = position.Y + 10;
            Decalage = decalage;
        }
        public void Update(GameTime time)
        {

            if (nb >= Decalage)
            {

                elapsedTime = time.ElapsedGameTime.Milliseconds / 16f;

                Position = new Vector2(Position.X, Position.Y + Velocite * elapsedTime);

                if (Position.Y <= top)
                {
                    Velocite = Velocite * -1;
                }
                else if (Position.Y >= bot)
                {
                    Velocite = Velocite * -1;
                }
            }
            else
            {
                nb++;
            }



        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }


    }
}
