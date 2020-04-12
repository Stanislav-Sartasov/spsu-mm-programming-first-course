package com.company;

import org.junit.Test;

import javax.imageio.ImageIO;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;

import static org.junit.Assert.*;

public class ImageTest {

    private final String pathInput = "pics" + File.separator + "input.bmp";
    private final File inputFile = new File(pathInput);
    private BufferedImage inputImage;

    {
        try {
            inputImage = ImageIO.read(inputFile);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private final Image input = new Image(inputImage);

    private void check(String pathOutput, Image first) throws Exception {
        File outputFile = new File(pathOutput);
        BufferedImage outputImage = ImageIO.read(outputFile);
        Image second = new Image(outputImage);

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
    public void averaging() throws Exception {
        Image myResult = input;
        myResult.averaging(5);
        check("pics" + File.separator + "outputAveraging5.bmp", myResult);
    }

    @Test
    public void gauss() throws Exception {
        Image myResult = input;
        myResult.gauss(3);
        check("pics" + File.separator + "outputGauss3.bmp", myResult);
    }

    @Test
    public void sobelX() throws Exception {
        Image myResult = input;
        myResult.sobelX(3);
        check("pics" + File.separator + "outputSobelX.bmp", myResult);
    }

    @Test
    public void sobelY() throws Exception {
        Image myResult = input;
        myResult.sobelY(3);
        check("pics" + File.separator + "outputSobelY.bmp", myResult);
    }

    @Test
    public void toGrey() throws Exception {
        Image myResult = input;
        myResult.toGrey();
        check("pics" + File.separator + "outputToGrey.bmp", myResult);
    }
}