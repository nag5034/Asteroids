using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RandomExtensions;
using System.Timers;

namespace SpaceShipProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D enterpriseTexture;
        Texture2D klingonTexture;
        Spaceship enterprise;
        Texture2D asteroid1;
        Texture2D asteroid2;
        Texture2D asteroid3;
        Texture2D randText;
        Texture2D bulletText;
        SpriteFont font;
        //Follower klingon;
        const int numofAsteroids = 10;
        const int numofStars = 200;
        List<Asteroid> asteroids;
        List<Asteroid> asteroids2;
        List<Bullet> bullets;
        Random rand;
        Stars[] stars = new Stars[numofStars];
        KeyboardState OldKeyState;
        bool previoushit = false;
        bool newhit;
        Timer timer;
        int num;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            asteroids = new List<Asteroid>();
            asteroids2 = new List<Asteroid>();
            bullets = new List<Bullet>();
            enterpriseTexture = Content.Load<Texture2D>("spaceshipTexture");
            asteroid1 = Content.Load<Texture2D>("Asteroid1");
            asteroid2 = Content.Load<Texture2D>("Asteroid2");
            asteroid3 = Content.Load<Texture2D>("asteroid3");
            bulletText = Content.Load<Texture2D>("bullet");
            klingonTexture = Content.Load<Texture2D>("Klingon");
            font = Content.Load<SpriteFont>("font");
            enterprise = new Spaceship(spriteBatch, GraphicsDevice, enterpriseTexture);
            rand = new Random();
            timer = new Timer();
            //timer.Start();
            timer.Interval = 1;
            num = enterprise.lives;

            for (int i = 0; i < numofAsteroids; i++)
            {
                int randnumtext = rand.Next(0, 3);   
                if(randnumtext == 0)
                {
                    randText = asteroid1;
                }
                if (randnumtext == 1)
                {
                    randText = asteroid2;
                }
                if (randnumtext == 2)
                {
                    randText = asteroid3;
                }
                else
                {
                    randText = asteroid1;
                }
                int randsize = (int)rand.Gaussian(75, 10);
                int randx = rand.Next(randsize, GraphicsDevice.Viewport.Width);
                int randy = rand.Next(randsize, GraphicsDevice.Viewport.Height);
                int rands = (int)rand.Gaussian(3,1);
                asteroids.Add(new Asteroid(enterprise, spriteBatch, GraphicsDevice, randText,randx, randy,rands,randsize,randsize));
                //followers[i].position = new Vector2(randx, randy);
            }
            for (int i = 0; i < numofStars; i++)
            {
                int randx = (int)rand.Gaussian(GraphicsDevice.Viewport.Width/2, GraphicsDevice.Viewport.Width/2);
                int randy = (int)rand.Gaussian(GraphicsDevice.Viewport.Height/2, GraphicsDevice.Viewport.Height/2);
                stars[i] = new Stars(spriteBatch, GraphicsDevice,randx,randy);
            }
            //klingon = new Follower(enterprise, spriteBatch, GraphicsDevice, klingonTexture);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            KeyboardState NewKeyState = Keyboard.GetState();
            if (NewKeyState.IsKeyDown(Keys.Space) && OldKeyState.IsKeyUp(Keys.Space))
            {
                bullets.Add(new Bullet(spriteBatch, GraphicsDevice, bulletText, enterprise));
                //fix for broken loops 
                //if(bullets.Count() > 1)
                //{
                //    bullets.RemoveAt(0);
                //}
            }
            OldKeyState = NewKeyState;

            // TODO: Add your update logic here
            enterprise.Update();

            foreach (Bullet b in bullets)
            {
                b.Update();
            }

            //first asteroids 
            for (int i = 0; i < asteroids.Count(); i++)
            {
                asteroids[i].Update();
                if(asteroids[i].IsColliding())
                {
                    if (num == enterprise.lives)
                    {
                        timer.Start();
                        //enterprise.lives = enterprise.lives - 1;
                    }
                }
                timer.Stop();
                num = enterprise.lives;
                for (int b = 0; b < bullets.Count(); b++)
                {
                    asteroids[i].isColldingBullet(bullets[b]);
                    if(asteroids[i].isColldingBullet(bullets[b]) == true)
                    {
                        int randnumtext = rand.Next(0, 3);
                        if (randnumtext == 0)
                        {
                            randText = asteroid1;
                        }
                        if (randnumtext == 1)
                        {
                            randText = asteroid2;
                        }
                        if (randnumtext == 2)
                        {
                            randText = asteroid3;
                        }
                        else
                        {
                            randText = asteroid1;
                        }
                        int randsize = (int)rand.Gaussian(45, 10);
                        int x = (int)asteroids[i].position.X;
                        int y = (int)asteroids[i].position.Y;
                        int dir = (int)asteroids[i].rotation;
                        asteroids.RemoveAt(i);
                        bullets.RemoveAt(b);
                        int rands = (int)rand.Gaussian(3, 1);
                        asteroids2.Add(new Asteroid(enterprise, spriteBatch, GraphicsDevice, randText, x, y, rands, randsize, randsize));
                        asteroids2[asteroids2.Count() - 1].rotation = dir + 20;
                        asteroids2.Add(new Asteroid(enterprise, spriteBatch, GraphicsDevice, randText, x, y, rands, randsize, randsize));
                        asteroids2[asteroids2.Count() - 1].rotation = dir - 20;
                        enterprise.score = enterprise.score + 20;
                    }
                }
            }

            //second asteroids
            for (int i = 0; i < asteroids2.Count(); i++)
            {
                asteroids2[i].Update();
                if (asteroids2[i].IsColliding())
                {
                    if (num == enterprise.lives)
                    {
                        enterprise.lives = enterprise.lives - 1;
                    }
                }
                num = enterprise.lives;
                for (int b = 0; b < bullets.Count(); b++)
                {
                    asteroids2[i].isColldingBullet(bullets[b]);
                    if (asteroids2[i].isColldingBullet(bullets[b]) == true)
                    {
                        asteroids2.RemoveAt(i);
                        bullets.RemoveAt(b);
                        enterprise.score = enterprise.score + 50;
                    }
                }
            }

            timer.AutoReset = true;
            timer.Elapsed += OnTimedEvent;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if(asteroids.Count() == 0 && asteroids2.Count() ==0)
            {
                spriteBatch.DrawString(font, "YOU WIN!, GAME OVER", new Vector2(GraphicsDevice.Viewport.Width / 3, GraphicsDevice.Viewport.Height / 3), Color.Red, 0, new Vector2(0, 0), 2, SpriteEffects.None, 0);
            }
            if(enterprise.lives < 0)
            {
                //spriteBatch.DrawString(font, "YOU LOSE, GAME OVER", new Vector2(GraphicsDevice.Viewport.Width / 3, GraphicsDevice.Viewport.Height / 3), Color.Red, 0, new Vector2(0, 0), 2, SpriteEffects.None, 0);
            }
            spriteBatch.DrawString(font, ("Lives: " + enterprise.lives), new Vector2(0, 0), Color.Red);
            spriteBatch.DrawString(font, ("Score: " + enterprise.score), new Vector2(0, 20), Color.Red);
            foreach (Stars i in stars)
            {
                i.DrawPoint(Color.White);
            }
            foreach (Asteroid i in asteroids)
            {
                i.Draw();
            }
            foreach (Asteroid i in asteroids2)
            {
                i.Draw();
            }
            foreach (Bullet i in bullets)
            {
                i.Draw();
            }
            enterprise.Draw();

            spriteBatch.End();
            base.Draw(gameTime);
        }
        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            //enterprise.lives -= 1;
        }
    }
}
