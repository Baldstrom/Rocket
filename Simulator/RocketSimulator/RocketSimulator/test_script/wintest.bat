@ECHO OFF
:: This batch file tests all STLs in bin
ECHO.
TITLE SIMULATOR CMDLN TEST
FOR %%i IN (..\bin\Debug\*.stl) DO (
	START /B /WAIT "" "..\bin\Debug\RocketSimulator.exe" -stl %%i
	ECHO.
) 
PAUSE