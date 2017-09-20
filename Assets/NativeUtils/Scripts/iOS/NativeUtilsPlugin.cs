#if UNITY_IOS && !UNITY_EDITOR

using System.Runtime.InteropServices;

public class NativeUtilsPlugin {

    [DllImport("__Internal")]
    private static extern string _NativeUtils_getShortVersionName();

	[DllImport("__Internal")]
	private static extern string _NativeUtils_getVersionName();

	[DllImport("__Internal")]
	private static extern float _NativeUtils_getWidth();

	[DllImport("__Internal")]
	private static extern float _NativeUtils_getHeight();

	[DllImport("__Internal")]
	private static extern float _NativeUtils_getScale();

	[DllImport("__Internal")]
	private static extern void _NativeUtils_showWebView(string url);

    [DllImport("__Internal")]
    private static extern string _NativeUtils_getLanguage();

    [DllImport("__Internal")]
    private static extern void _NativeUtils_openReview(string appId);

    [DllImport("__Internal")]
    private static extern void _NativeUtils_openReviewDialog(string appId, string title, string message, string okButton, string cancelButton);

    [DllImport("__Internal")]
    private static extern void _NativeUtils_alert(string title, string message);


	public static string GetVersion() {
        return _NativeUtils_getShortVersionName();
	}

    public static string GetBuildNumber() {
        return _NativeUtils_getVersionName();
    }

	public static float GetWidth() {
		return _NativeUtils_getWidth();
	}

	public static float GetHeight() {
		return _NativeUtils_getHeight();
	}

	public static float GetScale() {
		return _NativeUtils_getScale();
	}

	public static void ShowWebView(string url) {
		_NativeUtils_showWebView(url);
	}

    public static string GetLanguage() {
        return _NativeUtils_getLanguage();
    }

    public static void OpenReview(string appId) {
        _NativeUtils_openReview(appId);
    }

    public static void OpenReviewDialog(string appId, string title, string message, string okButton = "OK", string cancelButton = "Cancel") {
        _NativeUtils_openReviewDialog(appId, title, message, okButton, cancelButton);
    }

    public static void Alert(string title, string message) {
        _NativeUtils_alert(title, message);
    }
}

#endif
