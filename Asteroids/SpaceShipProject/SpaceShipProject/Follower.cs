using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShipProject
{
    class Follower
    {
        Random random;
        Texture2D texture;
        SpriteBatch spriteBatch;
        GraphicsDevice graphicsDevice;
        Spaceship spaceShip;

        public Vector2 position;
        Rectangle enemyRect;
        Rectangle followerRect;

        float width;
        float height;

        public Vector2 pivot;
        Vector2 direction;
        float acceleration = 0;
        float accelerationRate = 0.1f;
        float rotationRate = 0.1f;
        int maxAcceleration;
        public float rotation;
        Rectangle rectangle;
        private Texture2D pixel;
        SpriteEffects effect = SpriteEffects.None;
        bool track = false;

        public Follower(Spaceship spaceship, SpriteBatch spritebatch, GraphicsDevice Graphicsdevice, Texture2D Texture, int x, int y, int speed, int Width, int Height)
        {
            width = Width;
            height = Height;
            if(speed == 0)
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
            //randomize spawn position
            //position = new Vector2(random.Next(50,graphicsDevice.Viewport.Width), random.Next(30,graphicsDevice.Viewport.Height));
            //rotation point
            pivot = new Vector2(texture.Width / 2, texture.Height / 2);
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
        }

        public void Follow()
        {
            if (position.X != spaceShip.getRectangle.X && position.Y != spaceShip.getRectangle.Y)
            {
                rotation = (float)Math.Atan2(position.Y - spaceShip.getRectangle.Y, position.X - spaceShip.getRectangle.X);
                effect = SpriteEffects.None;
                direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
                direction.Normalize();
                position -= direction * acceleration;
                acceleration += accelerationRate;
                if (acceleration > maxAcceleration)
                {
                    acceleration = maxAcceleration;
                }
            }
        }

        public void Run()
        {
            rotation = (float)Math.Atan2(position.Y - spaceShip.getRectangle.Y, position.X - spaceShip.getRectangle.X);
            effect = SpriteEffects.FlipHorizontally;
            direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            direction.Normalize();
            position += direction * acceleration;
            acceleration += accelerationRate;
            if (acceleration > maxAcceleration)
            {
                acceleration = maxAcceleration;
            }
        }

        public void Update()
        {
            if(Keyboard.GetState().IsKeyDown(Keys.RightShift))
            {
                track = true;
            }
            else
            {
                track = false;
            }
            if(Keyboard.GetState().IsKeyDown(Keys.RightControl))
            {
                Run();
            }
            else { Follow(); }
            //ScreenWrap();
        }

        public void Draw()
        {
            if (track == true) { Track(); }// for debugging
            followerRect = new Rectangle((int)position.X, (int)position.Y, (int)width, (int)height);
            spriteBatch.Draw(texture, followerRect, null, Color.White, rotation, pivot, effect, 0f);
        }

        /*public bool isTriggered()
        {
            enemyRect = spaceShip.getRectangle;
        }*/

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

        public void Track()
        {
            DrawLine(spaceShip.getRectangle.X, spaceShip.getRectangle.Y, followerRect.X, followerRect.Y, 1, Color.Red);
        }
    }
}
