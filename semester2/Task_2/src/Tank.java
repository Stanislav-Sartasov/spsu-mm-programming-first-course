public abstract class Tank
{
    public String country;
    public int weight_in_tons;
    public int crew;
    public float speed_in_km_in_h;
    public int engine_power;

    public Tank(String country, int weight_in_tons, int crew, float speed_in_km_in_h, int engine_power)
    {
        this.country = country;
        this.weight_in_tons = weight_in_tons;
        this.crew = crew;
        this.speed_in_km_in_h = speed_in_km_in_h;
        this.engine_power = engine_power;
    }

    public abstract  void printTank();
}
