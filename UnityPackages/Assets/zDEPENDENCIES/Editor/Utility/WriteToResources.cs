using System.IO;
using System.Text;
using UnityEngine;

namespace Utility
{
	public static class WriteToResourcesUtil
	{
		private static readonly string basePath = @$"{Application.dataPath}/Resources/";

		/// <summary>
		/// Write to a file in the resources folder (a new will be created if it does not exist)
		/// </summary>
		/// <param name="lines">The lines to write to the file</param>
		/// <param name="fileName">The name of the file (with extension)</param>
		/// <param name="subFolders">Any subpath within the Resources folder</param>
		public static void WriteToResources(string[] lines, string fileName, string subFolders = null)
		{
			string path = basePath;

			StringBuilder stringBuilder = new StringBuilder(path);

			if (subFolders != null)
			{
				subFolders = subFolders.TrimStart('/', '\\');
				stringBuilder.Append(subFolders);

				if (!subFolders.EndsWith('/') && !subFolders.EndsWith('\\'))
				{
					stringBuilder.Append('/');
				}
			}

			Directory.CreateDirectory(stringBuilder.ToString()); // Create a directory if it does not already exist

			stringBuilder.Append(fileName);
			path = stringBuilder.ToString();
			
			File.WriteAllLines(path, lines);
		}
	}
}