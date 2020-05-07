package project2.derivedclass.tank;

import org.junit.Assert;
import org.junit.Test;

class TigerTest
{
    @Test
    public void printTankTest()
    {
        T34 tiger = new T34("USSR", 26, 4, 54.4f, 500, 1942);
        Assert.assertEquals("USSR", tiger.country);
        Assert.assertEquals(26, tiger.weight_in_tons);
        Assert.assertEquals(4, tiger.crew);
        Assert.assertEquals(54.4f, tiger.speed_in_km_in_h, 0.0001);
        Assert.assertEquals(500, tiger.engine_power);
        Assert.assertEquals(1942, tiger.year_of_creation);
        Assert.assertEquals("Name: T_34\n" +
                "Country: USSR\n" +
                "Weight(in tins): 26\n" +
                "Crew: 4 people\n" +
                "Speed(in km in hour): 54.4\n" +
                "Power: 500\n" +
                "Year of creation: 1942\n", tiger.printTank());
    }
}