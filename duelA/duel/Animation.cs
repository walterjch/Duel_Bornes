using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class Animation
    {
        //L'image qui représente la collection d'image utilisées pour l'animation
        Texture2D spriteStrip;

        //L'écart utilisé pour afficher le sprite strip
        float scale;

        //Le temps depuis la dernière fois qu'on met à jour l'image
        int elapsedTime;

        //Le temps pour afficher une image jusqu'à la prochaine
        int frameTime;

        //Le nombre d'images que l'animation contient
        int frameCount;

        //L'index de l'image actuelle qu'on affiche
        int currentFrame;

        //La couleur de l'image qu'on va afficher
        Color color;

        //La zone de la collection d'image qu'on veut afficher
        Rectangle sourceRect = new Rectangle();

        //La zone où on veut afficher la collection d'images dans le jeu
        Rectangle destinationRect = new Rectangle();

        //Largeur d'une image donnée
        public int FrameWidth;

        //Hauteur d'une image donnée
        public int FrameHeight;

        //L'état de l'animation
        public bool Active;

        //Determine si l'animation va continuer ou se désactive après un tour
        public bool Looping;

        public Vector2 Position;

        public void Initialize(Texture2D texture, Vector2 position, int frameWidth, int frameHeight, int frameCount, int frametime, Color color, float scale, bool looping)
        {
            //Garder une copie locale des valeurs transmises
            this.color = color;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = frametime;
            this.scale = scale;

            Looping = looping;
            Position = position;
            spriteStrip = texture;

            //Définir le temps à zéro
            elapsedTime = 0;
            currentFrame = 0;

            //Définir l'animation à active par défaut
            Active = true;
        }

        public void Update(GameTime gameTime)
        {
            //Ne pas mettre à jour le jeu si nous sommes actifs
            if (Active == false) return;

            //Mettre à jour le temps écoulé
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            //Si elapsed est plus grand que framtime
            if (elapsedTime > frameTime)
            {
                //On doit changer d'image
                //Passe à la prochaine image
                currentFrame++;

                //Si currentFrame est égual à frameCount, remettre currentFrame à zéro
                if (currentFrame == frameCount)
                {
                    currentFrame = 0;

                    //Si on ne fait pas de tours, désactiver l'animation
                    if (Looping == false)
                    {
                        Active = false;
                    }

                    //Remettre elapsedTime à zéro
                    elapsedTime = 0;
                }
                //Prendre la bonne image de la collection en multipliant l'index currentFrame par la largeur du frame
                sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);
                destinationRect = new Rectangle((int)Position.X - (int)(FrameWidth * scale) / 2,
                    (int)Position.Y - (int)(FrameHeight * scale) / 2,
                    (int)(FrameWidth * scale),
                    (int)(FrameHeight * scale));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Dessiner l'animation seulement si nous sommes actifs
            if (Active)
            {
                spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color);
            }
        }
    }
}
