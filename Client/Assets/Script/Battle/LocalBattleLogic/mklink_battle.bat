@echo off
>nul 2>&1 "%SYSTEMROOT%\system32\cacls.exe" "%SYSTEMROOT%\system32\config\system"
if '%errorlevel%' NEQ '0' (
goto UACPrompt
) else ( goto gotAdmin )
:UACPrompt
echo Set UAC = CreateObject^("Shell.Application"^) > "%temp%\getadmin.vbs"
echo UAC.ShellExecute "%~s0", "", "", "runas", 1 >> "%temp%\getadmin.vbs"
"%temp%\getadmin.vbs"
exit /B
:gotAdmin
if exist "%temp%\getadmin.vbs" ( del "%temp%\getadmin.vbs" )

cd /d %~dp0



FOR /D %%i IN ("BattleLogic\\*") DO RD /S /Q "%%i" & DEL /Q "BattleLogic\\*.*"
RD /Q "BattleLogic"

mklink /d BattleLogic ..\..\..\..\..\001_GameFramework_Battle\BattleProject\BattleLogic
pause