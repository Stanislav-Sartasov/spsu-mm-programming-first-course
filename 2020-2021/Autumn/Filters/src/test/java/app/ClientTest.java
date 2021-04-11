package app;

import javafx.fxml.FXMLLoader;
import javafx.scene.Node;
import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.control.ComboBox;
import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.scene.layout.AnchorPane;
import javafx.stage.Stage;
import org.junit.Before;
import org.junit.Test;
import org.testfx.framework.junit.ApplicationTest;

import java.io.File;

import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertNull;

// Сначала нужно запустить сервер

//TODO: beforeEach?

public class ClientTest extends ApplicationTest {
    private ComboBox filter;
    private ImageView photoAfter;
    private Button start;
    private Controller myController;
    private Button cancel;

    public <T extends Node> T find(final String query) {
        return lookup(query).query();
    }

    @Before
    public void setUp() {
        filter = find("#filter");
        ImageView photoBefore = find("#photoBefore");
        photoAfter = find("#photoAfter");
        start = find("#start");
        cancel = find("#cancel");
        myController.image = new File(getClass().getResource("/app/input.bmp").getFile());
        photoBefore.setImage(new Image(getClass().getResource("/app/input.bmp").toString()));
        myController.centerImage(photoBefore);
    }

    @Override
    public void start(Stage stage) throws Exception {
        FXMLLoader loader = new FXMLLoader();
        loader.setLocation(Controller.class.getResource("/app/uiMenu.fxml"));
        AnchorPane pane = loader.load();
        myController = loader.getController();
        stage.setScene(new Scene(pane));
        stage.show();
    }

    @Test
    public void testStart() throws InterruptedException {
        clickOn(filter);
        clickOn("Averaging 5x5");
        assertFalse(start.isDisable());
        clickOn(start);

        Thread.sleep(5000);

        clickOn(filter);
        clickOn("SobelX");
        clickOn(start);

        Thread.sleep(5000);

        clickOn(filter);
        clickOn("Grey");
        clickOn(start);

        Thread.sleep(5000);

        clickOn(filter);
        clickOn("SobelY");
        clickOn(filter);
        clickOn("Averaging 3x3");
        clickOn(filter);
        clickOn("Gauss 5x5");
        clickOn(filter);
        clickOn("Gauss 3x3");
    }

    @Test
    public void testCancel() throws InterruptedException {
        int[] time = new int[]{100, 700, 1100};
        clickOn(filter);
        clickOn("Averaging 5x5");
        for (int t: time) {
            clickOn(start);
            Thread.sleep(t);
            clickOn(cancel);
            assertNull(photoAfter.getImage());
            Thread.sleep(5000 - t);
            assertNull(photoAfter.getImage());
        }
    }
}