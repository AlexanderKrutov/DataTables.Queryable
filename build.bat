@echo off

set PROJECT_PATH=DataTables.Queryable.proj 

for /f "usebackq tokens=1* delims=: " %%i in (`.nuget\vswhere.exe -latest -requires Microsoft.Component.MSBuild`) do (
  if /i "%%i"=="installationPath" set InstallDir=%%j
)

if exist "%InstallDir%\MSBuild\15.0\Bin\MSBuild.exe" (
  "%InstallDir%\MSBuild\15.0\Bin\MSBuild.exe" %PROJECT_PATH%
) else (
  echo MSBuild Not Found!
  goto fail 
)

set BUILD_STATUS=%ERRORLEVEL% 
if %BUILD_STATUS%==0 goto end 
if not %BUILD_STATUS%==0 goto fail 
 
:fail 
exit /b 1 

:end
@echo Completed
