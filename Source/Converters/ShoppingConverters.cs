using MealPlanner.Source.Models;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MealPlanner.Source.Converters;

internal class ShoppingBoughtForegroundConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => Application.Current.FindResource((bool)value ? "Dark" : "Accent3");
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}

internal class ShoppingBoughtTextDecorationsConverter : IValueConverter
{
	public object? Convert(object value, Type targetType, object parameter, CultureInfo culture) => (bool)value ? TextDecorations.Strikethrough : null;
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}

internal class ShoppingCostConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (double)value is not 0 ? $"total cost: £{value:N2}" : "hooray!\n\nyou have bought all your shopping items.";
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}

internal class ShoppingCountConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => $"{((ReadOnlyObservableCollection<object>)value).Cast<ShoppingModel>().Count(x => !x.Bought)} {(((ReadOnlyObservableCollection<object>)value).Cast<ShoppingModel>().Count(x => !x.Bought) is not 1 ? "items" : "item")}";
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}

internal class ShoppingSubitleTextConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => $"£{value:N2}";
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}

internal class ShoppingTitleTextConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => $"* {values[0]}, {values[1]}";
	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => [Binding.DoNothing, Binding.DoNothing];
}