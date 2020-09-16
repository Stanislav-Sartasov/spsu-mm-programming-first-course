package com.company.cards;

public class Card {
    private final Suits suit;
    private final Values value;

    public Card(Suits s, Values v) {
        suit = s;
        value = v;
    }

    public Suits getSuit() {
        return suit;
    }

    public Values getValue() {
        return value;
    }

    public int getIntValue() {
        switch (value) {
            case TWO -> {
                return 2;
            }
            case THREE -> {
                return 3;
            }
            case FOUR -> {
                return 4;
            }
            case FIVE -> {
                return 5;
            }
            case SIX -> {
                return 6;
            }
            case SEVEN -> {
                return  7;
            }
            case EIGHT -> {
                return 8;
            }
            case NINE -> {
                return 9;
            }
            case TEN, JACK, QUEEN, KING -> {
                return 10;
            }
            case ACE -> {
                return 11;
            }
            default -> throw new IllegalStateException("Unexpected value: " + value);
        }
    }
}

