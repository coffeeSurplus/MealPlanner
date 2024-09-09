﻿using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MealPlanner.Source.Converters;

internal class GeneralBoolToBoolInvertedConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !(bool)value;
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => !(bool)value;
}

internal class GeneralBoolToVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (bool)value ? Visibility.Visible : Visibility.Collapsed;
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}

internal class GeneralBoolToVisibilityInvertedConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !(bool)value ? Visibility.Visible : Visibility.Collapsed;
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}

internal class GeneralIntToRadioConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (int)value == int.Parse((string)parameter);
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (bool)value ? parameter : Binding.DoNothing;
}

internal class GeneralIntToVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (int)value == int.Parse((string)parameter) ? Visibility.Visible : Visibility.Collapsed;
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}