using System.ComponentModel;
using System.Windows.Data;

namespace MealPlanner.Source.CollectionViews;

internal abstract class CollectionViewBase<T>
{
	private readonly CollectionViewSource collectionViewSource;

	public ICollectionView CollectionView { get => collectionViewSource.View; }

	public CollectionViewBase(List<T> list)
	{
		collectionViewSource = new() { Source = list };
		CollectionView.Filter = Filter;
	}

	public virtual void UpdateView() => CollectionView.Refresh();

	protected virtual bool Filter(object parameter) => true;
}