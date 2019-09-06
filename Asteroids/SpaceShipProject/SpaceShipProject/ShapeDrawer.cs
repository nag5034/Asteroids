using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingTools
{
	class ShapeDrawer
	{
		// Should know about sprite batch to do its own drawing
		private SpriteBatch spriteBatch;

		// Need a pixel to stretch
		private Texture2D pixel;

		/// <summary>
		/// Initializes the shape drawer with the specified
		/// sprite batch object to use when drawing
		/// </summary>
		/// <param name="sb">The sprite batch to use when drawing</param>
		/// <param name="gd">The graphics device to making textures</param>
		public ShapeDrawer(SpriteBatch sb, GraphicsDevice gd)
		{
			this.spriteBatch = sb;

			// Set up the texture also
			pixel = new Texture2D(gd, 1, 1);
			pixel.SetData<Color>(new Color[] { Color.White });
		}

		/// <summary>
		/// Draws a point (single pixel)
		/// </summary>
		/// <param name="x">The x coordinate</param>
		/// <param name="y">The y coordinate</param>
		/// <param name="color">The color of the point</param>
		public void DrawPoint(int x, int y, Color color)
		{
			spriteBatch.Draw(
				pixel,
				new Vector2(x, y),
				color);
		}

		/// <summary>
		/// Draws an axis-aligned rectangle
		/// </summary>
		/// <param name="x">The x coordinate of the top left corner</param>
		/// <param name="y">The y coordinate of the top left corner</param>
		/// <param name="width">The width of the rectangle</param>
		/// <param name="height">The height of the rectangle</param>
		/// <param name="color">The color of the rectangle</param>
		public void DrawRectangle(int x, int y, int width, int height, Color color)
		{
			spriteBatch.Draw(
				pixel,
				new Rectangle(x,y,width,height),
				color);
		}

		/// <summary>
		/// Draws an arbitary line
		/// </summary>
		/// <param name="x0">X coord of first point</param>
		/// <param name="y0">Y coord of first point</param>
		/// <param name="x1">X coord of second point</param>
		/// <param name="y1">Y coord of second point</param>
		/// <param name="thickness">The thickness of the line</param>
		/// <param name="color">The color of the line</param>
		public void DrawLine(int x0, int y0, int x1, int y1, int thickness, Color color)
		{
			// Calculate the distance between the points
			float dist = Vector2.Distance(new Vector2(x0, y0), new Vector2(x1, y1));

			// Get the angle of the line
			float angleOfLine = (float)Math.Atan2(y1 - y0, x1 - x0);

			// Create an axis aligned rectangle of the correct size
			Rectangle rect = new Rectangle(
				x0,
				y0,
				(int)Math.Ceiling(dist),
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

		/// <summary>
		/// Draws a circle (no fill)
		/// </summary>
		/// <param name="x">The center's x coord</param>
		/// <param name="y">The center's y coord</param>
		/// <param name="radius">Radius of the circle</param>
		/// <param name="segments">How many segments?  (More = smoother)</param>
		/// <param name="color">Circle's color</param>
		public void DrawCircle(int x, int y, int radius, int segments, Color color)
		{
			// Verify valid params
			if (segments <= 0) return;

			// Starting point
			float currentX = x + radius;
			float currentY = y;

			// Angle per segment
			float step = MathHelper.TwoPi / segments;
			float currentAngle = step;

			// Loop through the requested number of segments
			for (int i = 0; i < segments; i++)
			{
				// Calc new point on unit circle
				float newX = (float)Math.Cos(currentAngle);
				float newY = (float)Math.Sin(currentAngle);

				// Move to desired location
				newX = newX * radius + x;
				newY = newY * radius + y;

				// Draw from current to new
				DrawLine((int)currentX, (int)currentY, (int)newX, (int)newY, 1, color);

				// Save values
				currentX = newX;
				currentY = newY;

				// Adjust angle
				currentAngle += step;
			}
		}
	}
}
