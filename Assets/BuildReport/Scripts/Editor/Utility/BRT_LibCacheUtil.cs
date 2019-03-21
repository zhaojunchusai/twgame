using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;



public static class BRT_LibCacheUtil
{
	// assetPath is expected to start with "Assets/"
	public static long GetImportedFileSize(string assetPath)
	{
		long result = -1;

		assetPath = BuildReportTool.Util.MyHtmlDecode(assetPath);

		if (BuildReportTool.Util.IsFileInAPath(assetPath, "/StreamingAssets/"))
		{
			return -1;
		}
		if (BuildReportTool.Util.IsUselessFile(assetPath))
		{
			return -1;
		}
		if (BuildReportTool.Util.IsFileAUnixHiddenFile(assetPath))
		{
			return -1;
		}

		if (!string.IsNullOrEmpty(assetPath))
		{
			string guid = AssetDatabase.AssetPathToGUID(assetPath);
			if (guid.Length < 2)
			{
				//Debug.Log(assetPath + " has no guid? value is \"" + guid + "\"");
				return -1;
			}

			string assetImportedPath = Path.GetFullPath(Application.dataPath + "../../Library/cache/" + guid.Substring(0, 2) + "/" + guid);

			if (File.Exists(assetImportedPath))
			{
				result = BuildReportTool.Util.GetFileSizeInBytes(assetImportedPath);
			}
			else
			{
				//Debug.Log(assetPath + " not found: " + assetImportedPath);
				assetImportedPath = Path.GetFullPath(Application.dataPath + "../../Library/metadata/" + guid.Substring(0, 2) + "/" + guid);
				if (File.Exists(assetImportedPath))
				{
					result = BuildReportTool.Util.GetFileSizeInBytes(assetImportedPath);
				}
			}
		}

		return result;
	}
}
