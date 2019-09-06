using System;

/// <summary>
/// You can either CHANGE this namespace to match the namespace
/// in your other file, or ADD a using statement to your other files:
/// 
/// using RandomExtensions;
/// </summary>
namespace RandomExtensions
{
	/// <summary>
	/// This class contains EXTENSION METHODS, which are standalone methods that
	/// can also appear to be part of existing objects.  In this case,
	/// these methods are extensions to C#'s Random object, as if the Random
	/// class contained these all along.
	/// </summary>
	public static class RandomExtensionMethods
	{
		/// <summary>
		/// Generates a random number within a gaussian distribution, given
		/// a particular mean and standard deviation.
		/// </summary>
		/// <param name="mean">The value in the middle of the possible range</param>
		/// <param name="standardDeviation">
		/// The difference between one band to the next.  This number * 4 is 
		/// the farthest possible value from the mean that this method could generate.
		/// </param>
		/// <returns></returns>
		public static double Gaussian(this Random rng, double mean, double standardDeviation)
		{
			// Generate two values
			double val1 = rng.NextDouble();
			double val2 = rng.NextDouble();

			// Use a Box-Muller Transformation to ensure we
			// have the distribution we want.  More info here:
			// http://mathworld.wolfram.com/Box-MullerTransformation.html
			double gaussValue =
				Math.Sqrt(-2.0 * Math.Log(val1)) *
				Math.Sin(2.0 * Math.PI * val2);

			// Adjust the value to take our parameters into account
			return mean + standardDeviation * gaussValue;
		}



		// PERLIN NOISE -------------------------------------------------------
		// Implementation from:
		// https://gist.github.com/Flafla2/1a0b9ebef678bbce3215

		// Original can be found at ken perlin's website:
		// http://mrl.nyu.edu/~perlin/noise/


		// Perlin noise - required fields
		private static bool perlinInitialized = false;
		private static int[] p = new int[512];

		// Hash lookup table as defined by Ken Perlin.  This is a randomly
		// arranged array of all numbers from 0-255 inclusive.
		private static readonly int[] permutation = { 151,160,137,91,90,15,
			131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
			190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
			88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
			77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
			102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
			135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
			5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
			223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
			129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
			251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
			49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
			138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
		};

		/// <summary>
		/// Calculates 1D Perlin Noise based on the given value
		/// </summary>
		/// <param name="x">The value in the x dimension</param>
		/// <returns>A single smoothed random number between 0 and 1</returns>
		public static double PerlinNoise(this Random rng, double x)
		{
			return PerlinNoise(rng, x, 0, 0);
		}

		/// <summary>
		/// Calculates 2D Perlin Noise based on the 2 given values
		/// </summary>
		/// <param name="x">The value in the x dimension</param>
		/// <param name="y">The value in the y dimension</param>
		/// <returns>A single smoothed random number between 0 and 1</returns>
		public static double PerlinNoise(this Random rng, double x, double y)
		{
			return PerlinNoise(rng, x, y, 0);
		}

		/// <summary>
		/// Calculates a layered 3D Perlin Noise based on the 3 given values,
		/// plus the number of layers (octaves) and how each layer contributes
		/// </summary>
		/// <param name="x">The value in the x dimension</param>
		/// <param name="y">The value in the y dimension</param>
		/// <param name="z">The value in the z dimension</param>
		/// <param name="octaves">The number of layers</param>
		/// <param name="persistence">How much each layer contributes</param>
		/// <returns>A layered, smoothed random value</returns>
		public static double PerlinNoise(this Random rng, double x, double y, double z, int octaves, double persistence)
		{
			double total = 0;
			double frequency = 1;
			double amplitude = 1;
			double maxValue = 0;  // Used for normalizing result to 0.0 - 1.0
			for (int i = 0; i < octaves; i++)
			{
				total += PerlinNoise(rng, x * frequency, y * frequency, z * frequency) * amplitude;

				maxValue += amplitude;

				amplitude *= persistence;
				frequency *= 2;
			}

			return total / maxValue;
		}

