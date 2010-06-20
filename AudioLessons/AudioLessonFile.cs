using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ionic.Zip;
using Ionic.Zlib;
using ChaosUtil;
using System.Security.Cryptography;
using System.Diagnostics;

namespace AudioLessons
{
	public static class AudioLessonFile
	{
		private const long MaxSize = 300 * 1024 * 1024;

		public static void Load(string fileName, out string replay, out Stream audio)
		{
			using (FileStream stream = File.OpenRead(fileName))
			{
				Load(stream, out replay, out audio);
			}
		}


		public static void Load(Stream lessonFile, out string replay, out Stream audio)
		{
			if (lessonFile.Length > MaxSize)
				throw new InvalidDataException("File too large");

			replay = null;
			audio = null;
			Debug.Assert(lessonFile.Position == 0);
			Header.Verify(lessonFile);
			lessonFile.Seek(0, SeekOrigin.Begin);
			using (ZipFile zip = ZipFile.Read(lessonFile))
			{
				if (zip[0].FileName != "fileinfo")
					throw new InvalidDataException("First Entry must be \"fileinfo\"");
				if (zip["fileinfo"].CompressionMethod != CompressionMethod.None)
					throw new InvalidDataException("fileinfo must be uncompressed");
				if (zip["AudioLesson\\Audio.ogg"].CompressionMethod != CompressionMethod.None)
					throw new InvalidDataException("Audio.ogg must be uncompressed");
				double totalSize = (double)zip["AudioLesson\\Audio.ogg"].UncompressedSize +
					((double)zip["AudioLesson\\Replay.gor"].UncompressedSize) * 2;//Double to prevent int overflow, *2 for UTF8->UTF16
				if (totalSize > MaxSize)
					throw new InvalidDataException("File too large");

				MemoryStream replayStream = new MemoryStream();
				zip["AudioLesson\\Replay.gor"].Extract(replayStream);
				replay = Encoding.UTF8.GetString(replayStream.ToArray());
				audio = new MemoryStream();
				zip["AudioLesson\\Audio.ogg"].Extract(audio);
				audio.Seek(0, SeekOrigin.Begin);
			}
		}

		public static void FakeLoad(out string replay, out Stream audio)
		{
			replay = File.ReadAllText("Replay.gor", Encoding.UTF8);
			audio = new MemoryStream(File.ReadAllBytes("Audio.ogg"));
		}


		public static void Save(string filename, string replay, Stream audio)
		{
			using (FileStream fs = File.Create(filename))
			{
				Save(fs, replay, audio);
			}
		}



		public static void Save(Stream lessonFile, string replay, Stream audio)
		{
			if (lessonFile.Length != 0)
				throw new NotImplementedException();
			if (audio.Position != 0)
				throw new ArgumentException();

			byte[] fakeHeader = Header.CreateFake();

			ZipFile zip = new ZipFile(Encoding.UTF8);
			zip.CompressionLevel = CompressionLevel.None;
			ZipEntry fileinfoEntry = zip.AddEntry("fileinfo", fakeHeader);
			fileinfoEntry.EmitTimesInWindowsFormatWhenSaving = false;
			fileinfoEntry.EmitTimesInUnixFormatWhenSaving = false;
			zip.CompressionLevel = CompressionLevel.BestCompression;
			zip.AddEntry("AudioLesson\\Replay.gor", replay, Encoding.UTF8);
			zip.CompressionLevel = CompressionLevel.None;
			zip.AddEntry("AudioLesson\\Audio.ogg", audio);
			zip.Save(lessonFile);

			Header.Write(lessonFile);
		}
	}
}
