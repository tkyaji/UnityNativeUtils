# UnityNativeUtils

iOS / Android Native Convenient Functions Summary.


# Usage

Import UnityNativeUtils.unitypackage


# API

## string NativeUtils.GetDataDirectory()
Return the data directory of the application. Android returns `getFilesDir`.

## string NativeUtils.GetCacheDirectory()
Return the cache directory of the application. Android returns `getCacheDir`.

## string NativeUtils.GetVersion()
Return display version of application. Same value as `Application.identifier`.

## string NativeUtils.GetBuildNumber()
Return application build number.

## float NativeUtils.GetWidth()
Return native view width.

## float NativeUtils.GetHeight()
Return native view height.

## float NativeUtils.GetScale()
Return display density.

## float NativeUtils.GetSafeAreaTop()
Return safeArea top size of iOS.

## float NativeUtils.GetSafeAreaBottom()
Return safeArea bottom size of iOS.

## void NativeUtils.ShowWebView(string url)
Open URL. For Android, wrapper for `Application.OpenURL()`.
For iOS, open it with SafariViewController.

## string NativeUtils.GetLanguage()
Get language setting from native.

## void NativeUtils.Alert(string title, string message)
Show native alert.

## void NativeUtils.OpenReview(string appId)
Show app review page.
For iOS 10.3 or later, call `SKStoreReviewController requestReview`.
In other case, open appstore page.
For the value of `appId`, specify 10 digits for iOS and package name for Android.

## void NativeUtils.OpenReviewDialog(string appId, string title, string message, string okButton, string cancelButton, Action<bool> callback)
A confirmation dialog is displayed, and if 'OK', the review page is opened.
For iOS 10.3 or later, don't display dialog and call `SKStoreReviewController requestReview`.
For the value of `appId`, specify 10 digits for iOS and package name for Android.
When clicked ok button, return value of callback parameter is true. If used `SKStoreReviewController` is always true.

## void NativeUtils.HapticFeedback(NativeUtils.FeedbackType type)
Call iOS HapticFeedback. For Android, emulate with vibration.

## bool NativeUtils.IsHapticFeedbackSupported()
Returns if the device supports HapticFeedback. Always true for Android.
