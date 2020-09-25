package app;

import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.stage.Stage;


public class Main extends Application {
    public static final String FXMLPATH = "/resources/uiMenu.fxml";
    public static final String TITLE = "Function Graphs Drawer";

    @Override
    public void start(Stage stage) throws Exception {
        /*File file = new File(MENU_PATH);
        String absolutePath = file.getAbsolutePath();
        System.out.println(absolutePath);*/

        FXMLLoader loader = new FXMLLoader(getClass().getResource(FXMLPATH));

        Parent root = loader.load();
        Scene startScene = new Scene(root, 920, 620);
        stage.setScene(startScene);
        stage.centerOnScreen();
        stage.setResizable(false);
        stage.setTitle(TITLE);
        stage.show();
    }

    public static void main(String[] args) {
        launch(args);
    }
}
