using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RandomExtensions;

namespace SpaceShipProject
{
    class Stars
    {
        SpriteBatch spriteBatch;
        GraphicsDevice graphicsDevice;
        Texture2D pixel;
        int X;
        int Y;

        public Stars(SpriteBatch spritebatch, GraphicsDevice Graphicsdevice, int x, int y)
        {
            spriteBatch = spritebatch;
            graphicsDevice = Graphicsdevice;
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
            X = x;
            Y = y;
        }

        public void DrawPoint(Color color)
        {
            Rectangle rectToDraw = new Rectangle(X, Y, 1, 1);
            spriteBatch.Draw(pixel, rectToDraw, color);
        }
    }
}
