package server;

import app.BmpImage;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.net.Socket;

public class MonoThreadClientHandler implements Runnable {

    private static Socket clientDialog;
    private static int id;
    private volatile Thread thread;
    private volatile boolean isRunning;

    public MonoThreadClientHandler(Socket client, int id) {
        MonoThreadClientHandler.clientDialog = client;
        this.id = id;
    }

    public void stop() {
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
                    out.writeUTF("Averaging 5x5");
                    out.writeUTF("Averaging 3x3");
                    out.writeUTF("SobelX");
                    out.writeUTF("SobelY");
                    out.writeUTF("Gauss 5x5");
                    out.writeUTF("Gauss 3x3");
                    out.writeUTF("Grey");
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
                        if (isRunning) {
                            out.writeDouble(ans);
                            out.flush();
                            String outputPath = "output" + id + ".bmp";
                            bmpImage.write(outputPath);
                            out.writeUTF(outputPath);
                            out.flush();
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