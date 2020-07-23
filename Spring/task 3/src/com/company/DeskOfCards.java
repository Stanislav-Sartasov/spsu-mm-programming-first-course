package com.company;

import java.util.Random;
import java.util.concurrent.ThreadLocalRandom;

public class DeskOfCards {
    private final Card[] deckOfCards = new Card[4 * 8 * 13];

    private static void shuffleArray(Card[] ar) { //Fisher–Yates shuffle
        Random rnd = ThreadLocalRandom.current();
        for (int i = ar.length - 1; i > 0; i--) {
            int index = rnd.nextInt(i + 1);
            String s = ar[index].getSuit();
            String v = ar[index].getValue();
            ar[index] = ar[i];
            ar[i] = new Card(s, v);
        }
    }

    public DeskOfCards() {
        String[] suits = {"Heart", "Club", "Spade", "Diamond"};
        int k = 0;
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 4; j++) {
                deckOfCards[k++] = new Card(suits[j], "2");
                deckOfCards[k++] = new Card(suits[j], "3");
                deckOfCards[k++] = new Card(suits[j], "4");
                deckOfCards[k++] = new Card(suits[j], "5");
                deckOfCards[k++] = new Card(suits[j], "6");
                deckOfCards[k++] = new Card(suits[j], "7");
                deckOfCards[k++] = new Card(suits[j], "8");
                deckOfCards[k++] = new Card(suits[j], "9");
                deckOfCards[k++] = new Card(suits[j], "10");
                deckOfCards[k++] = new Card(suits[j], "J");
                deckOfCards[k++] = new Card(suits[j], "Q");
                deckOfCards[k++] = new Card(suits[j], "K");
                deckOfCards[k++] = new Card(suits[j], "A");
            }
        }
        shuffleArray(deckOfCards);
    }

    public Card[] getDeckOfCards() {
        return deckOfCards;
    }

    public Card getCard(int i) {
        return deckOfCards[i];
    }

}
