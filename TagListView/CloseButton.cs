using System;
using UIKit;
using Foundation;
using CoreGraphics;

namespace XplatSolutions
{
	[Register("CloseButton")]
    internal class CloseButton : UIButton
	{
		public float IconSize { get; set; } = 10.0f;
		public float LineWidth { get; set; } = 1.0f;
		public UIColor LineColor { get; set; } = UIColor.White.ColorWithAlpha(0.54f);
		public TagButton TagButton { get; set; }

		public CloseButton(IntPtr handle) : base(handle)
		{
		}

		public CloseButton()
		{
		}

		[Export("initWithCoder:")]
		public CloseButton(NSCoder coder) : base(coder)
		{
		}

		public override void Draw(CGRect rect)
		{
			var path = new UIBezierPath();
			path.LineWidth = LineWidth;
			path.LineCapStyle = CGLineCap.Round;


			var iconFrame = new CGRect(
					x: (rect.Width - IconSize) / 2.0,
					y: (rect.Height - IconSize) / 2.0,
					width: IconSize,
					height: IconSize
			);

			path.MoveTo(iconFrame.Location);
			path.AddLineTo(new CGPoint(iconFrame.GetMaxX(), iconFrame.GetMaxY()));
			path.MoveTo(new CGPoint(iconFrame.GetMaxX(), iconFrame.GetMinY()));
			path.AddLineTo(new CGPoint(iconFrame.GetMinX(), iconFrame.GetMaxY()));

			LineColor.SetStroke();
			path.Stroke();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			TagButton = null;
		}
	}
}
