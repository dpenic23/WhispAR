Firebase Unity SDK
==================

The Firebase Unity SDK provides Unity packages for the following Firebase
features on *iOS* and *Android*:

| Feature                            | Unity Package                     |
|:----------------------------------:|:---------------------------------:|
| Firebase Analytics                 | FirebaseAnalytics.unitypackage    |
| Firebase Authentication            | FirebaseAuth.unitypackage         |
| Firebase Realtime Database         | FirebaseDatabase.unitypackage     |
| Firebase Invites and Dynamic Links | FirebaseInvites.unitypackage      |
| Firebase Messaging                 | FirebaseMessaging.unitypackage    |
| Firebase Realtime Database         | FirebaseDatabase.unitypackage     |
| Firebase Remote Config             | FirebaseRemoteConfig.unitypackage |
| Firebase Storage                   | FirebaseStorage.unitypackage      |

## AdMob

The AdMob Unity plugin is distributed separately and is available from the
[AdMob Get Started](https://firebase.google.com/docs/admob/unity/start) guide.

## Stub Implementations

Stub (non-functional) implementations are provided for convenience when
building for Windows, OSX and Linux so that you don't need to conditionally
compile code when also targeting the desktop.

Setup
-----

You need to follow the
[SDK setup instructions](https://firebase.google.com/preview/unity).
Each Firebase package requires configuration in the
[Firebase Console](https://firebase.google.com/console).  If you fail to
configure your project your app's initialization will fail.

Support
-------

[Firebase Support](http://firebase.google.com/support/)

Release Notes
-------------

## 1.1.0
  - Overview
    - Added support for Firebase Storage and bug fixes.
  - Changes
    - Added support for Firebase Storage.
    - Fixed crash in Firebase Analytics when logging arrays of parameters.
    - Fixed crash in Firebase Messaging when receiving messages with empty
      payloads on Android.
    - Fixed random hang when initializing Firebase Messaging on iOS.
    - Fixed topic subscriptions in Firebase Messaging.
    - Fixed an issue that resulted in a missing app icon when using Firebase
      Messaging on Android.
    - Fixed exception in error message construction when FirebaseApp
      initialization fails.
    - Fixed reporting of null events in the Firebase Realtime Database.
    - Fixed unsubscribe for complex queries in the Firebase Realtime Database.
    - Fixed service account authentication in the Firebase Realtime Database.
    - Fixed Firebase.Database.Unity being stripped from iOS builds.
    - Fixed support for building with Firebase plugins in Microsoft
      Visual Studio.
    - Fixed scene transitions causing event routing to break across all
      components.
    - Changed editor plugins for Firebase Authentication and Invites to
      return success for all operations instead of raising exceptions.
    - Changed editor plugin to read JAVA_HOME from the Unity editor
      preferences.
    - Changed editor plugin to scan all google-services.json and
      GoogleService-Info.plist files in the project and select the config file
      matching the project's current bundle ID.
    - Improved the performance of AAR / JAR resolution when the Android config
      is selected and auto-resolution is enabled.
    - Improved error messages in the editor plugin.
  - Known Issues
    - Proguard is not integrated into Android builds. We have distributed
      proguard files that can be manually integrated into Android builds
      within AAR files matching the following pattern in each
      Unity package:
      `Firebase/m2repository/com/google/firebase/firebase-*-unity/*firebase-*.srcaar`
    - Incompatible AARs are not resolved correctly when building for Android.
      This can require manual intervention when using multiple plugins
      (e.g Firebase + AdMob + Google Play Games).  A workaround is documented
      on the
      [AdMob Unity plugin issue tracker](https://github.com/googleads/googleads-mobile-unity/issues/314).

## 1.0.1
  - Overview
    - Bug fixes.
  - Changes
    - Fixed Realtime Database restricted access from the Unity Editor on
      Windows.
    - Fixed load and build errors when iOS support is not installed.
    - Fixed an issue that prevented the creation of multiple FirebaseApp
      instances and customization of the default instance on iOS.
    - Removed all dependencies on Python for Android resource generation on
      Windows.
    - Fixed an issue with pod tool discovery when the Ruby Gem binary directory
      is modified from the default location.
    - Fixed problems when building for Android with the IL2CPP scripting
      backend.
  - Known Issues
    - Proguard is not integrated into Android builds. We have distributed
      proguard files that can be manually integrated into Android builds
      within AAR files matching the following pattern in each
      Unity package:
      `Firebase/m2repository/com/google/firebase/firebase-*-unity/*firebase-*.srcaar`

## 1.0.0
  - Overview
    - First public release with support for Firebase Analytics,
      Authentication, Real-time Database, Invites, Dynamic Links and
      Remote Config.
      See our
      [setup guide](https://firebase.google.com/docs/unity/setup) to
      get started.
  - Known Issues
    - Proguard is not integrated into Android builds.  We have distributed
      proguard files that can be manually integrated into Android builds
      within AAR files matching the following pattern in each
      Unity package:
      `Firebase/m2repository/com/google/firebase/firebase-*-unity/*firebase-*.srcaar`
