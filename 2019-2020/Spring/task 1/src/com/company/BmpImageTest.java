package com.company;

import org.junit.Test;

import java.io.File;

public class BmpImageTest {

    private final String pathInput = "pics" + File.separator + "input.bmp";
    private BmpImage inputImage;

    {
        try {
            inputImage = new BmpImage(pathInput);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private void check(String pathOutput, BmpImage first) throws Exception {
        BmpImage second = new BmpImage(pathOutput);

        if (first.getHeight() != second.getHeight() || first.getWidth() != second.getWidth())
            throw new Exception("Wrong filter.");
        for (int i = 0; i < first.getHeight(); i++) {
            for (int j = 0; j < first.getWidth(); j++) {
                if (first.getInput(i, j, 0) != second.getInput(i, j, 0) ||
                        first.getInput(i, j, 1) != second.getInput(i, j, 1) ||
                        first.getInput(i, j, 2) != second.getInput(i, j, 2)) {
                    throw new Exception("Wrong filter.");
                }
            }
        }

    }

    @Test
    public void testAveraging() throws Exception {
        BmpImage myResult = inputImage;
        myResult.averaging(5);
        check("pics" + File.separator + "outputAveraging5.bmp", myResult);
    }

    @Test
    public void testGauss() throws Exception {
        BmpImage myResult = inputImage;
        myResult.gauss(3);
        check("pics" + File.separator + "outputGauss3.bmp", myResult);
    }

    @Test
    public void testSobelX() throws Exception {
        BmpImage myResult = inputImage;
        myResult.sobelX(3);
        check("pics" + File.separator + "outputSobelX.bmp", myResult);
    }

    @Test
    public void testSobelY() throws Exception {
        BmpImage myResult = inputImage;
        myResult.sobelY(3);
        check("pics" + File.separator + "outputSobelY.bmp", myResult);
    }

    @Test
    public void testToGrey() throws Exception {
        BmpImage myResult = inputImage;
        myResult.toGrey();
        check("pics" + File.separator + "outputToGrey.bmp", myResult);
    }

}