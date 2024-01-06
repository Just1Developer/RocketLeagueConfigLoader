namespace RocketLeagueConfigManager
{
	internal static class Path
	{
		private const bool Debugging = false;

		internal static readonly string DEFAULT_CONFIG_PATH =
			$"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/My Games/Rocket League/TAGame/Config{(Debugging ? "2" : "")}/";

		internal const string CONFIG_FILE_NAME = "TAInput.ini";
		private const string CONFIG_PREFIX = "TAInput ";
		private const string CONFIG_SUFFIX = ".ini";

		internal const byte FLAG_CONTROLLER_ENABLED = 0b10;
		internal const byte FLAG_DISABLE_MOUSEMOVE = 0b01;

		private static string GetAttributeFlags(byte flags)
		{
			string gamepad = (flags & FLAG_CONTROLLER_ENABLED) != 0 ? " -gamepad" : "";
			string noMouseMove = (flags & FLAG_DISABLE_MOUSEMOVE) != 0 ? " -noMouseMove" : "";
			return $"{gamepad}{noMouseMove}";
		}

		private static byte GenerateFlags(bool Controller, bool DisableMouseMovement)
		{
			return (byte) ((Controller ? FLAG_CONTROLLER_ENABLED : 0) | (DisableMouseMovement ? FLAG_DISABLE_MOUSEMOVE : 0));
		}

		internal static string GetFileName(Platform platform, bool Controller, bool DisableMouseMovement)
		{
			if (platform == Platform.RocketLeague) return CONFIG_FILE_NAME;
			byte b_flags = GenerateFlags(Controller, DisableMouseMovement);
			return GetFileName(platform, b_flags);
		}

		internal static string GetFileName(Platform platform, byte b_flags)
		{
			string flags = GetAttributeFlags(b_flags);
			return $"{CONFIG_PREFIX}{platform}{flags}{CONFIG_SUFFIX}";
		}

		internal static string GetNewFilePath(Platform platform, bool Controller, bool DisableMouseMovement)
			=> $"{DEFAULT_CONFIG_PATH}{GetFileName(platform, Controller, DisableMouseMovement)}";

		internal static string GetNewFilePath(string filename)
			=> $"{DEFAULT_CONFIG_PATH}{filename}";

		internal static string[] AllFilenamesOf(Platform platform)
		{
			byte maxFlag = FLAG_CONTROLLER_ENABLED | FLAG_DISABLE_MOUSEMOVE;
			string[] files = new string[maxFlag + 1];
			for (byte flag = 0; flag <= maxFlag; flag++)
			{
				files[flag] = GetFileName(platform, flag);
			}
			return files;
		}
	}

	internal enum Platform
	{
		Steam, Epic, RocketLeague
	}
}
