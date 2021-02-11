using System;

namespace FilterService
{
    public static class MatrixCreator
    {
        public static double[,] CreateMatrix(string filterName)
        {
            double[,] matrix;
            switch (filterName)
            {
                case "SobelX":
                    matrix = new double[,]
                    {
                        {  1,  2,  1 },
                        {  0,  0,  0 },
                        { -1, -2, -1 }
                    };
                    break;
                case "SobelY":
                    matrix = new double[,]
                    {
                        { -1,  0,  1 },
                        { -2,  0,  2 },
                        { -1,  0,  1 }
                     };
                    break;
                case "Gauss3x3":
                    matrix = new double[,]
                    {
                        { 1 / 16.0, 2 / 16.0, 1 / 16.0 },
                        { 2 / 16.0, 4 / 16.0, 2 / 16.0 },
                        { 1 / 16.0, 2 / 16.0, 1 / 16.0 }
                    };
                    break;
                case "Gauss5x5":
                    matrix = new double[,]
                    {
                        { 1 / 256.0, 4  / 256.0, 6  / 256.0, 4  / 256.0, 1 / 256.0 },
                        { 4 / 256.0, 16 / 256.0, 24 / 256.0, 16 / 256.0, 4 / 256.0 },
                        { 6 / 256.0, 24 / 256.0, 36 / 256.0, 24 / 256.0, 6 / 256.0 },
                        { 4 / 256.0, 16 / 256.0, 24 / 256.0, 16 / 256.0, 4 / 256.0 },
                        { 1 / 256.0, 4  / 256.0, 6  / 256.0, 4  / 256.0, 1 / 256.0 }
                    };
                    break;
                case "Average":
                    matrix = new double[,]
                    {
                        { 1 / 9.0, 1 / 9.0, 1 / 9.0 },
                        { 1 / 9.0, 1 / 9.0, 1 / 9.0 },
                        { 1 / 9.0, 1 / 9.0, 1 / 9.0 }
                    };
                    break;
                default:
                    throw new ArgumentException("Invalid filter name");
            }
            return matrix;
        }
    }
}
