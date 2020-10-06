package app;

import math.Ellipse;
import math.Graph;
import math.Hyperbola;
import math.Parabola;

import javax.swing.*;
import java.awt.*;

public class GraphicApp {
    private static final String TITLE = "Function Graphs Drawer";

    private JFrame frame;
    private final JComboBox<Graph> graphBox = new JComboBox<>();
    private final JButton incScaleBtn = new JButton("+");
    private final JButton decScaleBtn = new JButton("-");
    private final JLabel scaleText = new JLabel("Scale:");
    private final JLabel scaleLabel = new JLabel("1.0");
    private GraphicPanel graphicPanel;

    public JComboBox<Graph> getGraphBox() {
        return graphBox;
    }

    public JButton getIncScaleBtn() {
        return incScaleBtn;
    }

    public JButton getDecScaleBtn() {
        return decScaleBtn;
    }

    public JLabel getScaleLabel() {
        return scaleLabel;
    }

    public GraphicApp(){
        createFrame();
        initElements();
    }

    private void createFrame() {
        frame = new JFrame(TITLE);
        frame.setBounds(300,300,920,620);
        frame.setDefaultCloseOperation(WindowConstants.EXIT_ON_CLOSE);
        frame.setResizable(false);
    }

    private void addActions() {
        graphBox.addActionListener(e -> {
            try {
                graphicPanel.draw(graphicPanel.getGraphics(), (Graph) graphBox.getSelectedItem());
            } catch (Exception exception) {
                exception.printStackTrace();
            }
            incScaleBtn.setEnabled(true);
            decScaleBtn.setEnabled(true);
        });

        incScaleBtn.addActionListener(e -> {
            try {
                incScale();
            } catch (Exception exception) {
                exception.printStackTrace();
            }
            if (graphicPanel.getScale() == 3)
                incScaleBtn.setEnabled(false);
            if (graphicPanel.getScale() != 0.1f)
                decScaleBtn.setEnabled(true);
        });

        decScaleBtn.addActionListener(e -> {
            try {
                decScale();
            } catch (Exception exception) {
                exception.printStackTrace();
            }
            if (graphicPanel.getScale() != 3)
                incScaleBtn.setEnabled(true);
            if (graphicPanel.getScale() == 0.1f)
                decScaleBtn.setEnabled(false);
        });
    }

    private void initGraphBox() {
        graphBox.addItem(new Ellipse(1,1, (float) -300, (float) 300));
        graphBox.addItem(new Ellipse(5, 3, (float) (-300), (float) 300));
        graphBox.addItem(new Ellipse(1, 2,(float) (-300), (float) 300));
        graphBox.addItem(new Hyperbola(1, 1, (float) (-300), (float) 300));
        graphBox.addItem(new Hyperbola(2, 4, (float) (-300), (float) 300));
        graphBox.addItem(new Parabola(2, (float) (-300), (float) 300));
        graphBox.addItem(new Parabola(-4, (float) (-300), (float) 300));

        graphBox.setMaximumSize(new Dimension(200, 30));
    }

    private void initElements() {
        Container container = frame.getContentPane();
        container.setLayout(new BoxLayout(container, BoxLayout.X_AXIS));

        graphicPanel = new GraphicPanel();
        graphicPanel.setMaximumSize(new Dimension(600, 500));
        container.add(graphicPanel);

        initGraphBox();
        addActions();

        Box leftBox = Box.createVerticalBox();
        leftBox.add(Box.createVerticalStrut(200));
        leftBox.add(graphBox);

        Box scaleBox = Box.createHorizontalBox();

        scaleBox.add(scaleText);
        scaleBox.add(Box.createHorizontalStrut(20));

        scaleBox.add(scaleLabel);
        scaleBox.add(Box.createHorizontalStrut(20));

        incScaleBtn.setEnabled(false);
        scaleBox.add(incScaleBtn);
        scaleBox.add(Box.createHorizontalStrut(20));

        decScaleBtn.setEnabled(false);
        scaleBox.add(decScaleBtn);

        leftBox.add(scaleBox);
        leftBox.add(Box.createVerticalStrut(200));

        leftBox.setMaximumSize(new Dimension(300, 500));

        container.add(leftBox);
    }

    private void changeScaleLabel() {
        scaleLabel.setText(Float.toString(graphicPanel.getScale()));
    }

    private void incScale() throws Exception {
        graphicPanel.incScale();
        graphicPanel.draw(graphicPanel.getGraphics(), (Graph) graphBox.getSelectedItem());
        changeScaleLabel();
    }
    private void decScale() throws Exception {
        graphicPanel.decScale();
        graphicPanel.draw(graphicPanel.getGraphics(), (Graph) graphBox.getSelectedItem());
        changeScaleLabel();
    }

    public void show(){
        frame.setVisible(true);
    }

    public static void main(String[] args) {
        GraphicApp app = new GraphicApp();
        app.show();
    }
}

