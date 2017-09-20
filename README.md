# UnityNativeUtils

iOS / Android Native Convenient Functions Summary.


# Usage

Import UnityNativeUtils.unitypackage


# API

## NativeUtils.GetDataDirectory()
Return the data directory of the application. Android returns `getFilesDir`.

## NativeUtils.GetCacheDirectory()
Return the cache directory of the application. Android returns `getCacheDir`.

## NativeUtils.GetVersion()
Return display version of application. Same value as `Application.identifier`.

## NativeUtils.GetBuildNumber()
Return application build number.

## NativeUtils.GetWidth()
Return native view width.

## NativeUtils.GetHeight()
Return native view height.

## NativeUtils.GetScale()
Return display density.

## NativeUtils.ShowWebView(string url)
Open URL. For Android, wrapper for `Application.OpenURL()`.
For iOS, open it with SafariViewController.

## NativeUtils.GetLanguage()
Get language setting from native.

## NativeUtils.Alert(string title, string message)
Show native alert.

## NativeUtils.OpenReview(string appId)
Show app review page.
For iOS 10.3 or later, call `SKStoreReviewController requestReview`.
In other case, open appstore page.
For the value of `appId`, specify 10 digits for iOS and package name for Android.

## NativeUtils.OpenReviewDialog(string appId, string title, string message, string okButton, string cancelButton)
A confirmation dialog is displayed, and if 'OK', the review page is opened.
For iOS 10.3 or later, don't display dialog and call `SKStoreReviewController requestReview`.
For the value of `appId`, specify 10 digits for iOS and package name for Android.


