package com.company.player;

public class TheMostCleverPlayer extends Player {

    public TheMostCleverPlayer(int x) {
        super(x);
    }

    @Override
    public String makeMove(int dealersCard) {
        int s = this.sum();
        if (s <= 8)
            return "take";
        if (s <= 12 && !(s == 12 && dealersCard <= 6))
            return "take";
        if (!this.haveA()) {
            if (s <= 16 && dealersCard <= 6)
                return "pass";
            if ((s == 15 || s == 16) && dealersCard >= 10)
                return "pass";
            if (s >= 17)
                return "pass";
            return "take";
        } else {
            if (s <= 17)
                return "take";
            if (s == 18 && dealersCard >= 9)
                return "take";
            return "pass";
        }
    }

    @Override
    public String ifBlackJack() {
        return "wait";
    }
}
