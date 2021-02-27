#if UNITY_IOS

using UnityEngine;
using System.Runtime.InteropServices;
using System;
using AOT;

public class NativeUtilsPlugin_iOS : NativeUtilsPlugin {

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
    private static extern float _NativeUtils_getSafeAreaTop();

    [DllImport("__Internal")]
    private static extern float _NativeUtils_getSafeAreaBottom();

    [DllImport("__Internal")]
    private static extern void _NativeUtils_showWebView(string url);

    [DllImport("__Internal")]
    private static extern string _NativeUtils_getLanguage();

    [DllImport("__Internal")]
    private static extern void _NativeUtils_openReview(string appId);

    [DllImport("__Internal")]
    private static extern void _NativeUtils_openReviewDialog(string appId, string title, string message, string okButton, string cancelButton, IntPtr actionPtr, _NativeUtils_cs_callback_method callback);

    [DllImport("__Internal")]
    private static extern void _NativeUtils_alert(string title, string message);

    [DllImport("__Internal")]
    private static extern void _NativeUtils_hapticfeedback(NativeUtils.FeedbackType type);

    [DllImport("__Internal")]
    private static extern bool _NativeUtils_isHapticfeedbackSupported();

    public string GetDataDirectory() {
        return Application.persistentDataPath;
    }

    public string GetCacheDirectory() {
        return Application.temporaryCachePath;
    }

    public string GetVersion() {
        return _NativeUtils_getShortVersionName();
    }

    public string GetBuildNumber() {
        return _NativeUtils_getVersionName();
    }

    public float GetWidth() {
        return _NativeUtils_getWidth();
    }

    public float GetHeight() {
        return _NativeUtils_getHeight();
    }

    public float GetScale() {
        return _NativeUtils_getScale();
    }

    public float GetSafeAreaTop() {
        return _NativeUtils_getSafeAreaTop();
    }

    public float GetSafeAreaBottom() {
        return _NativeUtils_getSafeAreaBottom();
    }

    public void ShowWebView(string url) {
        _NativeUtils_showWebView(url);
    }

    public string GetLanguage() {
        return _NativeUtils_getLanguage();
    }

    public void OpenReview(string appId) {
        _NativeUtils_openReview(appId);
    }

    public void OpenReviewDialog(string appId, string title, string message, string okButton = "OK", string cancelButton = "Cancel", Action<bool> callback = null) {
        if (callback == null) {
            callback = (bool b) => {};
        }
        IntPtr callbackIntPtr = (IntPtr)GCHandle.Alloc(callback);
        _NativeUtils_openReviewDialog(appId, title, message, okButton, cancelButton, callbackIntPtr, _NativeUtils_cs_callback_method_impl);
    }

    delegate void _NativeUtils_cs_callback_method(IntPtr gameObjectPtr, bool isOK);

    [MonoPInvokeCallback(typeof(_NativeUtils_cs_callback_method))]
    private static  void _NativeUtils_cs_callback_method_impl(IntPtr gameObjectPtr, bool isOK) {
        GCHandle handle = (GCHandle)gameObjectPtr;
        Action<bool> callback = handle.Target as Action<bool>;
        handle.Free();
        callback.Invoke(isOK);
    }

    public void Alert(string title, string message) {
        _NativeUtils_alert(title, message);
    }

    public bool IsHapticFeedbackSupported() {
        return _NativeUtils_isHapticfeedbackSupported();
    }

    public void HapticFeedback(NativeUtils.FeedbackType type) {
        _NativeUtils_hapticfeedback(type);
    }

    public bool IsTablet() {
        return SystemInfo.deviceModel.Contains("iPad");
    }
}

#endif
