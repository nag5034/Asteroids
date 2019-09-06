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
    class Spaceship
    {
        //variables
        Texture2D texture;
        public Vector2 position = new Vector2(250,250);
        public Vector2 pivot;
        public Vector2 direction;
        float acceleration = 0;
        float accelerationRate = 0.1f;
        float rotationRate = 0.1f;
        int maxAcceleration = 10;
        public float rotation;
        Rectangle rectangle;
        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;
        public float width = 50;
        public float height = 50;
        public int radius;
        Color color = Color.White;
        public int lives = 3;
        public float score = 0;
        //float[] trailPositions = new float[64];
        //constructor
        public Spaceship(SpriteBatch spritebatch, GraphicsDevice Graphicsdevice, Texture2D Texture)
        {
            texture = Texture;
            spriteBatch = spritebatch;
            graphicsDevice = Graphicsdevice;
            pivot = new Vector2(texture.Width / 2, texture.Height / 2);
            radius = 25;
        }

        public Vector2 postion { get { return position; } set { position = value; } }
        public Rectangle getRectangle { get { return rectangle; } }
        //Move the Ship, Place in Update
        public void Update()
        {
            ScreenWrap();

            //Turn 
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                rotation -= rotationRate;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                rotation += rotationRate;
            }
            //Move Foward or Backwords
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
                position += direction * acceleration;
                acceleration += accelerationRate;
                if(acceleration > maxAcceleration)
                {
                    acceleration = maxAcceleration;
                }
            }
            //slow ship to stop
            if(isMoving() == false && acceleration > 0)
            {
                acceleration -= accelerationRate;
                position += direction * acceleration;
            }
        }

        public void Draw()
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)width, (int)height);
            spriteBatch.Draw(texture, rectangle, null, Color.White, rotation, pivot, SpriteEffects.None, 0f);
        }

        public bool isMoving()
        {
            if(Keyboard.GetState().IsKeyDown(Keys.Up))// || Keyboard.GetState().IsKeyDown(Keys.Down))//keyboardState.GetPressedKeys().Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isTurning()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ScreenWrap()
        {
            if(position.X > graphicsDevice.Viewport.Width)
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
        /*public void DrawTrail()
        {
            float previousPositionX = position.X;
            float previousPositionY = position.Y;
            float previousX = previousPositionX;
            float previousY = previousPositionY;
            
        }*/
    }
}
