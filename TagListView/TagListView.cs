using System;
using UIKit;
using Foundation;
using System.ComponentModel;
using CoreGraphics;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using TagListView;

namespace TagListView
{
	[Register("TagListView"), DesignTimeVisible(true)]
	public partial class TagListView : UIView
	{
		readonly CompositeDisposable Disposables = new CompositeDisposable();

		public IList<TagButton> TagViews { get; private set; } = new List<TagButton>();

		IList<UIView> _tagBackgroundViews = new List<UIView>();
		IList<UIView> _rowViews = new List<UIView>();
		nfloat _tagViewHeight = 0.0f;

		int _rows = 0;
		int Rows
		{
			get
			{
				return _rows;
			}

			set
			{
				_rows = value;
				InvalidateIntrinsicContentSize();
			}
		}

		UIColor _textColor = UIColor.White;
		[Export("TextColor"), Browsable(true)]
		public UIColor TextColor
		{
			get
			{
				return _textColor;
			}

			set
			{
				_textColor = value;
				foreach (var tagView in TagViews)
				{
					tagView.TextColor = _textColor;
				}
				SetNeedsDisplay();
			}
		}

		UIColor _selectedTextColor = UIColor.White;
		[Export("SelectedTextColor"), Browsable(true)]
		public UIColor SelectedTextColor
		{
			get
			{
				return _selectedTextColor;
			}

			set
			{
				_selectedTextColor = value;
				foreach (var tagView in TagViews)
				{
					tagView.SelectedTextColor = _selectedTextColor;
				}
				SetNeedsDisplay();
			}
		}

		UIColor _tagBackgroundColor = UIColor.Gray;
		[Export("TagBackgroundColor"), Browsable(true)]
		public UIColor TagBackgroundColor
		{
			get
			{
				return _tagBackgroundColor;
			}

			set
			{
				_tagBackgroundColor = value;
				foreach (var tagView in TagViews)
				{
					tagView.TagBackgroundColor = _tagBackgroundColor;
				}
				SetNeedsDisplay();
			}
		}

		UIColor _tagHighlightedBackgroundColor;
		[Export("TagHighlightedBackgroundColor"), Browsable(true)]
		public UIColor TagHighlightedBackgroundColor
		{
			get
			{
				return _tagHighlightedBackgroundColor;
			}

			set
			{
				_tagHighlightedBackgroundColor = value;
				foreach (var tagView in TagViews)
				{
					tagView.HighlightedBackgroundColor = _tagHighlightedBackgroundColor;
				}
				SetNeedsDisplay();
			}
		}

		UIColor _tagSelectedBackgroundColor;
		[Export("TagSelectedBackgroundColor"), Browsable(true)]
		public UIColor TagSelectedBackgroundColor
		{
			get
			{
				return _tagSelectedBackgroundColor;
			}

			set
			{
				_tagSelectedBackgroundColor = value;
				foreach (var tagView in TagViews)
				{
					tagView.SelectedBackgroundColor = _tagSelectedBackgroundColor;
				}
				SetNeedsDisplay();
			}
		}

		float _cornerRadius = 0;
		[Export("CornerRadius"), Browsable(true)]
		public float CornerRadius
		{
			get
			{
				return _cornerRadius;
			}

			set
			{
				_cornerRadius = value;
				foreach (var tagView in TagViews)
				{
					tagView.CornerRadius = _cornerRadius;
				}
				SetNeedsDisplay();
			}
		}

		float _borderWidth = 0;
		[Export("BorderWidth"), Browsable(true)]
		public float BorderWidth
		{
			get
			{
				return _borderWidth;
			}

			set
			{
				_borderWidth = value;
				foreach (var tagView in TagViews)
				{
					tagView.BorderWidth = _borderWidth;
				}
				SetNeedsDisplay();
			}
		}

		UIColor _borderColor = UIColor.Clear;
		[Export("BorderColor"), Browsable(true)]
		public UIColor BorderColor
		{
			get
			{
				return _borderColor;
			}

			set
			{
				_borderColor = value;
				foreach (var tagView in TagViews)
				{
					tagView.BorderColor = _borderColor;
				}
				SetNeedsDisplay();
			}
		}

		UIColor _selectedBorderColor = UIColor.Clear;
		[Export("SelectedBorderColor"), Browsable(true)]
		public UIColor SelectedBorderColor
		{
			get
			{
				return _borderColor;
			}

			set
			{
				_borderColor = value;
				foreach (var tagView in TagViews)
				{
					tagView.SelectedBorderColor = _selectedBorderColor;
				}
				SetNeedsDisplay();
			}
		}

