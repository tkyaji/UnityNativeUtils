#import <StoreKit/StoreKit.h>
#import "NativeUtils.h"

#ifdef __cplusplus
extern "C" {
#endif
    static inline const char *__copyString(NSString *str) {
        const char *adNameStr = [str UTF8String];
        char *retStr = (char *)malloc(strlen(adNameStr) + 1);
        strcpy(retStr, adNameStr);
        return retStr;
    }
    
    const char *_NativeUtils_getShortVersionName() {
        NSString *bundleVersion = [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleShortVersionString"];
        return __copyString(bundleVersion);
    }
    
    const char *_NativeUtils_getVersionName() {
        NSString *bundleVersion = [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleVersion"];
        return __copyString(bundleVersion);
    }
    
    float _NativeUtils_getWidth() {
        return UnityGetMainWindow().frame.size.width;
    }
    
    float _NativeUtils_getHeight() {
        return UnityGetMainWindow().frame.size.height;
    }
    
    float _NativeUtils_getScale() {
        return [[UIScreen mainScreen] scale];
    }
    
    void _NativeUtils_showWebView(const char *urlStr) {
        NSURL *url = [NSURL URLWithString:[NSString stringWithUTF8String:urlStr]];
        SFSafariViewController *safariViewController = [[SFSafariViewController alloc] initWithURL:url];
        [UnityGetGLViewController() presentViewController:safariViewController animated:YES completion:nil];
    }
    
    const char *_NativeUtils_getLanguage() {
        NSArray *languages = [NSLocale preferredLanguages];
        NSString *lang = [[languages objectAtIndex:0] substringToIndex:2];
        return __copyString(lang);
    }
    
    void __NativeUtils_openReviewOnBrowser(NSString *appId) {
        NSString *url = [NSString stringWithFormat:@"https://itunes.apple.com/app/id%@?action=write-review", appId];
        NSURL *nsURL = [NSURL URLWithString:url];
        if ([[UIApplication sharedApplication] canOpenURL:nsURL]) {
            if ([[[UIDevice currentDevice] systemVersion] floatValue] < 10.0) {
                [[UIApplication sharedApplication] openURL:nsURL];
            } else {
                [[UIApplication sharedApplication] openURL:nsURL options:@{} completionHandler:nil];
            }
        }
    }
    
    void _NativeUtils_openReview(const char *appId) {
        if (NSClassFromString(@"SKStoreReviewController")) {
            [SKStoreReviewController requestReview];
        } else {
            __NativeUtils_openReviewOnBrowser([NSString stringWithUTF8String:appId]);
        }
    }
    
    void _NativeUtils_openReviewDialog(const char *appId, const char *title, const char *message, const char *okButton, const char *cancelButton, void *callbackAction, _NativeUtils_cs_callback_method callbackMethod) {
        NSString *appIdStr = [NSString stringWithUTF8String:appId];
        if (NSClassFromString(@"SKStoreReviewController")) {
            [SKStoreReviewController requestReview];
            callbackMethod(callbackAction, true);
        } else {
            UIAlertController *alertController = [UIAlertController alertControllerWithTitle:[NSString stringWithUTF8String:title]
                                                                                     message:[NSString stringWithUTF8String:message]
                                                                              preferredStyle:UIAlertControllerStyleAlert];
            UIAlertAction *ok = [UIAlertAction actionWithTitle:[NSString stringWithUTF8String:okButton]
                                                         style:UIAlertActionStyleDefault
                                                       handler:^(UIAlertAction *action) {
                                                           __NativeUtils_openReviewOnBrowser(appIdStr);
                                                           callbackMethod(callbackAction, true);
                                                       }];
            UIAlertAction *cancel = [UIAlertAction actionWithTitle:[NSString stringWithUTF8String:cancelButton]
                                                             style:UIAlertActionStyleCancel
                                                           handler:^(UIAlertAction *action) {
                                                               callbackMethod(callbackAction, false);
                                                           }];
            [alertController addAction:ok];
            [alertController addAction:cancel];
            [UnityGetGLViewController() presentViewController:alertController animated:YES completion:nil];
        }
    }
    
    void _NativeUtils_alert(const char *title, const char *message) {
        UIAlertController *alertController = [UIAlertController alertControllerWithTitle:[NSString stringWithUTF8String:title]
                                                                                 message:[NSString stringWithUTF8String:message]
                                                                          preferredStyle:UIAlertControllerStyleAlert];
        UIAlertAction *ok = [UIAlertAction actionWithTitle:@"OK"
                                                     style:UIAlertActionStyleDefault
                                                   handler:nil];
        [alertController addAction:ok];
        [UnityGetGLViewController() presentViewController:alertController animated:YES completion:nil];
    }
    
#ifdef __cplusplus
}
#endif

