package app;

import javafx.collections.FXCollections;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.scene.Node;
import javafx.scene.control.Alert;
import javafx.scene.control.Button;
import javafx.scene.control.ComboBox;
import javafx.scene.control.ProgressIndicator;
import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.scene.input.MouseEvent;
import javafx.stage.FileChooser;
import javafx.stage.Window;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.File;
import java.io.IOException;
import java.net.Socket;
import java.util.ArrayList;

public class Controller {
    @FXML
    private ComboBox filter;
    @FXML
    private ImageView photoBefore;
    @FXML
    private ImageView photoAfter;
    @FXML
    private Button start;
    @FXML
    private Button cancel;
    @FXML
    private ProgressIndicator indicator;
    private File image;
    private Socket socket;
    private DataOutputStream oos;
    private DataInputStream ois;
    private volatile boolean isCanceled;

    @FXML
    public void initialize() {
        indicator.setVisible(false);
        try {
            connectToServer();
            initFilters();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    void connectToServer() throws IOException {
        socket = new Socket("localhost", 3345);
        oos = new DataOutputStream(socket.getOutputStream());
        ois = new DataInputStream(socket.getInputStream());
    }

    void initFilters() throws IOException {
        ArrayList<String> toBox = getFilters();
        filter.setItems(FXCollections.observableArrayList(toBox));
    }

    private ArrayList<String> getFilters() throws IOException {
        oos.writeUTF("get filters");
        oos.flush();
        ArrayList<String> f = new ArrayList<>();
        while (ois.available() <= 0) {
            Thread.yield();
        }
        while (ois.available() > 0) {
            f.add(ois.readUTF());
        }
        return f;
    }

    @FXML
    void fileSelector(ActionEvent event) {
        Window window = ((Node) (event.getSource())).getScene().getWindow();
        FileChooser fileChooser = new FileChooser();
        File file = fileChooser.showOpenDialog(window);
        event.consume();
        image = file;

        if (!getFileExtension(file).equals("bmp")) {
            makeAlert("Invalid file format", "You must upload a bmp image");
            return;
        }
        Image pic = new Image(file.toURI().toString());
        photoAfter.setImage(null);
        photoBefore.setImage(pic);
        centerImage(photoBefore);
    }

    private static String getFileExtension(File file) {
        String fileName = file.getName();
        if (fileName.lastIndexOf(".") != -1 && fileName.lastIndexOf(".") != 0)
            return fileName.substring(fileName.lastIndexOf(".") + 1);
        else return "";
    }

    @FXML
    void startFilters(MouseEvent event) {
        if (photoBefore == null) {
            makeAlert("Invalid image", "You must upload an image to apply a filter to it");
            return;
        }
        if (filter.getValue() == null) {
            makeAlert("Invalid filter", "You must select a filter");
            return;
        }
        if (socket.isOutputShutdown()) {
            try {
                connectToServer();
            } catch (IOException e) {
                e.printStackTrace();
            }
            makeAlert("Server error", "Try again later");
            return;
        }
        start.setDisable(true);
        isCanceled = false;
        photoAfter.setImage(null);
        indicator.setVisible(true);
        indicator.setProgress(0);

        String clientCommand = image.getAbsolutePath() + "|" + filter.getValue();

        try {
            oos.writeUTF(clientCommand);
            oos.flush();
        } catch (IOException e) {
            makeAlert("Server error", "Try again later");
        }
        new Thread(() -> {
            try {
                double readiness = ois.readDouble();
                while (readiness < 1 && !isCanceled) {
                    indicator.setProgress(readiness);
                    readiness = ois.readDouble();
                }
                if (!isCanceled) {
                    indicator.setProgress(1);
                    String outputImagePath = ois.readUTF();
                    indicator.setVisible(false);
                    photoAfter.setImage(new Image("file:" + outputImagePath));
                    centerImage(photoAfter);
                    start.setDisable(false);
                }
            } catch (IOException e) {
                makeAlert("Server error", "Try again later");
            }
        }).start();
        event.consume();
    }

    @FXML
    void cancel(MouseEvent event) {
        try {
            isCanceled = true;
            oos.writeUTF("cancel");
            oos.flush();
        } catch (IOException e) {
            e.printStackTrace();
        }
        start.setDisable(false);
        indicator.setVisible(false);
        event.consume();
    }

    private void makeAlert(String header, String additionalText) {
        Alert errorAlert = new Alert(Alert.AlertType.ERROR);
        errorAlert.setHeaderText(header);
        errorAlert.setContentText(additionalText);
        errorAlert.showAndWait();
    }

    private void centerImage(ImageView imageView) {
        Image img = imageView.getImage();
        if (img != null) {
            double ratioX = imageView.getFitWidth() / img.getWidth();
            double ratioY = imageView.getFitHeight() / img.getHeight();

            double reducCoeff = Math.min(ratioX, ratioY);

            double w = img.getWidth() * reducCoeff;
            double h = img.getHeight() * reducCoeff;

            imageView.setX((imageView.getFitWidth() - w) / 2);
            imageView.setY((imageView.getFitHeight() - h) / 2);

        }
    }

    public void shutdown() {
        try {
            oos.writeUTF("quit");
            oos.flush();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

}