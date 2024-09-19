using System.Windows;

namespace MealPlanner.Source.Data;

internal static class ExtensionMethods
{
	public static string ToPageTitle(this int pageNumber) => pageNumber switch { 0 => "meals", 1 => "inventory", 2 => "shopping", _ => "recipes" };
	public static int ToRelativeDays(this DateOnly date) => date.DayNumber - DateOnly.FromDateTime(DateTime.Today).DayNumber;
	public static int ToRelativeWeeks(this DateOnly date) => (date.FirstDayOfWeek().DayNumber - DateOnly.FromDateTime(DateTime.Today).FirstDayOfWeek().DayNumber) / 7;
	public static int UKDayOfWeek(this DateOnly date) => (int)date.DayOfWeek switch { 1 => 0, 2 => 1, 3 => 2, 4 => 3, 5 => 4, 6 => 5, _ => 6 };
	public static DateOnly FirstDayOfWeek(this DateOnly date) => date.AddDays(date.UKDayOfWeek() * -1);
}

internal static class Popup
{
	public static void Message(this string message) => MessageBox.Show(message, "MealPlanner");
}