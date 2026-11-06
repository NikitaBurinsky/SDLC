#!/usr/bin/env bash
# ==============================================================================
# Скрипт автоматического ответа Менеджера проекта на Pull Request Заказчика
# Автор: Буринский Н.А. (гр. 450504)
# ==============================================================================

set -e

REPO="NikitaBurinsky/SDLC"
PR_NUMBER="$1"

echo "=========================================================="
echo " [NutriTrack] Автоматический ответ Менеджера проекта на PR"
echo "=========================================================="

if ! command -v gh &> /dev/null; then
    echo "[-] GitHub CLI (gh) не найден. Вы можете скопировать ответ вручную из PM_RESPONSE_TEMPLATE.md"
    exit 1
fi

if [ -z "$PR_NUMBER" ]; then
    echo "[+] Поиск открытых PR в $REPO..."
    PR_NUMBER=$(gh pr list --repo "$REPO" --state open --json number --jq '.[0].number')

    if [ -z "$PR_NUMBER" ] || [ "$PR_NUMBER" = "null" ]; then
        echo "[-] Открытых Pull Request сейчас не найдено."
        echo "    Как только напарник создаст PR, запустите: bash reply_to_pr.sh"
        exit 0
    fi
fi

TEMPLATE_FILE="PM_RESPONSE_TEMPLATE.md"
if [ ! -f "$TEMPLATE_FILE" ]; then
    TEMPLATE_FILE="$(dirname "$0")/PM_RESPONSE_TEMPLATE.md"
fi

echo "[+] Публикуем официальный ответ Менеджера проекта в PR #$PR_NUMBER..."
gh pr comment "$PR_NUMBER" --repo "$REPO" --body-file "$TEMPLATE_FILE"
echo "[✓] Ответ успешно опубликован!"

echo "[+] Выполняем слияние (Merge) PR #$PR_NUMBER..."
gh pr merge "$PR_NUMBER" --repo "$REPO" --merge

echo "[+] Обновляем локальный репозиторий..."
git checkout lab3
git pull origin lab3

echo ""
echo "=========================================================="
echo " [УСПЕХ] PR #$PR_NUMBER принят, ответ опубликован в GitHub!"
echo " Ссылка: https://github.com/$REPO/pull/$PR_NUMBER"
echo "=========================================================="
