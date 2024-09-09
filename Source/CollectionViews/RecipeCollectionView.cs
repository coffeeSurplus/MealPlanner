using MealPlanner.Source.Converters;
using MealPlanner.Source.Models;
using System.ComponentModel;
using System.Windows.Data;

namespace MealPlanner.Source.CollectionViews;

internal class RecipeCollectionView : CollectionViewBase<RecipeModel>
{
	public RecipeCollectionView(List<RecipeModel> list) : base(list)
	{
		CollectionView.GroupDescriptions.Add(new PropertyGroupDescription("Category", new CollectionViewEnumConverter()));
		CollectionView.SortDescriptions.Add(new("Category", ListSortDirection.Ascending));
		CollectionView.SortDescriptions.Add(new("Title", ListSortDirection.Ascending));
	}
}