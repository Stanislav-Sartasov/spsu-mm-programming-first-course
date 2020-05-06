package project2.derivedclass.tank;

import project2.baseclass.tank.Tank;
public class Tiger extends Tank
{
    String name = "Tiger";
    int number_of_tanks_released;

    public Tiger(String country, int weight_in_tons, int crew, float speed_in_km_in_h, int engine_power, int number_of_tanks_released)
    {
        super(country, weight_in_tons, crew, speed_in_km_in_h, engine_power);
        this.number_of_tanks_released = number_of_tanks_released;
    }

    @Override
    public String printTank()
    {
        return("Name: " + name + "\n"
                + "Country: " + country + "\n"
                + "Weight(in tins): " + weight_in_tons + "\n"
                + "Crew: " + crew + " people" + "\n"
                + "Speed(in km in hour): " + speed_in_km_in_h + "\n"
                + "Power: " + engine_power + "\n"
                + "Number of tanks released: " + number_of_tanks_released + "\n");
    }
}