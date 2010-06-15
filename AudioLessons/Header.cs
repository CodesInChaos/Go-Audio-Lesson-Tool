using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;

namespace AudioLessons
{
	internal static class Header
	{
		private static readonly byte[] fileinfoMagicBytes = Encoding.UTF8.GetBytes("fileinfo");
		private static readonly byte[] magicBytes = Encoding.UTF8.GetBytes("GameAudioLesson\0");
		private static readonly byte[] gameMagicBytes = Encoding.UTF8.GetBytes("Go\0\0\0\0\0\0\0\0\0\0\0\0\0\0");
		private const int relSizePos = 16 + 16 + 2 + 2;
		private const int relHashPos = relSizePos + 8;
		private const int headerSize = relHashPos + 20 + 4;
		private const int headerFileinfoPos = 30;
		private const int headerPos = headerFileinfoPos + 8;
		private const UInt16 VersionMayor = 1;
		private const UInt16 VersionMinor = 0;
		private const UInt32 HeaderCrc32 = 0xD1CE0DD5;
		private static readonly FakeCrc32 fakeCrc = new FakeCrc32();

		private static bool AreArraysIdentical<T>(T[] a1, T[] a2)
		{
			int length = a1.Length;
			if (length != a2.Length)
				return false;
			var comparer = EqualityComparer<T>.Default;
			for (int i = 0; i < length; i++)
			{
				if (!comparer.Equals(a1[i], a2[i]))
					return false;
			}
			return true;
		}

		private static byte[] HashStream(Stream lessonFile)
		{//Fixme: Create Version without copying the whole file
			//Copy Stream
			byte[] lessonData = new byte[lessonFile.Length];
			lessonFile.Position = 0;
			lessonFile.Read(lessonData, 0, lessonData.Length);
			MemoryStream lessonFileCopy = new MemoryStream(lessonData);
			//Mask Hash & Crc32 Faker
			lessonFileCopy.Position = headerPos + relHashPos;
			lessonFileCopy.Write(new byte[24], 0, 24);
			//Hash masked stream
			lessonFileCopy.Seek(0, SeekOrigin.Begin);
			SHA1 sha = new SHA1CryptoServiceProvider();
			return sha.ComputeHash(lessonFileCopy);
		}

		public static void Verify(Stream lessonFile)
		{
			long savedPosition = lessonFile.Position;
			BinaryReader reader = new BinaryReader(lessonFile);

			lessonFile.Seek(headerFileinfoPos, SeekOrigin.Begin);
			byte[] readfileinfoMagicBytes = reader.ReadBytes(fileinfoMagicBytes.Length);
			if (!AreArraysIdentical(readfileinfoMagicBytes, fileinfoMagicBytes))
				throw new InvalidDataException("Invalid Header, this is no Audio Lesson");
			byte[] readMagicBytes = reader.ReadBytes(magicBytes.Length);
			if (!AreArraysIdentical(readMagicBytes, magicBytes))
				throw new InvalidDataException("Invalid Header, this is no Audio Lesson");
			byte[] readGameMagicBytes = reader.ReadBytes(gameMagicBytes.Length);
			if (!AreArraysIdentical(readGameMagicBytes, gameMagicBytes))
				throw new InvalidDataException("Unsupported Game");
			if (reader.ReadBigEndian(2) != VersionMayor)
				throw new InvalidDataException("Unsupported File Version");
			if (reader.ReadBigEndian(2) > VersionMinor)
			{
				//Warn?
			}
			long readSize = (long)reader.ReadBigEndian(8);
			if (readSize != lessonFile.Length)
				throw new InvalidDataException("Wrong length of file");
			byte[] readHash = reader.ReadBytes(20);
			byte[] hash = HashStream(lessonFile);
			if (!AreArraysIdentical(hash, readHash))
				throw new InvalidDataException("Wrong hash of file");
		}

		public static byte[] CreateFake()
		{
			byte[] fakeHeader = new byte[headerSize];
			fakeCrc.FixChecksum(fakeHeader, 0, HeaderCrc32);
			return fakeHeader;
		}

		public static void Write(Stream lessonFile)
		{
			byte[] header = new byte[headerSize];
			BinaryWriter writer = new BinaryWriter(new MemoryStream(header));
			//Header/MagicBytes
			writer.Write(magicBytes);
			writer.Write(gameMagicBytes);
			//Version.Mayor
			writer.WriteBigEndian(VersionMayor, 2);
			//Version.Minor
			writer.WriteBigEndian(VersionMinor, 2);
			//Size
			UInt64 len = (UInt64)lessonFile.Length;
			Debug.Assert(relSizePos == writer.BaseStream.Position);
			writer.WriteBigEndian(len, 8);
			writer.Flush();
			//Write out header
			lessonFile.Seek(headerPos, SeekOrigin.Begin);
			lessonFile.Write(header.ToArray(), 0, (int)header.Length);
			//Fill in hash
			lessonFile.Seek(0, SeekOrigin.Begin);
			SHA1 sha = new SHA1CryptoServiceProvider();
			byte[] hash = sha.ComputeHash(lessonFile);
			Debug.Assert(relHashPos == writer.BaseStream.Position);
			writer.Write(hash, 0, hash.Length);
			writer.Flush();
			//Fix hash
			fakeCrc.FixChecksum(header, header.Length - 4, HeaderCrc32);
			//Write out header
			lessonFile.Seek(headerPos, SeekOrigin.Begin);
			lessonFile.Write(header.ToArray(), 0, (int)header.Length);
		}
	}
}
