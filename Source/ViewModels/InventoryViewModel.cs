using MealPlanner.Source.CollectionViews;
using MealPlanner.Source.Data;
using MealPlanner.Source.Enums;
using MealPlanner.Source.Models;
using MealPlanner.Source.MVVM;

namespace MealPlanner.Source.ViewModels;

internal class InventoryViewModel : MVVMBase
{
	private readonly DataManager<InventoryModel> dataManager = new("Inventory");
	private readonly List<InventoryModel> inventoryList;
	private bool newInventory = true;
	private InventoryModel currentInventory = new();

	private int sortMode = 0;
	private bool orderMode = true;
	private bool inventoryEditorVisible = false;
	private int inventoryFridgeCount = 0;
	private int inventoryFreezerCount = 0;
	private int inventoryCupboardCount = 0;
	private int inventoryOtherCount = 0;
	private string currentInventoryTitle = string.Empty;
	private string currentInventoryQuantity = string.Empty;
	private string currentInventoryExpiryDate = string.Empty;
	private bool currentInventoryNoExpiry = false;
	private int currentInventoryCategory = 1;
	private bool currentInventoryOpened = false;

	public int SortMode
	{
		get => sortMode;
		set => SetValue(ref sortMode, value);
	}
	public bool OrderMode
	{
		get => orderMode;
		set => SetValue(ref orderMode, value);
	}
	public bool InventoryEditorVisible
	{
		get => inventoryEditorVisible;
		set => SetValue(ref inventoryEditorVisible, value);
	}
	public int InventoryFridgeCount
	{
		get => inventoryFridgeCount;
		set => SetValue(ref inventoryFridgeCount, value);
	}
	public int InventoryFreezerCount
	{
		get => inventoryFreezerCount;
		set => SetValue(ref inventoryFreezerCount, value);
	}
	public int InventoryCupboardCount
	{
		get => inventoryCupboardCount;
		set => SetValue(ref inventoryCupboardCount, value);
	}
	public int InventoryOtherCount
	{
		get => inventoryOtherCount;
		set => SetValue(ref inventoryOtherCount, value);
	}
	public string CurrentInventoryTitle
	{
		get => currentInventoryTitle;
		set => SetValue(ref currentInventoryTitle, value);
	}
	public string CurrentInventoryQuantity
	{
		get => currentInventoryQuantity;
		set => SetValue(ref currentInventoryQuantity, value);
	}
	public string CurrentInventoryExpiryDate
	{
		get => currentInventoryExpiryDate;
		set => SetValue(ref currentInventoryExpiryDate, value);
	}
	public bool CurrentInventoryNoExpiry
	{
		get => currentInventoryNoExpiry;
		set => SetValue(ref currentInventoryNoExpiry, value);
	}
	public int CurrentInventoryCategory
	{
		get => currentInventoryCategory;
		set => SetValue(ref currentInventoryCategory, value);
	}
	public bool CurrentInventoryOpened
	{
		get => currentInventoryOpened;
		set => SetValue(ref currentInventoryOpened, value);
	}

	public RelayCommand UpdateViewCommand { get; }
	public RelayCommand RemoveExpiredCommand { get; }
	public RelayCommand NewInventoryCommand { get; }
	public RelayCommand CancelEditInventoryCommand { get; }
	public RelayCommand SaveEditInventoryCommand { get; }
	public RelayCommand<InventoryModel> EditInventoryCommand { get; }
	public RelayCommand<InventoryModel> RemoveInventoryCommand { get; }

	public InventoryCollectionView InventoryFridgeCollectionView { get; }
	public InventoryCollectionView InventoryFreezerCollectionView { get; }
	public InventoryCollectionView InventoryCupboardCollectionView { get; }
	public InventoryCollectionView InventoryOtherCollectionView { get; }

	public InventoryViewModel()
	{
		inventoryList = dataManager.Data;
		InventoryFridgeCollectionView = new(inventoryList, InventoryCategory.Fridge);
		InventoryFreezerCollectionView = new(inventoryList, InventoryCategory.Freezer);
		InventoryCupboardCollectionView = new(inventoryList, InventoryCategory.Cupboard);
		InventoryOtherCollectionView = new(inventoryList, InventoryCategory.Other);
		UpdateViewCommand = new(UpdateView);
		RemoveExpiredCommand = new(RemoveExpired);
		NewInventoryCommand = new(NewInventory);
		CancelEditInventoryCommand = new(CancelEditInventory);
		SaveEditInventoryCommand = new(SaveEditInventory);
		EditInventoryCommand = new(EditInventory);
		RemoveInventoryCommand = new(RemoveInventory);
		UpdateView();
	}

