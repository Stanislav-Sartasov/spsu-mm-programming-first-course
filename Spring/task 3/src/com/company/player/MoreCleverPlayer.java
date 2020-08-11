package com.company.player;

public class MoreCleverPlayer extends Player {

    public MoreCleverPlayer(int x) {
        super(x);
    }

    @Override
    public String makeMove(int dealersCard) {
        if (this.sum() <= 11) {
            return "take";
        }
        if (this.haveA() && this.sum() <= 17) {
            return "take";
        }
        return "pass";
    }

    @Override
    public String ifBlackJack() {
        double rand = Math.random();
        if (rand < 0.5)
            return "take";
        else
            return "wait";
    }
}

