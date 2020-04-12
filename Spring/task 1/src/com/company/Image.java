package com.company;

import java.awt.image.BufferedImage;
import java.awt.Color;

public class Image {
    private int[] input;
    private int height;
    private int width;

    public int getHeight() {
        return height;
    }

    public int getWidth() {
        return width;
    }

    public int getInput(int i, int j, int k) {
        return input[(i * width + j) * 3 + k];
    }

    public Image (BufferedImage image) {
        height = image.getHeight();
        width = image.getWidth();
        input = new int[3 * height * width];
        for (int x = 0; x < height; x++) {
            for (int y = 0; y < width; y++) {
                Color rgb = new Color(image.getRGB(y, x));
                input[(x * width + y) * 3] = rgb.getRed();
                input[(x * width + y) * 3 + 1] = rgb.getGreen();
                input[(x * width + y) * 3 + 2] = rgb.getBlue();
            }
        }
    }

    private final double sigma = 0.6;

    private void add_filter(int size, boolean fl, double[] mask) {
        int[] output = new int[3 * height * width];
        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                double[] result = {0, 0, 0};
                double d = 0;
                for (int y = 0; y < size; y++) {
                    for (int x = 0; x < size; x++) {
                        if ((i + y - 1) >= 0  && (i + y - 1) < height && (j + x - 1) >= 0 && (j + x - 1) < width) {
                            d += mask[y * size + x];
                            for (int k = 0; k < 3; k++)
                                result[k] += input[((i + y - 1) * width + j + x - 1) * 3 + k] * mask[y * size + x];
                        }
                    }
                }
                if (fl) {
                    for (int k = 0; k < 3; k++)
                        output[(i * width + j) * 3 + k] = (int)(result[k] / d);
                } else {
                    int x = 0;
                    if ((result[0] + result[1] + result[2]) > 384)
                        x = 255;
                    for (int k = 0; k < 3; k++)
                        output[(i * width + j) * 3 + k] = x;
                }
            }
        }
        for (int i = 0; i < height * width * 3; i++)
            input[i] = output[i];
    }

    public void averaging(int size) {
        double[] mask = new double[size * size];
        for (int i = 0; i < size * size ; i++)
            mask[i] = 1;
        add_filter( size, true, mask);
    }

    public void gauss(int size) {
        double[] mask = new double[size * size];
        for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
                mask[x * size + y] = 1.0 / Math.sqrt(2 * Math.PI * sigma) * Math.exp(-(x * x + y * y) / (2 * sigma * sigma));
        add_filter(size, true, mask);
    }

    public void sobelX(int size) {
        int[] a = {-1, 0, 1, -2, 0, 2, -1, 0, 1};
        double[] mask = new double[size * size];
        for (int i = 0; i < size * size; i++)
            mask[i] = a[i];
        add_filter(size, false, mask);
    }

    public void sobelY(int size) {
        int[] a =  {-1, -2, -1, 0, 0, 0, 1, 2, 1};
        double[] mask = new double[size * size];
        for (int i = 0; i < size * size; i++)
            mask[i] = a[i];
        add_filter(size, false, mask);
    }

    public void toGrey() {
        for (int i = 0; i < height * width; i++) {
            int res = (2126 * input[i * 3] + 7152 * input[i * 3 + 1] + 722 * input[i * 3 + 2]) / 10000;
            for (int k = 0; k < 3; k++) {
                input[i * 3 + k] = res;
            }
        }
    }
}