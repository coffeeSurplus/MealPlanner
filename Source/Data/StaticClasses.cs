using System.Windows;

namespace MealPlanner.Source.Data;

internal static class ExtensionMethods
{
	public static string ToPageTitle(this int pageNumber) => pageNumber switch { 0 => "meals", 1 => "inventory", 2 => "shopping", _ => "recipes" };
	public static int ToRelativeDays(this DateOnly date) => date.DayNumber - DateOnly.FromDateTime(DateTime.Today).DayNumber;
}

internal static class Popup
{
	public static void Message(this string message) => MessageBox.Show(message, "MealPlanner");
}