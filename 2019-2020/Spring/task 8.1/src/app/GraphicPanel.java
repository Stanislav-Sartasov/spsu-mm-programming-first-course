package app;

import math.Graph;

import javax.swing.*;
import java.awt.*;
import java.awt.geom.Point2D;
import java.util.ArrayList;


public class GraphicPanel extends JPanel {
    private final Color graphicColor = Color.blue;
    private final Point2D zeroPoint;
    private float scale = 1f;
    private int pixStep = 30;

    public int width;
    public int height;

    public GraphicPanel() {
        width = getWidth();
        height = getHeight();

        try {
            pixStep = (int) getStep();
        } catch (Exception exception) {
            exception.printStackTrace();
        }

        zeroPoint = new Point2D.Double((getWidth() >> 1) - 2, (getHeight() >> 1) - 2);
    }

    @Override
    public void paint(Graphics g) {
        super.paint(g);
        drawCoordinateSystem(g,1f);
    }


    private void drawGraphic(Graphics g, Graph graph) {
        g.setColor(graphicColor);

        ArrayList<Point2D.Double>[] quarters = graph.makePoints(pixStep);

        drawQuarter(g, quarters[0]);
        drawQuarter(g, quarters[1]);
        drawQuarter(g, quarters[2]);
        drawQuarter(g, quarters[3]);
    }

    private boolean inRange(double x, double y) {
        return x >= 0 && x < getWidth() && y >= 0 && y < getHeight();
    }

    private void drawQuarter(Graphics g, ArrayList<Point2D.Double> points) {
        ArrayList<Integer> x = new ArrayList<>();
        ArrayList<Integer> y = new ArrayList<>();
        for (Point2D.Double point : points) {
            double sendX = point.getX() + (getWidth() >> 1);
            double sendY = point.getY() + (getHeight() >> 1);
            if (inRange(sendX, sendY)) {
                x.add((int) sendX);
                y.add((int) sendY);
            }
        }
        g.drawPolyline(x.stream().mapToInt(i -> i).toArray(),
                y.stream().mapToInt(i -> i).toArray(), x.size());
    }

    private void drawZeroPoint(Graphics g) {
        g.setColor(Color.BLACK);
        g.fillOval((int)zeroPoint.getX(), (int)zeroPoint.getY(), 4, 4);
        drawNumX(g,0);
    }

    private float getStep() throws Exception {
        float step;
        if (scale == 0.1f) {
            step = 12f;
        } else if (scale == 0.2f) {
            step = 5f;
        } else if (scale > 0.2f && scale < 1f) {
            step = 3f;
        } else if (scale >= 1f && scale < 2f) {
            step = 1f;
        } else if (scale >= 2f && scale <= 3f){
            step = 0.5f;
        } else {
            throw new Exception("Wrong Scale!");
        }
        return step;
    }

    public void drawCoordinateSystem(Graphics g, float step) {
        float midWidth = (float) (getWidth() / 2);
        float midHeight = (float) (getHeight() / 2);

        drawZeroPoint(g);
        g.drawLine(0, getHeight() / 2, getWidth(), getHeight() / 2);
        g.drawLine(getWidth() / 2, 0, getWidth() / 2, getHeight());

        pixStep = (int) (midWidth / 10 * scale);
        float delta = pixStep * step;
        for (float x = midWidth + delta; x < getWidth(); x += delta) {
            g.drawLine((int)x, (int)midHeight - 5, (int)x, (int)midHeight + 5);
            drawNumX(g, (x - midWidth) / pixStep);
        }
        for (float x = midWidth - delta; x >= 0; x -= delta) {
            g.drawLine((int)x, (int)midHeight - 5, (int)x, (int)midHeight + 5);
            drawNumX(g, (x - midWidth) / pixStep);
        }
        for (float y = midHeight + delta; y < getHeight(); y += delta) {
            g.drawLine((int)midWidth - 5, (int)y, (int)midWidth + 5, (int)y);
            drawNumY(g, (y - midHeight) / pixStep);
        }
        for (float y = midHeight - delta; y >= 0; y -= delta) {
            g.drawLine((int)midWidth - 5, (int)y, (int)midWidth + 5, (int)y);
            drawNumY(g, (y - midHeight) / pixStep);
        }
    }

    private void drawNumX(Graphics g, float num) {
        num *= 10;
        int d = Math.round(num);
        num = (float) d / 10;

        if (num == Math.ceil(num)) {
            g.drawString(Integer.toString((int) num), (int) (getWidth() / 2 + num * pixStep - 5), getHeight() / 2 + 18);
        }
        else {
            g.drawString(Float.toString(num), (int) (getWidth() / 2 + num * pixStep - 5), getHeight() / 2 + 18);
        }
    }

    private void drawNumY(Graphics g, float num) {
        num *= 10;
        int d = Math.round(num);
        num = (float) d / 10;
        if (num == Math.ceil(num)) {
            g.drawString(Integer.toString((int) -num), getWidth() / 2 + 15, (int) (getHeight() / 2 + num * pixStep + 5));
        }
        else {
            g.drawString(Float.toString(-num), getWidth() / 2 + 15, (int) (getHeight() / 2 + num * pixStep + 5));
        }
    }


    public void incScale() {
        this.scale += 0.1f;
        scale *= 10;
        int d = Math.round(scale);
        scale = (float) d / 10;
        if (scale > 3f) {
            scale = 3f;
        }
    }

    public void decScale() {
        this.scale -= 0.1f;
        scale *= 10;
        int d = Math.round(scale);
        scale = (float) d / 10;
        if (scale < 0.1f) {
            scale = 0.1f;
        }
    }

    public float getScale() {
        return this.scale;
    }


    public void draw(Graphics g, Graph graph) throws Exception {
        g.clearRect(0, 0, getWidth(), getHeight());
        drawCoordinateSystem(g, getStep());
        drawGraphic(g, graph);
    }
}
