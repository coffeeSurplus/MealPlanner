using MealPlanner.Source.Enums;
using MealPlanner.Source.MVVM;
using System.Collections.ObjectModel;

namespace MealPlanner.Source.Models;

internal class MealModel : MVVMBase
{
	private string title = string.Empty;
	private DateOnly date = DateOnly.MaxValue;
	private MealCategory category = MealCategory.Breakfast;
	private ObservableCollection<IngredientModel> ingredients = [];

	public string Title
	{
		get => title;
		set => SetValue(ref title, value);
	}
	public DateOnly Date
	{
		get => date;
		set => SetValue(ref date, value);
	}
	public MealCategory Category
	{
		get => category;
		set => SetValue(ref category, value);
	}
	public ObservableCollection<IngredientModel> Ingredients
	{
		get => ingredients;
		set => SetValue(ref ingredients, value);
	}
}