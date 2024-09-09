using MealPlanner.Source.MVVM;

namespace MealPlanner.Source.Models;

internal class MethodModel : MVVMBase
{
	private int index = 0;
	private string step = string.Empty;

	public int Index
	{
		get => index;
		set => SetValue(ref index, value);
	}
	public string Step
	{
		get => step;
		set => SetValue(ref step, value);
	}
}