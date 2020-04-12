package com.company;

abstract public class tank{
    private int yearOfIssue;
    private int numberOfTowers;
    private int numberOfCrewMembers;
    private double weight;
    private String country;
    private String model;
    private String weightCategory;

    public int getYearOfIssue() {
        return yearOfIssue;
    }

    public int getNumberOfTowers() {
        return numberOfTowers;
    }

    public int getNumberOfCrewMembers() {
        return numberOfCrewMembers;
    }
    public double getWeight() {
        return getWeight();
    }

    public  String getCountry() {
        return country;
    }

    public String getModel() {
        return model;
    }

    public String getWeightCategory() {
        return weightCategory;
    }

    public tank (int year, int towers, int crew, double w,  String nameOfCountry, String name, String category) {
        yearOfIssue = year;
        numberOfTowers = towers;
        numberOfCrewMembers = crew;
        weight = w;
        country = nameOfCountry;
        model = name;
        weightCategory = category;
    }

    public String getTheCharacteristic() {
        String result = "Model: " + model + "\n";
        result += "Year of issue: " + yearOfIssue + "\n";
        result += "Country: " + country + "\n";
        result += "Number of towers: " + numberOfTowers + "\n";
        result += "Number of crew members: " + numberOfCrewMembers + "\n";
        result += "Weight: " + weight + " tons" + "\n";
        result += "Weight category: " + weightCategory + "\n";
        return result;
    }
}

