package com.company;

import javax.imageio.IIOException;
import javax.imageio.ImageIO;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;

import static java.lang.System.exit;
import static java.lang.System.out;

public class Main {

    private static boolean checkFilterName(String name) {
        return switch (name) {
            case "Averaging3", "Averaging5", "Gauss3", "Gauss5", "SobelX", "SobelY", "ToGrey" -> true;
            default -> false;
        };
    }

    public static void main(String[] args) throws Exception {
        if (args.length != 3) {
            throw new Exception("Number of parameters is incorrect!\n");
        }

        String pathInput = args[0];
        String filterName = args[1];
        String pathOutput = args[2];

        if (!checkFilterName(filterName)){
            throw new Exception("Filter name is incorrect!\n");
        }

        File file = new File(pathInput);
        BufferedImage image = ImageIO.read(file);
        Image result = new Image(image);

        switch (filterName) {
            case "Averaging3" ->  result.averaging(3);
            case "Averaging5" ->  result.averaging(5);
            case "Gauss3" ->  result.gauss(3);
            case "Gauss5" ->  result.gauss(5);
            case "SobelX" ->  result.sobelX(3);
            case "SobelY" ->  result.sobelY(3);
            case "ToGrey" ->  result.toGrey();
        }

        BufferedImage outputImage = new BufferedImage(result.getWidth(), result.getHeight(), image.getType());
        for (int i = 0; i < result.getHeight(); i++) {
            for (int j = 0; j < result.getWidth(); j++) {
                Color newColor = new Color(result.getInput(i, j, 0), result.getInput(i, j, 1), result.getInput(i, j, 2));
                outputImage.setRGB(j, i, newColor.getRGB());
            }
        }
        File output = new File(pathOutput);
        try {
            ImageIO.write(outputImage, "bmp", output);
            System.out.println("Successfully!");
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}

