using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ionic.Zip;
using Ionic.Zlib;
using Chaos.Util;
using System.Security.Cryptography;
using System.Diagnostics;

namespace AudioLessons
{
	public class AudioLessonFile
	{
		private const string ReplayFilename = "AudioLesson\\Replay.GoReplay";
		private const string AudioFilename = "AudioLesson\\Audio.ogg";

		private const long MaxSize = 300 * 1024 * 1024;

		public static void Load(string fileName, out string replay, out Stream audio)
		{
			using (FileStream stream = File.OpenRead(fileName))
			{
				Load(stream, out replay, out audio);
			}
		}

		public static bool IsAudioLesson(string fileName)
		{
			using (FileStream stream = File.OpenRead(fileName))
			{
				return Header.IsAudioLesson(stream);
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
				if (zip[AudioFilename].CompressionMethod != CompressionMethod.None)
					throw new InvalidDataException("Audio.ogg must be uncompressed");
				double totalSize = (double)zip[AudioFilename].UncompressedSize +
					((double)zip[ReplayFilename].UncompressedSize) * 2;//Double to prevent int overflow, *2 for UTF8->UTF16
				if (totalSize > MaxSize)
					throw new InvalidDataException("File too large");

				MemoryStream replayStream = new MemoryStream();
				zip[ReplayFilename].Extract(replayStream);
				replay = Encoding.UTF8.GetString(replayStream.ToArray());
				audio = new MemoryStream();
				zip[AudioFilename].Extract(audio);
				audio.Seek(0, SeekOrigin.Begin);
			}
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
			zip.AddEntry(ReplayFilename, replay, Encoding.UTF8);
			zip.CompressionLevel = CompressionLevel.None;
			zip.AddEntry(AudioFilename, audio);
			zip.Save(lessonFile);

			Header.Write(lessonFile);
		}
	}
}
