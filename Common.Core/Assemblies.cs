using System.Reflection;

namespace Common.Core;

public static class Assemblies
{
	public static string? GetExecutingDirectory() => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
}