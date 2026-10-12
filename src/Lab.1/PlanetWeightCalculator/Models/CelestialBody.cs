namespace PlanetWeightCalculator.Models;

/// <summary>
/// Тип небесного тела.
/// </summary>
public enum CelestialBodyType
{
    Planet,
    DwarfPlanet,
    Moon
}

/// <summary>
/// Информация о небесном теле Солнечной системы и его гравитационных параметрах.
/// </summary>
public class CelestialBody
{
    public string Id { get; }
    public string NameRu { get; }
    public string NameEn { get; }
    public double SurfaceGravity { get; } // м/с^2
    public double RelativeGravity { get; } // относительно Земли (g_planet / g_earth)
    public CelestialBodyType BodyType { get; }
    public string Icon { get; }
    public string Description { get; }

    public CelestialBody(
        string id,
        string nameRu,
        string nameEn,
        double surfaceGravity,
        CelestialBodyType bodyType,
        string icon,
        string description)
    {
        Id = id;
        NameRu = nameRu;
        NameEn = nameEn;
        SurfaceGravity = surfaceGravity;
        RelativeGravity = surfaceGravity / 9.80665;
        BodyType = bodyType;
        Icon = icon;
        Description = description;
    }

    /// <summary>
    /// Возвращает список всех поддерживаемых небесных тел.
    /// </summary>
    public static IReadOnlyList<CelestialBody> GetAllBodies() =>
    [
        new("mercury", "Меркурий", "Mercury", 3.70, CelestialBodyType.Planet, "☿", "Ближайшая к Солнцу планета"),
        new("venus", "Венера", "Venus", 8.87, CelestialBodyType.Planet, "♀", "Вторая планета с плотной атмосферой"),
        new("earth", "Земля", "Earth", 9.80665, CelestialBodyType.Planet, "♁", "Базовая планета отсчёта (1.00 g)"),
        new("moon", "Луна", "Moon", 1.62, CelestialBodyType.Moon, "☾", "Спутник Земли, гравитация в ~6 раз ниже"),
        new("mars", "Марс", "Mars", 3.72, CelestialBodyType.Planet, "♂", "Красная планета с разреженной атмосферой"),
        new("jupiter", "Юпитер", "Jupiter", 24.79, CelestialBodyType.Planet, "♃", "Крупнейший газовый гигант Солнечной системы"),
        new("saturn", "Сатурн", "Saturn", 10.44, CelestialBodyType.Planet, "♄", "Газовый гигант со знаменитыми кольцами"),
        new("uranus", "Уран", "Uranus", 8.69, CelestialBodyType.Planet, "♅", "Ледяной гигант с сильным наклоном оси"),
        new("neptune", "Нептун", "Neptune", 11.15, CelestialBodyType.Planet, "♆", "Самая удалённая планета Солнечной системы"),
        new("pluto", "Плутон", "Pluto", 0.62, CelestialBodyType.DwarfPlanet, "♇", "Карликовая планета в поясе Койпера")
    ];
}
