using MealPlanner.Source.CollectionViews;
using MealPlanner.Source.Data;
using MealPlanner.Source.Enums;
using MealPlanner.Source.Models;
using MealPlanner.Source.MVVM;

namespace MealPlanner.Source.ViewModels;

internal class MealViewModel : MVVMBase
{
	private readonly DataManager<MealModel> dataManager = new("Meal");
	private readonly List<MealModel> mealList;
	private bool newMeal = true;
	private bool newIngredient = true;
	private MealModel currentMeal = new();
	private IngredientModel currentIngredient = new();

	private int currentPageNumber = 0;
	private bool mealEditorVisible = false;
	private bool ingredientEditorVisible = false;
	private bool todayDefaultMessageVisible = false;
	private bool thisWeekDefaultMessageVisible = false;
	private bool nextWeekDefaultMessageVisible = false;
	private string currentMealTitle = string.Empty;
	private string currentMealDate = string.Empty;
	private int currentMealCategory = 1;
	private string currentIngredientTitle = string.Empty;
	private string currentIngredientQuantity = string.Empty;

	public int CurrentPageNumber
	{
		get => currentPageNumber;
		set => SetValue(ref currentPageNumber, value);
	}
	public bool MealEditorVisible
	{
		get => mealEditorVisible;
		set => SetValue(ref mealEditorVisible, value);
	}
	public bool IngredientEditorVisible
	{
		get => ingredientEditorVisible;
		set => SetValue(ref ingredientEditorVisible, value);
	}
	public bool TodayDefaultMessageVisible
	{
		get => todayDefaultMessageVisible;
		set => SetValue(ref todayDefaultMessageVisible, value);
	}
	public bool ThisWeekDefaultMessageVisible
	{
		get => thisWeekDefaultMessageVisible;
		set => SetValue(ref thisWeekDefaultMessageVisible, value);
	}
	public bool NextWeekDefaultMessageVisible
	{
		get => nextWeekDefaultMessageVisible;
		set => SetValue(ref nextWeekDefaultMessageVisible, value);
	}
	public string CurrentMealTitle
	{
		get => currentMealTitle;
		set => SetValue(ref currentMealTitle, value);
	}
	public string CurrentMealDate
	{
		get => currentMealDate;
		set => SetValue(ref currentMealDate, value);
	}
	public int CurrentMealCategory
	{
		get => currentMealCategory;
		set => SetValue(ref currentMealCategory, value);
	}
	public string CurrentIngredientTitle
	{
		get => currentIngredientTitle;
		set => SetValue(ref currentIngredientTitle, value);
	}
	public string CurrentIngredientQuantity
	{
		get => currentIngredientQuantity;
		set => SetValue(ref currentIngredientQuantity, value);
	}

	public RelayCommand NewMealCommand { get; }
	public RelayCommand CancelEditMealCommand { get; }
	public RelayCommand SaveEditMealCommand { get; }
	public RelayCommand CancelEditIngredientCommand { get; }
	public RelayCommand SaveEditIngredientCommand { get; }
	public RelayCommand<MealModel> AddIngredientCommand { get; }
	public RelayCommand<MealModel> EditMealCommand { get; }
	public RelayCommand<MealModel> RemoveMealCommand { get; }
	public RelayCommand<IngredientModel> EditIngredientCommand { get; }
	public RelayCommand<(MealModel Meal, IngredientModel Ingredient)> RemoveIngredientCommand { get; }

	public MealCollectionView MealTodayCollectionView { get; }
	public MealCollectionView MealThisWeekCollectionView { get; }
	public MealCollectionView MealNextWeekCollectionView { get; }

	public MealViewModel()
	{
		mealList = dataManager.Data;
		MealTodayCollectionView = new(mealList, 0, true);
		MealThisWeekCollectionView = new(mealList, 0, false);
		MealNextWeekCollectionView = new(mealList, 1, false);
		NewMealCommand = new(NewMeal);
		CancelEditMealCommand = new(CancelEditMeal);
		SaveEditMealCommand = new(SaveEditMeal);
		CancelEditIngredientCommand = new(CancelEditIngredient);
		SaveEditIngredientCommand = new(SaveEditIngredient);
		AddIngredientCommand = new(AddIngredient);
		EditMealCommand = new(EditMeal);
		RemoveMealCommand = new(RemoveMeal);
		EditIngredientCommand = new(EditIngredient);
		RemoveIngredientCommand = new(RemoveIngredient);
		UpdateData();
	}

