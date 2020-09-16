package com.company.cards;

import java.util.Random;
import java.util.concurrent.ThreadLocalRandom;

public class DeskOfCards {
    private final Card[] deckOfCards = new Card[4 * 8 * 13];

    private static void shuffleArray(Card[] ar) { //Fisher–Yates shuffle
        Random rnd = ThreadLocalRandom.current();
        for (int i = ar.length - 1; i > 0; i--) {
            int index = rnd.nextInt(i + 1);
            Suits s = ar[index].getSuit();
            Values v = ar[index].getValue();
            ar[index] = ar[i];
            ar[i] = new Card(s, v);
        }
    }

    public DeskOfCards() {
        Suits[] suits = {Suits.CLUB, Suits.DIAMOND, Suits.HEART, Suits.SPADE};
        int k = 0;
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 4; j++) {
                deckOfCards[k++] = new Card(suits[j], Values.TWO);
                deckOfCards[k++] = new Card(suits[j], Values.THREE);
                deckOfCards[k++] = new Card(suits[j], Values.FOUR);
                deckOfCards[k++] = new Card(suits[j], Values.FIVE);
                deckOfCards[k++] = new Card(suits[j], Values.SIX);
                deckOfCards[k++] = new Card(suits[j], Values.SEVEN);
                deckOfCards[k++] = new Card(suits[j], Values.EIGHT);
                deckOfCards[k++] = new Card(suits[j], Values.NINE);
                deckOfCards[k++] = new Card(suits[j], Values.TEN);
                deckOfCards[k++] = new Card(suits[j], Values.JACK);
                deckOfCards[k++] = new Card(suits[j], Values.QUEEN);
                deckOfCards[k++] = new Card(suits[j], Values.KING);
                deckOfCards[k++] = new Card(suits[j], Values.ACE);
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
