package server;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.atomic.AtomicInteger;


public class MultiThreadServer {

    static AtomicInteger curId = new AtomicInteger(0);
    static List<Thread> threads = new ArrayList<>();

    public static void main(String[] args) {
        try (ServerSocket server = new ServerSocket(3345)) {
            while (!server.isClosed()) {
                Socket client = server.accept();
                Thread newThread = new Thread(() -> new MonoThreadClientHandler(client, curId.getAndIncrement()).run());
                threads.add(newThread);
                newThread.start();
            }
            for (Thread thread: threads) {
                try {
                    thread.join();
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}