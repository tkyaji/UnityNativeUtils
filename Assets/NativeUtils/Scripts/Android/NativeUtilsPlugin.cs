#if UNITY_ANDROID && !UNITY_EDITOR

using UnityEngine;
using System;

public class NativeUtilsPlugin {

    private class AndroidOnClickListener : AndroidJavaProxy {
        private Action<int> action;
        public AndroidOnClickListener(Action<int> action) : base("android.content.DialogInterface$OnClickListener") {
            this.action = action;
        }
        public void onClick(AndroidJavaObject dialog, int which) {
            this.action.Invoke(which);
        }
    }

    public static string GetVersion() {
		AndroidJavaObject currentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity").Call<AndroidJavaObject>("getApplicationContext");
		AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");
		AndroidJavaObject packageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", currentActivity.Call<string>("getPackageName"), 0);
		return packageInfo.Get<string>("versionName");
	}

    public static string GetBuildNumber() {
        AndroidJavaObject currentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity").Call<AndroidJavaObject>("getApplicationContext");
        AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");
        AndroidJavaObject packageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", currentActivity.Call<string>("getPackageName"), 0);
        int versionCode = packageInfo.Get<int>("versionCode");
        return versionCode.ToString();
    }

	public static float GetWidth() {
		AndroidJavaObject metrics = getMetrics();
		int width = metrics.Get<int>("widthPixels");
		float scaledDensity = metrics.Get<float>("scaledDensity");
		return width / scaledDensity;
	}

	public static float GetHeight() {
		AndroidJavaObject metrics = getMetrics();
		int height = metrics.Get<int>("heightPixels");
		float scaledDensity = metrics.Get<float>("scaledDensity");
		return height / scaledDensity;
	}

	public static float GetScale() {
		AndroidJavaObject metrics = getMetrics();
		return metrics.Get<float>("scaledDensity");
	}

    public static float GetSafeAreaTop() {
        return 0;
    }

    public static float GetSafeAreaBottom() {
        return 0;
    }

	public static void ShowWebView(string url) {
		Application.OpenURL(url);
	}

    public static string GetLanguage() {
        AndroidJavaClass locale = new AndroidJavaClass("java.util.Locale");
        AndroidJavaObject def = locale.CallStatic<AndroidJavaObject>("getDefault");
        return def.Call<string>("getLanguage");
    }

    public static void OpenReview(string appId) {
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + appId);
    }

    public static void OpenReviewDialog(string appId, string title, string message, string okButton = "OK", string cancelButton = "Cancel", Action<bool> callback = null) {
        var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        var alertDialog = new AndroidJavaObject("android.app.AlertDialog$Builder", activity);
        alertDialog.Call<AndroidJavaObject>("setTitle", title);
        alertDialog.Call<AndroidJavaObject>("setMessage", message);
        alertDialog.Call<AndroidJavaObject>("setPositiveButton", okButton, new AndroidOnClickListener((int which) => {
            Application.OpenURL("https://play.google.com/store/apps/details?id=" + appId);
            if (callback != null) {
                callback.Invoke(true);
            }
        }));
        alertDialog.Call<AndroidJavaObject>("setNegativeButton", cancelButton, new AndroidOnClickListener((int which) => {
            if (callback != null) {
                callback.Invoke(false);
            }
        }));
        alertDialog.Call<AndroidJavaObject>("show");
    }

    public static void Alert(string title, string message) {
        var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        var alertDialog = new AndroidJavaObject("android.app.AlertDialog$Builder", activity);
        alertDialog.Call<AndroidJavaObject>("setTitle", title);
        alertDialog.Call<AndroidJavaObject>("setMessage", message);
        alertDialog.Call<AndroidJavaObject>("setPositiveButton", "OK", null);
        alertDialog.Call<AndroidJavaObject>("show");
    }

	private static AndroidJavaObject getMetrics() {
		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject windowManager = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity").Call<AndroidJavaObject>("getWindowManager");
		AndroidJavaObject display = windowManager.Call<AndroidJavaObject>("getDefaultDisplay");
		AndroidJavaObject metrics = new AndroidJavaObject("android.util.DisplayMetrics");

		if (getApiLevel() >= 17) {
			display.Call("getRealMetrics", metrics);
		} else {
			display.Call("getMetrics", metrics);
		}

		return metrics;
	}

	private static int getApiLevel() {
		return (new AndroidJavaClass("android.os.Build$VERSION")).GetStatic<int>("SDK_INT");
	}

}

#endif
