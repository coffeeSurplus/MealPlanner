using MealPlanner.Source.CollectionViews;
using MealPlanner.Source.Data;
using MealPlanner.Source.Models;
using MealPlanner.Source.MVVM;
using System.Collections.ObjectModel;

namespace MealPlanner.Source.ViewModels;

internal class RecipeViewModel : MVVMBase
{
	private readonly DataManager<RecipeModel> dataManager = new("Recipe");
	private readonly List<RecipeModel> recipeList;

	private bool newRecipe = true;
	private bool editMode = false;
	private RecipeModel currentRecipe = new();
	private string currentRecipeTitle = string.Empty;
	private string currentRecipeCategory = string.Empty;
	private string currentRecipePreparationTime = string.Empty;
	private string currentRecipeCookingTime = string.Empty;
	private string currentRecipeServes = string.Empty;
	private ObservableCollection<IngredientModel> currentRecipeIngredients = [];
	private ObservableCollection<MethodModel> currentRecipeMethod = [];

	public bool NewRecipe
	{
		get => newRecipe;
		set => SetValue(ref newRecipe, value);
	}
	public bool EditMode
	{
		get => editMode;
		set => SetValue(ref editMode, value);
	}
	public RecipeModel CurrentRecipe
	{
		get => currentRecipe;
		set => SetValue(ref currentRecipe, value);
	}
	public string CurrentRecipeTitle
	{
		get => currentRecipeTitle;
		set => SetValue(ref currentRecipeTitle, value);
	}
	public string CurrentRecipeCategory
	{
		get => currentRecipeCategory;
		set => SetValue(ref currentRecipeCategory, value);
	}
	public string CurrentRecipePreparationTime
	{
		get => currentRecipePreparationTime;
		set => SetValue(ref currentRecipePreparationTime, value);
	}
	public string CurrentRecipeCookingTime
	{
		get => currentRecipeCookingTime;
		set => SetValue(ref currentRecipeCookingTime, value);
	}
	public string CurrentRecipeServes
	{
		get => currentRecipeServes;
		set => SetValue(ref currentRecipeServes, value);
	}
	public ObservableCollection<IngredientModel> CurrentRecipeIngredients
	{
		get => currentRecipeIngredients;
		set => SetValue(ref currentRecipeIngredients, value);
	}
	public ObservableCollection<MethodModel> CurrentRecipeMethod
	{
		get => currentRecipeMethod;
		set => SetValue(ref currentRecipeMethod, value);
	}

	public RelayCommand AddNewRecipeCommand { get; }
	public RelayCommand RemoveRecipeCommand { get; }
	public RelayCommand CancelEditRecipeCommand { get; }
	public RelayCommand SaveEditRecipeCommand { get; }
	public RelayCommand AddIngredientCommand { get; }
	public RelayCommand AddMethodCommand { get; }
	public RelayCommand<IngredientModel> RemoveIngredientCommand { get; }
	public RelayCommand<MethodModel> RemoveMethodCommand { get; }
	public RelayCommand<RecipeModel> RecipeSelectedCommand { get; }

	public RecipeCollectionView RecipeCollectionView { get; }

	public RecipeViewModel()
	{
		recipeList = dataManager.Data;
		RecipeCollectionView = new(recipeList);
		AddNewRecipeCommand = new(AddNewRecipe);
		CancelEditRecipeCommand = new(CancelEditRecipe);
		SaveEditRecipeCommand = new(SaveEditRecipe);
		AddIngredientCommand = new(AddIngredient);
		AddMethodCommand = new(AddMethod);
		RemoveIngredientCommand = new(RemoveIngredient);
		RemoveMethodCommand = new(RemoveMethod);
		RecipeSelectedCommand = new(RecipeSelected);
		RemoveRecipeCommand = new(RemoveRecipe);
		UpdateView();
	}

