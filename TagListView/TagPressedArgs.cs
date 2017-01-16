namespace TagListView
{
	public struct TagPressedArgs
	{
		public object Sender { get; private set; }
		public TagButton TagView { get; private set; }

		public string Title
		{
			get
			{
				return TagView?.CurrentTitle ?? "";
			}
		}

		public TagPressedArgs(TagButton tagView, object sender)
		{
			TagView = tagView;
			Sender = sender;
		}
	}
}