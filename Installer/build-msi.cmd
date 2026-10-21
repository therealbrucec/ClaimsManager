@echo off
rem ---------------------------------------------------------------------------
rem  Builds Claims Manager in Release, then packages it as an MSI.
rem
rem  Run from a "Developer Command Prompt for VS 2022" so msbuild is on the path.
rem  The WiX tool is needed once per machine:
rem
rem      dotnet tool install --global wix
rem
rem  WiX v7 refuses to run until you accept the Open Source Maintenance Fee EULA
rem  (error WIX7015). This script does NOT accept it for you, because that is a
rem  licensing decision. Pick one of:
rem
rem    a) Accept it for the build, by setting this first:
rem           set ACCEPT_WIX_EULA=1
rem       which passes  -acceptEula wix7  to wix.exe. Override the id with
rem       WIX_EULA_ID if a later WiX version uses a different one.
rem    b) Accept it once per user, per machine. See the "accept the EULA"
rem       section of https://docs.firegiant.com/wix/osmf/
rem    c) Use WiX v5, which predates the fee entirely:
rem           dotnet tool uninstall --global wix
rem           dotnet tool install --global wix --version 5.*
rem ---------------------------------------------------------------------------
setlocal enabledelayedexpansion

rem Work from the project root, whatever directory this was launched from.
cd /d "%~dp0.."

echo.
echo === Building ClaimsManager.exe (Release) ===
where msbuild >nul 2>nul
if errorlevel 1 (
    echo.
    echo msbuild was not found on the path.
    echo Open a "Developer Command Prompt for VS 2022" and run this script again.
    goto :failed
)

msbuild ClaimsManager.sln /p:Configuration=Release /v:minimal /nologo
if errorlevel 1 goto :failed

if not exist "bin\Release\ClaimsManager.exe" (
    echo.
    echo bin\Release\ClaimsManager.exe was not produced. Nothing to package.
    goto :failed
)

echo.
echo === Locating the WiX build tool ===

rem A global dotnet tool lands in %USERPROFILE%\.dotnet\tools, and that directory is
rem added to the *user* PATH when the tool is first installed. An already-open command
rem prompt keeps its old copy of PATH, so the tool is found by looking for it directly
rem rather than trusting the current session.
set "WIX_EXE="

where wix >nul 2>nul && set "WIX_EXE=wix"

if not defined WIX_EXE if exist "%USERPROFILE%\.dotnet\tools\wix.exe" (
    set "WIX_EXE=%USERPROFILE%\.dotnet\tools\wix.exe"
)
if not defined WIX_EXE if defined DOTNET_TOOLS_PATH if exist "%DOTNET_TOOLS_PATH%\wix.exe" (
    set "WIX_EXE=%DOTNET_TOOLS_PATH%\wix.exe"
)
if not defined WIX_EXE if exist "%LOCALAPPDATA%\Microsoft\dotnet\tools\wix.exe" (
    set "WIX_EXE=%LOCALAPPDATA%\Microsoft\dotnet\tools\wix.exe"
)

if not defined WIX_EXE (
    echo.
    echo The WiX build tool could not be found.
    echo.
    echo   1. Install it, if you have not already:
    echo          dotnet tool install --global wix
    echo.
    echo   2. Confirm it is installed:
    echo          dotnet tool list --global
    echo.
    echo   3. Looked for wix.exe in:
    echo          the current PATH
    echo          %USERPROFILE%\.dotnet\tools
    echo          %LOCALAPPDATA%\Microsoft\dotnet\tools
    echo.
    echo      If it is somewhere else, add that folder to PATH and try again,
    echo      or simply open a new Developer Command Prompt.
    goto :failed
)

echo   every wix.exe on PATH, in the order Windows finds them:
for /f "delims=" %%W in ('where wix 2^>nul') do echo       %%W
echo   using: !WIX_EXE!

for /f "delims=" %%V in ('"!WIX_EXE!" --version 2^>nul') do set "WIX_VERSION=%%V"
echo   version: !WIX_VERSION!

