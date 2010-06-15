using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioLessons
{
	internal class FakeCrc32
	{
		public const uint poly = 0xedb88320;
		public const uint startxor = 0xffffffff;

		static uint[] table = null;
		static uint[] revtable = null;

		public void FixChecksum(byte[] bytes, int fixpos, uint wantcrc)
		{
			int length = bytes.Length;
			if (fixpos + 4 > length)
				throw new ArgumentException();

			uint crc = startxor;
			for (int i = 0; i < fixpos; i++)
			{
				crc = (crc >> 8) ^ table[(crc ^ bytes[i]) & 0xff];
			}

			Array.Copy(BitConverter.GetBytes(crc), 0, bytes, fixpos, 4);

			crc = wantcrc ^ startxor;
			for (int i = length - 1; i >= fixpos; i--)
			{
				crc = (crc << 8) ^ revtable[crc >> (3 * 8)] ^ bytes[i];
			}

			Array.Copy(BitConverter.GetBytes(crc), 0, bytes, fixpos, 4);
		}

		public FakeCrc32()
		{
			if (FakeCrc32.table == null)
			{
				uint[] table = new uint[256];
				uint[] revtable = new uint[256];

				uint fwd, rev;
				for (int i = 0; i < table.Length; i++)
				{
					fwd = (uint)i;
					rev = (uint)(i) << (3 * 8);
					for (int j = 8; j > 0; j--)
					{
						if ((fwd & 1) == 1)
						{
							fwd = (uint)((fwd >> 1) ^ poly);
						}
						else
						{
							fwd >>= 1;
						}

						if ((rev & 0x80000000) != 0)
						{
							rev = ((rev ^ poly) << 1) | 1;
						}
						else
						{
							rev <<= 1;
						}
					}
					table[i] = fwd;
					revtable[i] = rev;
				}

				FakeCrc32.table = table;
				FakeCrc32.revtable = revtable;
			}
		}
	}
}
