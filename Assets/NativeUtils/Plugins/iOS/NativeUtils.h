#import <Foundation/Foundation.h>
#import <SafariServices/SafariServices.h>

const char *_NativeUtils_getShortVersionName();
const char *_NativeUtils_getVersionName();
float _NativeUtils_getWidth();
float _NativeUtils_getHeight();
float _NativeUtils_getScale();
void _NativeUtils_showWebView(const char *urlStr);
const char *_NativeUtils_getLanguage();
void _NativeUtils_openReview(const char *appId);
void _NativeUtils_openReviewDialog(const char *appId, const char *title, const char *message, const char *okButton, const char *cancelButton);
void _NativeUtils_alert(const char *title, const char *message);
