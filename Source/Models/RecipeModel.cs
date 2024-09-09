using MealPlanner.Source.MVVM;
using System.Collections.ObjectModel;

namespace MealPlanner.Source.Models;

internal class RecipeModel : MVVMBase
{
	private string title = string.Empty;
	private string category = string.Empty;
	private TimeSpan preparationTime = TimeSpan.Zero;
	private TimeSpan cookingTime = TimeSpan.Zero;
	private int serves = 0;
	private ObservableCollection<IngredientModel> ingredients = [];
	private ObservableCollection<MethodModel> method = [];

	public string Title
	{
		get => title;
		set => SetValue(ref title, value);
	}
	public string Category
	{
		get => category;
		set => SetValue(ref category, value);
	}
	public TimeSpan PreparationTime
	{
		get => preparationTime;
		set => SetValue(ref preparationTime, value);
	}
	public TimeSpan CookingTime
	{
		get => cookingTime;
		set => SetValue(ref cookingTime, value);
	}
	public int Serves
	{
		get => serves;
		set => SetValue(ref serves, value);
	}
	public ObservableCollection<IngredientModel> Ingredients
	{
		get => ingredients;
		set => SetValue(ref ingredients, value);
	}
	public ObservableCollection<MethodModel> Method
	{
		get => method;
		set => SetValue(ref method, value);
	}
}