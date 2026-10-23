package NutriTrackUseCases {
    doc
    /*
     * Модель прецедентов (Use Case) системы «NutriTrack»
     * Язык спецификации: OMG SysML v2
     * Автор: Буринский Н.А., гр. 450504
     */

    // Определение акторов
    part def User {
        doc /* Зарегистрированный пользователь приложения, отслеживающий свой рацион питания. */
    }

    part def OpenFoodFactsServer {
        doc /* Внешний публичный сервис базы данных продуктов питания. */
    }

    // Определение вариантов использования
    use case def RegisterAndSetupProfile {
        doc /* Регистрация учетной записи, ввод физических параметров (вес, рост, возраст, активность) и расчет норм TDEE/КБЖУ. */
    }

    use case def LogFoodIntake {
        doc /* Добавление продукта или блюда в один из приемов пищи (Завтрак, Обед, Ужин, Перекус) с указанием веса. */
    }

    use case def SearchProductInDatabase {
        doc /* Поиск пищевого продукта по названию или штрихкоду с получением нутриентов. */
    }

    use case def SyncWithOpenFoodFacts {
        doc /* Запрос к внешнему API Open Food Facts при отсутствии продукта в локальной базе. */
    }

    use case def CalculateCookedRecipe {
        doc /* Ввод сырых ингредиентов, фактического веса готового блюда и расчет калорийности на 100 г готового продукта. */
    }

    use case def TrackWaterIntake {
        doc /* Фиксация выпитой воды в течение дня и проверка выполнения гидробаланса. */
    }

    use case def ViewNutritionalAnalytics {
        doc /* Просмотр суточных и недельных графиков калорийности, соотношения белков/жиров/углеводов. */
    }

    // Контекст системы с акторами и прецедентами
    part nutrisystemContext {
        part user : User;
        part offServer : OpenFoodFactsServer;

        use case uc1 : RegisterAndSetupProfile;
        use case uc2 : LogFoodIntake;
        use case uc3 : SearchProductInDatabase;
        use case uc4 : SyncWithOpenFoodFacts;
        use case uc5 : CalculateCookedRecipe;
        use case uc6 : TrackWaterIntake;
        use case uc7 : ViewNutritionalAnalytics;
    }
}
