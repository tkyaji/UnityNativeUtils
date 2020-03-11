using UnityEngine;

public class NativeUtils {

	public enum FeedbackType {
		ImpactLight = 0,
		ImpactMedium,
		ImpactHeavy,
		NotificationSuccess,
		NotificationWarning,
		NotificationError,
		Selection,
	}


#if UNITY_IOS && !UNITY_EDITOR
	private static NativeUtilsPlugin plugin = new NativeUtilsPlugin_iOS();
#elif UNITY_ANDROID && !UNITY_EDITOR
	private static NativeUtilsPlugin plugin = new NativeUtilsPlugin_Android();
#else
	private static NativeUtilsPlugin plugin = new NativeUtilsPlugin_Stub();
#endif

	public static string GetDataDirectory() {
		return plugin.GetDataDirectory();
	}

	public static string GetCacheDirectory() {
		return plugin.GetCacheDirectory();
	}

	public static string GetVersion() {
		return plugin.GetVersion();
	}

	public static string GetBuildNumber() {
		return plugin.GetBuildNumber();
	}

	public static float GetWidth() {
		return plugin.GetWidth();
	}

	public static float GetHeight() {
		return plugin.GetHeight();
	}

	public static float GetScale() {
		return plugin.GetScale();
	}

	public static float GetSafeAreaTop() {
		return plugin.GetSafeAreaTop();
	}

	public static float GetSafeAreaBottom() {
		return plugin.GetSafeAreaBottom();
	}

	public static void ShowWebView(string url) {
		plugin.ShowWebView(url);
	}

	public static string GetLanguage() {
		return plugin.GetLanguage();
	}

	public static void OpenReview(string iosAppId) {
#if UNITY_IOS
		string appId = iosAppId;
#else
		string appId = Application.identifier;
#endif
		plugin.OpenReview(appId);
	}

	public static void OpenReviewDialog(string iosAppId, string title, string message, string okButton = "OK", string cancelButton = "Cancel", System.Action<bool> callback = null) {
#if UNITY_IOS
		string appId = iosAppId;
#else
		string appId = Application.identifier;
#endif
		plugin.OpenReviewDialog(appId, title, message, okButton, cancelButton, callback);
	}

	public static void Alert(string title, string message) {
		plugin.Alert(title, message);
	}

	public static bool IsHapticFeedbackSupported() {
		return plugin.IsHapticFeedbackSupported();
	}

	public static void HapticFeedback(NativeUtils.FeedbackType type) {
		plugin.HapticFeedback(type);
	}
}


public interface NativeUtilsPlugin {
	string GetDataDirectory();
	string GetCacheDirectory();
	string GetVersion();
	string GetBuildNumber();
	float GetWidth();
	float GetHeight();
	float GetScale();
	float GetSafeAreaTop();
	float GetSafeAreaBottom();
	void ShowWebView(string url);
	string GetLanguage();
	void OpenReview(string appId);
	void OpenReviewDialog(string appId, string title, string message, string okButton = "OK", string cancelButton = "Cancel", System.Action<bool> callback = null);
	void Alert(string title, string message);
	bool IsHapticFeedbackSupported();
	void HapticFeedback(NativeUtils.FeedbackType type);
}
