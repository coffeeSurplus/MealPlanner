using MealPlanner.Source.Data;
using System.Globalization;
using System.Windows.Data;

namespace MealPlanner.Source.Converters;

internal class InventoryCountConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => $"{value} {((int)value is not 1 ? "items" : "item")}";
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}

internal class InventorySubtitleTextConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => $"   {((DateOnly)values[0] != DateOnly.MaxValue ? $"{((DateOnly)values[0]).ToRelativeDays() switch { < 0 => "expired", 1 => "1 day", 7 => "1 week", 14 => "2 weeks", 21 => "3 weeks", 28 => "1 month", < 28 => $"{((DateOnly)values[0]).ToRelativeDays()} days", _ => $"{((DateOnly)values[0]).ToRelativeDays() / 28} months" }} - {(DateOnly)values[0]:d/M}" : "no expiry date")} - {((bool)values[1] ? "opened" : "unopened")}";
	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => [Binding.DoNothing, Binding.DoNothing];
}

internal class InventoryTitleTextConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => $"* {values[0]} - {values[1]}";
	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => [Binding.DoNothing, Binding.DoNothing];
}