using MealPlanner.Source.MVVM;

namespace MealPlanner.Source.Models;

internal class IngredientModel : MVVMBase
{
	private string title = string.Empty;
	private string? quantity = null;

	public string Title
	{
		get => title;
		set => SetValue(ref title, value);
	}
	public string? Quantity
	{
		get => quantity;
		set => SetValue(ref quantity, value);
	}
}