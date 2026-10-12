using PlanetWeightCalculator.Controllers;
using PlanetWeightCalculator.Models;
using PlanetWeightCalculator.Views;

namespace PlanetWeightCalculator;

internal static class Program
{
    /// <summary>
    /// Главная точка входа для приложения.
    /// Инициализирует компоненты паттерна MVC, связывает их и запускает графический интерфейс.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // 1. Создание Модели (хранит данные, логику расчётов и событие ModelChanged)
        var model = new PlanetWeightModel();

        // 2. Создание Представления (MainForm) с передачей ссылки на активную модель для подписки
        var mainView = new MainForm(model);

        // 3. Создание Контроллера (обрабатывает действия пользователя и управляет вводом)
        var controller = new PlanetWeightController(model, mainView);

        // 4. Связывание Представления с Контроллером
        mainView.SetController(controller);

        // 5. Запуск цикла обработки сообщений Windows Forms
        Application.Run(mainView);
    }
}