	private void NewMeal()
	{
		newMeal = true;
		currentMeal = new();
		CurrentMealTitle = string.Empty;
		CurrentMealDate = string.Empty;
		CurrentMealCategory = 1;
		MealEditorVisible = true;
	}
	private void CancelEditMeal() => MealEditorVisible = false;
	private void SaveEditMeal()
	{
		if (CheckMealValues())
		{
			currentMeal.Title = CurrentMealTitle.Trim().ToLower();
			currentMeal.Date = DateOnly.Parse(CurrentMealDate);
			currentMeal.Category = (MealCategory)CurrentMealCategory;
			if (newMeal)
			{
				mealList.Add(currentMeal);
			}
			MealEditorVisible = false;
			UpdateData();
		}
	}
	private void CancelEditIngredient() => IngredientEditorVisible = false;
	private void SaveEditIngredient()
	{
		if (CheckIngredientValues())
		{
			currentIngredient.Title = CurrentIngredientTitle.Trim().ToLower();
			currentIngredient.Quantity = !string.IsNullOrWhiteSpace(CurrentIngredientQuantity) ? CurrentIngredientQuantity.Trim().ToLower() : null;
			if (newIngredient)
			{
				currentMeal.Ingredients.Add(currentIngredient);
			}
			IngredientEditorVisible = false;
			UpdateData();
		}
	}
	private void AddIngredient(MealModel parameter)
	{
		newIngredient = true;
		currentMeal = parameter;
		currentIngredient = new();
		CurrentIngredientTitle = string.Empty;
		CurrentIngredientQuantity = string.Empty;
		IngredientEditorVisible = true;
	}
	private void EditMeal(MealModel parameter)
	{
		newMeal = false;
		currentMeal = parameter;
		CurrentMealTitle = currentMeal.Title;
		CurrentMealDate = $"{currentMeal.Date:d/M}";
		CurrentMealCategory = (int)currentMeal.Category;
		MealEditorVisible = true;
	}
	private void RemoveMeal(MealModel parameter)
	{
		mealList.Remove(parameter);
		UpdateData();
	}
	private void EditIngredient(IngredientModel parameter)
	{
		newIngredient = false;
		currentIngredient = parameter;
		CurrentIngredientTitle = currentIngredient.Title;
		CurrentIngredientQuantity = currentIngredient.Quantity ?? string.Empty;
		IngredientEditorVisible = true;
	}
	private void RemoveIngredient((MealModel Meal, IngredientModel Ingredient) parameter)
	{
		parameter.Meal.Ingredients.Remove(parameter.Ingredient);
		UpdateData();
	}

	private bool CheckMealValues()
	{
		if (!string.IsNullOrWhiteSpace(CurrentMealTitle))
		{
			if (DateOnly.TryParse(CurrentMealDate, out DateOnly newDate) && newDate.ToRelativeDays() is >= 0 && newDate.FirstDayOfWeek().ToRelativeWeeks() is 0 or 1)
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
			Popup.Message("Invalid title.");
		}
		MealEditorVisible = true;
		return false;
	}
	private bool CheckIngredientValues()
	{
		if (!string.IsNullOrWhiteSpace(CurrentIngredientTitle))
		{
			return true;
		}
		else
		{
			Popup.Message("Invalid title.");
		}
		IngredientEditorVisible = true;
		return false;
	}
	private void UpdateData()
	{
		mealList.RemoveAll(x => x.Date.ToRelativeDays() is < 0 || x.Date.FirstDayOfWeek().ToRelativeWeeks() is not 0 and not 1);
		dataManager.UpdateData();
		MealTodayCollectionView.UpdateView();
		MealThisWeekCollectionView.UpdateView();
		MealNextWeekCollectionView.UpdateView();
		TodayDefaultMessageVisible = mealList.All(x => x.Date.ToRelativeDays() is not 0);
		ThisWeekDefaultMessageVisible = mealList.All(x => x.Date.FirstDayOfWeek().ToRelativeWeeks() is not 0);
		NextWeekDefaultMessageVisible = mealList.All(x => x.Date.FirstDayOfWeek().ToRelativeWeeks() is not 1);
	}
}