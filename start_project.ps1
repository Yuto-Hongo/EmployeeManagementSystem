<#
=============================================
Employee Management System Launcher
Powered by ASP.NET Core + Vue (Windows 11)
=============================================
#>

# === 設定 ===
$backendPath = "$PSScriptRoot\backend"
$frontendPath = "$PSScriptRoot\frontend"
$dbFile = "$backendPath\app.db"
$backendUrl = "http://localhost:5211"
$frontendUrl = "http://localhost:5173"

Write-Host ""
Write-Host "============================================="
Write-Host " EMPLOYEE MANAGEMENT SYSTEM" -ForegroundColor Cyan
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

# === バックエンド起動（非表示） ===
Write-Host "Starting backend (ASP.NET Core)..."
$backendProc = Start-Process "dotnet" -ArgumentList "run --urls=$backendUrl" `
    -WorkingDirectory $backendPath `
    -WindowStyle Hidden -PassThru
Write-Host "Backend started (PID: $($backendProc.Id))"

# === フロントエンド起動（非表示） ===
Write-Host "Starting frontend (Vue/Vite)..."
$frontendProc = Start-Process "npm" -ArgumentList "run dev -- --port 5173" `
    -WorkingDirectory $frontendPath `
    -WindowStyle Hidden -PassThru
Write-Host "Frontend started (PID: $($frontendProc.Id))"
Write-Host ""

# === ブラウザ起動 ===
Write-Host "Waiting for servers to start..."
Start-Sleep -Seconds 5
Start-Process $frontendUrl
Write-Host "Browser launched: $frontendUrl"
Write-Host ""

# === 終了待機 ===
Write-Host "============================================="
Write-Host " Press 'Q' to stop all servers"
Write-Host "============================================="
Write-Host ""

do {
    $key = [Console]::ReadKey($true)
} until ($key.Key -eq "Q")

Write-Host ""
Write-Host "Stopping backend and frontend..."

# === 安全停止 ===
Stop-Process -Id $backendProc.Id -Force -ErrorAction SilentlyContinue

try {
    $childNodePids = Get-CimInstance Win32_Process |
        Where-Object { $_.ParentProcessId -eq $frontendProc.Id -and $_.Name -eq "node.exe" } |
        Select-Object -ExpandProperty ProcessId

    foreach ($pid in $childNodePids) {
        Stop-Process -Id $pid -Force -ErrorAction SilentlyContinue
    }

    Stop-Process -Id $frontendProc.Id -Force -ErrorAction SilentlyContinue
}
catch {
    Write-Host "Failed to stop frontend completely (already exited?)."
}

Write-Host "All services stopped."
Start-Sleep -Seconds 2
exit
