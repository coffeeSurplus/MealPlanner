using MealPlanner.Source.Data;
using MealPlanner.Source.MVVM;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace MealPlanner.Source.ViewModels;

internal class MainWindowViewModel : MVVMBase
{
	private WindowState currentState;
	private bool sidepanelCollapsed = false;
	private int currentPageNumber = 0;

	public WindowState CurrentState
	{
		get => currentState;
		set => SetValue(ref currentState, value);
	}
	public bool SidepanelCollapsed
	{
		get => sidepanelCollapsed;
		set => SetValue(ref sidepanelCollapsed, value);
	}
	public int CurrentPageNumber
	{
		get => currentPageNumber;
		set => SetValue(ref currentPageNumber, value);
	}

	public MealViewModel Meal { get; } = new();
	public InventoryViewModel Inventory { get; } = new();
	public ShoppingViewModel Shopping { get; } = new();
	public RecipeViewModel Recipe { get; } = new();

	public RelayCommand MinimiseWindowCommand { get; }
	public RelayCommand MaximiseWindowCommand { get; }
	public RelayCommand CloseWindowCommand { get; }

	public MainWindowViewModel()
	{
		if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
		{
			Popup.Message("Another instance of this application is already running.");
			Application.Current.Shutdown();
		}
		Directory.CreateDirectory(Environment.ExpandEnvironmentVariables($@"%AppData%\MealPlanner\"));
		MinimiseWindowCommand = new(MinimiseWindow);
		MaximiseWindowCommand = new(MaximiseWindow);
		CloseWindowCommand = new(CloseWindow);
	}

	private void MinimiseWindow() => CurrentState = WindowState.Minimized;
	private void MaximiseWindow() => CurrentState = CurrentState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
	private void CloseWindow() => Application.Current.Shutdown();
}