using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShipProject
{
    class Asteroid
    {
        Random random;
        Texture2D texture;
        SpriteBatch spriteBatch;
        GraphicsDevice graphicsDevice;
        Spaceship spaceShip;
        public Vector2 position;
        Rectangle asteroidRect;
        float width;
        float height;
        public Vector2 pivot;
        Vector2 direction;
        float acceleration = 1;
        int maxAcceleration;
        public float rotation;
        private Texture2D pixel;
        SpriteEffects effect = SpriteEffects.None;
        bool track = false;
        public int radius;
        Color color = Color.White;
        Game1 game;

        //public Timer timer = new Timer(5000);


        public Asteroid(Spaceship spaceship, SpriteBatch spritebatch, GraphicsDevice Graphicsdevice, Texture2D Texture, int x, int y, int speed, int Width, int Height)
        {
            width = Width;
            height = Height;
            if (speed == 0)
            {
                speed = 1;
            }
            maxAcceleration = speed;
            spaceShip = spaceship;
            spriteBatch = spritebatch;
            graphicsDevice = Graphicsdevice;
            texture = Texture;
            random = new Random();
            position = new Vector2(x, y);
            //position = new Vector2(random.Next(50,graphicsDevice.Viewport.Width), random.Next(50,graphicsDevice.Viewport.Height));
            //rotation point
            pivot = new Vector2(texture.Width / 2, texture.Height / 2);
            radius = (int)width;
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
            rotation = random.Next(x, x+100);
            //game = new Game1();
        }

        public Vector2 Postion { get { return position; } set { position = value; } }
        public void DrawLine(int x0, int y0, int x1, int y1, int thickness, Color color)
        {
            // Calculate the distance between the points
            float distance = Vector2.Distance(new Vector2(x0, y0), new Vector2(x1, y1));

            // Get the angle of the line
            float angleOfLine = (float)Math.Atan2(y1 - y0, x1 - x0);
            rotation = angleOfLine;
            // Create an axis aligned rectangle of the correct size
            Rectangle rect = new Rectangle(
                x0,
                y0,
                (int)Math.Ceiling(distance),
                thickness);

            // Draw
            spriteBatch.Draw(
                pixel,
                rect,
                null,
                color,
                angleOfLine,
                new Vector2(0, 0.5f),
                SpriteEffects.None,
                0.0f);
        }
        public void ScreenWrap()
        {
            if (position.X > graphicsDevice.Viewport.Width)
            {
                position.X = 0;
            }
            if (position.X < 0)
            {
                position.X = graphicsDevice.Viewport.Width;
            }
            if (position.Y < 0)
            {
                position.Y = graphicsDevice.Viewport.Height;
            }
            if (position.Y > graphicsDevice.Viewport.Height)
            {
                position.Y = 0;
            }
        }

        public void Move()
        {
            direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            position += direction * acceleration;
        }
        public void Draw()
        {
            if (track == true //and points are over certain amount
                ) { Track(); }// for debugging
            asteroidRect = new Rectangle((int)position.X, (int)position.Y, (int)width, (int)height);
            spriteBatch.Draw(texture, asteroidRect, null, color, rotation, pivot, effect, 0f);
        }
        public void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.RightShift))
            {
                track = true;
            }
            else
            {
                track = false;
            }
            ScreenWrap();
            IsColliding();
            Move();
        }
        public void Track()
        {
            DrawLine(spaceShip.getRectangle.X, spaceShip.getRectangle.Y, asteroidRect.X, asteroidRect.Y, 1, Color.Red);
            if(spaceShip.score > 1000)
            {
                spaceShip.score = spaceShip.score - 10;
            }
            else
            {
                spaceShip.score = spaceShip.score - 1;
            }
        }

        public bool IsColliding()
        {
            int distanceX = (int)(position.X) - (int)(spaceShip.postion.X);
            int distanceY = (int)(position.Y)- (int)(spaceShip.postion.Y);
            distanceX = distanceX * distanceX;
            distanceY = distanceY * distanceY;
            //absolute value 
            if (distanceX < 0)
            {
                distanceX = distanceX * -1;
            }
            if (distanceY < 0)
            {
                distanceY = distanceY * -1;
            }
            int distance = (int)Math.Sqrt(distanceX + distanceY);
            //check if less than radius
            if (distance < (radius/2) + (spaceShip.radius))
            { 
                color = Color.Red;
                return true;
            }
            else
            {
                color = Color.White;
                return false;
            }
        }

        public bool isColldingBullet(Bullet bullet)
        {
            int distanceX = (int)(position.X) - (int)(bullet.postion.X);
            int distanceY = (int)(position.Y) - (int)(bullet.postion.Y);
            if (distanceX < 0)
            {
                distanceX = distanceX * -1;
            }
            if (distanceY < 0)
            {
                distanceY = distanceY * -1;
            }
            distanceX = distanceX * distanceX;
            distanceY = distanceY * distanceY;
            //absolute value 
            if (distanceX < 0)
            {
                distanceX = distanceX * -1;
            }
            if (distanceY < 0)
            {
                distanceY = distanceY * -1;
            }
            int distance = (int)Math.Sqrt(distanceX + distanceY);
            //check if less than radius
            if (distance < (radius/2) + (bullet.radius))
            {
                color = Color.Blue;
                return true;
            }
            else
            {
                color = Color.White;
                return false;
            }
        }
        
    }
}
