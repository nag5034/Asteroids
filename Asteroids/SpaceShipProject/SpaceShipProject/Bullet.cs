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
    class Bullet
    {
        //variables
        Spaceship spaceShip;
        Texture2D texture;
        private Vector2 position = new Vector2(250, 250);
        public Vector2 pivot;
        Vector2 direction;
        float acceleration = 10;
        //float accelerationRate = 0.1f;
        //float rotationRate = 0.1f;
        int maxAcceleration = 30;
        public float rotation;
        Rectangle rectangle;
        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;
        public float width = 10;
        public float height = 10;
        public int radius;
        Color color = Color.White;
        //float[] trailPositions = new float[64];
        //constructor
        public Bullet(SpriteBatch spritebatch, GraphicsDevice Graphicsdevice, Texture2D Texture, Spaceship spaceship)
        {
            spaceShip = spaceship;
            texture = Texture;
            spriteBatch = spritebatch;
            graphicsDevice = Graphicsdevice;
            rotation = spaceShip.rotation;
            direction = spaceShip.direction;
            pivot = new Vector2(texture.Width / 2, texture.Height / 2);
            position = spaceShip.postion;
            radius = 5;
        }

        public Vector2 postion { get { return position; } set { position = value; } }
        public Rectangle getRectangle { get { return rectangle; } }
        //Move the Ship, Place in Update
        public void Update()
        {
            //ScreenWrap();
            direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            position += direction * acceleration;
            //acceleration += accelerationRate;
            if (acceleration > maxAcceleration)
            {
                acceleration = maxAcceleration;
            }
            //slow ship to stop
            /*if (isMoving() == false && acceleration > 0)
            {
                acceleration -= accelerationRate;
                position += direction * acceleration;
            }*/
        }

        public void Draw()
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)width, (int)height);
            spriteBatch.Draw(texture, rectangle, null, Color.White, rotation, pivot, SpriteEffects.None, 0f);
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

        public bool isColldingBullet(Asteroid asteroid)
        {
            int distanceX = (int)(position.X) - (int)(asteroid.Postion.X);
            int distanceY = (int)(position.Y) - (int)(asteroid.Postion.Y);
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
            if (distance < (radius / 2) + (asteroid.radius))
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
