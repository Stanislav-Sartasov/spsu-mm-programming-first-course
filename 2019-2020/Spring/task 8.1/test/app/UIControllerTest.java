/*
    VM options: --add-exports javafx.graphics/com.sun.javafx.application=ALL-UNNAMED
 */

package app;

import org.junit.Assert;
import org.junit.Before;
import org.junit.Test;

public class UIControllerTest {
    private GraphicApp app;

    public void initGraphs() {
        app = new GraphicApp();
        app.show();
    }

    @Before
    public void setUp() {
        initGraphs();
    }

    @Test
    public void testDrawingEnable() {
        Assert.assertFalse(app.getDecScaleBtn().isEnabled());
        Assert.assertFalse(app.getIncScaleBtn().isEnabled());
        app.getGraphBox().setSelectedIndex(0);
        Assert.assertTrue(app.getDecScaleBtn().isEnabled());
        Assert.assertTrue(app.getIncScaleBtn().isEnabled());
        app.getGraphBox().setSelectedIndex(1);
        app.getGraphBox().setSelectedIndex(2);
        app.getGraphBox().setSelectedIndex(3);
        app.getGraphBox().setSelectedIndex(4);
        app.getGraphBox().setSelectedIndex(5);
        app.getGraphBox().setSelectedIndex(6);
    }

    @Test
    public void testChangingScaleBtn() {
        app.getGraphBox().setSelectedIndex(0);
        app.getDecScaleBtn().doClick();
        Assert.assertEquals("0.9", app.getScaleLabel().getText());
        app.getIncScaleBtn().doClick();
        Assert.assertEquals("1.0", app.getScaleLabel().getText());
        int clickCnt = 40;
        for (int i = 0; i < clickCnt; i++) {
            app.getDecScaleBtn().doClick();
        }
        Assert.assertEquals("0.1", app.getScaleLabel().getText());
        for (int i = 0; i < clickCnt; i++) {
            app.getIncScaleBtn().doClick();
        }
        Assert.assertEquals("3.0", app.getScaleLabel().getText());
    }
}