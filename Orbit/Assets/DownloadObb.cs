using UnityEngine;
using System.Collections;

public class DownloadObb : MonoBehaviour
{

	void Awake()
	{
#if UNITY_ANDROID
		if (GooglePlayDownloader.RunningOnAndroid())
		{
			string expPath = GooglePlayDownloader.GetExpansionFilePath();
			if (expPath != null)
			{
				string mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
				string patchPath = GooglePlayDownloader.GetPatchOBBPath(expPath);
				if (mainPath == null || patchPath == null)
					GooglePlayDownloader.FetchOBB();
			}
		}
#endif
		/*
				if (!GooglePlayDownloader.RunningOnAndroid())
				{
					GUI.Label(new Rect(10, 10, Screen.width-10, 20), "Use GooglePlayDownloader only on Android device!");
					return;
				}

				string expPath = GooglePlayDownloader.GetExpansionFilePath();
				if (expPath == null)
				{
						GUI.Label(new Rect(10, 10, Screen.width-10, 20), "External storage is not available!");
				}
				else
				{
					string mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
					string patchPath = GooglePlayDownloader.GetPatchOBBPath(expPath);

					GUI.Label(new Rect(10, 10, Screen.width-10, 20), "Main = ..."  + ( mainPath == null ? " NOT AVAILABLE" :  mainPath.Substring(expPath.Length)));
					GUI.Label(new Rect(10, 25, Screen.width-10, 20), "Patch = ..." + (patchPath == null ? " NOT AVAILABLE" : patchPath.Substring(expPath.Length)));
					if (mainPath == null || patchPath == null)
						if (GUI.Button(new Rect(10, 100, 100, 100), "Fetch OBBs"))
							GooglePlayDownloader.FetchOBB();
				}
			*/
	}
}
