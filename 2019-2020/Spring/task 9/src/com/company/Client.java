package com.company;

import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.UnknownHostException;
import java.util.HashSet;
import java.util.Scanner;
import java.util.Set;

public class Client {
    private Scanner scanner = new Scanner(System.in);
    private final Channel channel = new Channel();
    private boolean running = true;

    void setRunning(boolean running) {
        this.running = running;
    }

    public Channel getChannel() {
        return channel;
    }

    void setScanner(Scanner scanner) {
        this.scanner = scanner;
    }

    String getHost() {
        try {
            return InetAddress.getLocalHost().getHostAddress();
        } catch (UnknownHostException e) {
            System.out.println("[Error] Internet connection error");
        }
        return "";
    }

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

    public void start() {
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

        while (channel.bind(getHost(), sourcePort)) {
            System.out.print("> enter your port: ");
            sourcePort = Integer.parseInt(scanner.nextLine());
        }
        InetSocketAddress myAddress = new InetSocketAddress(getHost(), sourcePort);
        System.out.println("> Your address is " + myAddress);

        channel.start();
        channel.setAddresses(updateFriends());
        channel.setReceiveMessage(true);
        channel.send(myAddress.getHostString() + ":" + sourcePort + "~");

        while (running) {

            String msg = "";
            if (scanner.hasNext())
                msg = scanner.nextLine();


            if (msg.isEmpty())
                continue;

            if (msg.equals("--add-connections")) {
                Set<InetSocketAddress> s = updateFriends();
                channel.setAddresses(s);
                /*for (InetSocketAddress a: s)
                    channel.update(String.valueOf(a.getHostString()), String.valueOf(a.getPort()));*/
                continue;
            }

            if (msg.equals("--exit")) {
                String[] address = {String.valueOf(myAddress).split("/")[1]};
                disconnect(address, true);
                scanner.close();
                channel.stop();
                System.out.println("Closed.");
                break;
            }

            if (msg.equals("--list-of-connections")) {
                Set<InetSocketAddress> lst = channel.getAddresses();
                if (lst.isEmpty() || (lst.size() == 1 && lst.contains(myAddress))) {
                    System.out.println("> No connections.");
                    continue;
                }
                System.out.println("> Your connections: ");
                for (InetSocketAddress address: lst) {
                    if (!(address.equals(myAddress)))
                        System.out.println("\t> " + address.getAddress() + ":" + address.getPort());
                }
                System.out.println("> Now you can communicate.");
                continue;
            }

            if (msg.equals("--disconnect")) {
                System.out.println("> enter IP:port to disconnect with (space separated): ");
                String[] input = scanner.nextLine().strip().split(" ");
                disconnect(input, false);
                continue;
            }

            msg = myAddress.getHostString() + ":" + sourcePort + "~[" + name + "]:  " + msg;

           channel.send(msg);
        }
    }

    private void disconnect(String[] input, boolean skip) {
        for (String inputValue: input) {
            if (inputValue.strip().isEmpty())
                continue;
            String[] address = inputValue.strip().split(":");
            if (address.length != 2) {
                System.out.println("[Error] Wrong address, try again (enter it in the format IP:port)");
                disconnect(scanner.nextLine().strip().split(" "), skip);
            }
            channel.disconnect(address[0], address[1], skip);
        }
    }

}