package com.company;

import org.junit.Test;

import javax.imageio.IIOException;
import java.io.File;
import java.io.IOException;

import static org.junit.Assert.*;

public class MainTest {

    @Test
    public void main() throws Exception {
        String[] testKit1 = {"incorrectInputFile.jpg", "ToGrey", "output.bmp"};
        Main a;
        try {
            Main.main(testKit1);
        } catch (Exception e) {
            if (!(e instanceof IIOException || e instanceof NullPointerException))
                throw e;
        }

        String[] testKit2 = {"pics" + File.separator + "input.bmp", "ToGrey"};
        try {
            Main.main(testKit2);
        } catch (Exception e) {
            if (e.getMessage() != "Number of parameters is incorrect!\n")
                throw e;
        }

        String[] testKit3 = {"pics" + File.separator + "input.bmp", "incorrectFilterName", "output.bmp"};
        try {
            Main.main(testKit3);
        } catch (Exception e) {
            if (e.getMessage() != "Filter name is incorrect!\n")
                throw e;
        }
    }
}