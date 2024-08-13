namespace WeatherForecastingService.API.Models;

public class ForecastResponse
{
    public string Cod { get; set; }
    public int Message { get; set; }
    public int Cnt { get; set; }
    public List[] List { get; set; }
    public City City { get; set; }
}

public class List
{
    public int Dt { get; set; }
    public Main Main { get; set; }
    public Weather[] Weather { get; set; }
    public Clouds Clouds { get; set; }
    public Wind Wind { get; set; }
    public Sys Sys { get; set; }
    public string Dt_Txt { get; set; }
}

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Coord Coord { get; set; }
    public string Country { get; set; }
    public int Population { get; set; }
    public int Timezone { get; set; }
    public int Sunrise { get; set; }
    public int Sunset { get; set; }
}
