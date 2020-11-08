package com.company;

public class Main {
    public static void main(String[] args) {
        System.out.println("INFO:");
        System.out.println("After entering your name, port and friends IP:port, you can use the following commands:");
        System.out.println("\t--exit to close chat");
        System.out.println("\t--add-connections to add new connections");
        System.out.println("\t--list-of-connections to find out who you are chatting with");
        System.out.println("\t--disconnect to remove someone from your chat");
        Client client = new Client();
        client.start();
    }
}
