using System;
using System.Reactive.Disposables;
namespace TagListView
{
	internal static class ReactiveExtensions
	{
		public static void AddTo(this IDisposable disposable, CompositeDisposable compositeDisposables)
		{
			if (disposable != null &&
				compositeDisposables != null)
			{
				compositeDisposables.Add(disposable);
			}
		}
	}
}
