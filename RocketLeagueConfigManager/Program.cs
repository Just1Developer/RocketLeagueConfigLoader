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
 * Print an Image:
 * 1. Get Path (of images)
		string downloads = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/../Downloads/";
 * 2. Print image as string
		ImageHandler.PrintImageString(downloads + "Steam_icon_logo.png");
 * 3. done.
 */