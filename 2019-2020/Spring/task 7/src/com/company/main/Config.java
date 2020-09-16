package com.company.main;

import com.company.cards.DeskOfCards;
import com.company.game.Game;
import com.company.player.*;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.Scope;

@Configuration
public class Config {
    @Bean
    @Scope("prototype")
    public RandomPlayer randomPlayer() {
        return new RandomPlayer(800);
    }

    @Bean
    @Scope("prototype")
    public MoreCleverPlayer moreCleverPlayer() {
        return new MoreCleverPlayer(800);
    }

    @Bean
    @Scope("prototype")
    public TheMostCleverPlayer theMostCleverPlayer() {
        return new TheMostCleverPlayer(800);
    }

    @Bean
    @Scope("prototype")
    public Game game() {
        Player[] players = {randomPlayer(), moreCleverPlayer(), theMostCleverPlayer()};
        return new Game(players);
    }

    @Bean
    @Scope("prototype")
    public Dealer dealer() {
        return new Dealer();
    }

    @Bean
    @Scope("prototype")
    public DeskOfCards deskOfCards() {
        return new DeskOfCards();
    }


}