#!/usr/bin/env bash
# Startet alle Anwendungen des Maintenance Systems

set -e

PROJECT_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

echo "=== Maintenance System — Start ==="

# Backend
echo "[1/2] Starte Backend (ASP.NET Core) auf http://localhost:5000 ..."
cd "$PROJECT_ROOT"
dotnet run --project src/MaintenanceSystem.API/MaintenanceSystem.API.csproj --urls "http://localhost:5000" &
BACKEND_PID=$!

# Kurz warten bis Backend bereit ist
sleep 3

# Web Frontend
echo "[2/2] Starte Web Frontend (Vue/Vite) auf http://localhost:5173 ..."
cd "$PROJECT_ROOT/web"
npm run dev &
FRONTEND_PID=$!

echo ""
echo "=== Alle Dienste gestartet ==="
echo "  Backend:  http://localhost:5000"
echo "  Frontend: http://localhost:5173"
echo ""
echo "Zum Beenden: Ctrl+C"

# Auf Ctrl+C warten und beide Prozesse beenden
trap "echo ''; echo 'Beende alle Dienste...'; kill $BACKEND_PID $FRONTEND_PID 2>/dev/null; exit 0" INT TERM

wait
