using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketLeagueConfigManager
{
	internal class HoverPainterOverlay : Panel
	{
		public HoverPainterOverlay()
		{
			this.DoubleBuffered = true;
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.Dock = DockStyle.Fill;
			
			// MouseMove detection is relevant here.
			this.MouseMove += (s, e) => Form1.MouseMoved(this, e);
			this.MouseDown += (s, e) =>
			{
				// Query the relevant info to the Dragable beneath.
				foreach (var drag in Form1.GetDragables())
				{
					if (!drag.Bounds.Contains(e.Location)) continue;
					drag.OnMouseDown(s, e);
					return;
				}
			};
			this.MouseUp += (s, e) =>
			{
				// Query the relevant info to the Dragable beneath.
				foreach (var drag in Form1.GetDragables())
				{
					if (!drag.Bounds.Contains(e.Location)) continue;
					drag.OnMouseUp(s, e);
					return;
				}
				Dragable.LetGo();
			};
		}

		public float Opacity { get; set; } = 0.6f; // Default opacity is 60%
		protected override void OnPaint(PaintEventArgs e)
		{
			// Go through every Dragable in the form and paint its image at its location too.
			// Yup. That's gonna be my solution. Suck it.
			foreach (var drag in Form1.GetDragables())
			{
				if (Dragable.DoHighlightDragCompleted[(int) drag.Platform] > 0)
				{
					e.Graphics.FillRectangle(Brushes.LightGreen, drag.Bounds);
					e.Graphics.DrawImage(drag.Image, drag.Bounds);
					e.Graphics.DrawRectangle(new Pen(Brushes.LimeGreen, 3f), drag.Bounds);
				}
				else if (drag.Bounds.Contains(Form1.MouseLoc)
					&& Dragable.IsHeld
				    && drag.Platform != Dragable.HeldPlatform)
				{
					e.Graphics.FillRectangle(Brushes.LightGray, drag.Bounds);
					e.Graphics.DrawImage(drag.Image, drag.Bounds);
					e.Graphics.DrawRectangle(new Pen(Brushes.Gray, 3f), drag.Bounds);
				}
				else
				{
					e.Graphics.DrawImage(drag.Image, drag.Bounds);
				}
			}

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
			Rectangle drawRect = new Rectangle(Form1.MouseLoc.X, Form1.MouseLoc.Y, Form1.HOVER_SIZE, Form1.HOVER_SIZE);
			e.Graphics.DrawImage(_image, drawRect, 0, 0, _image.Width, _image.Height, GraphicsUnit.Pixel, imageAttributes);
		}
	}
}