	private void AddNewRecipe()
	{
		NewRecipe = true;
		CurrentRecipe = new();
		CurrentRecipeTitle = string.Empty;
		CurrentRecipeCategory = string.Empty;
		CurrentRecipePreparationTime = string.Empty;
		CurrentRecipeCookingTime = string.Empty;
		CurrentRecipeServes = string.Empty;
		CurrentRecipeIngredients = [];
		CurrentRecipeMethod = [];
		EditMode = true;
	}
	private void RemoveRecipe()
	{
		recipeList.Remove(CurrentRecipe);
		RecipeSelected(new());
		UpdateData();
	}
	private void CancelEditRecipe() => EditMode = false;
	private void SaveEditRecipe()
	{
		if (CurrentRecipe is not null && CheckRecipeValues())
		{
			CurrentRecipe.Title = CurrentRecipeTitle.Trim().ToLower();
			CurrentRecipe.Category = CurrentRecipeCategory.Trim().ToLower();
			CurrentRecipe.PreparationTime = TimeSpan.Parse(CurrentRecipePreparationTime);
			CurrentRecipe.CookingTime = TimeSpan.Parse(CurrentRecipeCookingTime);
			CurrentRecipe.Serves = int.Parse(CurrentRecipeServes);
			CurrentRecipe.Ingredients = CurrentRecipeIngredients;
			CurrentRecipe.Method = CurrentRecipeMethod;
			for (int index = 1; index <= currentRecipe.Method.Count; index++)
			{
				currentRecipe.Method[index - 1].Index = index;
			}
			foreach (IngredientModel ingredientModel in CurrentRecipe.Ingredients)
			{
				ingredientModel.Title = ingredientModel.Title.Trim().ToLower();
				ingredientModel.Quantity = !string.IsNullOrWhiteSpace(ingredientModel.Quantity) ? ingredientModel.Quantity.Trim().ToLower() : null;
			}
			foreach (MethodModel methodModel in CurrentRecipe.Method)
			{
				methodModel.Step = methodModel.Step.Trim().ToLower();
			}
			if (NewRecipe)
			{
				recipeList.Add(CurrentRecipe);
			}
			RecipeSelected(currentRecipe);
			UpdateData();
		}
	}
	private void AddIngredient() => CurrentRecipeIngredients.Add(new());
	private void AddMethod() => CurrentRecipeMethod.Add(new());
	private void RemoveIngredient(IngredientModel parameter) => CurrentRecipeIngredients.Remove(parameter);
	private void RemoveMethod(MethodModel parameter) => CurrentRecipeMethod.Remove(parameter);
	private void RecipeSelected(RecipeModel parameter)
	{
		EditMode = false;
		NewRecipe = false;
		CurrentRecipe = parameter;
		CurrentRecipeTitle = CurrentRecipe.Title;
		CurrentRecipeCategory = CurrentRecipe.Category;
		CurrentRecipePreparationTime = $"{CurrentRecipe.PreparationTime:h':'mm}";
		CurrentRecipeCookingTime = $"{CurrentRecipe.CookingTime:h':'mm}";
		CurrentRecipeServes = $"{CurrentRecipe.Serves}";
		CurrentRecipeIngredients = CurrentRecipe.Ingredients;
		CurrentRecipeMethod = CurrentRecipe.Method;
	}

	private bool CheckRecipeValues()
	{
		if (!string.IsNullOrWhiteSpace(CurrentRecipeTitle))
		{
			if (!string.IsNullOrWhiteSpace(CurrentRecipeCategory))
			{
				if (TimeSpan.TryParse(CurrentRecipePreparationTime, out TimeSpan newPreparationTime) && newPreparationTime >= TimeSpan.Zero)
				{
					if (TimeSpan.TryParse(CurrentRecipeCookingTime, out TimeSpan newCookingTime) && newCookingTime >= TimeSpan.Zero)
					{
						if (int.TryParse(CurrentRecipeServes, out int newServes) && newServes is >= 1)
						{
							if (CurrentRecipeIngredients.Count is > 0 && CurrentRecipeIngredients.All(x => !string.IsNullOrWhiteSpace(x.Title)))
							{
								if (CurrentRecipeMethod.Count is > 0 && CurrentRecipeMethod.All(x => !string.IsNullOrWhiteSpace(x.Step)))
								{
									return true;
								}
								else
								{
									Popup.Message("Invalid method.");
								}
							}
							else
							{
								Popup.Message("Invalid ingredients.");
							}
						}
						else
						{
							Popup.Message("Invalid serves amount.");
						}
					}
					else
					{
						Popup.Message("Invalid preparation time.");
					}
				}
				else
				{
					Popup.Message("Invalid preparation time.");
				}
			}
			else
			{
				Popup.Message("Invalid category.");
			}
		}
		else
		{
			Popup.Message("Invalid title.");
		}
		return false;
	}
	private void UpdateData()
	{
		dataManager.UpdateData();
		UpdateView();
	}
	private void UpdateView() => RecipeCollectionView.UpdateView();
}