rem WiX v6 introduced the Open Source Maintenance Fee and v7 enforces it, so a v6 or v7
rem tool will refuse to build until the EULA is accepted.
set "WIX_GATED="
if "!WIX_VERSION:~0,2!"=="6." set "WIX_GATED=1"
if "!WIX_VERSION:~0,2!"=="7." set "WIX_GATED=1"

if defined WIX_GATED if not "%ACCEPT_WIX_EULA%"=="1" (
    echo.
    echo   Heads up: this is WiX !WIX_VERSION!, which will not build until the Open Source
    echo   Maintenance Fee EULA is accepted. If you meant to be on WiX v5, the list above
    echo   shows where the newer one is coming from - check with:
    echo       dotnet tool list --global
    echo   A copy installed by something other than dotnet, Scoop or winget for instance,
    echo   will sit earlier on PATH and win.
    echo.
)

rem Opt in explicitly by setting ACCEPT_WIX_EULA=1. Deliberately not the default.
rem -acceptEula takes the EULA id as its value, so it is passed as a pair.
if not defined WIX_EULA_ID set "WIX_EULA_ID=wix7"
set "EULA_FLAG="
if /i "%ACCEPT_WIX_EULA%"=="1" (
    set "EULA_FLAG=-acceptEula %WIX_EULA_ID%"
    echo   ACCEPT_WIX_EULA is set, passing -acceptEula %WIX_EULA_ID%
)

echo.
echo === Building the MSI ===
rem x86 on purpose: the Jet database provider is 32-bit only, so the application runs as
rem a 32-bit process and belongs under Program Files (x86).
set "WIX_LOG=%TEMP%\claimsmanager-wix-build.log"
"!WIX_EXE!" build !EULA_FLAG! Installer\ClaimsManager.wxs -arch x86 -o Installer\ClaimsManager.msi > "!WIX_LOG!" 2>&1
set "WIX_RESULT=!errorlevel!"
type "!WIX_LOG!"

if not "!WIX_RESULT!"=="0" (
    rem Only offer the EULA advice if that is genuinely what went wrong.
    findstr /c:"WIX7015" "!WIX_LOG!" >nul 2>nul && goto :eulafailed
    goto :failed
)
del "!WIX_LOG!" >nul 2>nul

echo.
echo === Done ===
echo   Plain executable : bin\Release\ClaimsManager.exe
echo   Installer        : Installer\ClaimsManager.msi
echo.
echo === For Intune ===
echo   Install command   : msiexec /i "ClaimsManager.msi" /qn /norestart
echo   Uninstall command : msiexec /x "{C7782104-92E8-41CD-AEC4-1B4F46B8E184}" /qn /norestart
echo   Install behaviour : System
echo   Detection rule    : MSI, product code {C7782104-92E8-41CD-AEC4-1B4F46B8E184}
echo   Applicable OS     : 64-bit (the package itself is 32-bit, which is intended)
echo.
echo   The product code is pinned in ClaimsManager.wxs, so it survives a rebuild.
echo   Change it when you change ProductVersion.
echo.
exit /b 0

:eulafailed
echo.
echo WiX is waiting for you to accept the Open Source Maintenance Fee EULA.
echo It is not accepted automatically here, because whether the fee applies to
echo you is a licensing question, not a build setting. Three ways forward:
echo.
echo   1. Accept it for the build:
echo          set ACCEPT_WIX_EULA=1
echo          Installer\build-msi.cmd
echo      which passes  -acceptEula %WIX_EULA_ID%  to wix.exe.
echo.
echo   2. Accept it once per user, per machine. See the "accept the EULA"
echo      section of the page below.
echo.
echo   3. Use WiX v5, which predates the fee:
echo          dotnet tool uninstall --global wix
echo          dotnet tool install --global wix --version 5.*
echo.
echo   Terms and payment status: https://docs.firegiant.com/wix/osmf/
echo.
goto :failed

:failed
echo.
echo *** BUILD FAILED ***
exit /b 1
