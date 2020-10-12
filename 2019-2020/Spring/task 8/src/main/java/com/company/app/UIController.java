package com.company.app;

import javafx.collections.FXCollections;
import javafx.fxml.FXML;
import javafx.scene.control.Button;
import javafx.scene.control.ComboBox;
import javafx.scene.control.Label;
import javafx.scene.layout.AnchorPane;
import javafx.scene.layout.Pane;
import com.company.math.*;

import java.util.ArrayList;

public class UIController {
    private DrawingField drawingField;
    @FXML
    private AnchorPane pane;
    @FXML
    private Pane graphPane;
    @FXML
    private ComboBox<Graph> graphBox;
    @FXML
    private Button incScaleBtn;
    @FXML
    private Button decScaleBtn;
    @FXML
    private Label scaleLabel;

    private void changeScaleLabel() {
        scaleLabel.setText(Float.toString(drawingField.getScale()));
    }

    private void incScale() throws Exception {
        drawingField.incScale();
        drawingField.draw(graphBox.getValue());
        changeScaleLabel();
    }
    private void decScale() throws Exception {
        drawingField.decScale();
        drawingField.draw(graphBox.getValue());
        changeScaleLabel();
    }

    @FXML
    public void initialize() throws Exception {
        drawingField = new DrawingField();
        pane.getChildren().add(drawingField);
        drawingField.setParentPane(graphPane);
        drawingField.setId("drawingField");
        initgraphs();
        graphPane.setStyle("-fx-border-color: black");
        scaleLabel.setStyle("-fx-border-color: black");

        incScaleBtn.setOnAction(actionEvent -> {
            try {
                incScale();
            } catch (Exception e) {
                e.printStackTrace();
            }
        });

        decScaleBtn.setOnAction(actionEvent -> {
            try {
                decScale();
            } catch (Exception e) {
                e.printStackTrace();
            }
        });

        graphBox.setOnAction(actionEvent -> {
            try {
                drawingField.draw(graphBox.getValue());
            } catch (Exception e) {
                e.printStackTrace();
            }
            decScaleBtn.setDisable(false);
            incScaleBtn.setDisable(false);
        });
    }

    public void initgraphs() {
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
}