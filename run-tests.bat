@echo off
echo ========================================
echo Water Jug Challenge - Unit Test Runner
echo ========================================
echo.

echo Building solution...
dotnet build water-jug.sln
if %ERRORLEVEL% neq 0 (
    echo Build failed!
    pause
    exit /b 1
)

echo.
echo Running unit tests for WaterJugService...
echo ========================================
dotnet test WaterJugChallenge.Tests/WaterJugChallenge.Tests.csproj --verbosity normal --logger "console;verbosity=normal"

echo.
echo ========================================
echo Unit test execution completed!
echo ========================================
pause 