	private void RemoveExpired()
	{
		List<string> items = [.. inventoryList.Where(x => x.ExpiryDate.ToRelativeDays() is 0).Select(x => x.Title)];
		inventoryList.RemoveAll(x => x.ExpiryDate.ToRelativeDays() is < 0);
		if (items.Count is > 0)
		{
			Popup.Message($"{items.Count} {(items.Count is not 1 ? "items" : "item")} removed:\n{string.Join("\n", items)}");
		}
		UpdateData();
	}
	private void NewInventory()
	{
		newInventory = true;
		currentInventory = new();
		CurrentInventoryTitle = string.Empty;
		CurrentInventoryQuantity = string.Empty;
		CurrentInventoryExpiryDate = string.Empty;
		CurrentInventoryNoExpiry = false;
		CurrentInventoryCategory = 1;
		CurrentInventoryOpened = false;
		InventoryEditorVisible = true;
	}
	private void CancelEditInventory() => InventoryEditorVisible = false;
	private void SaveEditInventory()
	{
		if (CheckInventoryValues())
		{
			currentInventory.Title = CurrentInventoryTitle.Trim().ToLower();
			currentInventory.Quantity = CurrentInventoryQuantity.Trim().ToLower();
			currentInventory.ExpiryDate = CurrentInventoryNoExpiry ? DateOnly.MaxValue : DateOnly.Parse(currentInventoryExpiryDate);
			currentInventory.Category = (InventoryCategory)CurrentInventoryCategory;
			currentInventory.Opened = CurrentInventoryOpened;
			if (newInventory)
			{
				inventoryList.Add(currentInventory);
			}
			InventoryEditorVisible = false;
			UpdateData();
		}
	}
	private void EditInventory(InventoryModel parameter)
	{
		newInventory = false;
		currentInventory = parameter;
		CurrentInventoryTitle = currentInventory.Title;
		CurrentInventoryQuantity = currentInventory.Quantity;
		CurrentInventoryExpiryDate = currentInventory.ExpiryDate != DateOnly.MaxValue ? $"{currentInventory.ExpiryDate:d/M}" : string.Empty;
		CurrentInventoryNoExpiry = currentInventory.ExpiryDate == DateOnly.MaxValue;
		CurrentInventoryCategory = (int)currentInventory.Category;
		CurrentInventoryOpened = currentInventory.Opened;
		InventoryEditorVisible = true;
	}
	private void RemoveInventory(InventoryModel parameter)
	{
		inventoryList.Remove(parameter);
		UpdateData();
	}

	private bool CheckInventoryValues()
	{
		if (!string.IsNullOrWhiteSpace(CurrentInventoryTitle))
		{
			if (!string.IsNullOrWhiteSpace(CurrentInventoryQuantity))
			{
				if ((DateOnly.TryParse(CurrentInventoryExpiryDate, out DateOnly newDate) && newDate.ToRelativeDays() is >= 0) || CurrentInventoryNoExpiry)
				{
					return true;
				}
				else
				{
					Popup.Message("Invalid date.");
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
		InventoryEditorVisible = true;
		return false;
	}
	private void UpdateData()
	{
		dataManager.UpdateData();
		UpdateView();
	}
	private void UpdateView()
	{
		InventoryFridgeCollectionView.UpdateView(SortMode, OrderMode);
		InventoryFreezerCollectionView.UpdateView(SortMode, OrderMode);
		InventoryCupboardCollectionView.UpdateView(SortMode, OrderMode);
		InventoryOtherCollectionView.UpdateView(SortMode, OrderMode);
		InventoryFridgeCount = inventoryList.Count(x => x.Category is InventoryCategory.Fridge);
		InventoryFreezerCount = inventoryList.Count(x => x.Category is InventoryCategory.Freezer);
		InventoryCupboardCount = inventoryList.Count(x => x.Category is InventoryCategory.Cupboard);
		InventoryOtherCount = inventoryList.Count(x => x.Category is InventoryCategory.Other);
	}
}