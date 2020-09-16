package com.company.t34;

import com.company.abstractTank.Tank;
import org.junit.Assert;

public class T34Test {

    @org.junit.Test
    public void getTheCharacteristic() throws Exception {
        Tank firstT34 = new T34();
        Assert.assertEquals(firstT34.getCountry(), "USSR");
        Assert.assertEquals(firstT34.getModel(), "T-34");
        Assert.assertEquals(firstT34.getNumberOfCrewMembers(), 4);
        Assert.assertEquals(firstT34.getNumberOfTowers(), 1);
        Assert.assertEquals(firstT34.getWeightCategory(), "medium tank");
        Assert.assertEquals(firstT34.getYearOfIssue(), 1940);
        Assert.assertEquals(firstT34.getTheCharacteristic(), "Model: T-34\nYear of issue: 1940\n" +
                                                                    "Country: USSR\nNumber of towers: 1\n" +
                                                                    "Number of crew members: 4\n" +
                                                                    "Weight: 27.5 tons\nWeight category: medium tank\n");

        Tank secondT34 = new T34(1943);
        Assert.assertEquals(secondT34.getCountry(), "USSR");
        Assert.assertEquals(secondT34.getModel(), "T-34");
        Assert.assertEquals(secondT34.getNumberOfCrewMembers(), 4);
        Assert.assertEquals(secondT34.getNumberOfTowers(), 1);
        Assert.assertEquals(secondT34.getWeightCategory(), "medium tank");
        Assert.assertEquals(secondT34.getYearOfIssue(), 1943);
        Assert.assertEquals(secondT34.getTheCharacteristic(), "Model: T-34\nYear of issue: 1943\n" +
                                                                    "Country: USSR\nNumber of towers: 1\n" +
                                                                    "Number of crew members: 4\n" +
                                                                    "Weight: 27.5 tons\nWeight category: medium tank\n");

    }

}