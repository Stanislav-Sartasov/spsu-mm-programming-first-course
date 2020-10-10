package com.company.app;

import javafx.collections.FXCollections;
import javafx.fxml.FXMLLoader;
import javafx.scene.Node;
import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.control.ComboBox;
import javafx.scene.control.Label;
import javafx.scene.input.KeyCode;
import javafx.scene.input.MouseButton;
import javafx.scene.layout.AnchorPane;
import javafx.stage.Stage;
import com.company.math.*;
import org.junit.After;
import org.junit.Before;
import org.junit.Test;
import org.testfx.api.FxToolkit;
import org.testfx.framework.junit.ApplicationTest;

import java.util.ArrayList;

import static org.junit.Assert.*;

public class UIControllerTest extends ApplicationTest {
    private Button incScaleBtn, decScaleBtn;
    private Label scaleLabel;
    private ComboBox<Graph> graphBox;
    private DrawingField drawingField;

    public void initGraphs() {
        System.out.println("world");
        ArrayList<Graph> toBox = new ArrayList<>();
        toBox.add(new Ellipse(1,1, (float) (-drawingField.getWidth() / 2), (float) drawingField.getWidth() / 2));
        toBox.add(new Ellipse(5, 3, (float) (-drawingField.getWidth() / 2), (float) drawingField.getWidth() / 2));
        toBox.add(new Ellipse(1, 2,(float) (-drawingField.getWidth() / 2), (float) drawingField.getWidth() / 2));
        toBox.add(new Hyperbola(1, 1, (float) (-drawingField.getWidth() / 2), (float) drawingField.getWidth() / 2));
        toBox.add(new Hyperbola(2, 4, (float) (-drawingField.getWidth() / 2), (float) drawingField.getWidth() / 2));
        toBox.add(new Parabola(2, (float) (-drawingField.getWidth() / 2), (float) drawingField.getWidth() / 2));
        toBox.add(new Parabola(-4, (float) (-drawingField.getWidth() / 2), (float) drawingField.getWidth() / 2));
        graphBox.setItems(FXCollections.observableArrayList(toBox));
    }

    @Before
    public void setUp() {
        incScaleBtn = find("#incScaleBtn");
        decScaleBtn = find("#decScaleBtn");
        scaleLabel = find("#scaleLabel");
        graphBox = find("#graphBox");
        drawingField = find("#drawingField");
        initGraphs();
    }

    @Override
    public void start(Stage stage) throws Exception {
        System.out.println("HI");
        FXMLLoader loader = new FXMLLoader();
        loader.setLocation(UIController.class.getResource(Main.FXMLPATH));
        AnchorPane pane = loader.load();
        stage.setScene(new Scene(pane));
        stage.show();
    }

    public <T extends Node> T find(final String query) {
        return lookup(query).query();
    }


    @Test
    public void testDrawingEnable() {
        assertTrue(decScaleBtn.isDisable());
        assertTrue(incScaleBtn.isDisable());
        clickOn(graphBox);
        clickOn("x^2 + y^2 = 1");
        assertFalse(decScaleBtn.isDisable());
        assertFalse(incScaleBtn.isDisable());
        clickOn(graphBox);
        clickOn("1/25 * x^2 + 1/9 * y^2 = 1");
        clickOn(graphBox);
        clickOn("x^2 + 1/4 * y^2 = 1");
        clickOn(graphBox);
        clickOn("x^2 - y^2 = 1");
        clickOn(graphBox);
        clickOn("1/4 * x^2 - 1/16 * y^2 = 1");
        clickOn(graphBox);
        clickOn("y^2 = 4*x");
        clickOn(graphBox);
        clickOn("y^2 = -8*x");
    }

    @Test
    public void testChangingScaleBtn() {
        clickOn(graphBox);
        clickOn("1/25 * x^2 + 1/9 * y^2 = 1");
        clickOn(decScaleBtn);
        assertEquals("0.9", scaleLabel.getText());
        clickOn(incScaleBtn);
        assertEquals("1.0", scaleLabel.getText());
        int clickCnt = 40;
        for (int i = 0; i < clickCnt; i++) {
            clickOn(decScaleBtn);
        }
        assertEquals("0.1", scaleLabel.getText());
        for (int i = 0; i < clickCnt; i++) {
            clickOn(incScaleBtn);
        }
        assertEquals("3.0", scaleLabel.getText());
    }

    @After
    public void tearDown() throws Exception {
        FxToolkit.hideStage();
        release(new KeyCode[]{});
        release(new MouseButton[]{});
    }
}