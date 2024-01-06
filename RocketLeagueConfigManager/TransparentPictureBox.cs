using System.Drawing.Imaging;

namespace RocketLeagueConfigManager
{
	internal class MouseHoverPictureBox : Panel
	{
		public MouseHoverPictureBox()
		{
			this.DoubleBuffered = true;
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.Size = new Size(Form1.HOVER_SIZE, Form1.HOVER_SIZE);

			// MouseMove detection is relevant here.
			this.MouseMove += (s, e) => Form1.MouseMoved(this, e);
		}

		public float Opacity { get; set; } = 0.6f; // Default opacity is 60%
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (!MouseHoverImageHandler.HasImage) return;

			Image? _image = MouseHoverImageHandler.Image;
			if (_image == null) throw new NullReferenceException();
			// Set the transparency
			ColorMatrix colorMatrix = new ColorMatrix
			{
				Matrix33 = Opacity
			};
			ImageAttributes imageAttributes = new ImageAttributes();
			imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

			// Draw the image with the opacity
			Rectangle drawRect = new Rectangle(new Point(0, 0), this.Size);
			e.Graphics.DrawImage(_image, drawRect, 0, 0, _image.Width, _image.Height, GraphicsUnit.Pixel, imageAttributes);
		}
	}
}

// Old:
//Rectangle drawRect = new Rectangle(MouseLoc.X, MouseLoc.Y, Form1.HOVER_SIZE, Form1.HOVER_SIZE);