package server;

import app.BmpImage;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.FileInputStream;
import java.net.Socket;
import java.util.Properties;

public class ClientHandler implements Runnable {

    private final Socket clientDialog;
    private final int id;
    private volatile Thread thread;
    private volatile boolean isRunning;

    public ClientHandler(Socket client, int id) {
        clientDialog = client;
        this.id = id;
    }

    private synchronized void stop() {
        isRunning = false;
        if (thread != null)
            thread.interrupt();
    }

    @Override
    public void run() {

        try {
            DataOutputStream out = new DataOutputStream(clientDialog.getOutputStream());
            DataInputStream in = new DataInputStream(clientDialog.getInputStream());

            while (!clientDialog.isClosed()) {
                String entry = in.readUTF();
                if (entry.equalsIgnoreCase("get filters")) {
                    Properties props = new Properties();
                    props.load(new FileInputStream(getClass().getResource("/server/filters.ini").getFile()));
                    String[] filters = props.getProperty("FILTERS").split(";");
                    for (String filter: filters) {
                        out.writeUTF(filter);
                        out.flush();
                    }
                    continue;
                }
                if (entry.equalsIgnoreCase("quit")) {
                    break;
                }
                if (entry.equalsIgnoreCase("cancel")) {
                    stop();
                    continue;
                }
                String[] request = entry.split("\\|");
                BmpImage bmpImage = new BmpImage(request[0]);
                String value = request[1];

                thread = new Thread(() -> bmpImage.addFilter(value));
                isRunning = true;
                thread.start();

                new Thread(() -> {
                    try {
                        double ans = 0;
                        while (ans < 1 && isRunning) {
                            Thread.sleep(50);
                            out.writeDouble(ans);
                            out.flush();
                            ans = bmpImage.percentageOfReadiness();
                        }
                        synchronized (this) {
                            if (isRunning) {
                                out.writeDouble(ans);
                                out.flush();
                                String outputPath = "output" + id + ".bmp";
                                bmpImage.write(outputPath);
                                out.writeUTF(outputPath);
                                out.flush();
                            }
                        }
                    } catch (Exception e) {
                        e.printStackTrace();
                    }
                }).start();
            }

            in.close();
            out.close();
            clientDialog.close();
        } catch (Exception e) {
            e.printStackTrace();
        }

    }
}