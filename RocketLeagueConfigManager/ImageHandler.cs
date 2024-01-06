using System.Drawing.Imaging;

namespace RocketLeagueConfigManager
{
	internal static class ImageHandler
	{
		internal static Image GetImageOf(Platform platform)
		{
			return platform switch
			{
				Platform.Steam => ImageFromStreamString(ImageStreams.STEAM_ICON_BMP),
				Platform.Epic => ImageFromStreamString(ImageStreams.EPIC_ICON_BMP),
				_ => ImageFromStreamString(ImageStreams.RL_ICON_BMP)
			};
		}

		static Image ImageFromStreamString(string streamData)
		{
			return Image.FromStream(new MemoryStream(Convert.FromBase64String(streamData)));
		}

		static string GetImageString(Image img)
		{
			using var ms = new MemoryStream();
			img.Save(ms, ImageFormat.Png);
			return Convert.ToBase64String(ms.ToArray());
		}

		// For Debug / Setup purposes
		internal static void PrintImageString(string filepath)
		{
			Image img = Image.FromFile(filepath);
			string str = GetImageString(img);
			if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debug.WriteLine(str);
			else Console.WriteLine(str);
		}
    }
}
