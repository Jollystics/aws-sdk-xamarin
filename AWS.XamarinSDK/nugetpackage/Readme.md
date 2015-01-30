#How to make .nupkg file

##Xamarin studio  
###(iOS and Android Platform)   


You must set up a environment in which you can build iOS application project (Classic and Unified API) and Android application project. （In Mac OSX, Xamarin Studio would likely be able to build both by default.)

Follow steps 1 to 5 below.

1. Open AWS.XamarinSDK/AWS.XamarinSDK.sln in Xamarin studio.
2. Build AWSSDK_Android project in "Release".
3. Build AWSSDK_iOS-Classic project in "Release".
4. Build AWSSDK_iOS project in "Release".
5. Execute AWS.XamarinSDK/nugetpackage/nupack_ios_android.sh in terminal.

You’ll find the .nupkg file in AWS.XamarinSDK/nugetpackage/