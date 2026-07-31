using System;
using System.IO;
using System.Reflection;

namespace Pocketable_Popcorn.NET
{
	public class GlobalHelper
	{
		public static string CombineWithAppDirectoryPath(string fileName)
		{
			string codeBase = Assembly.GetExecutingAssembly().GetName().CodeBase;
			string appDirectory = Path.GetDirectoryName(new Uri(codeBase).LocalPath);

			string filePath = Path.Combine(appDirectory, fileName);

			return filePath;
		}
	}
}
