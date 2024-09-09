using MealPlanner.Source.Enums;
using MealPlanner.Source.Models;
using System.ComponentModel;

namespace MealPlanner.Source.CollectionViews;

internal class InventoryCollectionView(List<InventoryModel> list, InventoryCategory category) : CollectionViewBase<InventoryModel>(list)
{
	private readonly InventoryCategory category = category;

	public void UpdateView(int sortMode, bool orderMode)
	{
		CollectionView.SortDescriptions.Clear();
		CollectionView.SortDescriptions.Add(new(sortMode switch { 0 => "Title", 1 => "ExpiryDate", _ => "Opened" }, orderMode != (sortMode is 2) ? ListSortDirection.Ascending : ListSortDirection.Descending));
		base.UpdateView();
	}

	protected override bool Filter(object parameter) => parameter is InventoryModel inventoryModel && inventoryModel.Category == category;
}