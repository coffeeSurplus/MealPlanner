using MealPlanner.Source.Enums;
using MealPlanner.Source.MVVM;

namespace MealPlanner.Source.Models;

internal class ShoppingModel : MVVMBase
{
	private string title = string.Empty;
	private string quantity = string.Empty;
	private double cost = 0;
	private ShoppingCategory category = ShoppingCategory.Fresh;
	private bool bought = false;

	public string Title
	{
		get => title;
		set => SetValue(ref title, value);
	}
	public string Quantity
	{
		get => quantity;
		set => SetValue(ref quantity, value);
	}
	public double Cost
	{
		get => cost;
		set => SetValue(ref cost, value);
	}
	public ShoppingCategory Category
	{
		get => category;
		set => SetValue(ref category, value);
	}
	public bool Bought
	{
		get => bought;
		set => SetValue(ref bought, value);
	}
}