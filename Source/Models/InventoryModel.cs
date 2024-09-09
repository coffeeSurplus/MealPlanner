using MealPlanner.Source.Enums;
using MealPlanner.Source.MVVM;

namespace MealPlanner.Source.Models;

internal class InventoryModel : MVVMBase
{
	private string title = string.Empty;
	private string quantity = string.Empty;
	private DateOnly expiryDate = DateOnly.MaxValue;
	private InventoryCategory category = InventoryCategory.Fridge;
	private bool opened = false;

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
	public DateOnly ExpiryDate
	{
		get => expiryDate;
		set => SetValue(ref expiryDate, value);
	}
	public InventoryCategory Category
	{
		get => category;
		set => SetValue(ref category, value);
	}
	public bool Opened
	{
		get => opened;
		set => SetValue(ref opened, value);
	}
}