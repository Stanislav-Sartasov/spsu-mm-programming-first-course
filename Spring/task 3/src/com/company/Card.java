package com.company;

public class Card {
    private String suit;
    private String value;

    public Card(String s, String v) {
        suit = s;
        value = v;
    }

    public String getSuit() {
        return suit;
    }

    public String getValue() {
        return value;
    }
}

