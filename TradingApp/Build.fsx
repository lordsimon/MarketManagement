// include Fake lib
#r @"packages\FAKE.2.4.8.0\tools\FakeLib.dll"
open Fake

// Properties
let buildDir = @"C:\cygwin\home\Simon\CodeRepo\buildOutput"

// Targets
Target "Clean" (fun _ ->
    CleanDir buildDir
)


Target "BuildApp" (fun _ ->
    !!(__SOURCE_DIRECTORY__+ @"\TradingApp\" + @"/*.fsproj") 
      |> MSBuildRelease buildDir "Build"
      |> Log "AppBuild-Output: "
)

// define test dlls
let testDir = @"TradingSystemTests\bin\Debug\"
let testDlls = !! (testDir + "/TradingSystemTests.*.dll")

Target "NUnitTest" (fun _ ->
    testDlls
        |> NUnit (fun p -> 
            {p with
                DisableShadowCopy = true; 
                OutputFile = testDir + "TestResults.xml"})
)

Target "Default" (fun _ ->
    trace "Hello World from FAKE"
)

// Dependencies
"Clean"
  ==> "BuildApp"
  ==> "Default"
  ==> "NUnitTest"

// start build
RunTargetOrDefault "NUnitTest"

    