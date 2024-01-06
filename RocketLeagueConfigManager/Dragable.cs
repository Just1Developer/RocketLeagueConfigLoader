namespace RocketLeagueConfigManager
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

			if (HeldPlatform == Platform.RocketLeague)
			{
				string fromFile = Path.CONFIG_FILE_NAME;
				// All Platform Files.
				var allFilesTo = Path.AllFilenamesOf(Platform);
				foreach (var toFile in allFilesTo)
				{
					ConfigurationWriter.InsertVersionOnly(fromFile, toFile);
				}
				HighlightDrag(Platform);
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
					ConfigurationWriter.InsertContentOnly(fromFile, toFile);
				}
			}
			else
			// Copying from held platform to RL icon.
			{
				string fromFile = Path.GetFileName(HeldPlatform, Form1.GetSettingFlags());
				string toFile = Path.CONFIG_FILE_NAME;
				ConfigurationWriter.WriteEntireFile(fromFile, toFile);
			}
			HighlightDrag(Platform);
		}

		static void HighlightDrag(Platform platform) => HighlightDrag((int)platform);
		static void HighlightDrag(int platform)
		{
			DoHighlightDragCompleted[platform]++;
			// Perform -- 1 second later
			new Thread(() =>
			{
				Thread.Sleep(HighlightTimeMS);
				DoHighlightDragCompleted[platform]--;
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


/*
 *
 *
 *		internal Dragable(Platform platform, Point Location)
   {
   	//this.DoubleBuffered = true;
   	//SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

   	_image = new PictureBox()
   	{
   		Image = ImageHandler.GetImageOf(platform),
   		Dock = DockStyle.Fill,
   		SizeMode = PictureBoxSizeMode.Zoom
   	};

   	this.Size = new Size(140, 140);

   	this.Controls.Add(_image);
   	this.Size = _image.Size;
   	this.Location = Location;

   	this._image.MouseDown += OnMouseDown;
   	this._image.MouseUp += OnMouseUp;
   	this._image.MouseMove += (s, e) =>
   	{
   		Form1.MouseMoved(this, e);
   		Invalidate();
   	};
   }
 *
 */