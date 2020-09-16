package com.company.player;

public class MoreCleverPlayer extends Player {

    public MoreCleverPlayer(int x) {
        super(x);
    }

    @Override
    public Action makeMove(int dealersCard) {
        if (this.sum() <= 11) {
            return Action.TAKE;
        }
        if (this.haveA() && this.sum() <= 17) {
            return Action.TAKE;
        }
        return Action.PASS;
    }

    @Override
    public Action ifBlackJack() {
        double rand = Math.random();
        if (rand < 0.5)
            return Action.TAKE;
        else
            return Action.WAIT;
    }
}

