#if UNITY_ANDROID

using UnityEngine;
using System;

public class NativeUtilsPlugin_Android : NativeUtilsPlugin {

    private class AndroidOnClickListener : AndroidJavaProxy {
        private Action<int> action;
        public AndroidOnClickListener(Action<int> action) : base("android.content.DialogInterface$OnClickListener") {
            this.action = action;
        }
        public void onClick(AndroidJavaObject dialog, int which) {
            this.action.Invoke(which);
        }
    }

    private static AndroidJavaObject jCurrentActivity {
        get {
            return new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        }
    }

    private static int apiLevel {
        get {
            return new AndroidJavaClass("android.os.Build$VERSION").GetStatic<int>("SDK_INT");
        }
    }


    public string GetDataDirectory() {
        var filesDir = jCurrentActivity.Call<AndroidJavaObject>("getFilesDir");
        return filesDir.Call<string>("getCanonicalPath");
    }

    public string GetCacheDirectory() {
        var filesDir = jCurrentActivity.Call<AndroidJavaObject>("getCacheDir");
        return filesDir.Call<string>("getCanonicalPath");
    }

    public string GetVersion() {
        AndroidJavaObject packageManager = jCurrentActivity.Call<AndroidJavaObject>("getPackageManager");
        AndroidJavaObject packageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", jCurrentActivity.Call<string>("getPackageName"), 0);
        return packageInfo.Get<string>("versionName");
    }

    public string GetBuildNumber() {
        AndroidJavaObject packageManager = jCurrentActivity.Call<AndroidJavaObject>("getPackageManager");
        AndroidJavaObject packageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", jCurrentActivity.Call<string>("getPackageName"), 0);
        int versionCode = packageInfo.Get<int>("versionCode");
        return versionCode.ToString();
    }

    public float GetWidth() {
        AndroidJavaObject metrics = getMetrics();
        int width = metrics.Get<int>("widthPixels");
        float scaledDensity = metrics.Get<float>("scaledDensity");
        return width / scaledDensity;
    }

    public float GetHeight() {
        AndroidJavaObject metrics = getMetrics();
        int height = metrics.Get<int>("heightPixels");
        float scaledDensity = metrics.Get<float>("scaledDensity");
        return height / scaledDensity;
    }

    public float GetScale() {
        AndroidJavaObject metrics = getMetrics();
        return metrics.Get<float>("scaledDensity");
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
        AndroidJavaClass locale = new AndroidJavaClass("java.util.Locale");
        AndroidJavaObject def = locale.CallStatic<AndroidJavaObject>("getDefault");
        return def.Call<string>("getLanguage");
    }

    public void OpenReview(string appId) {
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + appId);
    }

    public void OpenReviewDialog(string appId, string title, string message, string okButton = "OK", string cancelButton = "Cancel", Action<bool> callback = null) {
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

    public void Alert(string title, string message) {
        var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        var alertDialog = new AndroidJavaObject("android.app.AlertDialog$Builder", activity);
        alertDialog.Call<AndroidJavaObject>("setTitle", title);
        alertDialog.Call<AndroidJavaObject>("setMessage", message);
        alertDialog.Call<AndroidJavaObject>("setPositiveButton", "OK", null);
        alertDialog.Call<AndroidJavaObject>("show");
    }

    private AndroidJavaObject getMetrics() {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject windowManager = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity").Call<AndroidJavaObject>("getWindowManager");
        AndroidJavaObject display = windowManager.Call<AndroidJavaObject>("getDefaultDisplay");
        AndroidJavaObject metrics = new AndroidJavaObject("android.util.DisplayMetrics");

        if (apiLevel >= 17) {
            display.Call("getRealMetrics", metrics);
        } else {
            display.Call("getMetrics", metrics);
        }

        return metrics;
    }

    public bool IsHapticFeedbackSupported() {
        return true;
    }

    public void HapticFeedback(NativeUtils.FeedbackType type) {
        switch (type) {
            case NativeUtils.FeedbackType.ImpactLight:
                hapticFeedbackImpact(1);
                break;

            case NativeUtils.FeedbackType.ImpactMedium:
                hapticFeedbackImpact(5);
                break;

            case NativeUtils.FeedbackType.ImpactHeavy:
                hapticFeedbackImpact(10);
                break;

            case NativeUtils.FeedbackType.NotificationSuccess:
                hapticFeedbackWave(new long[] { 0, 5, 100, 5 });
                break;

            case NativeUtils.FeedbackType.NotificationWarning:
                hapticFeedbackWave(new long[] { 0, 5, 200, 5 });
                break;

            case NativeUtils.FeedbackType.NotificationError:
                hapticFeedbackWave(new long[] { 0, 5, 50, 5, 50, 5 });
                break;

            case NativeUtils.FeedbackType.Selection:
                hapticFeedbackImpact(1);
                break;
        }

    }

    private void hapticFeedbackImpact(long time) {
        var jVibratorService = new AndroidJavaClass("android.content.Context").GetStatic<string>("VIBRATOR_SERVICE");
        var jVibrator = jCurrentActivity.Call<AndroidJavaObject>("getSystemService", jVibratorService);

        if (apiLevel >= 26) {
            var jEffectCls = new AndroidJavaClass("android.os.VibrationEffect");
            var jEffect = jEffectCls.CallStatic<AndroidJavaObject>("createOneShot", time, jEffectCls.GetStatic<int>("DEFAULT_AMPLITUDE"));
            jVibrator.Call("vibrate", jEffect);

        } else {
            jVibrator.Call("vibrate", time);
        }
    }

    private void hapticFeedbackWave(long[] times) {
        var jVibratorService = new AndroidJavaClass("android.content.Context").GetStatic<string>("VIBRATOR_SERVICE");
        var jVibrator = jCurrentActivity.Call<AndroidJavaObject>("getSystemService", jVibratorService);

        if (apiLevel >= 26) {
            var jEffectCls = new AndroidJavaClass("android.os.VibrationEffect");
            IntPtr jTimes = AndroidJNI.ToLongArray(times);
            jvalue[] jParams1 = new jvalue[2];
            jParams1[0].l = jTimes;
            jParams1[1].i = -1;
            IntPtr jmidCreateWaveForm = AndroidJNIHelper.GetMethodID(jEffectCls.GetRawClass(), "createWaveform", "([JI)Landroid/os/VibrationEffect;", true);
            var jEffect = AndroidJNI.CallStaticObjectMethod(jEffectCls.GetRawClass(), jmidCreateWaveForm, jParams1);

            IntPtr jmidVibrate = AndroidJNIHelper.GetMethodID(jVibrator.GetRawClass(), "vibrate", "(Landroid/os/VibrationEffect;)V");
            jvalue[] jParams2 = new jvalue[2];
            jParams2[0].l = jEffect;
            AndroidJNI.CallVoidMethod(jVibrator.GetRawObject(), jmidVibrate, jParams2);

        } else {
            jVibrator.Call("vibrate", times);
        }
    }

}

#endif
