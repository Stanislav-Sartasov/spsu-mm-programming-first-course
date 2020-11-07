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
    private int myPort;

    public void setReceiveMessage(boolean f) {
        receiveMessage = f;
    }

    public Set<InetSocketAddress> getAddresses() {
        return addresses;
    }

    public void setAddresses(Set<InetSocketAddress> a) throws UnknownHostException {
        for (InetSocketAddress address: a) {
            if (address.isUnresolved()) {
                System.out.println("[Error] Wrong address " + address);
                continue;
            }
            byte[] buffer = ("?" + String.valueOf(InetAddress.getByName("localhost")).split("/")[1]+ ":" + myPort).getBytes();
            DatagramPacket packet = new DatagramPacket(buffer, buffer.length);
            packet.setSocketAddress(address);
            try {
                socket.send(packet);
            } catch (IOException e) {
                System.out.println("[Error] No user with such address (" + address.getPort() +
                        "). If you are not mistaken in writing the address, ask him to connect and then add it again.");
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

    public boolean bind(int port) {
        try {
            myPort = port;
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

    protected void update(String host, String port) throws UnknownHostException {
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

                if (address.getPort() == myPort  && address.getAddress() == InetAddress.getByName("localhost"))
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
            if (!(myPort == Integer.parseInt(port) &&
                    host.equals(String.valueOf(InetAddress.getByName("localhost")).split("/")[1])))
                System.out.println(">>> Connected to " + host + ":" + port);
            addresses.add(newConnection);
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
                    byte[] b = ("!" +  String.valueOf(InetAddress.getByName("localhost")).split("/")[1] + ":" + myPort).getBytes();
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
                    addresses.add(new InetSocketAddress(msg.substring(1).split(":")[0],
                            Integer.parseInt(msg.split(":")[1])));
                    continue;
                }
                String port = msg.split("~")[0];
                //System.out.println(port);
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

    public void send(String msg) throws UnknownHostException {
        byte[] buffer = msg.getBytes();
        String port = msg.split("~")[0];
        String[] a = port.split(":");
        update(a[0], a[1]);
        DatagramPacket packet = new DatagramPacket(buffer, buffer.length);
        for (InetSocketAddress address: addresses) {
            if (myPort == address.getPort() &&
                    address.getHostString().equals(String.valueOf(InetAddress.getByName("localhost")).split("/")[1]))
                continue;
            packet.setSocketAddress(address);
            try {
                socket.send(packet);
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
    }
}