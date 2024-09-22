using MealPlanner.Source.Converters;
using MealPlanner.Source.Data;
using MealPlanner.Source.Models;
using System.ComponentModel;
using System.Windows.Data;

namespace MealPlanner.Source.CollectionViews;

internal class MealCollectionView : CollectionViewBase<MealModel>
{
	private readonly bool dayView;
	private readonly int week;

	public MealCollectionView(List<MealModel> list, int week, bool dayView) : base(list)
	{
		this.dayView = dayView;
		this.week = week;
		CollectionView.SortDescriptions.Add(new("Date", ListSortDirection.Ascending));
		CollectionView.SortDescriptions.Add(new("Category", ListSortDirection.Ascending));
		CollectionView.GroupDescriptions.Add(new PropertyGroupDescription(dayView ? "Category" : "Date", dayView ? new CollectionViewEnumConverter() : new CollectionViewMealDateConverter()));
	}

	protected override bool Filter(object parameter) => parameter is MealModel mealModel && (dayView ? mealModel.Date.ToRelativeDays() is 0 : mealModel.Date.FirstDayOfWeek().ToRelativeWeeks() == week && mealModel.Date.ToRelativeDays() is >= 0);
}