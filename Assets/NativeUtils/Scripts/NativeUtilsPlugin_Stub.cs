using UnityEngine;

public class NativeUtilsPlugin_Stub : NativeUtilsPlugin {

    public string GetDataDirectory() {
        return Application.persistentDataPath;
    }

    public string GetCacheDirectory() {
        return Application.temporaryCachePath;
    }

    public string GetVersion() {
        return Application.version;
    }

    public string GetBuildNumber() {
        return Application.version;
    }

    public float GetWidth() {
        return Screen.width / 3.0f;
    }

    public float GetHeight() {
        return Screen.height / 3.0f;
    }

    public float GetScale() {
        return 3.0f;
    }

    public float GetSafeAreaTop() {
        return 0;
    }

    public float GetSafeAreaBottom() {
        return 0;
    }

    public void ShowWebView(string url) {
        Application.OpenURL(url);
    }

    public string GetLanguage() {
        return Application.systemLanguage.ToString();
    }

    public void OpenReview(string appId) {
#if UNITY_IOS
        Application.OpenURL("https://itunes.apple.com/jp/app/id" + appId);
#else
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + appId);
#endif
    }

    public void OpenReviewDialog(string appId, string title, string message, string okButton = "OK", string cancelButton = "Cancel", System.Action<bool> callback = null) {
#if UNITY_EDITOR
        if (UnityEditor.EditorUtility.DisplayDialog(title, message, okButton, cancelButton)) {
            OpenReview(appId);
            if (callback != null) {
                callback.Invoke(true);
            }
        } else {
            if (callback != null) {
                callback.Invoke(false);
            }
        }
#endif
    }

    public void Alert(string title, string message) {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.DisplayDialog(title, message, "OK");
#endif
    }

    public bool IsHapticFeedbackSupported() {
        return false;
    }

    public void HapticFeedback(NativeUtils.FeedbackType type) {
    }

    public bool IsTablet() {
        return false;
    }

}
