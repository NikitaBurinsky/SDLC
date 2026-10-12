// SysML v2 Architecture Definition
// Лабораторная работа №1 по дисциплине ЖЦРПО
// Архитектурная модель калькулятора веса на планетах (MVC)

package PlanetWeightCalculatorPackage {

    doc /* Описание системы калькулятора веса на планетах с использованием архитектуры MVC */

    // Порты взаимодействия
    port def DataFlowPort;
    port def NotificationPort;
    port def UserActionPort;

    // Определение частей системы (MVC Blocks)
    part def PlanetWeightModel {
        doc /* Активная модель: хранит массу на Земле и выполняет расчет веса для 10 небесных тел */
        attribute earthWeight : ScalarValues::Real;
        attribute isCalculated : ScalarValues::Boolean;

        port notifyOut : NotificationPort;
        
        action def CalculatePlanetWeights {
            in weight : ScalarValues::Real;
            out results : ScalarValues::Real[10];
        }
    }

    part def PlanetWeightView {
        doc /* Графический интерфейс Windows Forms */
        port userActionOut : UserActionPort;
        port notifyIn : NotificationPort;
    }

    part def PlanetWeightController {
        doc /* Контроллер: обрабатывает действия пользователя, управляет модальным окном ввода и передает данные модели */
        attribute lastEnteredWeight : ScalarValues::Real;

        port userActionIn : UserActionPort;
        port modelControlOut : DataFlowPort;
    }

    // Главная композитная система
    part def PlanetWeightCalculatorSystem {
        part model : PlanetWeightModel;
        part view : PlanetWeightView;
        part controller : PlanetWeightController;

        // Архитектурные связи активной модели
        connect view.userActionOut to controller.userActionIn;
        connect controller.modelControlOut to model.notifyOut;
        connect model.notifyOut to view.notifyIn; // Прямое уведомление View от Активной Модели
    }
}
