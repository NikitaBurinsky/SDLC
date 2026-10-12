namespace PlanetWeightCalculator.Models;

/// <summary>
/// Аргументы события изменения данных в активной модели.
/// </summary>
public class ModelChangedEventArgs : EventArgs
{
    public double EarthWeight { get; }
    public IReadOnlyList<PlanetCalculationResult> Results { get; }

    public ModelChangedEventArgs(double earthWeight, IReadOnlyList<PlanetCalculationResult> results)
    {
        EarthWeight = earthWeight;
        Results = results;
    }
}
