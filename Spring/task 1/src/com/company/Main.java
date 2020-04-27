package com.company;

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

        BmpImage result = new BmpImage(pathInput);

        switch (filterName) {
            case "Averaging3" ->  result.averaging(3);
            case "Averaging5" ->  result.averaging(5);
            case "Gauss3" ->  result.gauss(3);
            case "Gauss5" ->  result.gauss(5);
            case "SobelX" ->  result.sobelX(3);
            case "SobelY" ->  result.sobelY(3);
            case "ToGrey" ->  result.toGrey();
        }

        result.write(pathOutput);
        System.out.println("Successfully!");
    }
}

