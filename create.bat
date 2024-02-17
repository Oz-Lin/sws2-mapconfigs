@REM Ensure script runs from its own folder
pushd "%~dp0"

@REM Install package
dotnet new install SwiftlyS2.CS2.PluginTemplate || exit /b %ERRORLEVEL%

@REM Ask for project name
set /p PROJECT_NAME="Enter your project name (no spaces): "
if "%PROJECT_NAME%"=="" (
    echo Project name cannot be empty.
    popd
    exit /b 1
)

@REM Ask for project description
set /p PROJECT_DESCRIPTION="Enter your project description: "
if "%PROJECT_DESCRIPTION%"=="" (
    echo Project description cannot be empty.
    popd
    exit /b 1
)

@REM Default values
set AUTHOR="criskkky"
set VERSION="1.0.0"

@REM Create project in temp folder
set "TMPFOLDER=__tmp_%PROJECT_NAME%__"
if exist "%TMPFOLDER%" rd /s /q "%TMPFOLDER%"
dotnet new swplugin -n "%PROJECT_NAME%" -o "%TMPFOLDER%" --PluginName "%PROJECT_NAME%" --PluginAuthor "%AUTHOR%" --PluginVersion "%VERSION%" --PluginDescription "%PROJECT_DESCRIPTION%" || (popd & exit /b %ERRORLEVEL%)

@REM Copy everything except README.md
robocopy "%TMPFOLDER%" "." /E /MT:8 /XF README.md >nul

@REM If doesn't exist README.md in destination, copy the one from the template
if not exist "README.md" if exist "%TMPFOLDER%\\README.md" copy "%TMPFOLDER%\\README.md" . >nul

@REM Delete temp folder
rd /S /Q "%TMPFOLDER%"

@REM Create Metadata.cs file in src folder
if not exist "src" mkdir "src"
(
    echo using SwiftlyS2.Shared;
    echo.
    echo namespace %PROJECT_NAME%;
    echo.
    echo [PluginMetadata^(
    echo     Id = "%PROJECT_NAME%",
    echo     Version = "%VERSION%",
    echo     Name = "%PROJECT_NAME%",
    echo     Author = "%AUTHOR%",
    echo     Description = "%PROJECT_DESCRIPTION%",
    echo ^)]
    echo public partial class %PROJECT_NAME%;
) > "src\\Metadata.cs"
