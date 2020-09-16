package com.company.abstractTank;


abstract public class Tank {
    private final int yearOfIssue;
    private final int numberOfTowers;
    private final int numberOfCrewMembers;
    private final double weight;
    private final String country;
    private final String model;
    private final String weightCategory;

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
        return weight;
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

    public Tank(int year, int towers, int crew, double w, String nameOfCountry, String name, String category) {
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
