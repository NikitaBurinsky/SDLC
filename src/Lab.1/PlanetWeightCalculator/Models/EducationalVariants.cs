namespace PlanetWeightCalculator.Models;

/// <summary>
/// Демонстрационный класс: Пассивная модель MVC (для сравнительного анализа в отчёте).
/// В отличие от активной модели, пассивная модель НЕ генерирует событий и не уведомляет View.
/// Контроллер после вызова модели обязан сам запросить результаты и обновить View.
/// </summary>
public class PassivePlanetWeightModel
{
    public double? EarthWeight { get; private set; }
    public List<PlanetCalculationResult> Results { get; } = [];

    public void Calculate(double weight)
    {
        if (weight <= 0 || weight > 1000)
            throw new ArgumentOutOfRangeException(nameof(weight), "Недопустимый вес");

        EarthWeight = weight;
        Results.Clear();
        foreach (var body in CelestialBody.GetAllBodies())
        {
            Results.Add(new PlanetCalculationResult(body, weight));
        }
        // ВНИМАНИЕ: Нет вызова событий! View ничего не знает об изменении автоматически.
    }
}

/// <summary>
/// Демонстрационный класс: Контроллер, совмещённый с пассивной моделью
/// (упрощённый архитектурный вариант согласно заданию).
/// Здесь логика вычислений, хранение состояния и управление представлением объединены в одном классе.
/// </summary>
public class CombinedPlanetWeightController
{
    private double? _weight;
    private readonly List<PlanetCalculationResult> _results = [];

    public double? Weight => _weight;
    public IReadOnlyList<PlanetCalculationResult> Results => _results;

    public void ProcessInputAndCalculate(string rawInput, Action<string> onError, Action<IReadOnlyList<PlanetCalculationResult>> onSuccess)
    {
        if (string.IsNullOrWhiteSpace(rawInput))
        {
            onError("Значение веса не может быть пустым!");
            return;
        }

        string normalized = rawInput.Trim().Replace(',', '.');
        if (!double.TryParse(normalized, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double parsedWeight))
        {
            onError("Введено некорректное числовое значение!");
            return;
        }

        if (parsedWeight <= 0 || parsedWeight > 1000)
        {
            onError("Вес должен быть в диапазоне от 0.1 до 1000 кг!");
            return;
        }

        _weight = parsedWeight;
        _results.Clear();
        foreach (var body in CelestialBody.GetAllBodies())
        {
            _results.Add(new PlanetCalculationResult(body, parsedWeight));
        }

        onSuccess(_results);
    }
}
