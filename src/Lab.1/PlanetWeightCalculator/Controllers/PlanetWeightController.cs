using PlanetWeightCalculator.Models;
using PlanetWeightCalculator.Views;

namespace PlanetWeightCalculator.Controllers;

/// <summary>
/// Контроллер в паттерне MVC.
/// Отвечает за:
/// 1. Обработку команд пользователя (нажатие кнопки «Ввести данные»).
/// 2. Управление жизненным циклом дополнительного окна ввода (DataEntryForm).
/// 3. Запоминание последних введённых данных и их восстановление при повторном вводе.
/// 4. Передачу проверенных данных в модель.
/// 5. Перехват ошибок и делегирование показа ошибок представлению.
/// </summary>
public class PlanetWeightController
{
    private readonly PlanetWeightModel _model;
    private readonly IMainView _view;

    /// <summary>
    /// Запоминание последних введённых данных в соответствии с требованием варианта:
    /// «Последние введенные данные нужно запоминать в главной форме и восстанавливать при повторном вводе».
    /// </summary>
    public double? LastEnteredWeight { get; private set; }

    public PlanetWeightController(PlanetWeightModel model, IMainView view)
    {
        _model = model ?? throw new ArgumentNullException(nameof(model));
        _view = view ?? throw new ArgumentNullException(nameof(view));
    }

    /// <summary>
    /// Обрабатывает команду открытия диалога ввода данных.
    /// Восстанавливает последнее введённое значение и при успешном вводе обновляет модель.
    /// </summary>
    public void HandleEnterDataCommand(IWin32Window parentWindow)
    {
        try
        {
            // Создание диалогового окна с восстановлением предыдущего введённого веса
            using var dialog = new DataEntryForm(LastEnteredWeight);

            var result = dialog.ShowDialog(parentWindow);

            if (result == DialogResult.OK && dialog.ResultWeight.HasValue)
            {
                double weight = dialog.ResultWeight.Value;

                // Запоминаем последнее введенное значение
                LastEnteredWeight = weight;

                // Передаем данные в активную модель
                // Модель сама произведёт перерасчёт и вызовет событие ModelChanged
                _model.SetEarthWeight(weight);
            }
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _view.ShowError("Ошибка диапазона", ex.Message);
        }
        catch (Exception ex)
        {
            _view.ShowError("Непредвиденная ошибка", $"Произошла ошибка при обработке данных: {ex.Message}");
        }
    }
}
