namespace PlanetWeightCalculator.Models;

/// <summary>
/// Результат расчёта веса для одного небесного тела (DTO).
/// </summary>
public class PlanetCalculationResult
{
    public CelestialBody Body { get; }
    public double EarthWeight { get; }
    public double CalculatedWeight { get; }
    public double WeightDifference { get; }
    public double PercentageOfEarthWeight => Body.RelativeGravity * 100.0;

    public PlanetCalculationResult(CelestialBody body, double earthWeight)
    {
        Body = body ?? throw new ArgumentNullException(nameof(body));
        EarthWeight = earthWeight;
        CalculatedWeight = Math.Round(earthWeight * body.RelativeGravity, 2);
        WeightDifference = Math.Round(CalculatedWeight - earthWeight, 2);
    }
}
