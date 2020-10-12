package com.company.math;

import java.util.ArrayList;

public class Hyperbola extends Graph {
    private float a, b;

    public Hyperbola(float a, float b, float from, float to) {
        super(from, to);
        this.a = a;
        this.b = b;
        buildEquation();
    }

    @Override
    public ArrayList<Float> getYPoints(float x) {
        float aSqr = a * a;
        float bSqr = b * b;
        ArrayList<Float> yRes = new ArrayList<>();
        float y = (float) Math.sqrt(bSqr * ((x * x) / aSqr - 1));
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
        StringBuilder builder = new StringBuilder("");
        int ta = (int) a, tb = (int) b;
        if (ta == a) {
            if (ta * ta != 1) {
                builder.append("1/");
                builder.append(ta * ta);
                builder.append(" * ");
            }
        }
        else {
            builder.append("1/");
            builder.append(a * a);
            builder.append(" * ");
        }
        builder.append("x^2 - ");
        if (tb == b) {
            if (tb * tb != 1) {
                builder.append("1/");
                builder.append(tb * tb);
                builder.append(" * ");
            }
        }
        else {
            builder.append("1/");
            builder.append(b * b);
            builder.append(" * ");
        }
        builder.append("y^2");
        builder.append(" = 1");
        equation = builder.toString();
    }

}