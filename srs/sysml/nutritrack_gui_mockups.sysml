package NutriTrackGuiMockups {
    doc
    /*
     * Спецификация пользовательского интерфейса (GUI / Wireframes) на языке SysML v2
     * Автор: Буринский Н.А., гр. 450504
     */

    // Базовые элементы интерфейса
    part def ViewComponent;
    part def NavigationBar :> ViewComponent;
    part def MacroSummaryCard :> ViewComponent;
    part def MealSection :> ViewComponent;
    part def ModalDialog :> ViewComponent;
    part def FormInput :> ViewComponent;
    part def ActionButton :> ViewComponent;

    // Главный экран - Дашборд дневника питания
    part def MainDashboardView :> ViewComponent {
        part navBar : NavigationBar {
            doc /* Верхняя панель: Логотип «NutriTrack», вкладки «Дневник», «База», «Рецепты», «Аналитика», профиль пользователя */
        }
        part dateSelector : ViewComponent {
            doc /* Переключатель дат: выбор дня для просмотра и редактирования рациона */
        }
        part summaryCard : MacroSummaryCard {
            doc /* Карточка сводки: кольцевой/линейный индикатор калорий (Факт / План / Остаток) и полосы прогресса БЖУ */
        }
        part breakfastSection : MealSection { doc /* Блок «Завтрак»: список блюд, граммовка, КБЖУ, кнопка добавления */ }
        part lunchSection : MealSection { doc /* Блок «Обед»: список блюд, граммовка, КБЖУ, кнопка добавления */ }
        part dinnerSection : MealSection { doc /* Блок «Ужин»: список блюд, граммовка, КБЖУ, кнопка добавления */ }
        part snackSection : MealSection { doc /* Блок «Перекусы»: список блюд, граммовка, КБЖУ, кнопка добавления */ }
        part waterWidget : ViewComponent {
            doc /* Виджет гидробаланса: счетчик стаканов (по 250 мл) и прогресс-бар нормы */
        }
    }

    // Модальное окно добавления продукта (с интеграцией Open Food Facts)
    part def AddFoodEntryModal :> ModalDialog {
        part searchInput : FormInput { doc /* Поле ввода для поиска продукта по наименованию или штрихкоду */ }
        part resultsList : ViewComponent { doc /* Выпадающий список найденных продуктов с указанием КБЖУ на 100 г */ }
        part weightInput : FormInput { doc /* Поле ввода размера порции в граммах */ }
        part calculatedPreview : ViewComponent { doc /* Динамический пересчет КБЖУ для указанной массы в реальном времени */ }
        part mealSelector : FormInput { doc /* Выпадающий список выбора приема пищи (Завтрак, Обед, Ужин, Перекус) */ }
        part submitBtn : ActionButton { doc /* Кнопка подтверждения добавления в дневник */ }
    }

    // Экран калькулятора сложных рецептов с учетом уварки/ужарки (Бизнес-изюминка)
    part def RecipeCalculatorView :> ViewComponent {
        part titleInput : FormInput { doc /* Наименование блюда/рецепта */ }
        part ingredientsTable : ViewComponent { doc /* Таблица сырых ингредиентов: компонент, масса, калории, Б, Ж, У */ }
        part addIngredientBtn : ActionButton { doc /* Кнопка поиска и добавления нового компонента в рецепт */ }
        part rawTotalsBanner : ViewComponent { doc /* Суммарный вес сырых продуктов и суммарное КБЖУ до готовки */ }
        part cookedWeightInput : FormInput { doc /* Поле ввода фактического взвешенного веса готового блюда после термообработки */ }
        part shrinkageIndicator : ViewComponent { doc /* Автоматический расчет коэффициента уварки/ужарки в процентах */ }
        part finalNutrientSummary : ViewComponent { doc /* Финальный расчет калорийности и БЖУ на 100 грамм готового блюда */ }
        part saveToUserDbBtn : ActionButton { doc /* Кнопка сохранения блюда в личную базу для быстрого добавления в дневник */ }
    }
}
