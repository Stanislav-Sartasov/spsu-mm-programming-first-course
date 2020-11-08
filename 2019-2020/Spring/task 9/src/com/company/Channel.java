package com.company;

import java.io.IOException;
import java.net.*;
import java.util.HashSet;
import java.util.Set;

public class Channel implements Runnable {
    private DatagramSocket socket;
    private boolean running;
    private boolean receiveMessage;
    private Set<InetSocketAddress> addresses;
    private InetSocketAddress myAddress;

    public void setReceiveMessage(boolean f) {
        receiveMessage = f;
    }

    public Set<InetSocketAddress> getAddresses() {
        return addresses;
    }

    public void setAddresses(Set<InetSocketAddress> a) {
        for (InetSocketAddress address: a) {
            if (address.isUnresolved()) {
                System.out.println("[Error] Wrong address " + address);
                continue;
            }
            byte[] buffer = ("?" + String.valueOf(myAddress).split("/")[1]).getBytes();
            DatagramPacket packet = new DatagramPacket(buffer, buffer.length);
            packet.setSocketAddress(address);
            try {
                socket.send(packet);
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
        try {
            Thread.sleep(100 * a.size());
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        for (InetSocketAddress address: a) {
            if (!addresses.contains(address))
                System.out.println("[Error] No user with such address (" + address.getPort() +
                        "). If you are not mistaken in writing the address, ask him to connect and then add it again.");
        }
    }

    public boolean bind(String host, int port) {
        try {
            myAddress = new InetSocketAddress(host, port);
            socket = new DatagramSocket(port);
            addresses = new HashSet<>();
            return false;
        } catch (SocketException e) {
            System.out.println("[Error] This port is busy, try again.");
            return true;
        }
    }

    public void start() {
        receiveMessage = false;
        Thread thread = new Thread(this);
        thread.start();
    }

    public void stop() {
        running = false;
        socket.close();
    }

    protected void update(String host, String port) {
        InetSocketAddress newConnection = new InetSocketAddress(host, Integer.parseInt(port));
        if (!addresses.contains(newConnection)) {
            for (InetSocketAddress address: addresses) {
                byte[] buf = (address.getHostString() + ":" + address.getPort() + "~").getBytes();
                DatagramPacket p = new DatagramPacket(buf, buf.length);

                p.setSocketAddress(newConnection);
                try {
                    socket.send(p);
                } catch (IOException e) {
                    e.printStackTrace();
                }

                if (address.equals(myAddress))
                    continue;

                buf = (host + ":" + port + "~").getBytes();
                p = new DatagramPacket(buf, buf.length);

                p.setSocketAddress(address);
                try {
                    socket.send(p);
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
            if (!(myAddress.equals(newConnection)) && !addresses.contains(newConnection)) {
                System.out.println(">>> Connected to " + host + ":" + port);
                addresses.add(newConnection);
            }
        }
    }

    @Override
    public void run() {
        byte[] buffer = new byte[1024];
        DatagramPacket packet = new DatagramPacket(buffer, buffer.length);

        running = true;
        while(running) {
            try {
                socket.receive(packet);
                String msg = new String(buffer, 0, packet.getLength());
                if (msg.isEmpty())
                    continue;

                if (msg.charAt(0) == '?') {
                    byte[] b = ("!" +  String.valueOf(myAddress).split("/")[1]).getBytes();
                    DatagramPacket p = new DatagramPacket(b, b.length);
                    p.setSocketAddress(new InetSocketAddress(msg.substring(1).split(":")[0],
                                            Integer.parseInt(msg.split(":")[1])));
                    try {
                        socket.send(p);
                    } catch (IOException e) {
                        e.printStackTrace();
                    }
                    continue;
                }

                if (msg.charAt(0) == '!') {
                    update(msg.substring(1).split(":")[0], msg.split(":")[1]);
                    continue;
                }

                if (msg.charAt(0) == '-') {
                    msg = msg.substring(1);
                    String[] a = msg.split(":");
                    InetSocketAddress del = new InetSocketAddress(a[0], Integer.parseInt(a[1]));
                    addresses.remove(del);
                    continue;
                }

                String port = msg.split("~")[0];
                update(port.split(":")[0], port.split(":")[1]);
                if (msg.split("~").length <= 1)
                    continue;
                String s = msg.split("~")[1].strip();
                if (!s.isEmpty() && receiveMessage)
                    System.out.println(s);
            }
            catch (IOException e) {
                break;
            }
        }
    }

    public void send(String msg) {
        byte[] buffer = msg.getBytes();
        String port = msg.split("~")[0];
        String[] a = port.split(":");
        update(a[0], a[1]);
        DatagramPacket packet = new DatagramPacket(buffer, buffer.length);
        for (InetSocketAddress address: addresses) {
            if (address.equals(myAddress))
                continue;
            packet.setSocketAddress(address);
            try {
                socket.send(packet);
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
    }

    protected void disconnect(String host, String port, boolean skip) {
        InetSocketAddress oldAddress = new InetSocketAddress(host, Integer.parseInt(port));
        addresses.remove(oldAddress);
        byte[] buffer = ("-" + myAddress.getHostString() + ":" + myAddress.getPort()).getBytes();;
        DatagramPacket packet = new DatagramPacket(buffer, buffer.length);
        packet.setSocketAddress(oldAddress);
        try {
            socket.send(packet);
        } catch (IOException e) {
            e.printStackTrace();
        }
        for (InetSocketAddress address: addresses) {
            if (!skip) {
                buffer = ("-" + address.getHostString() + ":" + address.getPort()).getBytes();
                packet = new DatagramPacket(buffer, buffer.length);
                packet.setSocketAddress(oldAddress);
                try {
                    socket.send(packet);
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }

            buffer = ("-" + host + ":" + port).getBytes();
            packet = new DatagramPacket(buffer, buffer.length);
            packet.setSocketAddress(address);
            try {
                socket.send(packet);
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
    }
}