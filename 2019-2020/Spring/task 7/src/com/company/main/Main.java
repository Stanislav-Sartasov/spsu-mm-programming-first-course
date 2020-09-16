package com.company.main;

import com.company.game.*;
import org.springframework.context.annotation.AnnotationConfigApplicationContext;

public class Main {
    private static final AnnotationConfigApplicationContext context = new AnnotationConfigApplicationContext(Config.class);

    public static void main(String[] args) {
        /*Player[] players = {context.getBean("randomPlayer", RandomPlayer.class),
                            context.getBean("moreCleverPlayer", MoreCleverPlayer.class),
                            context.getBean("theMostCleverPlayer", TheMostCleverPlayer.class)};*/
        Game game = context.getBean("game", Game.class);
        game.play();
    }
}

