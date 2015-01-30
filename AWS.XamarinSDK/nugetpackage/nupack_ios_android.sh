#!/bin/bash

#!echo Building for iOS Classic...
#!/Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool -v build --configuration:Release ./AWSSDK_iOS/AWSSDK_iOS.csproj

#!echo Building for iOS Unified API...
#!/Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool -v build --configuration:Release ./AWSSDK_iOS/AWSSDK_iOS_Unified.csproj

#!echo Building for Android...
#!/Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool -v build --configuration:Release ./AWSSDK_Android/AWSSDK_Android.csproj

echo packaging...
nuget pack AWSSDK-IOS_Android.nuspec -BasePath ../
echo end
