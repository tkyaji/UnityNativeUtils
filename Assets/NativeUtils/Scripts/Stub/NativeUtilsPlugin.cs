﻿#if UNITY_EDITOR || (!UNITY_IOS && !UNITY_ANDROID)

using UnityEngine;

public class NativeUtilsPlugin {

    public static string GetVersion() {
        return Application.version;
	}

    public static string GetBuildNumber() {
        return Application.version;
    }

	public static float GetWidth() {
        return Screen.width;
	}

	public static float GetHeight() {
        return Screen.height;
	}

	public static float GetScale() {
        return 1.0f;
	}

	public static void ShowWebView(string url) {
		Application.OpenURL(url);
	}

    public static string GetLanguage() {
        return Application.systemLanguage.ToString();
    }

    public static void OpenReview(string appId) {
#if UNITY_IOS
        Application.OpenURL("https://itunes.apple.com/jp/app/id" + appId);
#else
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + appId);
#endif
    }

    public static void OpenReviewDialog(string appId, string title, string message, string okButton = "OK", string cancelButton = "Cancel") {
#if UNITY_EDITOR
        if (UnityEditor.EditorUtility.DisplayDialog(title, message, okButton, cancelButton)) {
            OpenReview(appId);
        }
#endif
    }

    public static void Alert(string title, string message) {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.DisplayDialog(title, message, "OK");
#endif
    }
}

#endif