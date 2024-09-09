using System.Globalization;
using System.Windows.Data;

namespace MealPlanner.Source.Converters;

internal class CollectionViewMealDateConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => $"{(DateOnly)value:dddd d/M}".ToLower();
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}

internal class CollectionViewEnumConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => $"{value}".ToLower();
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}