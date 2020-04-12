package com.company;

import static org.junit.Assert.*;

public class Test {

    @org.junit.Test
    public void main() throws Exception {
        tank firstT_34 = new T_34();
        assertEquals(firstT_34.getCountry(), "USSR");
        assertEquals(firstT_34.getModel(), "T-34");
        assertEquals(firstT_34.getNumberOfCrewMembers(), 4);
        assertEquals(firstT_34.getNumberOfTowers(), 1);
        assertEquals(firstT_34.getWeightCategory(), "medium tank");
        assertEquals(firstT_34.getYearOfIssue(), 1940);

        tank secondT_34 = new T_34(1943);
        assertEquals(secondT_34.getCountry(), "USSR");
        assertEquals(secondT_34.getModel(), "T-34");
        assertEquals(secondT_34.getNumberOfCrewMembers(), 4);
        assertEquals(secondT_34.getNumberOfTowers(), 1);
        assertEquals(secondT_34.getWeightCategory(), "medium tank");
        assertEquals(secondT_34.getYearOfIssue(), 1943);

        MkV firstMkV = new MkV(28);
        assertEquals(firstMkV.getCountry(), "United Kingdom");
        assertEquals(firstMkV.getModel(), "Mark V");
        assertEquals(firstMkV.getNumberOfCrewMembers(), 8);
        assertEquals(firstMkV.getNumberOfTowers(), 0);
        assertEquals(firstMkV.getWeightCategory(), "heavy tank");
        assertEquals(firstMkV.getYearOfIssue(), 1918);
        assertEquals(firstMkV.getModification(), "female");

        MkV secondMkV = new MkV(1920, 30);
        assertEquals(secondMkV.getCountry(), "United Kingdom");
        assertEquals(secondMkV.getModel(), "Mark V");
        assertEquals(secondMkV.getNumberOfCrewMembers(), 8);
        assertEquals(secondMkV.getNumberOfTowers(), 0);
        assertEquals(secondMkV.getWeightCategory(), "heavy tank");
        assertEquals(secondMkV.getYearOfIssue(), 1920);
        assertEquals(secondMkV.getModification(), "male");
    }
}