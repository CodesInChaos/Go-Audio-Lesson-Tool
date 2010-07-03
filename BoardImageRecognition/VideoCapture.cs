using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ScreenShots;
using System.Drawing.Imaging;

namespace BoardImageRecognition
{
	public class VideoCapture
	{
		PixelPool pixelPool;
		public int PoolSize { get { return pixelPool.Size; } }
		public int CacheMissAllocs { get { return pixelPool.CacheMissCount; } }
		public int TotalAllocs { get { return pixelPool.AllocCount; } }

		public RawColor[,] Capture(IntPtr windowHandle)
		{
			Bitmap bmp = ScreenCapture.CaptureWindow(windowHandle);
			RawColor[,] pixels = BitmapToPixels(bmp);
			bmp.Dispose();
			return pixels;
		}

		public void Release(RawColor[,] pix)
		{
			pixelPool.Release(pix);
		}

		public VideoCapture()
		{
			pixelPool = new PixelPool(10);
		}

		private RawColor[,] BitmapToPixels(Bitmap bmp)
		{
			RawColor[,] pix = pixelPool.Alloc(bmp.Width, bmp.Height);
			//RawColor[,] pix = new RawColor[bmp.Width, bmp.Height];
			unsafe
			{
				BitmapData data = null;
				try
				{
					data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

					for (int y = 0; y < data.Height; y++)
					{
						uint* p = (uint*)((byte*)data.Scan0 + y * data.Stride);
						for (int x = 0; x < data.Width; x++)
						{
							Color c = Color.FromArgb((int)*p);
							pix[x, y] = RawColor.FromArgb(*p);
							p++;
						}
					}
				}
				finally
				{
					if (data != null)
						bmp.UnlockBits(data);
				}
			}
			return pix;
		}
	}
}
