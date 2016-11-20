@echo off

set PATH_MSBUILD=C:\Program Files (x86)\MSBuild\14.0\Bin\
"%PATH_MSBUILD%msbuild" DataTables.Queryable.proj 

set BUILD_STATUS=%ERRORLEVEL% 
if %BUILD_STATUS%==0 goto end 
if not %BUILD_STATUS%==0 goto fail 
 
:fail 
exit /b 1 

:end
@echo Completed
