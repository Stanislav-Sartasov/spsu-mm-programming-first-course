package math;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import java.util.ArrayList;

import static org.junit.jupiter.api.Assertions.*;

class GraphTest {

    private Graph ellipse;
    private Graph parabola;
    private Graph hyperbola;
    @BeforeEach
    public void setUp() {
        ellipse = new Ellipse(2,3, -100, 100);
        parabola = new Parabola(2, -100, 100);
        hyperbola = new Hyperbola(5,6, -100, 100);
    }

    @Test
    public void getYPointsEllipseTest() {
        ArrayList<Float> points = ellipse.getYPoints(0);
        assertEquals(points.size(), 2);
        assertEquals(points.get(0), 3);
        assertEquals(points.get(1), -3);
        points = ellipse.getYPoints(2);
        assertEquals(points.size(), 1);
        assertEquals(points.get(0), 0);
    }
    @Test
    public void getYPointsHyperbolaTest() {
        ArrayList<Float> points = hyperbola.getYPoints(5);
        assertEquals(points.size(), 1);
        assertEquals(points.get(0), 0);
    }
    @Test
    public void getYPointsParabolaTest() {
        ArrayList<Float> points = parabola.getYPoints(0);
        assertEquals(points.size(), 1);
        assertEquals(points.get(0), 0);
        points = parabola.getYPoints(1);
        assertEquals(points.size(), 2);
        assertEquals(points.get(0), 2);
        assertEquals(points.get(1), -2);

    }
}