		/// <summary>
		/// Calculates 3D Perlin Noise based on the 3 given values
		/// </summary>
		/// <param name="x">The value in the x dimension</param>
		/// <param name="y">The value in the y dimension</param>
		/// <param name="z">The value in the z dimension</param>
		/// <returns>A single smoothed random number between 0 and 1</returns>
		public static double PerlinNoise(this Random rng, double x, double y, double z)
		{
			// Have we initialized the perlin noise yet?
			if (!perlinInitialized)
			{
				// Set up our larger array
				for (int i = 0; i < 512; i++)
					p[i] = permutation[i % 256];

				// We only need to do this once
				perlinInitialized = true;
			}

			// Calculate the "unit cube" that the point asked will be located in
			// The left bound is ( |_x_|,|_y_|,|_z_| ) and the right bound is that
			// plus 1.  Next we calculate the location (from 0.0 to 1.0) in that cube.
			// We also fade the location to smooth the result.
			int xi = (int)x & 255;
			int yi = (int)y & 255;
			int zi = (int)z & 255;
			double xf = x - (int)x;
			double yf = y - (int)y;
			double zf = z - (int)z;
			double u = Fade(xf);
			double v = Fade(yf);
			double w = Fade(zf);

			// This here is Perlin's hash function.  We take our x value (remember,
			// between 0 and 255) and get a random value (from our p[] array above) between
			// 0 and 255.  We then add y to it and plug that into p[], and add z to that.
			// Then, we get another random value by adding 1 to that and putting it into p[]
			// and add z to it.  We do the whole thing over again starting with x+1.  Later
			// we plug aa, ab, ba, and bb back into p[] along with their +1's to get another set.
			// in the end we have 8 values between 0 and 255 - one for each vertex on the unit cube.
			// These are all interpolated together using u, v, and w below.
			int a = p[xi] + yi;
			int aa = p[a] + zi;
			int ab = p[a + 1] + zi;
			int b = p[xi + 1] + yi;
			int ba = p[b] + zi;
			int bb = p[b + 1] + zi;


			// This is where the "magic" happens.  We calculate a new set of p[] values and use that to get
			// our final gradient values.  Then, we interpolate between those gradients with the u value to get
			// 4 x-values.  Next, we interpolate between the 4 x-values with v to get 2 y-values.  Finally,
			// we interpolate between the y-values to get a z-value.

			// When calculating the p[] values, remember that above, p[a+1] expands to p[xi]+yi+1 -- so you are
			// essentially adding 1 to yi.  Likewise, p[ab+1] expands to p[p[xi]+yi+1]+zi+1] -- so you are adding
			// to zi.  The other 3 parameters are your possible return values (see grad()), which are actually
			// the vectors from the edges of the unit cube to the point in the unit cube itself.
			double x1, x2, y1, y2;
			x1 = Lerp(Grad(p[aa], xf, yf, zf), Grad(p[ba], xf - 1, yf, zf), u);
			x2 = Lerp(Grad(p[ab], xf, yf - 1, zf), Grad(p[bb], xf - 1, yf - 1, zf), u);
			y1 = Lerp(x1, x2, v);

			x1 = Lerp(Grad(p[aa + 1], xf, yf, zf - 1), Grad(p[ba + 1], xf - 1, yf, zf - 1), u);
			x2 = Lerp(Grad(p[ab + 1], xf, yf - 1, zf - 1), Grad(p[bb + 1], xf - 1, yf - 1, zf - 1), u);
			y2 = Lerp(x1, x2, v);

			// For convenience we bound it to 0 - 1 (theoretical min/max before is -1 - 1)
			return (Lerp(y1, y2, w) + 1) / 2;
		}

		/// <summary>
		/// Helper method to calculate a gradient value using some advanced bitwise operations
		/// </summary>
		/// <param name="hash">The hash value to start with</param>
		/// <param name="x">The x value</param>
		/// <param name="y">The y value</param>
		/// <param name="z">The z value</param>
		/// <returns>The adjusted value</returns>
		public static double Grad(int hash, double x, double y, double z)
		{
			// Determine which formula to use
			switch (hash & 0xF)
			{
				case 0x0: return x + y;
				case 0x1: return -x + y;
				case 0x2: return x - y;
				case 0x3: return -x - y;
				case 0x4: return x + z;
				case 0x5: return -x + z;
				case 0x6: return x - z;
				case 0x7: return -x - z;
				case 0x8: return y + z;
				case 0x9: return -y + z;
				case 0xA: return y - z;
				case 0xB: return -y - z;
				case 0xC: return y + x;
				case 0xD: return -y + z;
				case 0xE: return y - x;
				case 0xF: return -y - z;
				default: return 0; // Should never happen
			}
		}

		/// <summary>
		/// Fade function as defined by Ken Perlin.  This eases coordinate values
		/// so that they will "ease" towards integral values.  This ends up smoothing
		/// the final output.
		/// </summary>
		/// <param name="t">The input value</param>
		/// <returns>The faded value</returns>
		public static double Fade(double t)
		{
			// 6t^5 - 15t^4 + 10t^3
			return t * t * t * (t * (t * 6 - 15) + 10);
		}

		/// <summary>
		/// Helper method to linearly interpolate between two values
		/// </summary>
		/// <param name="v0">The first value</param>
		/// <param name="v1">The second value</param>
		/// <param name="t">How far between the values (0 - 1)</param>
		/// <returns>The interpolated value</returns>
		private static double Lerp(double v0, double v1, double t)
		{
			return v0 + t * (v1 - v0);
		}

	}

}
