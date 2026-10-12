namespace PlanetWeightCalculator.Models;

/// <summary>
/// Активная модель MVC.
/// Хранит состояние (вес на Земле и результаты вычислений), выполняет предметную логику
/// и автоматически оповещает подписчиков (View) об изменениях своего состояния через событие ModelChanged.
/// </summary>
public class PlanetWeightModel
{
    private readonly List<CelestialBody> _bodies;
    private double? _earthWeight;
    private readonly List<PlanetCalculationResult> _results = [];

    /// <summary>
    /// Событие активной модели, инициирующее уведомление подписчиков (View).
    /// </summary>
    public event EventHandler<ModelChangedEventArgs>? ModelChanged;

    public double? EarthWeight => _earthWeight;
    public IReadOnlyList<PlanetCalculationResult> Results => _results.AsReadOnly();
    public bool HasData => _earthWeight.HasValue && _results.Count > 0;

    public PlanetWeightModel()
    {
        _bodies = CelestialBody.GetAllBodies().ToList();
    }

    /// <summary>
    /// Устанавливает вес на Земле, проводит валидацию бизнес-правил,
    /// выполняет перерасчёт для всех небесных тел и оповещает подписчиков.
    /// </summary>
    /// <param name="weight">Вес в килограммах.</param>
    public void SetEarthWeight(double weight)
    {
        // Валидация бизнес-правил модели
        if (weight <= 0.0)
        {
            throw new ArgumentOutOfRangeException(nameof(weight), "Вес должен быть строго больше нуля!");
        }

        if (weight > 1000.0)
        {
            throw new ArgumentOutOfRangeException(nameof(weight), "Вес не может превышать 1000 кг (максимально допустимое значение)!");
        }

        _earthWeight = Math.Round(weight, 2);
        _results.Clear();

        foreach (var body in _bodies)
        {
            _results.Add(new PlanetCalculationResult(body, _earthWeight.Value));
        }

        // Оповещение слушателей (принцип Активной Модели в MVC)
        OnModelChanged(new ModelChangedEventArgs(_earthWeight.Value, _results.AsReadOnly()));
    }

    /// <summary>
    /// Метод вызова события изменения модели.
    /// </summary>
    protected virtual void OnModelChanged(ModelChangedEventArgs e)
    {
        ModelChanged?.Invoke(this, e);
    }
}
