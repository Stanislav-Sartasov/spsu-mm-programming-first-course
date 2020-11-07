package com.company;

import java.net.UnknownHostException;

public class Main {
    public static void main(String[] args) throws UnknownHostException {
        System.out.println("INFO:");
        System.out.println("After entering your name, port and friends IP:port, you can use the following commands:");
        System.out.println("\t--exit to close chat");
        System.out.println("\t--add-connections to add new connections");
        System.out.println("\t--list-of-connections to find out who you are chatting with");
        Client client = new Client();
        client.start();
    }
}
