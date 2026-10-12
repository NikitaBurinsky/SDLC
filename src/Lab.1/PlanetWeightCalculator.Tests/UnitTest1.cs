using PlanetWeightCalculator.Models;

namespace PlanetWeightCalculator.Tests;

public class PlanetWeightCalculatorTests
{
    [Fact]
    public void GetAllBodies_Returns10CelestialBodies()
    {
        var bodies = CelestialBody.GetAllBodies();
        Assert.Equal(10, bodies.Count);
        Assert.Contains(bodies, b => b.Id == "earth" && Math.Abs(b.RelativeGravity - 1.0) < 0.001);
        Assert.Contains(bodies, b => b.Id == "moon");
        Assert.Contains(bodies, b => b.Id == "jupiter");
    }

    [Theory]
    [InlineData(70.0)]
    [InlineData(85.5)]
    [InlineData(100.0)]
    public void ActiveModel_CalculatesCorrectWeights_AndFiresEvent(double earthWeight)
    {
        var model = new PlanetWeightModel();
        bool eventFired = false;
        double firedEarthWeight = 0;
        IReadOnlyList<PlanetCalculationResult>? firedResults = null;

        model.ModelChanged += (s, e) =>
        {
            eventFired = true;
            firedEarthWeight = e.EarthWeight;
            firedResults = e.Results;
        };

        model.SetEarthWeight(earthWeight);

        Assert.True(eventFired);
        Assert.Equal(earthWeight, firedEarthWeight);
        Assert.NotNull(firedResults);
        Assert.Equal(10, firedResults.Count);

        // Земля должна весить столько же
        var earthResult = firedResults.First(r => r.Body.Id == "earth");
        Assert.Equal(earthWeight, earthResult.CalculatedWeight);

        // Юпитер должен весить больше (g ~ 24.79, ratio ~ 2.527)
        var jupiterResult = firedResults.First(r => r.Body.Id == "jupiter");
        Assert.True(jupiterResult.CalculatedWeight > earthWeight * 2.5);

        // Луна должна весить меньше (ratio ~ 0.165)
        var moonResult = firedResults.First(r => r.Body.Id == "moon");
        Assert.True(moonResult.CalculatedWeight < earthWeight * 0.2);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(-15.0)]
    [InlineData(1000.1)]
    [InlineData(5000.0)]
    public void ActiveModel_InvalidWeight_ThrowsArgumentOutOfRangeException(double invalidWeight)
    {
        var model = new PlanetWeightModel();
        Assert.Throws<ArgumentOutOfRangeException>(() => model.SetEarthWeight(invalidWeight));
    }

    [Fact]
    public void PassiveModel_CalculatesWithoutFiringEvents()
    {
        var passiveModel = new PassivePlanetWeightModel();
        passiveModel.Calculate(80.0);

        Assert.Equal(80.0, passiveModel.EarthWeight);
        Assert.Equal(10, passiveModel.Results.Count);
    }

    [Fact]
    public void CombinedController_ProcessesValidAndInvalidInput()
    {
        var controller = new CombinedPlanetWeightController();
        string? errorMessage = null;
        IReadOnlyList<PlanetCalculationResult>? successResults = null;

        // Валидный ввод с запятой
        controller.ProcessInputAndCalculate(
            "75,5",
            err => errorMessage = err,
            res => successResults = res);

        Assert.Null(errorMessage);
        Assert.NotNull(successResults);
        Assert.Equal(75.5, controller.Weight);

        // Невалидный ввод
        errorMessage = null;
        controller.ProcessInputAndCalculate(
            "abc",
            err => errorMessage = err,
            res => successResults = res);

        Assert.NotNull(errorMessage);
    }
}
