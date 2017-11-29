@echo off
echo.
REM .. goto LIB

echo Compiling ..
set PATH=C:\Windows\System32;C:\Windows;C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319
REM csc /nologo /t:library constants.cs
csc /nologo /t:library install.cs
csc /nologo /r:install.dll service.cs 
goto EXIT

:EXIT
echo Done ..
REM .. pause
