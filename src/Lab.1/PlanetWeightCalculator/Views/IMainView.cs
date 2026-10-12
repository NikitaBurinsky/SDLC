using PlanetWeightCalculator.Models;

namespace PlanetWeightCalculator.Views;

/// <summary>
/// Интерфейс главного представления (Main View).
/// Описывает методы для отображения данных и сообщений, минимизируя связанность с контроллером.
/// </summary>
public interface IMainView
{
    /// <summary>
    /// Отобразить результаты расчёта веса на планетах.
    /// </summary>
    void DisplayResults(double earthWeight, IReadOnlyList<PlanetCalculationResult> results);

    /// <summary>
    /// Показать сообщение об ошибке.
    /// </summary>
    void ShowError(string title, string message);

    /// <summary>
    /// Показать информационное сообщение.
    /// </summary>
    void ShowInfo(string title, string message);
}
