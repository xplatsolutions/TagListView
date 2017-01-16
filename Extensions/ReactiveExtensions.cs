using System;
using System.Reactive.Disposables;

namespace XplatSolutions
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
