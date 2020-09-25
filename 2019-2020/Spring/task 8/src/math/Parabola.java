package math;

import math.Graph;

import java.util.ArrayList;

public class Parabola extends Graph {
    private float p;
    // y^2 = 2*p*x
    public Parabola(float p, float from, float to) {
        super(from, to);
        this.p = p;
        buildEquation();
    }

    @Override
    public ArrayList<Float> getYPoints(float x) {
        float doubledP = p * 2;
        ArrayList<Float> yRes = new ArrayList<>();
        float y = (float) Math.sqrt(doubledP * x);
        if (y == 0) {
            yRes.add((float) 0);
            return yRes;
        }
        yRes.add(y);
        yRes.add(-y);
        return yRes;
    }

    @Override
    public void buildEquation() {
        StringBuilder builder = new StringBuilder("y^2 = ");
        int tp = (int) p;
        if (tp == p) {
            builder.append(2 * tp);
        }
        else {
            builder.append(2 * p);
        }
        builder.append("*x");
        equation = builder.toString();
    }
}