		float _paddingY = 2f;
		[Export("PaddingY"), Browsable(true)]
		public float PaddingY
		{
			get
			{
				return _paddingY;
			}

			set
			{
				_paddingY = value;
				foreach (var tagView in TagViews)
				{
					tagView.PaddingY = _paddingY;
				}
				RearrangeViews();
				SetNeedsDisplay();
			}
		}

		float _paddingX = 5f;
		[Export("PaddingX"), Browsable(true)]
		public float PaddingX
		{
			get
			{
				return _paddingX;
			}

			set
			{
				_paddingX = value;
				foreach (var tagView in TagViews)
				{
					tagView.PaddingX = _paddingX;
				}
				RearrangeViews();
				SetNeedsDisplay();
			}
		}

		float _marginY = 2f;
		[Export("MarginY"), Browsable(true)]
		public float MarginY
		{
			get
			{
				return _marginY;
			}

			set
			{
				_marginY = value;
				RearrangeViews();
				SetNeedsDisplay();
			}
		}

		float _marginX = 5f;
		[Export("MarginX"), Browsable(true)]
		public float MarginX
		{
			get
			{
				return _marginX;
			}

			set
			{
				_marginX = value;
				RearrangeViews();
				SetNeedsDisplay();
			}
		}

		TagListViewAlignment _alignment = TagListViewAlignment.Left;
		[Export("Alignment"), Browsable(true)]
		public TagListViewAlignment Alignment
		{
			get
			{
				return _alignment;
			}

			set
			{
				_alignment = value;
				RearrangeViews();
				SetNeedsDisplay();
			}
		}

		UIColor _shadowColor = UIColor.Clear;
		[Export("ShadowColor"), Browsable(true)]
		public UIColor ShadowColor
		{
			get
			{
				return _shadowColor;
			}

			set
			{
				_shadowColor = value;
				RearrangeViews();
				SetNeedsDisplay();
			}
		}

		float _shadowRadius = 0f;
		[Export("ShadowRadius"), Browsable(true)]
		public float ShadowRadius
		{
			get
			{
				return _shadowRadius;
			}

			set
			{
				_shadowRadius = value;
				RearrangeViews();
				SetNeedsDisplay();
			}
		}

		CGSize _shadowOffset = CGSize.Empty;
		[Export("ShadowOffset"), Browsable(true)]
		public CGSize ShadowOffset
		{
			get
			{
				return _shadowOffset;
			}

			set
			{
				_shadowOffset = value;
				RearrangeViews();
				SetNeedsDisplay();
			}
		}

		float _shadowOpacity = 0f;
		[Export("ShadowOpacity"), Browsable(true)]
		public float ShadowOpacity
		{
			get
			{
				return _shadowOpacity;
			}

			set
			{
				_shadowOpacity = value;
				RearrangeViews();
				SetNeedsDisplay();
			}
		}

		bool _enableRemoveButton = false;
		[Export("EnableRemoveButton"), Browsable(true)]
		public bool EnableRemoveButton
		{
			get
			{
				return _enableRemoveButton;
			}

			set
			{
				_enableRemoveButton = value;
				foreach (var tagView in TagViews)
				{
					tagView.EnableRemoveButton = _enableRemoveButton;
				}
				RearrangeViews();
				SetNeedsDisplay();
			}
		}

		float _removeButtonIconSize = 12f;
		[Export("RemoveButtonIconSize"), Browsable(true)]
		public float RemoveButtonIconSize
		{
			get
			{
				return _removeButtonIconSize;
			}

			set
			{
				_removeButtonIconSize = value;
				foreach (var tagView in TagViews)
				{
					tagView.RemoveButtonIconSize = _removeButtonIconSize;
				}
				RearrangeViews();
				SetNeedsDisplay();
			}
		}

		float _removeIconLineWidth = 1f;
		[Export("RemoveIconLineWidth"), Browsable(true)]
		public float RemoveIconLineWidth
		{
			get
			{
				return _removeIconLineWidth;
			}

			set
			{
				_removeIconLineWidth = value;
				foreach (var tagView in TagViews)
				{
					tagView.RemoveIconLineWidth = _removeIconLineWidth;
				}
				RearrangeViews();
				SetNeedsDisplay();
			}
		}

		UIColor _removeIconLineColor = UIColor.White.ColorWithAlpha(0.54f);
		[Export("RemoveIconLineColor"), Browsable(true)]
		public UIColor RemoveIconLineColor
		{
			get
			{
				return _removeIconLineColor;
			}

			set
			{
				_removeIconLineColor = value;
				foreach (var tagView in TagViews)
				{
					tagView.RemoveIconLineWidth = _removeIconLineWidth;
				}
				RearrangeViews();
				SetNeedsDisplay();
			}
		}

