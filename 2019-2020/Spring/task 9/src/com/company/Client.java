package com.company;

import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.UnknownHostException;
import java.util.HashSet;
import java.util.Scanner;
import java.util.Set;

public class Client {
    private final Scanner scanner = new Scanner(System.in);

    private Set<InetSocketAddress> updateFriends() {
        Set<InetSocketAddress> addresses = new HashSet<>();

        System.out.println("> enter IP:port numbers of your friends (space separated) or press enter to skip:");
        String input = scanner.nextLine().strip();
        String[] inputValues = input.split(" ");
        for (String inputValue : inputValues) {
            if (inputValue.strip().isEmpty())
                continue;
            String[] address = inputValue.strip().split(":");
            if (address.length != 2) {
                System.out.println("[Error] Wrong address, try again (enter it in the format IP:port)");
                return updateFriends();
            }
            addresses.add(new InetSocketAddress(address[0], Integer.parseInt(address[1])));
        }
        System.out.println("> Now you can communicate.");
        return addresses;
    }

    public void start() throws UnknownHostException {
        String host = String.valueOf(InetAddress.getByName("localhost")).split("/")[1];
        System.out.print("> enter your name: ");
        String name = scanner.nextLine().strip();
        while (name.isEmpty()) {
            System.out.print("[Error] Name can't be empty, please enter it again: ");
            name = scanner.nextLine().strip();
        }

        System.out.print("> enter your port: ");
        String line = scanner.nextLine().strip();
        while (line.isEmpty()) {
            System.out.print("[Error] Port can't be empty, please enter it again: ");
            line = scanner.nextLine().strip();
        }
        int sourcePort = Integer.parseInt(line);
        Channel channel = new Channel();
        while (channel.bind(sourcePort)) {
            System.out.print("> enter your port: ");
            sourcePort = Integer.parseInt(scanner.nextLine());
        }

        channel.start();
        channel.setAddresses(updateFriends());
        channel.setReceiveMessage(true);
        channel.send(host + ":" + sourcePort + "~");

        while (true) {
            String msg = scanner.nextLine();

            if (msg.isEmpty())
                continue;

            if (msg.equals("--add-connections")) {
                Set<InetSocketAddress> s = updateFriends();
                for (InetSocketAddress a: s)
                    channel.update(String.valueOf(a.getHostName()), String.valueOf(a.getPort()));
                continue;
            }

            if (msg.equals("--exit")) {
                break;
            }

            if (msg.equals("--list-of-connections")) {
                Set<InetSocketAddress> lst = channel.getAddresses();
                if (lst.isEmpty() || (lst.size() == 1 && lst.contains(new InetSocketAddress("localhost", sourcePort)))) {
                    System.out.println("> No connections.");
                    continue;
                }
                System.out.println("> Your connections: ");
                for (InetSocketAddress address: lst) {
                    if (!(sourcePort == address.getPort() && address.getAddress() == InetAddress.getByName("localhost")))
                        System.out.println("\t> " + address.getAddress() + ":" + address.getPort());
                }
                System.out.println("> Now you can communicate.");
                continue;
            }

            msg = host + ":" + sourcePort + "~[" + name + "]:  " + msg;

           channel.send(msg);
        }

        scanner.close();
        channel.stop();

        System.out.println("Closed.");
    }

}