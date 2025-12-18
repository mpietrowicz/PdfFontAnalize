@echo off
setlocal

REM Folder skryptu (bez końcowego \ żeby nie psuć cudzysłowów)
set "ROOT=%~dp0"
if "%ROOT:~-1%"=="\" set "ROOT=%ROOT:~0,-1%"

set "IMAGE=pdfpoc:1.0.15"
set "OUT=%ROOT%\results"

if not exist "%OUT%" mkdir "%OUT%"

docker build -t %IMAGE% -f "%ROOT%\Dockerfile" "%ROOT%"
if errorlevel 1 exit /b %errorlevel%

docker run --rm -v "%OUT%:/data" %IMAGE%

endlocal
pause