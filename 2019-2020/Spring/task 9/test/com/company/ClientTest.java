package com.company;

import org.junit.jupiter.api.Test;

import java.io.ByteArrayInputStream;
import java.net.InetSocketAddress;
import java.util.HashSet;
import java.util.Scanner;
import java.util.Set;

import static org.junit.Assert.assertEquals;

class ClientTest {
    @Test
    void twoPlusThree() {
        Client Alice = new Client();
        String input = "a\n" +
                "8080\n" +
                "\n" ;
        Alice.setScanner(new Scanner(new ByteArrayInputStream(input.getBytes())));
        Alice.setRunning(false);
        Alice.start();

        Client Bob = new Client();
        input = "b\n" +
                "8081\n"
                + Alice.getHost() + ":8080\n";
        Bob.setScanner(new Scanner(new ByteArrayInputStream(input.getBytes())));
        Bob.setRunning(false);
        Bob.start();
        Set<InetSocketAddress> s1 = new HashSet<>();
        s1.add(new InetSocketAddress(Alice.getHost(), 8080));
        s1.add(new InetSocketAddress(Bob.getHost(), 8081));

        Client first = new Client();
        input = "a\n" +
                "8085\n" +
                "\n" ;
        first.setScanner(new Scanner(new ByteArrayInputStream(input.getBytes())));
        first.setRunning(false);
        first.start();

        Client second = new Client();
        input = "b\n" +
                "8086\n"
                + first.getHost() + ":8085\n";
        second.setScanner(new Scanner(new ByteArrayInputStream(input.getBytes())));
        second.setRunning(false);
        second.start();

        Client third = new Client();
        input = "b\n" +
                "8087\n"
                + first.getHost() + ":8086\n";
        third.setScanner(new Scanner(new ByteArrayInputStream(input.getBytes())));
        third.setRunning(false);
        third.start();


        Set<InetSocketAddress> s2 = new HashSet<>();
        s2.add(new InetSocketAddress(first.getHost(), 8085));
        s2.add(new InetSocketAddress(second.getHost(), 8086));
        s2.add(new InetSocketAddress(third.getHost(), 8087));

        assertEquals(Alice.getChannel().getAddresses(), s1);
        assertEquals(Bob.getChannel().getAddresses(), s1);

        assertEquals(first.getChannel().getAddresses(), s2);
        assertEquals(second.getChannel().getAddresses(), s2);
        assertEquals(third.getChannel().getAddresses(), s2);
    }

}