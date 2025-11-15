<#
=============================================
Employee Management System Launcher (Debug Mode)
Powered by ASP.NET Core + Vue (Windows 11)
=============================================
#>

# === 設定 ===
$backendPath = "$PSScriptRoot\backend"
$frontendPath = "$PSScriptRoot\frontend"
$dbFile = "$backendPath\app.db"
$backendUrl = "http://localhost:5211"
$frontendUrl = "http://localhost:5173"

# ログファイル（標準出力と標準エラー別）
$backendOutLog = "$backendPath\backend_out.log"
$backendErrLog = "$backendPath\backend_err.log"
$frontendOutLog = "$frontendPath\frontend_out.log"
$frontendErrLog = "$frontendPath\frontend_err.log"

Write-Host ""
Write-Host "============================================="
Write-Host " EMPLOYEE MANAGEMENT SYSTEM (Debug Mode)" -ForegroundColor Cyan
Write-Host " Powered by ASP.NET Core + Vue" -ForegroundColor Yellow
Write-Host "============================================="

# === 依存関係のインストール ===
Write-Host ""
Write-Host "Checking and installing dependencies..." -ForegroundColor Cyan

# バックエンド依存関係
if (Test-Path "$backendPath\*.csproj") {
    Write-Host "Restoring .NET dependencies..."
    Push-Location $backendPath
    dotnet restore | Out-Null
    Pop-Location
    Write-Host ".NET restore completed."
}

# フロントエンド依存関係
if (Test-Path "$frontendPath\package.json") {
    Write-Host "Installing npm packages..."
    Push-Location $frontendPath
    npm install --silent | Out-Null
    Pop-Location
    Write-Host "npm install completed."
}

# === DB確認 ===
Write-Host "Database file will be created automatically by .NET (Migrate)."

# === バックエンド起動（標準出力・標準エラー別ログ） ===
Write-Host "Starting backend (ASP.NET Core)..."
$backendProc = Start-Process "dotnet" -ArgumentList "run --urls=$backendUrl" `
    -WorkingDirectory $backendPath `
    -PassThru `
    -NoNewWindow `
    -RedirectStandardOutput $backendOutLog `
    -RedirectStandardError $backendErrLog
Write-Host "Backend started (PID: $($backendProc.Id))"
Write-Host "Check logs at $backendOutLog (stdout) and $backendErrLog (stderr)."

# === フロントエンド起動（標準出力・標準エラー別ログ） ===
Write-Host "Starting frontend (Vue/Vite)..."
$frontendProc = Start-Process "cmd.exe" -ArgumentList "/c npm run dev -- --port 5173" `
    -WorkingDirectory $frontendPath `
    -PassThru `
    -NoNewWindow `
    -RedirectStandardOutput $frontendOutLog `
    -RedirectStandardError $frontendErrLog
Write-Host "Frontend started (PID: $($frontendProc.Id))"
Write-Host "Check logs at $frontendOutLog (stdout) and $frontendErrLog (stderr)."

# === ブラウザ起動 ===
Write-Host "Waiting for servers to start..."
Start-Sleep -Seconds 15
Start-Process $frontendUrl
Write-Host "Browser launched: $frontendUrl"
Write-Host ""

# === 終了待機 ===
Write-Host "============================================="
Write-Host " Type 'Q' and press Enter to stop all servers"
Write-Host "============================================="
Write-Host ""

do {
    $input = Read-Host "Press Q to quit"
} until ($input -eq "Q" -or $input -eq "q")

Write-Host ""
Write-Host "Stopping backend and frontend..."

# === 安全停止 ===
if ($backendProc -and !$backendProc.HasExited) {
    Stop-Process -Id $backendProc.Id -Force -ErrorAction SilentlyContinue
    Write-Host "Backend stopped."
}

if ($frontendProc -and !$frontendProc.HasExited) {
    Stop-Process -Id $frontendProc.Id -Force -ErrorAction SilentlyContinue
    Write-Host "Frontend stopped."
}

Write-Host "All servers stopped. Exiting."
exit
