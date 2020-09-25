package math;

import javafx.geometry.Point2D;

import java.util.ArrayList;

public abstract class Graph {
    ArrayList<Point2D> points;
    String equation;
    float from, to;

    public Graph(float from, float to) {
        this.from = from;
        this.to = to;
    }

    public abstract ArrayList<Float> getYPoints(float x);

    public abstract void buildEquation();

    public ArrayList<Point2D>[] makePoints(int pixStep) {
        ArrayList<Point2D> firstQuarter = new ArrayList<>();
        ArrayList<Point2D> secondQuarter = new ArrayList<>();
        ArrayList<Point2D> threeQuarter = new ArrayList<>();
        ArrayList<Point2D> fourQuarter = new ArrayList<>();

        int stepConst = 2000;
        float step = (to - from) / pixStep / stepConst;
        for (float x = from; x < to; x += step) {
            ArrayList<Float> resY = getYPoints(x / pixStep);
            for (Float y : resY) {
                if (y * pixStep >= 0 && x >= 0) {
                    firstQuarter.add(new Point2D(x, y * pixStep));
                }
                if (y * pixStep >= 0 && x < 0) {
                    secondQuarter.add(new Point2D(x, y * pixStep));
                }
                if (y * pixStep < 0 && x < 0) {
                    threeQuarter.add(new Point2D(x, y * pixStep));
                }
                if (y * pixStep < 0 && x > 0) {
                    fourQuarter.add(new Point2D(x, y * pixStep));
                }
            }
        }
        return new ArrayList[] {firstQuarter, secondQuarter, threeQuarter, fourQuarter};
    }

    @Override
    public String toString() {
        return equation;
    }
}
