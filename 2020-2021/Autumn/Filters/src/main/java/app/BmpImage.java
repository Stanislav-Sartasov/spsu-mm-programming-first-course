package app;

import java.io.*;

public class BmpImage extends MyImage {

    public BmpImage(String inputPath) throws Exception {
        this.readFile(inputPath);
    }

    private static class BmpFile {
        short bfType;
        int bfSize;
        short bfReversedOne;
        short bfReversedTwo;
        int bfOffBits;
    }

    private static class BmpInfo {
        int size;
        int width;
        int height;
        short planes;
        short bitCount;
        int compression;
        int sizeImage;
        int xPelsPerMeter;
        int yPelsPerMeter;
        int colorUsed;
        int colorImportant;
    }

    private final BmpFile fileHeader = new BmpFile();
    private final BmpInfo infoHeader = new BmpInfo();

    private InputStream in;

    private int readInt() throws IOException {
        int b1 = in.read();
        int b2 = in.read();
        int b3 = in.read();
        int b4 = in.read();
        return ((b4 << 24) + (b3 << 16) + (b2 << 8) + (b1));
    }

    private short readShort() throws IOException {
        int b1 = in.read();
        int b2 = in.read();
        return (short)((b2 << 8) + b1);
    }

    private byte [] writeShort (int parValue) {
        byte[] retValue = new byte [2];
        retValue [0] = (byte) (parValue & 0x00FF);
        retValue [1] = (byte) ((parValue >> 8) & 0x00FF);
        return (retValue);
    }

    private byte [] writeInt (int parValue) {
        byte[] retValue = new byte [4];
        retValue [0] = (byte) (parValue & 0x00FF);
        retValue [1] = (byte) ((parValue >> 8) & 0x000000FF);
        retValue [2] = (byte) ((parValue >> 16) & 0x000000FF);
        retValue [3] = (byte) ((parValue >> 24) & 0x000000FF);
        return (retValue);
    }

    public void readFile(String path) throws Exception {
        try {
            in = new FileInputStream(path);
        } catch (java.io.FileNotFoundException e) {
            throw new Exception("No such input file");
        }

        fileHeader.bfType = readShort();
        if (fileHeader.bfType != 0x4d42)
            throw new Exception("Not a BMP file");
        //System.out.println(fileHeader.bfType);
        fileHeader.bfSize = readInt();
        //System.out.println(fileHeader.bfSize);
        fileHeader.bfReversedOne = readShort();
        //System.out.println(fileHeader.bfReversedOne);
        fileHeader.bfReversedTwo = readShort();
        //System.out.println(fileHeader.bfReversedTwo);
        fileHeader.bfOffBits = readInt();
        //System.out.println(fileHeader.bfOffBits);

        infoHeader.size = readInt();
        //System.out.println(infoHeader.size);
        infoHeader.width = readInt();
        //System.out.println(infoHeader.width);
        infoHeader.height = readInt();
        //System.out.println(infoHeader.height);
        infoHeader.planes = readShort();
        //System.out.println(infoHeader.planes);
        infoHeader.bitCount = readShort();
        //System.out.println(infoHeader.bitCount);
        infoHeader.compression = readInt();
        //System.out.println(infoHeader.compression);
        infoHeader.sizeImage = readInt();
        //System.out.println(infoHeader.sizeImage);
        infoHeader.xPelsPerMeter = readInt();
        //System.out.println(infoHeader.xPelsPerMeter);
        infoHeader.yPelsPerMeter = readInt();
        //System.out.println(infoHeader.yPelsPerMeter);
        infoHeader.colorUsed = readInt();
        //System.out.println(infoHeader.colorUsed);
        infoHeader.colorImportant = readInt();
        //System.out.println(infoHeader.colorImportant);

        height = infoHeader.height;
        width = infoHeader.width;

        if (infoHeader.bitCount != 24 && infoHeader.bitCount != 32) {
            throw new Exception("Error: the image in not 32-bit and not 24-bit.\nTry a different image!");
        }
        input = new int[infoHeader.height * infoHeader.width * 3];
        int scanLineSize = ((infoHeader.width * infoHeader.bitCount + 31) / 32) * 4;

        for (int i = 0; i < infoHeader.height; i++) {
            for (int j = 0; j < infoHeader.width; j++) {
                input[(i * infoHeader.width + j) * 3] = in.read();
                input[(i * infoHeader.width + j) * 3 + 1] = in.read();
                input[(i * infoHeader.width + j) * 3 + 2] = in.read();
                if (infoHeader.bitCount == 32) {
                    in.read();
                }
            }
        }
        for (int k = 0; k < scanLineSize; k++) {
            in.read();
        }
    }

    public void addFilter(String value) {
        if ("Averaging 3x3".equals(value)) {
            averaging(3);
        } else if ("Averaging 5x5".equals(value)) {
            averaging(5);
        } else if ("Gauss 3x3".equals(value)) {
            gauss(3);
        } else if ("Gauss 5x5".equals(value)) {
            gauss(5);
        } else if ("SobelX".equals(value)) {
            sobelX(3);
        } else if ("SobelY".equals(value)) {
            sobelY(3);
        } else if ("Grey".equals(value)) {
            toGrey();
        }
    }

    public void write(String path) throws Exception {
        OutputStream out = new FileOutputStream(path);
        int scanLineSize = ((infoHeader.width * infoHeader.bitCount + 31) / 32) * 4;

        out.write(writeShort(fileHeader.bfType));
        out.write(writeInt(fileHeader.bfSize));
        out.write(writeShort(fileHeader.bfReversedOne));
        out.write(writeShort(fileHeader.bfReversedTwo));
        out.write(writeInt(fileHeader.bfOffBits));

        out.write(writeInt(infoHeader.size));
        out.write(writeInt(infoHeader.width));
        out.write(writeInt(infoHeader.height));
        out.write(writeShort(infoHeader.planes));
        out.write(writeShort(infoHeader.bitCount));
        out.write(writeInt(infoHeader.compression));
        out.write(writeInt(infoHeader.sizeImage));
        out.write(writeInt(infoHeader.xPelsPerMeter));
        out.write(writeInt(infoHeader.yPelsPerMeter));
        out.write(writeInt(infoHeader.colorUsed));
        out.write(writeInt(infoHeader.colorImportant));

        for (int i = 0; i < infoHeader.height; i++) {
            for (int j = 0; j < infoHeader.width; j++) {
                out.write(input[(i * infoHeader.width + j) * 3]);
                out.write(input[(i * infoHeader.width + j) * 3 + 1]);
                out.write(input[(i * infoHeader.width + j) * 3 + 2]);
                if (infoHeader.bitCount == 32) {
                    out.write(0);
                }
            }
        }
        for (int k = 0; k < scanLineSize; k++) {
            out.write(0);
        }
    }
}

