using System;
using System.ComponentModel;
using Foundation;
using UIKit;
using CoreGraphics;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace TagListView
{
	[Register("TagButton"), DesignTimeVisible(true)]
	public class TagButton : UIButton
	{
		readonly CompositeDisposable Disposables = new CompositeDisposable();
		UILongPressGestureRecognizer _longPressGestureRecognizer;

		public CloseButton RemoveButton { get; private set; } = new CloseButton();

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
				Layer.CornerRadius = _cornerRadius;
				Layer.MasksToBounds = _cornerRadius > 0;
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
				Layer.BorderWidth = _borderWidth;
				SetNeedsDisplay();
        	}
    	}

		UIColor _borderColor;
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
				ReloadStyles();
				SetNeedsDisplay();
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
				ReloadStyles();
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
				ReloadStyles();
				SetNeedsDisplay();
			}
		}

		float _paddingY = 2;
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
				var insets = TitleEdgeInsets;
				insets.Top = _paddingY;
				insets.Bottom = _paddingY;
				TitleEdgeInsets = insets;
				SetNeedsDisplay();
			}
		}

		float _paddingX = 5;
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
				var insets = TitleEdgeInsets;
				insets.Left = _paddingX;
				TitleEdgeInsets = insets;
				UpdateRightInsets();
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
				ReloadStyles();
				SetNeedsDisplay();
			}
		}

		UIColor _highlightedBackgroundColor;
		[Export("HighlightedBackgroundColor"), Browsable(true)]
		public UIColor HighlightedBackgroundColor
		{
			get
			{
				return _highlightedBackgroundColor;
			}

			set
			{
				_highlightedBackgroundColor = value;
				ReloadStyles();
				SetNeedsDisplay();
			}
		}

		UIColor _selectedBorderColor;
		[Export("SelectedBorderColor"), Browsable(true)]
		public UIColor SelectedBorderColor
		{
			get
			{
				return _selectedBorderColor;
			}

			set
			{
				_selectedBorderColor = value;
				ReloadStyles();
				SetNeedsDisplay();
			}
		}

		UIColor _selectedBackgroundColor;
		[Export("SelectedBackgroundColor"), Browsable(true)]
		public UIColor SelectedBackgroundColor
		{
			get
			{
				return _selectedBackgroundColor;
			}

			set
			{
				_selectedBackgroundColor = value;
				ReloadStyles();
				SetNeedsDisplay();
			}
		}

		UIFont _textFont = UIFont.SystemFontOfSize(12);
		public UIFont TextFont
		{
			get
			{
				return _textFont;
			}

			set
			{
				_textFont = value;
				TitleLabel.Font = _textFont;
				SetNeedsDisplay();
			}
		}

		bool _isHighlighted;
		public bool IsHighlighted
		{
			get
			{
				return _isHighlighted;
			}

			set
			{
				_isHighlighted = value;
				ReloadStyles();
				SetNeedsDisplay();
			}
		}

		bool _isSelected;
		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}

			set
			{
				_isSelected = value;
				ReloadStyles();
				SetNeedsDisplay();
			}
		}

		void ReloadStyles()
		{
			if (IsHighlighted)
			{
				if (HighlightedBackgroundColor != null)
				{
					BackgroundColor = HighlightedBackgroundColor;
				}
			}
			else if (IsSelected)
			{
				BackgroundColor = SelectedBackgroundColor ?? TagBackgroundColor;
				if (SelectedBorderColor != null ||
					BorderColor != null)
				{
					Layer.BorderColor = SelectedBorderColor.CGColor ?? BorderColor.CGColor;
				}
				SetTitleColor(SelectedTextColor, new UIControlState());
			}
			else 
			{
				BackgroundColor = TagBackgroundColor;
				if (BorderColor != null)
				{
					Layer.BorderColor = BorderColor.CGColor;
				}
				SetTitleColor(TextColor, new UIControlState());
			}
		}

		bool _enableRemoveButton;
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
				RemoveButton.Hidden = !_enableRemoveButton;
				UpdateRightInsets();
				RemoveButton.SetNeedsDisplay();
				SetNeedsDisplay();
			}
		}

		float _removeButtonIconSize = 12;
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
				RemoveButton.IconSize = _removeButtonIconSize;
				UpdateRightInsets();
				SetNeedsDisplay();
			}
		}

		float _removeIconLineWidth = 3;
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
				RemoveButton.LineWidth = _removeIconLineWidth;
				RemoveButton.SetNeedsDisplay();
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
				RemoveButton.LineColor = _removeIconLineColor;
				RemoveButton.SetNeedsDisplay();
				SetNeedsDisplay();
			}
		}

		readonly Subject<TagButton> _longPressedSubject = new Subject<TagButton>();
		public IObservable<TagButton> OnLongPressed
		{
			get
			{
				return _longPressedSubject;
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

		public TagButton(IntPtr handle) : base(handle)
		{
		}

		public TagButton(string title) : base(CGRect.Empty)
		{
			SetTitle(title, new UIControlState());
			SetupView();
		}

		[Export("initWithCoder:")]
		public TagButton(NSCoder coder) : base(coder)
		{
			SetupView();
		}

		void SetupView()
		{
			var frame = Frame;
			frame.Size = IntrinsicContentSize;
			Frame = frame;
			Add(RemoveButton);
			RemoveButton.TagButton = this;

			_longPressGestureRecognizer = new UILongPressGestureRecognizer(HandleLongPressAction);
			AddGestureRecognizer(_longPressGestureRecognizer);

			var tagViewTouchUpInsideObservable =
				Observable.FromEventPattern<EventHandler, EventArgs>(
					h => TouchUpInside += h,
					h => TouchUpInside -= h);

			var tagViewRemoveButtonTouchUpInsideObservable =
				Observable.FromEventPattern<EventHandler, EventArgs>(
					h => RemoveButton.TouchUpInside += h,
					h => RemoveButton.TouchUpInside -= h);

			tagViewTouchUpInsideObservable.Subscribe(args => 
			                                         _onTappedSubject.OnNext(new TagPressedArgs(args.Sender as TagButton, this)))
			                              .AddTo(Disposables);

			tagViewRemoveButtonTouchUpInsideObservable.Subscribe(args =>
			                                                     TagViewRemoveButtonTouchUpInsideEventHandler(args.Sender, args.EventArgs))
										  .AddTo(Disposables);
		}

		void TagViewRemoveButtonTouchUpInsideEventHandler(object sender, EventArgs e)
		{
			var closeButton = sender as CloseButton;
			if (closeButton != null &&
				closeButton.TagButton != null)
			{
				_onRemoveButtonTappedSubject.OnNext(new TagPressedArgs(closeButton.TagButton, this));
			}
		}

		void HandleLongPressAction(UILongPressGestureRecognizer sender)
		{
			_longPressedSubject.OnNext(this);
		}

		void UpdateRightInsets()
		{
			var insets = TitleEdgeInsets;
			if (EnableRemoveButton)
			{
				insets.Right = PaddingX + RemoveButtonIconSize + PaddingX;
			}
			else 
			{
				insets.Right = PaddingX;
			}
			TitleEdgeInsets = insets;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			if (EnableRemoveButton)
			{
				var removeButtonFrame = RemoveButton.Frame;
				var removeButtonFrameSize = removeButtonFrame.Size;
				removeButtonFrameSize.Width = PaddingX + RemoveButtonIconSize + PaddingX;
				removeButtonFrame.X = Frame.Width - RemoveButton.Frame.Width;
				removeButtonFrameSize.Height = Frame.Height;
				removeButtonFrame.Y = 0.0f;
				removeButtonFrame.Size = removeButtonFrameSize;
				RemoveButton.Frame = removeButtonFrame;
			}
		}

		public override CGSize IntrinsicContentSize
		{
			get
			{
				CGSize size;
				if (TitleLabel != null &&
				    !string.IsNullOrWhiteSpace(TitleLabel.Text))
				{
					size = TitleLabel.Text.StringSize(TextFont);
				}
				else
				{
					size = CGSize.Empty;
				}

				size.Height = TextFont.PointSize + PaddingY * 2;
				size.Width += PaddingX * 2;

				if (EnableRemoveButton) 
				{
					size.Width += RemoveButtonIconSize + PaddingX;
				}

				return size;
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			_longPressedSubject.Dispose();
			_onTappedSubject.Dispose();
			_onRemoveButtonTappedSubject.Dispose();
			RemoveGestureRecognizer(_longPressGestureRecognizer);
			_longPressGestureRecognizer.Dispose();
			RemoveButton.Dispose();
			Disposables.Dispose();
		}
	}
}