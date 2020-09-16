package com.company.player;

public class RandomPlayer extends Player {

    public RandomPlayer(int x) {
        super(x);
    }

    @Override
    public Action makeMove(int dealersCard) {
        if (this.sum() == 21)
            return Action.PASS;
        double rand = Math.random();
        if (rand < 0.5)
            return Action.TAKE;
        else
            return Action.PASS;
    }

    @Override
    public Action ifBlackJack() {
        return Action.TAKE;
    }
}

