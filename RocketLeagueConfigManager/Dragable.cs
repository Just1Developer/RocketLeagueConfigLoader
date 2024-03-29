﻿namespace RocketLeagueConfigManager
{
	internal class Dragable
	{
		internal static bool IsHeld = false;
		internal static Platform HeldPlatform = Platform.RocketLeague;
		internal static int[] DoHighlightDragCompleted = new int[3];	// 3 Platforms
		private const int HighlightTimeMS = 650;
		
		private readonly Image _image;
		internal Platform Platform;

		internal Image Image
		{
			get => _image;
		}

		internal Rectangle Bounds { get; private set; }

		internal Dragable(Platform platform) : this(platform, new Point(0, 0)) { }
		internal Dragable(Platform platform, Point Location)
		{
			this.Platform = platform;
			this._image = ImageHandler.GetImageOf(platform);
			this.Bounds = new Rectangle(Location, new Size(Form1.ICON_SIZE, Form1.ICON_SIZE));
		}

		internal void OnMouseDown(object? sender, MouseEventArgs e)
		{
			if (IsHeld) return;
			IsHeld = true;
			HeldPlatform = Platform;
			MouseHoverImageHandler.Image = this._image;
		}

		internal void OnMouseUp(object? sender, MouseEventArgs e)
		{
			LetGo();
			// Check location

			// HoverOver is the current platform (where it was dropped)
			if (HeldPlatform == Platform) return;
			// Dropped over a different Platform

			bool success = false;

			if (HeldPlatform == Platform.RocketLeague)
			{
				string fromFile = Path.CONFIG_FILE_NAME;
				// All Platform Files.
				var allFilesTo = Path.AllFilenamesOf(Platform);
				foreach (var toFile in allFilesTo)
				{
					bool result = ConfigurationWriter.InsertVersionOnly(fromFile, toFile);
					if (!success) success = result;
				}
				HighlightDrag(Platform, success);
				return;
			}

			if (Platform != Platform.RocketLeague)
			{
				// Copying from one platform to the other. Each with their flag.
				var allFilesTo = Path.AllFilenamesOf(Platform);
				for (byte flag = 0; flag < allFilesTo.Length; flag++)
				{
					string fromFile = Path.GetFileName(HeldPlatform, flag);
					string toFile = allFilesTo[flag];
					bool result = ConfigurationWriter.InsertContentOnly(fromFile, toFile);
					if (!success) success = result;
				}
			}
			else
			// Copying from held platform to RL icon.
			{
				string fromFile = Path.GetFileName(HeldPlatform, Form1.GetSettingFlags());
				string toFile = Path.CONFIG_FILE_NAME;
				success = ConfigurationWriter.WriteEntireFile(fromFile, toFile);
			}
			HighlightDrag(Platform, success);
		}

		// We are just gonna ignore that this cancels out for a time

		static void HighlightDrag(Platform platform, bool success = true)
		{
			if(success) HighlightDragGreen((int)platform);
			else HighlightDragRed((int)platform);
		}
		static void HighlightDragGreen(int platform)
		{
			DoHighlightDragCompleted[platform]++;
			// Perform -- 1 second later
			new Thread(() =>
			{
				Thread.Sleep(HighlightTimeMS);
				DoHighlightDragCompleted[platform]--;
				Form1.InvalidateHover();
			}).Start();
		}
		static void HighlightDragRed(int platform)
		{
			DoHighlightDragCompleted[platform]--;
			// Perform -- 1 second later
			new Thread(() =>
			{
				Thread.Sleep(HighlightTimeMS);
				DoHighlightDragCompleted[platform]++;
				Form1.InvalidateHover();
			}).Start();
		}

		internal static void LetGo()
		{
			if (!IsHeld) return;
			IsHeld = false;
			MouseHoverImageHandler.Image = null;
		}
	}
}