		UIFont _textFont = UIFont.SystemFontOfSize(12);
		[Export("TextFont"), Browsable(true)]
		public UIFont TextFont
		{
			get
			{
				return _textFont;
			}

			set
			{
				_textFont = value;
				foreach (var tagView in TagViews)
				{
					tagView.TextFont = _textFont;
				}
				RearrangeViews();
				SetNeedsDisplay();
			}
		}

		readonly Subject<TagPressedArgs> _onRemoveButtonTappedSubject = new Subject<TagPressedArgs>();
		public IObservable<TagPressedArgs> OnRemoveButtonTapped
		{
			get
			{
				return _onRemoveButtonTappedSubject;
			}
		}

		readonly Subject<TagPressedArgs> _onTappedSubject = new Subject<TagPressedArgs>();
		public IObservable<TagPressedArgs> OnTapped
		{
			get
			{
				return _onTappedSubject;
			}
		}

		public TagListView(IntPtr handle) : base(handle)
		{
		}

		public override void PrepareForInterfaceBuilder()
		{
			AddTag("Welcome");
			AddTag("to");
			AddTag("TagListView").IsSelected = true;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			RearrangeViews();
		}

		void RearrangeViews()
		{
			var views = TagViews.Cast<UIView>().Concat(_tagBackgroundViews).Concat(_rowViews);

			foreach (var view in views)
			{
				view.RemoveFromSuperview();
			}

			_rowViews.Clear();

			var currentRow = 0;
			UIView currentRowView = new UIView { UserInteractionEnabled = true };
			var currentRowTagCount = 0;
			nfloat currentRowWidth = 0;

			for (int i = 0; i < TagViews.Count(); i++)
			{
				var tagView = TagViews[i];

				var tagViewFrame = tagView.Frame;
				tagViewFrame.Size = tagView.IntrinsicContentSize;
				tagView.Frame = tagViewFrame;
				_tagViewHeight = tagView.Frame.Height;

				if (currentRowTagCount == 0 ||
					currentRowWidth + tagView.Frame.Width > Frame.Width)
				{
					currentRow += 1;
					currentRowWidth = 0;
					currentRowTagCount = 0;
					currentRowView = new UIView { UserInteractionEnabled = true };
					var currentRowViewFrame = currentRowView.Frame;
					currentRowViewFrame.Y = Convert.ToSingle((currentRow - 1) * (_tagViewHeight + MarginY));
					currentRowView.Frame = currentRowViewFrame;
					_rowViews.Add(currentRowView);
					Add(currentRowView);
				}

				var tagBackgroundView = _tagBackgroundViews[i];
				var tagBackgroundViewFrame = tagBackgroundView.Frame;
				tagBackgroundViewFrame.Location = new CGPoint(currentRowWidth, 0);
				tagBackgroundViewFrame.Size = tagView.Bounds.Size;
				tagBackgroundView.Frame = tagBackgroundViewFrame;

				tagBackgroundView.Layer.ShadowColor = ShadowColor.CGColor;
				tagBackgroundView.Layer.ShadowPath = UIBezierPath.FromRoundedRect(tagBackgroundView.Bounds, CornerRadius).CGPath;
				tagBackgroundView.Layer.ShadowOffset = ShadowOffset;
				tagBackgroundView.Layer.ShadowOpacity = ShadowOpacity;
				tagBackgroundView.Layer.ShadowRadius = ShadowRadius;

				tagBackgroundView.Add(tagView);
				currentRowView.Add(tagBackgroundView);

				currentRowTagCount += 1;
				currentRowWidth += tagView.Frame.Width + MarginX;

				var currentRowViewFrameForAlignment = currentRowView.Frame;
				switch (Alignment)
				{
					case TagListViewAlignment.Left:
						currentRowViewFrameForAlignment.X = 0;
						break;
					case TagListViewAlignment.Center:
						currentRowViewFrameForAlignment.X = (Frame.Width - (currentRowWidth - MarginX)) / 2;
						break;
					case TagListViewAlignment.Right:
						currentRowViewFrameForAlignment.X = Frame.Width - (currentRowWidth - MarginX);
						break;
				}

				var currentRowViewFrameForAlignmentSize = currentRowViewFrameForAlignment.Size;
				currentRowViewFrameForAlignmentSize.Width = currentRowWidth;
				currentRowViewFrameForAlignmentSize.Height = Convert.ToSingle(Math.Max(_tagViewHeight, currentRowView.Frame.Height));
				currentRowViewFrameForAlignment.Size = currentRowViewFrameForAlignmentSize;
				currentRowView.Frame = currentRowViewFrameForAlignment;
			}

			Rows = currentRow;
			InvalidateIntrinsicContentSize();
		}

