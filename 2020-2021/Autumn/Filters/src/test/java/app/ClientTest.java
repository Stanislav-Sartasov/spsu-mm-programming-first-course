package app;

import javafx.fxml.FXMLLoader;
import javafx.scene.Node;
import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.control.ComboBox;
import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.scene.input.KeyCode;
import javafx.scene.input.MouseButton;
import javafx.scene.layout.AnchorPane;
import javafx.stage.Stage;
import org.junit.After;
import org.junit.Before;
import org.junit.Test;
import org.testfx.api.FxToolkit;
import org.testfx.framework.junit.ApplicationTest;

import java.io.File;

import static org.junit.Assert.*;

// Сначала нужно запустить сервер


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
    public void test() throws InterruptedException {
        clickOn(filter);
        clickOn("Averaging 5x5");
        assertFalse(start.isDisable());
        clickOn(start);
        assertNull(photoAfter.getImage());

        Thread.sleep(4000);
        assertNotNull(photoAfter.getImage());

        clickOn(filter);
        clickOn("SobelX");
        clickOn(start);
        assertNull(photoAfter.getImage());

        Thread.sleep(4000);
        assertNotNull(photoAfter.getImage());

        clickOn(filter);
        clickOn("Grey");
        clickOn(start);
        assertNull(photoAfter.getImage());

        Thread.sleep(4000);
        assertNotNull(photoAfter.getImage());

        clickOn(filter);
        clickOn("Averaging 3x3");
        clickOn(filter);
        clickOn("Gauss 3x3");

        int[] time = new int[]{200, 650};
        clickOn(filter);
        clickOn("Gauss 5x5");
        for (int t: time) {
            clickOn(start);
            Thread.sleep(t);
            clickOn(cancel);
            assertNull(photoAfter.getImage());
            Thread.sleep(4000 - t);
            assertNull(photoAfter.getImage());
        }

        clickOn(filter);
        clickOn("SobelY");
        clickOn(start);
        assertNull(photoAfter.getImage());

        Thread.sleep(4000);
        assertNotNull(photoAfter.getImage());
    }

    @After
    public void tearDown() throws Exception {
        FxToolkit.hideStage();
        release(new KeyCode[]{});
        release(new MouseButton[]{});
    }
}