using MealPlanner.Source.CollectionViews;
using MealPlanner.Source.Data;
using MealPlanner.Source.Enums;
using MealPlanner.Source.Models;
using MealPlanner.Source.MVVM;

namespace MealPlanner.Source.ViewModels;

internal class ShoppingViewModel : MVVMBase
{
	private readonly DataManager<ShoppingModel> dataManager = new("Shopping");
	private readonly List<ShoppingModel> shoppingList;
	private bool newShopping = true;
	private ShoppingModel currentShopping = new();

	private bool sortMode = true;
	private bool orderMode = true;
	private bool showBought = true;
	private bool shoppingEditorVisible = false;
	private bool defaultMessageVisible = false;
	private double shoppingCost = 0;
	private string currentShoppingTitle = string.Empty;
	private string currentShoppingQuantity = string.Empty;
	private string currentShoppingCost = string.Empty;
	private int currentShoppingCategory = 1;

	public bool SortMode
	{
		get => sortMode;
		set => SetValue(ref sortMode, value);
	}
	public bool OrderMode
	{
		get => orderMode;
		set => SetValue(ref orderMode, value);
	}
	public bool ShowBought
	{
		get => showBought;
		set => SetValue(ref showBought, value);
	}
	public bool ShoppingEditorVisible
	{
		get => shoppingEditorVisible;
		set => SetValue(ref shoppingEditorVisible, value);
	}
	public bool DefaultMessageVisible
	{
		get => defaultMessageVisible;
		set => SetValue(ref defaultMessageVisible, value);
	}
	public double ShoppingCost
	{
		get => shoppingCost;
		set => SetValue(ref shoppingCost, value);
	}
	public string CurrentShoppingTitle
	{
		get => currentShoppingTitle;
		set => SetValue(ref currentShoppingTitle, value);
	}
	public string CurrentShoppingQuantity
	{
		get => currentShoppingQuantity;
		set => SetValue(ref currentShoppingQuantity, value);
	}
	public string CurrentShoppingCost
	{
		get => currentShoppingCost;
		set => SetValue(ref currentShoppingCost, value);
	}
	public int CurrentShoppingCategory
	{
		get => currentShoppingCategory;
		set => SetValue(ref currentShoppingCategory, value);
	}

	public RelayCommand UpdateDataCommand { get; }
	public RelayCommand UpdateViewCommand { get; }
	public RelayCommand RemoveBoughtCommand { get; }
	public RelayCommand NewShoppingCommand { get; }
	public RelayCommand CancelEditShoppingCommand { get; }
	public RelayCommand SaveEditShoppingCommand { get; }
	public RelayCommand<ShoppingModel> EditShoppingCommand { get; }
	public RelayCommand<ShoppingModel> RemoveShoppingCommand { get; }

	public ShoppingCollectionView ShoppingCollectionView { get; }

	public ShoppingViewModel()
	{
		shoppingList = dataManager.Data;
		ShoppingCollectionView = new(shoppingList);
		UpdateDataCommand = new(UpdateData);
		UpdateViewCommand = new(UpdateView);
		RemoveBoughtCommand = new(RemoveBought);
		NewShoppingCommand = new(NewShopping);
		CancelEditShoppingCommand = new(CancelEditShopping);
		SaveEditShoppingCommand = new(SaveEditShopping);
		EditShoppingCommand = new(EditShopping);
		RemoveShoppingCommand = new(RemoveShopping);
		UpdateView();
	}

	private void RemoveBought()
	{
		List<string> items = [.. shoppingList.Where(x => x.Bought).Select(x => x.Title)];
		shoppingList.RemoveAll(x => x.Bought);
		if (items.Count is > 0)
		{
			Popup.Message($"{items.Count} {(items.Count is not 1 ? "items" : "item")} removed:\n{string.Join("\n", items)}");
		}
		UpdateData();
	}
	private void NewShopping()
	{
		newShopping = true;
		currentShopping = new();
		CurrentShoppingTitle = string.Empty;
		CurrentShoppingQuantity = string.Empty;
		CurrentShoppingCost = string.Empty;
		CurrentShoppingCategory = 1;
		ShoppingEditorVisible = true;
	}
	private void CancelEditShopping() => ShoppingEditorVisible = false;
	private void SaveEditShopping()
	{
		if (CheckShoppingValues())
		{
			currentShopping.Title = CurrentShoppingTitle.ToLower();
			currentShopping.Quantity = CurrentShoppingQuantity.ToLower();
			currentShopping.Cost = double.Parse(CurrentShoppingCost);
			currentShopping.Category = (ShoppingCategory)CurrentShoppingCategory;
			if (newShopping)
			{
				shoppingList.Add(currentShopping);
			}
			ShoppingEditorVisible = false;
			UpdateData();
		}
	}
	private void EditShopping(ShoppingModel parameter)
	{
		newShopping = false;
		currentShopping = parameter;
		CurrentShoppingTitle = currentShopping.Title;
		CurrentShoppingQuantity = currentShopping.Quantity;
		CurrentShoppingCost = $"{currentShopping.Cost:N2}";
		CurrentShoppingCategory = (int)currentShopping.Category;
		ShoppingEditorVisible = true;
	}
	private void RemoveShopping(ShoppingModel parameter)
	{
		shoppingList.Remove(parameter);
		UpdateData();
	}

	private bool CheckShoppingValues()
	{
		if (!string.IsNullOrWhiteSpace(CurrentShoppingTitle))
		{
			if (!string.IsNullOrWhiteSpace(CurrentShoppingQuantity))
			{
				if (double.TryParse(CurrentShoppingCost, out double newCost) && newCost is > 0)
				{
					return true;
				}
				else
				{
					Popup.Message("Invalid cost.");
				}

			}
			else
			{
				Popup.Message("Invalid quantity.");
			}

		}
		else
		{
			Popup.Message("Invalid title.");
		}
		ShoppingEditorVisible = true;
		return false;
	}
	private void UpdateData()
	{
		dataManager.UpdateData();
		UpdateView();
	}
	private void UpdateView()
	{
		ShoppingCollectionView.UpdateView(SortMode, OrderMode, ShowBought);
		DefaultMessageVisible = shoppingList.Count is 0;
		ShoppingCost = shoppingList.Where(x => !x.Bought).Sum(x => x.Cost);
	}
}