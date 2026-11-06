# ==============================================================================
# Скрипт автоматического ответа Менеджера проекта на Pull Request Заказчика
# Автор: Буринский Н.А. (гр. 450504)
# ==============================================================================

param (
    [int]$PrNumber = 0,
    [switch]$AutoMerge = $true
)

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host " [NutriTrack] Автоматический ответ Менеджера проекта на PR" -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Cyan

# 1. Проверка наличия GitHub CLI
if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
    Write-Host "[-] GitHub CLI (gh) не найден. Вы можете скопировать ответ вручную из файла PM_RESPONSE_TEMPLATE.md" -ForegroundColor Yellow
    Exit 1
}

# 2. Поиск открытых PR
$repo = "NikitaBurinsky/SDLC"

if ($PrNumber -eq 0) {
    Write-Host "[+] Поиск открытых Pull Request в репозитории $repo..." -ForegroundColor Green
    $openPrsJson = gh pr list --repo $repo --state open --json number,title,headRefName,author | ConvertFrom-Json

    if (-not $openPrsJson -or $openPrsJson.Count -eq 0) {
        Write-Host "[-] Открытых Pull Request сейчас не найдено." -ForegroundColor Yellow
        Write-Host "    Как только напарник нажмет 'Create pull request' на GitHub, запустите этот скрипт снова:" -ForegroundColor White
        Write-Host "    .\reply_to_pr.ps1" -ForegroundColor Cyan
        Write-Host ""
        Write-Host "    Или укажите номер PR вручную: .\reply_to_pr.ps1 -PrNumber <НОМЕР>" -ForegroundColor Gray
        Exit 0
    }

    if ($openPrsJson.Count -eq 1) {
        $PrNumber = $openPrsJson[0].number
        $prTitle = $openPrsJson[0].title
        $prAuthor = $openPrsJson[0].author.login
        Write-Host "[+] Найден открытый PR #$PrNumber: '$prTitle' от @$prAuthor" -ForegroundColor Green
    } else {
        Write-Host "[?] Найдено несколько открытых PR. Выберите номер:" -ForegroundColor Yellow
        foreach ($pr in $openPrsJson) {
            Write-Host "    PR #$($pr.number): $($pr.title) (ветка: $($pr.headRefName))" -ForegroundColor White
        }
        $PrNumber = Read-Host "Введите номер PR для ответа"
    }
}

$templatePath = Join-Path $PSScriptRoot "PM_RESPONSE_TEMPLATE.md"
if (-not (Test-Path $templatePath)) {
    $templatePath = "PM_RESPONSE_TEMPLATE.md"
}

if (-not (Test-Path $templatePath)) {
    Write-Host "[-] Файл шаблона ответа PM_RESPONSE_TEMPLATE.md не найден!" -ForegroundColor Red
    Exit 1
}

# 3. Отправка официального ответа в PR
Write-Host "[+] Отправляем официальный ответ Менеджера проекта в PR #$PrNumber..." -ForegroundColor Green
gh pr comment $PrNumber --repo $repo --body-file $templatePath

Write-Host "[✓] Ответ успешно опубликован в Pull Request #$PrNumber!" -ForegroundColor Green

# 4. Принятие (Merge) PR
if ($AutoMerge) {
    Write-Host "[+] Выполняем слияние (Merge) PR #$PrNumber в базовую ветку..." -ForegroundColor Green
    gh pr merge $PrNumber --repo $repo --merge --auto=false
    Write-Host "[✓] Pull Request #$PrNumber успешно принят и объединен (Merged)!" -ForegroundColor Green

    # 5. Синхронизация локальной ветки lab3
    Write-Host "[+] Синхронизируем локальный репозиторий..." -ForegroundColor Green
    git checkout lab3
    git pull origin lab3
    Write-Host "[✓] Локальная ветка lab3 обновлена!" -ForegroundColor Green
}

Write-Host ""
Write-Host "==========================================================" -ForegroundColor Green
Write-Host " ВСЕ ШАГИ ВЫПОЛНЕНЫ! Замечания отработаны и зафиксированы " -ForegroundColor Green
Write-Host "==========================================================" -ForegroundColor Green
Write-Host "Ссылка на PR: https://github.com/$repo/pull/$PrNumber" -ForegroundColor Cyan
Write-Host ""
