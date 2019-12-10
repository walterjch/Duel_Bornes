using Game1;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace duel
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 

    public enum ScreenState {TitleScreen, Playing, EndScreen};

    public class Game1 : Game
    {
        //Sers pour la résolution du background
        GraphicsDeviceManager _graphics;

        //Déclare le spritebatch
        SpriteBatch _spriteBatch;

        //Déclare les états du clavier pour les pressions de touches
        KeyboardState _clavierActuel;
        KeyboardState _clavierPrecedent;

        //Déclare les vies
        VieP1 _vieP1;
        VieP2 _vieP2;

        List<Arrow> arrows;
        Arrow arrow;

        //Déclare le message/menu de fin 
        SpriteFont _messageFin;

        //Variables utiles pour le compteur
        SpriteFont _compteur;
        float timer;
        private int compteurTemps = 3;
        private int compteur = 3;
        private bool compteurActif = true;
        private string message = "";


        //Background _background;
        Texture2D _background;

        //Déclare les joueurs
        Player _joueur1;
        Player2 _joueur2;
        
        //Déclare les lasers
        Laser _laserP1;
        Laser2 _laserP2;

        //Déclare les explosions
        Texture2D _explosionP1;
        List<Explosion> _listeExplosionsP1;

        Texture2D _explosionP2;
        List<Explosion> _listeExplosionsP2;

        //Déclare les listes de lasers
        List<Laser> _faisceauLaserP1;
        List<Laser2> _faisceauLaserP2;

        //Sers pour la cadence de tir
        TimeSpan _laserSpawnTime;
        TimeSpan _previousLaserSpawnTime1;
        TimeSpan _previousLaserSpawnTime2;

        //Déclare les sons d'explosions
        SoundEffect _sonExplosion;
        SoundEffectInstance _sonExplosionInstance;

        //Déclare les sons des lasers
        SoundEffect _sonLaser;
        SoundEffectInstance _sonLaserInstance;

        //Déclare la musique du jeu
        Song _musiqueJeu;

        //Ett bouton recommencer
        bool btnOn = false;
        private ScreenState currentScreenState = ScreenState.TitleScreen;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Permet au jeu et aux composants de s'initialiser, essentiel avant que le jeu démarre.
        /// </summary>
        protected override void Initialize()
        {
            //Méthode pour changer la résolution
            this.Window.Position = new Point(0, 0);
            this.Window.IsBorderless = true;
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            _graphics.ApplyChanges();

            arrows = new List<Arrow>();

            //Initialise la vie des 2 joueurs
            _vieP1 = new VieP1();
            _vieP2 = new VieP2();

            //Initialise les 2 joueurs
            _joueur1 = new Player(this);
            _joueur2 = new Player2(this);

            //Initialise les 2 lasers
            _laserP1 = new Laser(this);
            _laserP2 = new Laser2(this);

            //Initialise les explosions
            _listeExplosionsP1 = new List<Explosion>();
            _listeExplosionsP2 = new List<Explosion>();

            //Initialise laser
            _faisceauLaserP1 = new List<Laser>();
            _faisceauLaserP2 = new List<Laser2>();

            //Valeurs de la cadence de tir
            const float SECONDES_EN_MINUTE = 60f;
            const float CADENCE_DE_TIR = 200f;

            //Assigne la cadence de tir
            _laserSpawnTime = TimeSpan.FromSeconds(SECONDES_EN_MINUTE / CADENCE_DE_TIR);
            _previousLaserSpawnTime1 = TimeSpan.Zero;
            _previousLaserSpawnTime2 = TimeSpan.Zero;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent est appelé une fois par partie et c'est l'endroit où on charge le contenu
        /// Tout le contenu
        /// </summary>
        protected override void LoadContent()
        {
            //Créé un nouveau spriteBatch, qui peut être utilisé pour dessiner des textures
            _spriteBatch = new SpriteBatch(GraphicsDevice);
             
            //Charge vie p1 et vie p2
            _vieP1.font = Content.Load<SpriteFont>("vieP1");
            _vieP2.font = Content.Load<SpriteFont>("vieP2");

            //Charge le message à la fin de la partie
            _messageFin = Content.Load<SpriteFont>("finPartie");

            //Charge le compteur du début
            _compteur = Content.Load<SpriteFont>("compteur");

            //Charge le background
            _background = Content.Load<Texture2D>("backgroundDuel");

            //Le joueur 1 est chargé à une position donnée
            Texture2D joueur1Texture = Content.Load<Texture2D>("tankP1");
            Vector2 joueur1Position = new Vector2(100, 100);
            _joueur1.Initialize(joueur1Texture, joueur1Position);

            //Le joueur 2 est chargé à une position donnée
            Texture2D joueur2Texture = Content.Load<Texture2D>("tankP2");
            Vector2 joueur2Position = new Vector2(GraphicsDevice.Viewport.Width - 200, 100);
            _joueur2.Initialize(joueur2Texture, joueur2Position);

            //Donne la texture à l'explosion
            _explosionP1 = Content.Load<Texture2D>("explosion");
            _explosionP2 = Content.Load<Texture2D>("explosion");

            //Donne le son de l'explosion
            _sonExplosion = Content.Load<SoundEffect>("sonExplosion");
            _sonExplosionInstance = _sonExplosion.CreateInstance();
            _sonExplosionInstance.Volume = 0.3f;

            //Donne le son de la musique
            _musiqueJeu = Content.Load<Song>("musiqueJeu");

            //Commencer la lecture de la musique
            MediaPlayer.Volume = 1.0f;
            MediaPlayer.Play(_musiqueJeu);

            arrow = new Arrow();
            arrow.Initialize(Content.Load<Texture2D>("exit"), new Vector2(GraphicsDevice.Viewport.Width - GraphicsDevice.Viewport.Width / 5 - 100, GraphicsDevice.Viewport.Height - 100), 45);
            arrows.Add(arrow);
            arrow = new Arrow();
            arrow.Initialize(Content.Load<Texture2D>("start"), new Vector2(GraphicsDevice.Viewport.Width / 2 - 50, GraphicsDevice.Viewport.Height - 100), 76);
            arrows.Add(arrow);
            arrow = new Arrow();
            //arrow.Initialize(Content.Load<Texture2D>("restart"), new Vector2(GraphicsDevice.Viewport.Width / 5, GraphicsDevice.Viewport.Height - 100), 10);
            //arrows.Add(arrow);
        }

        /// <summary>
        /// Permet d'afficher l'esxplosion du joueur 1 sur le joueur 2
        /// </summary>
        /// <param name="positionP2"></param>
        protected void AddExplosionP1(Vector2 positionP2)
        {
            Animation animationExplosionP1 = new Animation();
            animationExplosionP1.Initialize(_explosionP1, positionP2,
                134, 134, 12, 30, Color.White, 1.0f, true);
            Explosion explosionP1 = new Explosion();
            explosionP1.Initialize(animationExplosionP1, positionP2);
            _listeExplosionsP1.Add(explosionP1);
        }

        /// <summary>
        /// Permet d'afficher l'esxplosion du joueur 2 sur le joueur 1
        /// </summary>
        /// <param name="positionP1"></param>
        protected void AddExplosionP2(Vector2 positionP1)
        {
            Animation animationExplosionP2 = new Animation();
            animationExplosionP2.Initialize(_explosionP2, positionP1,
                134, 134, 12, 30, Color.White, 1.0f, true);
            Explosion explosionP2 = new Explosion();
            explosionP2.Initialize(animationExplosionP2, positionP1);
            _listeExplosionsP2.Add(explosionP2);
        }

        /// <summary>
        /// Ajoute un laser dans la liste des laser du joueur 1
        /// </summary>
        protected void AddLaserP1()
        {
            int decalageX = 90;
            int decalageY = 40;
            Laser laser1 = new Laser(this);
            Texture2D texturelaser1 = Content.Load<Texture2D>("laserRouge");
            Vector2 positionLaser1 = _joueur1._position;
            positionLaser1.X += decalageX;
            positionLaser1.Y += decalageY;

            //Charge le son du laser et son volume
            _sonLaser = Content.Load<SoundEffect>("sonLaser");
            _sonLaserInstance = _sonLaser.CreateInstance();
            _sonLaserInstance.Volume = 0.1f;

            //Défini sa texture et sa position
            laser1.Initialize(texturelaser1, positionLaser1);

            _faisceauLaserP1.Add(laser1);
        }

        /// <summary>
        /// Ajoute un laser dans la liste des laser du joueur 2
        /// </summary>
        protected void AddLaserP2()
        {
            int decalageX = 40;
            int decalageY = 40;
            Laser2 laser2 = new Laser2(this);
            Texture2D texturelaser2 = Content.Load<Texture2D>("laser");
            Vector2 positionLaser2 = _joueur2._position;
            positionLaser2.X -= decalageX;
            positionLaser2.Y += decalageY;

            //Charge le son du laser et son volume
            _sonLaser = Content.Load<SoundEffect>("sonLaser");
            _sonLaserInstance = _sonLaser.CreateInstance();
            _sonLaserInstance.Volume = 0.1f;

            //Défini sa texture et sa position
            laser2.Initialize(texturelaser2, positionLaser2);

            _faisceauLaserP2.Add(laser2);
        }

        /// <summary>
        /// Permet au joueur 1 de tirer
        /// </summary>
        /// <param name="gameTime"></param>
        protected void FireLaserP1(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - _previousLaserSpawnTime1 > _laserSpawnTime)
            {
                _previousLaserSpawnTime1 = gameTime.TotalGameTime;
                AddLaserP1();
                _sonLaserInstance.Play();
            }
        }

        /// <summary>
        /// Permet au joueur 2 de tirer
        /// </summary>
        /// <param name="gameTime"></param>
        protected void FireLaserP2(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - _previousLaserSpawnTime2 > _laserSpawnTime)
            {
                _previousLaserSpawnTime2 = gameTime.TotalGameTime;
                AddLaserP2();
                _sonLaserInstance.Play();
            }
        }

        /// <summary>
        /// Méthode qui affiche le message/menu de fin
        /// </summary>
        /// <param name="nomJoueur"></param>
        protected void AfficherMenu(string nomJoueur)
        {
            int decalageTexte = 600;
            btnOn = true;
            _spriteBatch.DrawString(_messageFin, "BRAVO, " + nomJoueur + " REMPORTE LA VICTOIRE !" , new Vector2((GraphicsDevice.Viewport.Width / 2) - decalageTexte, GraphicsDevice.Viewport.Height / 2), Color.Black);
            //_spriteBatch.DrawString(_messageFin, "RECOMMENCER ($)", new Vector2((GraphicsDevice.Viewport.Width / 2) - decalageTexte, (GraphicsDevice.Viewport.Height / 2) + 50), Color.Black);
            //_spriteBatch.DrawString(_messageFin, "QUITTER (BOUTON DE DROITE)", new Vector2((GraphicsDevice.Viewport.Width / 2) - decalageTexte, (GraphicsDevice.Viewport.Height / 2) + 100), Color.Black);

            foreach (Arrow a in arrows)
            {
                a.Draw(_spriteBatch);
            }
        }

        /// <summary>
        /// Méthode qui gère le compteur du début
        /// </summary>
        /// <param name="gameTime"></param>
        private void CompteARebours(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            compteurTemps -= (int)timer;
            if (timer >= 4f)
            {
                compteur = 0;
                message = string.Empty;
                compteurActif = false;
            }
            else if (timer >= 3f)
            {
                compteur = 1;
                message = "TIREZ";
            }
            else if (timer >= 2f)
            {
                compteur = 2;
                message = "    1  ";
            }
            else if (timer >= 1f)
            {
                compteur = 3;
                message = "    2  ";
            }
            else if (timer >= 0f)
            {
                compteur = 3;
                message = "    3  ";
            }
        }

        /// <summary>
        /// Permet de mettre le jeu à jour en temps réel
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            _clavierPrecedent = _clavierActuel;
            _clavierActuel = Keyboard.GetState();

            //Si btnOn == true
            if (btnOn == true)
            {
                //On rend disponible l'interraction avec le menu
                //Si la touche choisie est pressée
                if (_clavierActuel.IsKeyDown(Keys.D8))
                {
                    //On relance une partie
                    btnOn = false;
                    compteurActif = true;
                    timer = 0f;
                    Initialize();
                    LoadContent();
                }
                foreach (Arrow a in arrows)
                {
                    a.Update(gameTime);
                }
            }
            //Sinon si le compteur à rebours est fini, on met tout à jour
            else if (compteurActif == false)
            {
                //LaserP1
                //Tir un laser si la touche choisie est pressée
                if (_laserP1.clavierActuel.IsKeyDown(Keys.V))
                {
                    FireLaserP1(gameTime);
                }

                for (int i = 0; i < _faisceauLaserP1.Count; i++)
                {
                    //Test si le laser 2 touche le joueur 2
                    if (_faisceauLaserP1[i]._hitbox.Intersects(_joueur2._hitbox))
                    {
                        //On ajoute une explosion sur la position du joueur 2
                        AddExplosionP1(_faisceauLaserP1[i]._position);
                        //On joue un son d'explosion
                        _sonExplosionInstance.Play();
                        //On enlève 1 points de vie au joueur 2
                        _joueur2._vie --;
                        //On supprime le laser en question de la liste
                        _faisceauLaserP1.Remove(_faisceauLaserP1[i]);
                        break;
                    }
                    //Sinon si le laser du joueur 1 sort de l'écran
                    else if (_faisceauLaserP1[i]._position.X > GraphicsDevice.Viewport.Width)
                    {
                        //On le supprime de la liste en question
                        _faisceauLaserP1.Remove(_faisceauLaserP1[i]);
                        break;
                    }
                    //On met à jour la liste de laser du joueur 1
                    _faisceauLaserP1[i].Update(gameTime);
                }

                //LaserP2
                //Tir un laser si la touche choisie est pressée
                if (_laserP2.clavierActuel.IsKeyDown(Keys.NumPad1))
                {
                    FireLaserP2(gameTime);
                }

                for (int j = 0; j < _faisceauLaserP2.Count; j++)
                {
                    //Test si le laser 2 touche le joueur 1 
                    if (_faisceauLaserP2[j]._hitbox.Intersects(_joueur1._hitbox))
                    {
                        //On ajoute une explosion sur la position du joueur 1
                        AddExplosionP2(_faisceauLaserP2[j]._position);
                        //On joue un son d'explosion
                        _sonExplosionInstance.Play();
                        //Enlève 1 points de vie au joueur 1
                        _joueur1._vie --;
                        //On supprime le laser en question de la liste
                        _faisceauLaserP2.Remove(_faisceauLaserP2[j]);
                        break;
                    }
                    //Sinon si le laser du joueur 2 sort de l'écran
                    else if (_faisceauLaserP2[j]._position.X < 0)
                    {
                        //On le supprime de la liste en question
                        _faisceauLaserP2.Remove(_faisceauLaserP2[j]);
                        break;
                    }
                    //On met à jour la liste de laser du joueur 2
                    _faisceauLaserP2[j].Update(gameTime);
                }
                _joueur1.Update(gameTime);
                _joueur2.Update(gameTime);
                _laserP1.Update(gameTime);
                _laserP2.Update(gameTime);
                UpdateExplosions(gameTime);
            }

            //Permet de quitter le jeu en tout temps
            if (_clavierActuel.IsKeyDown(Keys.D0))
            {
                Exit();
            }

            //Permet de quitter le jeu avec la touche du clavier "Escape" au cas ou celui ne fonctionne pas correctement
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            { 
                Exit();
            }

            CompteARebours(gameTime);
            
            base.Update(gameTime);
        }

        /// <summary>
        /// Permet de mettre à jour les explosions pour les faire apparaître ou disparaître
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateExplosions(GameTime gameTime)
        {
            //Liste qui permet d'afficher plusieurs laser du joueur 1 à l'écran
            for (int i = 0; i < _listeExplosionsP1.Count; i++)
            {
                _listeExplosionsP1[i].Update(gameTime);
                //Si le laser du joueur 1 est inactif
                if (!_listeExplosionsP1[i]._actif)
                {
                    //On le supprime de la liste
                    _listeExplosionsP1.Remove(_listeExplosionsP1[i]);
                }
            }
            //Liste qui permet d'afficher plusieurs laser du joueur 2 à l'écran
            for (int i = 0; i < _listeExplosionsP2.Count; i++)
            {
                _listeExplosionsP2[i].Update(gameTime);
                //Si le laser du joueur 2 est inactif
                if (!_listeExplosionsP2[i]._actif)
                {
                    //On le supprime de la liste
                    _listeExplosionsP2.Remove(_listeExplosionsP2[i]);
                }
            }
        }

        /// <summary>
        /// Permet de dessiner tout les éléments du jeu
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            //Commence de dessiner
            _spriteBatch.Begin();

            //Dessine le background
            _spriteBatch.Draw(_background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            switch (currentScreenState)
            {
                case ScreenState.TitleScreen:
                    break;
                case ScreenState.Playing:
                    break;
                case ScreenState.EndScreen:
                    break;
                default:
                    break;
            }



            

            
            //Si le joueur 1 n'a plus de vie
            if (_joueur1._vie <= 0)
            {
                //On affiche le menu adapté à celui-ci
                AfficherMenu(_joueur2._nom);
                
            }
            //Sinon si le joueur 2 n'a plus de vie
            else if (_joueur2._vie <= 0)
            {
                //On affiche le menu adapté à celui-ci
                AfficherMenu(_joueur1._nom);
                
            }
            //Sinon
            else
            {
                //Si le compteur n'est pas fini
                if (compteur > 0)
                {
                    //On affiche le compte à rebours
                    _spriteBatch.DrawString(_compteur, message, new Vector2((GraphicsDevice.Viewport.Width / 2) - 200, GraphicsDevice.Viewport.Height / 2), Color.OrangeRed);
                }

                //Affiche vie p1
                _spriteBatch.DrawString(_vieP1.font, "Vie P1 : " + _joueur1._vie, new Vector2(0, 0), Color.Red);

                //Affiche vie p2
                _spriteBatch.DrawString(_vieP2.font, "Vie P2 : " + _joueur2._vie, new Vector2(GraphicsDevice.Viewport.Width - 200, 5), Color.Blue);

                //Dessine le joueur 1
                _joueur1.Draw(_spriteBatch);

                //Dessine le joueur 2
                _joueur2.Draw(_spriteBatch);

                //Dessine le laser du joueur 1
                foreach (var l1 in _faisceauLaserP1)
                {
                    l1.Draw(_spriteBatch);
                }

                //Dessine le laser du joueur 2
                foreach (var l2 in _faisceauLaserP2)
                {
                    l2.Draw(_spriteBatch);
                }

                //Dessine explosion sur le joueur 2
                foreach (var e in _listeExplosionsP1)
                {
                    e.Draw(_spriteBatch);
                }

                //Dessine explosion sur le joueur 1
                foreach (var e in _listeExplosionsP2)
                {
                    e.Draw(_spriteBatch);
                }
            }

            //Fin du dessin
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}