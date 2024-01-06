using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace RocketLeagueConfigManager
{
	internal static class ConfigurationWriter
	{
		private const string VersionHeader = "[IniVersion]";

		internal static bool WriteEntireFile(string fromFile, string toFile)
		{
			fromFile = Path.GetNewFilePath(fromFile);
			toFile = Path.GetNewFilePath(toFile);

			if (!File.Exists(fromFile))
			{
				Logger.Log($"ERROR: File {fromFile} does not exist.");
				return false;
			}

			File.WriteAllText(toFile, File.ReadAllText(fromFile));
			return true;
		}

		internal static string ExtractVersion(string fromFileEntirePath)
		{
			if (!File.Exists(fromFileEntirePath))
			{
				Logger.Log($"ERROR: File {fromFileEntirePath} does not exist.");
				return "";
			}

			string contentFrom = File.ReadAllText(fromFileEntirePath);
			string version = contentFrom.Split(VersionHeader)[1].Trim();
			return version;
		}

		internal static bool InsertVersionOnly(string fromFile, string toFile)
		{
			fromFile = Path.GetNewFilePath(fromFile);
			toFile = Path.GetNewFilePath(toFile);

			if (!File.Exists(fromFile))
			{
				Logger.Log($"ERROR: File {fromFile} does not exist.");
				return false;
			}
			if (!File.Exists(toFile))
			{
				Logger.Log($"ERROR: File {toFile} does not exist.");
				return false;
			}

			string version = ExtractVersion(fromFile);

			string contentTo = File.ReadAllText(toFile);
			StringBuilder builder = new StringBuilder(contentTo.Split(VersionHeader)[0]);
			builder.AppendLine(VersionHeader);
			builder.AppendLine(version);
			File.WriteAllText(toFile, builder.ToString());
			return true;
		}

		internal static bool InsertContentOnly(string fromFile, string toFile)
		{
			fromFile = Path.GetNewFilePath(fromFile);
			toFile = Path.GetNewFilePath(toFile);

			if (!File.Exists(fromFile))
			{
				Logger.Log($"ERROR: File {fromFile} does not exist.");
				return false;
			}
			if (!File.Exists(toFile))
			{
				Logger.Log($"ERROR: File {toFile} does not exist.");
				return false;
			}

			string contentFrom = File.ReadAllText(fromFile);
			contentFrom = contentFrom.Split(VersionHeader)[0];
			string version = ExtractVersion(toFile);

			StringBuilder builder = new StringBuilder(contentFrom);
			builder.AppendLine(VersionHeader);
			builder.AppendLine(version);
			File.WriteAllText(toFile, builder.ToString());
			return true;
		}
	}
}
