package app;

import javafx.application.Application;
import javafx.application.Platform;
import javafx.fxml.FXMLLoader;
import javafx.scene.Group;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.stage.Stage;

import java.io.FileInputStream;


public class Main extends Application {
    public static final String FXMLPATH = "/app/uiMenu.fxml";
    public static final String TITLE = "Filters app";

    @Override
    public void start(Stage stage) throws Exception {
        FXMLLoader loader = new FXMLLoader(getClass().getResource(FXMLPATH));
        Parent root = loader.load();
        Scene startScene = new Scene(root, 920, 620);
        stage.setScene(startScene);
        stage.centerOnScreen();
        stage.setResizable(false);
        stage.setTitle(TITLE);
        Controller controller = loader.getController();
        stage.setOnHidden(e -> {
            controller.shutdown();
            Platform.exit();
        });
        stage.show();
    }

    public static void main(String[] args) {
        launch(args);
    }
}