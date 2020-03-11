#import <StoreKit/StoreKit.h>
#import <Foundation/Foundation.h>
#import <SafariServices/SafariServices.h>

#ifdef __cplusplus
extern "C" {
#endif

typedef void (*_NativeUtils_cs_callback_method)(void *, bool);

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

float _NativeUtils_getSafeAreaTop() {
    if ([UIWindow instancesRespondToSelector:@selector(safeAreaInsets)]) {
        return [UIApplication sharedApplication].keyWindow.safeAreaInsets.top;
    }
    return 0;
}

float _NativeUtils_getSafeAreaBottom() {
    if ([UIWindow instancesRespondToSelector:@selector(safeAreaInsets)]) {
        return [UIApplication sharedApplication].keyWindow.safeAreaInsets.bottom;
    }
    return 0;
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

typedef NS_ENUM(NSInteger, Magicant_NativeUtils_UIFeedbackType) {
    ImpactLight = 0,
    ImpactMedium,
    ImpactHeavy,
    NotificationSuccess,
    NotificationWarning,
    NotificationError,
    Selection,
};

bool _NativeUtils_isHapticfeedbackSupported(Magicant_NativeUtils_UIFeedbackType type) {
    return (NSClassFromString(@"UIImpactFeedbackGenerator") != nil);
}

void _NativeUtils_hapticfeedback(Magicant_NativeUtils_UIFeedbackType type) {
    if (!NSClassFromString(@"UIImpactFeedbackGenerator")) return;
    
    switch (type) {
        case Magicant_NativeUtils_UIFeedbackType::ImpactLight:
        case Magicant_NativeUtils_UIFeedbackType::ImpactMedium:
        case Magicant_NativeUtils_UIFeedbackType::ImpactHeavy: {
            UIImpactFeedbackGenerator *generator = nil;
            switch (type) {
                case Magicant_NativeUtils_UIFeedbackType::ImpactLight:
                    generator = [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleLight];
                    break;
                case Magicant_NativeUtils_UIFeedbackType::ImpactMedium:
                    generator = [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleMedium];
                    break;
                case Magicant_NativeUtils_UIFeedbackType::ImpactHeavy:
                    generator = [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleHeavy];
                    break;
                default:
                    break;
            }
            [generator prepare];
            [generator impactOccurred];
            break;
        }
            
        case Magicant_NativeUtils_UIFeedbackType::NotificationSuccess:
        case Magicant_NativeUtils_UIFeedbackType::NotificationWarning:
        case Magicant_NativeUtils_UIFeedbackType::NotificationError: {
            UINotificationFeedbackGenerator *generator = [[UINotificationFeedbackGenerator alloc] init];
            [generator prepare];
            switch (type) {
                case Magicant_NativeUtils_UIFeedbackType::NotificationSuccess:
                    [generator notificationOccurred:UINotificationFeedbackTypeSuccess];
                    break;
                case Magicant_NativeUtils_UIFeedbackType::NotificationWarning:
                    [generator notificationOccurred:UINotificationFeedbackTypeWarning];
                    break;
                case Magicant_NativeUtils_UIFeedbackType::NotificationError:
                    [generator notificationOccurred:UINotificationFeedbackTypeError];
                    break;
                default:
                    break;
            }
            break;
        }
            
        case Magicant_NativeUtils_UIFeedbackType::Selection: {
            UISelectionFeedbackGenerator *generator = [[UISelectionFeedbackGenerator alloc] init];
            [generator prepare];
            [generator selectionChanged];
        }
    }
}
    
#ifdef __cplusplus
}
#endif

