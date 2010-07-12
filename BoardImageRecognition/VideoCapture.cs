using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ScreenShots;
using System.Drawing.Imaging;
using Chaos.Image;

namespace BoardImageRecognition
{
	public class VideoCapture
	{
		PixelPool pixelPool;
		public int PoolSize { get { return pixelPool.Size; } }
		public int CacheMissAllocs { get { return pixelPool.CacheMissCount; } }
		public int TotalAllocs { get { return pixelPool.AllocCount; } }

		public Pixels Capture(IntPtr windowHandle)
		{
			Bitmap bmp = ScreenCapture.CaptureWindow(windowHandle);
			Pixels pixels = BitmapToPixels(bmp);
			bmp.Dispose();
			return pixels;
		}

		private Pixels BitmapToPixels(Bitmap bmp)
		{
			Pixels pix = new Pixels(pixelPool.Alloc(bmp.Width, bmp.Height));
			pix.LoadFromBitmap(bmp);
			return pix;
		}

		public void Release(Pixels pix)
		{
			if (pix.Data != null)
				pixelPool.Release(pix.Data);
		}

		public VideoCapture()
		{
			pixelPool = new PixelPool(10);
		}


	}
}
