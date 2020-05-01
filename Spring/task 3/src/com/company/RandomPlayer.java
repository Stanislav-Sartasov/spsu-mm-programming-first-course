package com.company;

public class RandomPlayer extends Player {

    public RandomPlayer(int x) {
        super(x);
    }

    @Override
    public String makeMove(int dealersCard) {
        if (this.sum() == 21)
            return "pass";
        double rand = Math.random();
        if (rand < 0.5)
            return "take";
        else
            return "pass";
    }

    @Override
    public String ifBlackJack() {
        return "take";
    }
}