		TagButton CreateNewTagView(string title)
		{
			var tagView = new TagButton(title);

			tagView.TextColor = TextColor;
			tagView.SelectedTextColor = SelectedTextColor;
			tagView.TagBackgroundColor = TagBackgroundColor;
			tagView.HighlightedBackgroundColor = TagHighlightedBackgroundColor;
			tagView.SelectedBackgroundColor = TagSelectedBackgroundColor;
			tagView.CornerRadius = CornerRadius;
			tagView.BorderWidth = BorderWidth;
			tagView.BorderColor = BorderColor;
			tagView.SelectedBorderColor = SelectedBorderColor;
			tagView.PaddingX = PaddingX;
			tagView.PaddingY = PaddingY;
			tagView.TextFont = TextFont;
			tagView.RemoveIconLineWidth = RemoveIconLineWidth;
			tagView.RemoveButtonIconSize = RemoveButtonIconSize;
			tagView.EnableRemoveButton = EnableRemoveButton;
			tagView.RemoveIconLineColor = RemoveIconLineColor;

			tagView.OnTapped.Subscribe(args => _onTappedSubject.OnNext(args)).AddTo(Disposables);
			tagView.OnRemoveButtonTapped.Subscribe(args => _onRemoveButtonTappedSubject.OnNext(args)).AddTo(Disposables);
			tagView.OnLongPressed.Subscribe(tagButton =>
			{
				foreach (var tag in TagViews)
				{
					tag.IsSelected = (tag == tagButton);
				}
			}).AddTo(Disposables);

			return tagView;
		}

		public TagButton AddTag(string title)
		{
			return AddTagView(CreateNewTagView(title));
		}

		public IEnumerable<TagButton> AddTags(IEnumerable<string> titles)
		{
			var tagViews = new List<TagButton>();
			foreach (var title in titles)
			{
				tagViews.Add(CreateNewTagView(title));
			}
			return AddTagViews(tagViews);
		}

		public IEnumerable<TagButton> AddTagViews(IEnumerable<TagButton> tagViews)
		{
			foreach (var tagView in tagViews)
			{
				TagViews.Add(tagView);
				_tagBackgroundViews.Add(new UIView(frame: tagView.Bounds));
			}

			RearrangeViews();
			return tagViews;
		}

		public TagButton InsertTag(string title, int index)
		{
			return InsertTagView(CreateNewTagView(title), index);
		}

		public TagButton AddTagView(TagButton tagView)
		{
			TagViews.Add(tagView);
			_tagBackgroundViews.Add(new UIView(tagView.Bounds));
			RearrangeViews();
			return tagView;
		}

		public TagButton InsertTagView(TagButton tagView, int index)
		{
			TagViews.Insert(index, tagView);
			_tagBackgroundViews.Insert(index, new UIView(tagView.Bounds));
			RearrangeViews();
			return tagView;
		}

		public void RemoveTag(string title)
		{
			RemoveTagView(TagViews.FirstOrDefault(p => p.TitleLabel.Text.Equals(title)));
		}

		public void RemoveTagView(TagButton tagView)
		{
			tagView.RemoveFromSuperview();
			var index = TagViews.IndexOf(tagView);
			if (index > -1)
			{
				TagViews.RemoveAt(index);
				_tagBackgroundViews.RemoveAt(index);
			}
			RearrangeViews();
		}

		public void RemoveAllTags()
		{
			var views = TagViews.Cast<UIView>().Concat(_tagBackgroundViews);
			foreach (var view in views)
			{
				view.RemoveFromSuperview();
			}
			TagViews = new List<TagButton>();
			_tagBackgroundViews = new List<UIView>();
			RearrangeViews();
		}

		public IEnumerable<TagButton> SelectedTags()
		{
			return TagViews.Where(p => p.IsSelected);
		}

		public override CGSize IntrinsicContentSize
		{
			get
			{
				var height = Convert.ToSingle(Rows * (_tagViewHeight + MarginY));

				if (Rows > 0)
				{
					height -= MarginY;

				}
				return new CGSize(Frame.Width, height);
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			Disposables.Dispose();
			foreach (var tag in TagViews)
			{
				tag.Dispose();
			}
		}
	}
}