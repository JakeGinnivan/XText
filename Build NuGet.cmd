@echo off

start /wait notepad %~dp0src\XText\XText.nuspec
%~dp0src\.nuget\nuget.exe pack %~dp0src\XText\XText.csproj -build

pause