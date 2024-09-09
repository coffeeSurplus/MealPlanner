using System.IO;
using System.Text.Json;

namespace MealPlanner.Source.Data;

internal class DataManager<T>
{
	private readonly string path;

	public List<T> Data { get; }

	public DataManager(string path)
	{
		this.path = $"{path}List.json";
		Data = File.Exists(GetPath()) ? JsonSerializer.Deserialize<List<T>>(File.ReadAllText(GetPath()))! : [];
	}

	public void UpdateData() => File.WriteAllText(GetPath(), JsonSerializer.Serialize(Data));

	private string GetPath() => Environment.ExpandEnvironmentVariables(@$"%AppData%\MealPlanner\{path}");
}