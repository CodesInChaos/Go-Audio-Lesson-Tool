using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using ChaosUtil;

namespace ImageAnalyzer
{
	[StructLayout(LayoutKind.Sequential)]
	public struct RawColor
	{
		public readonly UInt32 Raw;
		public byte A { get { return (byte)(Raw >> 24); } }
		public byte R { get { return (byte)(Raw >> 16); } }
		public byte G { get { return (byte)(Raw >> 8); } }
		public byte B { get { return (byte)Raw; } }
		public uint RGB { get { return Raw & 0x00FFFFFF; } }

		private RawColor(uint raw)
		{
			Raw = raw;
		}

		public static RawColor FromRgb(byte r, byte g, byte b)
		{
			return new RawColor(0xFF000000u | (uint)r << 16 | (uint)g << 8 | (uint)b);
		}

		public static RawColor FromArgb(UInt32 argb)
		{
			return new RawColor(argb);
		}

		public static bool operator ==(RawColor c1, RawColor c2)
		{
			return c1.Raw == c2.Raw;
		}

		public static bool operator !=(RawColor c1, RawColor c2)
		{
			return c1.Raw != c2.Raw;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			return this == (RawColor)obj;
		}

		public override int GetHashCode()
		{
			return (int)Raw;
		}

		public float GetUnweightedBrightness()
		{
			return (R + G + B) * (1f / (255 * 3));
		}

		public override string ToString()
		{
			if (A == 0xFF)
				return RGB.ToHex(6);
			else
				return Raw.ToHex(8);
		}

		public static RawColor Transparent { get { return new RawColor(); } }
		public static RawColor Black { get { return new RawColor(0xFF000000); } }
		public static RawColor White { get { return new RawColor(0xFFFFFFFF); } }
		public static RawColor Red { get { return new RawColor(0xFFFF0000); } }
		public static RawColor Green { get { return new RawColor(0xFF00FF00); } }
		public static RawColor Blue { get { return new RawColor(0xFF0000FF); } }
		public static RawColor Yellow { get { return new RawColor(0xFFFFFF00); } }
		public static RawColor Magenta { get { return new RawColor(0xFFFF00FF); } }
		public static RawColor Cyan { get { return new RawColor(0xFF00FFFF); } }
	}
}
