@echo off
set FLAG=%1

if "%FLAG%"=="build" (
    cd backend\Training.Docker
    docker compose build
    cd ..\..\frontend
    docker build -t trainingfrontend .
) else if "%FLAG%"=="up" (
    cd deploy
    docker compose up -d
) else if "%FLAG%"=="down" (
    cd deploy
    docker compose down
) else if "%FLAG%"=="be-api" (
    cd backend\Training.Api
    dotnet watch
) else if "%FLAG%"=="fe" (
    cd frontend
    npm run dev
) else if "%FLAG%"=="db" (
    cd deploy\db-only
    docker compose up -d
)

