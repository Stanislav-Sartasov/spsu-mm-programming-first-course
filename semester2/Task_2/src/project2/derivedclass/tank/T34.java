package project2.derivedclass.tank;

import project2.baseclass.tank.Tank;

public class T34 extends Tank
{
    String name = "T_34";
    public int year_of_creation;

    public T34(String country, int weight_in_tons, int crew, float speed_in_km_in_h, int engine_power, int year_of_creation)
    {
        super(country, weight_in_tons, crew, speed_in_km_in_h, engine_power);
        this.year_of_creation = year_of_creation;
    }

    @Override
    public String printTank()
    {
        return ("Name: " + name + "\n"
                + "Country: " + country + "\n"
                + "Weight(in tins): " + weight_in_tons + "\n"
                + "Crew: " + crew + " people" + "\n"
                + "Speed(in km in hour): " + speed_in_km_in_h + "\n"
                + "Power: " + engine_power + "\n"
                + "Year of creation: " + year_of_creation + "\n");
    }
}
