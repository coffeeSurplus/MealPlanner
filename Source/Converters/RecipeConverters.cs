using MealPlanner.Source.Models;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MealPlanner.Source.Converters;

internal class RecipeCurrentSelectedConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => values[0] == values[1];
	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => [Binding.DoNothing, Binding.DoNothing];
}

internal class RecipeIngredientTextConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => $"* {values[0]}{(values[1] is not null ? $", {values[1]}" : string.Empty)}";
	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => [Binding.DoNothing, Binding.DoNothing];
}

internal class RecipePanelVisibilityConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => !(bool)values[0] && ((RecipeModel)values[1]).Title != string.Empty ? Visibility.Visible : Visibility.Collapsed;
	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => [Binding.DoNothing, Binding.DoNothing];
}

internal class RecipeSubtitleTextConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => $"* category: {values[0]}\n* serves: {values[1]}";
	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => [Binding.DoNothing, Binding.DoNothing];
}

internal class RecipeTimeTextConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => $"* preparation time: {((TimeSpan)values[0] != TimeSpan.Zero ? $"{((TimeSpan)values[0]).Hours switch { 0 => string.Empty, 1 => "1 hour", _ => $"{((TimeSpan)values[0]).Hours} hours" }} {((TimeSpan)values[0]).Minutes switch { 0 => string.Empty, 1 => "1 minute", _ => $"{((TimeSpan)values[0]).Minutes} minutes" }}" : "n/a")}\n* cooking time: {((TimeSpan)values[1] != TimeSpan.Zero ? $"{((TimeSpan)values[1]).Hours switch { 0 => string.Empty, 1 => "1 hour", _ => $"{((TimeSpan)values[1]).Hours} hours" }} {((TimeSpan)values[1]).Minutes switch { 0 => string.Empty, 1 => "1 minute", _ => $"{((TimeSpan)values[1]).Minutes} minutes" }}" : "n/a")}";
	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => [Binding.DoNothing, Binding.DoNothing];
}