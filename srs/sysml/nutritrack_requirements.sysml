package NutriTrackRequirements {
    doc
    /*
     * Спецификация требований к программной системе «NutriTrack»
     * Язык спецификации: OMG SysML v2
     * Проект: Трекер-калькулятор питания
     * Автор: Буринский Н.А., гр. 450504
     */

    import ScalarValues::*;

    // Корневое системное требование
    requirement def NutriTrackSystemRequirement {
        doc /* Система должна обеспечивать комплексный учет рациона, расчет норм КБЖУ, интеграцию с базой продуктов и калькулятор блюд с учетом уварки. */
        attribute reqId : String = "REQ-SYS-001";
    }

    // 1. Функциональные требования
    requirement def UserProfileAndNormsRequirement {
        doc /* Система должна рассчитывать суточную норму калорий (BMR/TDEE) по формуле Миффлина — Сан Жеора и БЖУ в соответствии с целями. */
        attribute reqId : String = "REQ-FUNC-001";
        attribute bmrFormula : String = "10*weight + 6.25*height - 5*age + s";
    }

    requirement def DailyFoodDiaryRequirement {
        doc /* Система должна вести учет суточных приемов пищи (Завтрак, Обед, Ужин, Перекус) с суммированием нутриентов. */
        attribute reqId : String = "REQ-FUNC-002";
        attribute maxMealsPerDay : Integer = 10;
    }

    requirement def RecipeThermalLossCalculatorRequirement {
        doc /* (Бизнес-изюминка) Система должна рассчитывать итоговую калорийность и БЖУ на 100 г составного блюда с учетом коэффициента термической обработки (уварки/ужарки). */
        attribute reqId : String = "REQ-FUNC-003";
        attribute formula : String = "KcalPer100g = (TotalRawKcal / FinishedCookedWeight) * 100";
    }

    requirement def WaterIntakeTrackingRequirement {
        doc /* Система должна позволять фиксировать потребление воды с шагом 250 мл и отображать прогресс суточной нормы. */
        attribute reqId : String = "REQ-FUNC-004";
    }

    // 2. Внешние интерфейсы и интеграция
    requirement def OpenFoodFactsIntegrationRequirement {
        doc /* Система должна выполнять поиск продуктов по названию и штрихкоду через REST API Open Food Facts с локальным кэшированием. */
        attribute reqId : String = "REQ-INT-001";
        attribute externalApiUrl : String = "https://world.openfoodfacts.org/api/v2";
    }

    // 3. Нефункциональные требования (QoS)
    requirement def ApiResponseTimeRequirement {
        doc /* Время отклика серверного API не должно превышать 200 мс для 95% запросов. */
        attribute reqId : String = "REQ-PERF-001";
        attribute maxLatencyMs : Integer = 200;
    }

    requirement def DataSecurityRequirement {
        doc /* Система должна обеспечивать безопасное хранение паролей (BCrypt), авторизацию на базе JWT токенов и изоляцию данных пользователей. */
        attribute reqId : String = "REQ-SEC-001";
    }

    requirement def ResponsiveWebUIRequirement {
        doc /* Пользовательский интерфейс должен корректно адаптироваться под экраны настольных ПК и мобильных устройств (React SPA). */
        attribute reqId : String = "REQ-PORT-001";
    }

    // Иерархия требований
    requirement rootSystemReq : NutriTrackSystemRequirement {
        requirement profileReq : UserProfileAndNormsRequirement;
        requirement diaryReq : DailyFoodDiaryRequirement;
        requirement recipeCalcReq : RecipeThermalLossCalculatorRequirement;
        requirement waterReq : WaterIntakeTrackingRequirement;
        requirement apiIntReq : OpenFoodFactsIntegrationRequirement;
        requirement perfReq : ApiResponseTimeRequirement;
        requirement secReq : DataSecurityRequirement;
        requirement uiReq : ResponsiveWebUIRequirement;
    }
}
