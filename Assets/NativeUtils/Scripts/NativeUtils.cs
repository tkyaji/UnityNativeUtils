using UnityEngine;

public class NativeUtils : NativeUtilsPlugin {

	public static string GetDataDirectory() {
#if UNITY_ANDROID && !UNITY_EDITOR
		var currentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
		var filesDir = currentActivity.Call<AndroidJavaObject>("getFilesDir");
		return filesDir.Call<string>("getCanonicalPath");
#else
		return Application.persistentDataPath;
#endif
	}

	public static string GetCacheDirectory() {
#if UNITY_ANDROID && !UNITY_EDITOR
		var currentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
		var filesDir = currentActivity.Call<AndroidJavaObject>("getCacheDir");
		return filesDir.Call<string>("getCanonicalPath");
#else
		return Application.temporaryCachePath;
#endif
	}
}
