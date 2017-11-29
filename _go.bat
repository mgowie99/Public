@echo off
set PATH=C:\Windows\System32;C:\Windows;C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319

ECHO Stopping old service ..
NET STOP "My New C# Windows Service"

ECHO Uninstalling old service ..
InstallUtil -u service.exe

ECHO Installing new service ..
InstallUtil -i service.exe

ECHO ---
ECHO Starting new service ..
NET START "My New C# Windows Service"

pause

