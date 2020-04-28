package com.company.markV;

import org.junit.Assert;

public class MkVTest {
    @org.junit.Test
    public void getTheCharacteristic() throws Exception {
        MkV firstMkV = new MkV(28);
        Assert.assertEquals(firstMkV.getCountry(), "United Kingdom");
        Assert.assertEquals(firstMkV.getModel(), "Mark V");
        Assert.assertEquals(firstMkV.getNumberOfCrewMembers(), 8);
        Assert.assertEquals(firstMkV.getNumberOfTowers(), 0);
        Assert.assertEquals(firstMkV.getWeightCategory(), "heavy tank");
        Assert.assertEquals(firstMkV.getYearOfIssue(), 1918);
        Assert.assertEquals(firstMkV.getModification(), "female");
        Assert.assertEquals(firstMkV.getTheCharacteristic(), "Model: Mark V\n" +
                                                                    "Year of issue: 1918\n" +
                                                                    "Country: United Kingdom\n" +
                                                                    "Number of towers: 0\n" +
                                                                    "Number of crew members: 8\n" +
                                                                    "Weight: 28.0 tons\n" +
                                                                    "Weight category: heavy tank\n" +
                                                                    "Modification: female\n");

        MkV secondMkV = new MkV(1920, 30);
        Assert.assertEquals(secondMkV.getCountry(), "United Kingdom");
        Assert.assertEquals(secondMkV.getModel(), "Mark V");
        Assert.assertEquals(secondMkV.getNumberOfCrewMembers(), 8);
        Assert.assertEquals(secondMkV.getNumberOfTowers(), 0);
        Assert.assertEquals(secondMkV.getWeightCategory(), "heavy tank");
        Assert.assertEquals(secondMkV.getYearOfIssue(), 1920);
        Assert.assertEquals(secondMkV.getModification(), "male");
        Assert.assertEquals(secondMkV.getTheCharacteristic(), "Model: Mark V\n" +
                                                                    "Year of issue: 1920\n" +
                                                                    "Country: United Kingdom\n" +
                                                                    "Number of towers: 0\n" +
                                                                    "Number of crew members: 8\n" +
                                                                    "Weight: 30.0 tons\n" +
                                                                    "Weight category: heavy tank\n" +
                                                                    "Modification: male\n");
    }
}