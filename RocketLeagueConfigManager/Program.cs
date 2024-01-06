namespace RocketLeagueConfigManager
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			ApplicationConfiguration.Initialize();
			Application.Run(new Form1());
		}
	}
}

/*
 * Print:
 *
   // Write All Images to the console
   string downloads = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/../Downloads/";
   ImageHandler.PrintImageString(downloads + "Steam_icon_logo.png");
   ImageHandler.PrintImageString(downloads + "Epic_Games_logo.png");
   ImageHandler.PrintImageString(downloads + "Rocket_League_logo.png");
	ImageHandler.PrintImageString(@"E:\Coding\C#\Forms\RocketLeagueConfigManager\RocketLeagueConfigManager\icon.png");
 */