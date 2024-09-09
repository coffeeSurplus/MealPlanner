using MealPlanner.Source.Converters;
using MealPlanner.Source.Models;
using System.ComponentModel;
using System.Windows.Data;

namespace MealPlanner.Source.CollectionViews;

internal class ShoppingCollectionView(List<ShoppingModel> list) : CollectionViewBase<ShoppingModel>(list)
{
	private bool showBought = true;

	public void UpdateView(bool sortMode, bool orderMode, bool showBought)
	{
		CollectionView.GroupDescriptions.Clear();
		if (sortMode)
		{
			CollectionView.GroupDescriptions.Add(new PropertyGroupDescription("Category", new CollectionViewEnumConverter()));
		}
		CollectionView.SortDescriptions.Clear();
		CollectionView.SortDescriptions.Add(new(sortMode ? "Category" : "Cost", orderMode ? ListSortDirection.Ascending : ListSortDirection.Descending));
		CollectionView.SortDescriptions.Add(new("Title", orderMode ? ListSortDirection.Ascending : ListSortDirection.Descending));
		this.showBought = showBought;
		base.UpdateView();
	}

	protected override bool Filter(object parameter) => showBought || parameter is ShoppingModel shoppingModel && !shoppingModel.Bought;
}