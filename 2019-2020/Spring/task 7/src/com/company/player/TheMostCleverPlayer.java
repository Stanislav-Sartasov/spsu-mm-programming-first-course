package com.company.player;

public class TheMostCleverPlayer extends Player {

    public TheMostCleverPlayer(int x) {
        super(x);
    }

    @Override
    public Action makeMove(int dealersCard) {
        int s = this.sum();
        if (s <= 8)
            return Action.TAKE;
        if (s <= 12 && !(s == 12 && dealersCard <= 6))
            return Action.TAKE;
        if (!this.haveA()) {
            if (s <= 16 && dealersCard <= 6)
                return Action.PASS;
            if ((s == 15 || s == 16) && dealersCard >= 10)
                return Action.PASS;
            if (s >= 17)
                return Action.PASS;
            return Action.TAKE;
        } else {
            if (s <= 17)
                return Action.TAKE;
            if (s == 18 && dealersCard >= 9)
                return Action.TAKE;
            return Action.PASS;
        }
    }

    @Override
    public Action ifBlackJack() {
        return Action.WAIT;
    }
}
