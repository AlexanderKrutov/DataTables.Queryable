@echo off

set PATH_MSBUILD=C:\Program Files (x86)\MSBuild\14.0\Bin\
"%PATH_MSBUILD%msbuild" DataTables.Queryable.sln /p:Configuration="Release" /p:Platform="Any CPU" /t:Clean;DataTables_Queryable

set BUILD_STATUS=%ERRORLEVEL% 
if %BUILD_STATUS%==0 goto pack 
if not %BUILD_STATUS%==0 goto fail 
 
:fail 
exit /b 1 
 
:pack
.nuget\nuget.exe pack "DataTables.Queryable\DataTables.Queryable.csproj" -Prop Configuration=Release -OutputDirectory "."