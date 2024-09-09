using MealPlanner.Source.Converters;
using MealPlanner.Source.Data;
using MealPlanner.Source.Models;
using System.ComponentModel;
using System.Windows.Data;

namespace MealPlanner.Source.CollectionViews;

internal class MealCollectionView : CollectionViewBase<MealModel>
{
	private readonly int min;
	private readonly int max;

	public MealCollectionView(List<MealModel> list, int min, int max, bool dayView) : base(list)
	{
		this.min = min;
		this.max = max;
		CollectionView.SortDescriptions.Add(new("Date", ListSortDirection.Ascending));
		CollectionView.SortDescriptions.Add(new("Category", ListSortDirection.Ascending));
		CollectionView.GroupDescriptions.Add(new PropertyGroupDescription(dayView ? "Category" : "Date", dayView ? new CollectionViewEnumConverter() : new CollectionViewMealDateConverter()));
	}

	protected override bool Filter(object parameter) => parameter is MealModel mealModel && mealModel.Date.ToRelativeDays() >= min && mealModel.Date.ToRelativeDays() < max;
}