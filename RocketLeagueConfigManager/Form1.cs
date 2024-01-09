namespace RocketLeagueConfigManager
{
	public partial class Form1 : Form
	{
		internal const int HOVER_SIZE = 80, ICON_SIZE = 140;

		private static Form1 instance;

		public Form1()
		{
			InitializeComponent();
			instance = this;
			this.CheckForIllegalCrossThreadCalls = false;	// For Invalidating

			this.DoubleBuffered = true;
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			UpdateStyles();

			this.Icon = System.Drawing.Icon.FromHandle(new System.Drawing.Bitmap(Image.FromStream(
				new System.IO.MemoryStream(Convert.FromBase64String(
					ImageStreams.APPLICATION_ICON
				)))).GetHicon());
		}

		internal static List<Dragable> GetDragables()
		{
			return instance.DragableElements;
		}

		private Dragable Steam, Epic, RL;
		private List<Dragable> DragableElements = new ();

		private CheckBox cb_ControllerCfg, cb_MouseMovement;

		private void Form1_Load(object sender, EventArgs e)
		{
			HoverPainterOverlay = new();
			Controls.Add(HoverPainterOverlay);

			int RLPadding = Height / 2 - ICON_SIZE / 2 - 25;
			RL = new Dragable(Platform.RocketLeague, new Point(RLPadding - 50, RLPadding));		// +10 compensate for visual background of the bar at the top
			Steam = new Dragable(Platform.Steam, new Point(Width - RLPadding - ICON_SIZE + 10, Height / 2 - ICON_SIZE - 80));
			Epic = new Dragable(Platform.Epic, new Point(Width - RLPadding - ICON_SIZE + 10, Height / 2));

			DragableElements.Add(Steam);
			DragableElements.Add(Epic);
			DragableElements.Add(RL);

			cb_ControllerCfg = new CheckBox
			{
				Location = new Point(Width / 2 - 320, Height - 115),
				Font = new Font(FontFamily.GenericSansSerif, 15f),
				Text = "Load Controller Configuration",
				AutoSize = true
			};
			cb_MouseMovement = new CheckBox
			{
				Location = new Point(cb_ControllerCfg.Location.X + cb_ControllerCfg.Width + 210, cb_ControllerCfg.Location.Y),
				Font = cb_ControllerCfg.Font,
				Text = "Use Mouse Movement for Airroll",
				AutoSize = true,
				Checked = true
			};

			HoverPainterOverlay.Controls.Add(cb_ControllerCfg);
			HoverPainterOverlay.Controls.Add(cb_MouseMovement);

			Label RLLabel = new Label
			{
				Text = "Rocket League Config",
				Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Bold),
				Location = new Point(RL.Bounds.Location.X - 64, RL.Bounds.Location.Y - 50),
				AutoSize = true
			};
			Label CustomLabel = new Label
			{
				Text = "Custom Configurations",
				Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Bold),
				Location = new Point(Steam.Bounds.Location.X - 70, Steam.Bounds.Location.Y - 50),
				AutoSize = true
			};

			HoverPainterOverlay.Controls.Add(RLLabel);
			HoverPainterOverlay.Controls.Add(CustomLabel);
		}

		internal static Point MouseLoc;
		private HoverPainterOverlay HoverPainterOverlay;

		internal static void MouseMoved(Control relativeControl, MouseEventArgs e) => instance._MouseMoved(relativeControl, e);
		internal void _MouseMoved(Control relativeControl, MouseEventArgs e)
		{
			MouseLoc = new Point(e.Location.X - HOVER_SIZE / 2 + relativeControl.Location.X,
				e.Location.Y - HOVER_SIZE / 2 + relativeControl.Location.Y);
			HoverPainterOverlay.Invalidate();
		}

		internal static void InvalidateHover()
		{
			instance.HoverPainterOverlay.Invalidate();
		}

		public static void UpdateImage()
		{
			instance.Invalidate();
		}

		internal static byte GetSettingFlags()
		{
			byte flag = 0;
			if (instance.cb_ControllerCfg.Checked) flag |= Path.FLAG_CONTROLLER_ENABLED;
			if (!instance.cb_MouseMovement.Checked) flag |= Path.FLAG_DISABLE_MOUSEMOVE;
			return flag;
		}
	}

	public static class Logger
	{
		public static void Log(string message)
		{
			if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debug.WriteLine($"[{DateTime.Now}] {message}");
			else System.Diagnostics.Debug.WriteLine($"[{DateTime.Now}] {message}");
		}
	}
}
