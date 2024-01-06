using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketLeagueConfigManager
{
	internal static class MouseHoverImageHandler
	{
		private static Image? _Image = null;
		public static Image? Image
		{
			get => _Image;
			set {
				_Image = value;
				Form1.UpdateImage();
				if (value == null) Form1.InvalidateHover();
			}
		}
		public static bool HasImage
		{
			get => _Image != null;
		}
	}
}
