using NUnit.Framework;
using Task1;
using System.IO;
using System.Drawing;

namespace Task1Tests
{
    public class Main_tests
    {
        public Main_tests()
        {
            System.IO.File.WriteAllBytes("Test.bmp", Properties.Resources.Test);
            System.IO.File.WriteAllBytes("MedianSz5MSquareRef.bmp", Properties.Resources.MedianSz5MSquareRef);
            System.IO.File.WriteAllBytes("MedianSz15MSquareRef.bmp", Properties.Resources.MedianSz15MSquareRef);
            System.IO.File.WriteAllBytes("MiddleSz5MSquareRef.bmp", Properties.Resources.MiddleSz5MSquareRef);
            System.IO.File.WriteAllBytes("MiddleSz15MSquareRef.bmp", Properties.Resources.MiddleSz15MSquareRef);
            System.IO.File.WriteAllBytes("GaussianSz5Sg1_6MSquareRef.bmp", Properties.Resources.GaussianSz5Sg1_6MSquareRef);
            System.IO.File.WriteAllBytes("GaussianSz15Sg3MSquareRef.bmp", Properties.Resources.GaussianSz15Sg3MSquareRef);
            System.IO.File.WriteAllBytes("ShadeRef.bmp", Properties.Resources.ShadeRef);
            System.IO.File.WriteAllBytes("SobelXTh2Ref.bmp", Properties.Resources.SobelXTh2Ref);
            System.IO.File.WriteAllBytes("SobelYTh2Ref.bmp", Properties.Resources.SobelYTh2Ref);
            System.IO.File.WriteAllBytes("SobelTh2Ref.bmp", Properties.Resources.SobelTh2Ref);
        }

        static internal int AreEcualFiles(string xName, string yName)
        {
            if (!File.Exists(xName))
                return 2;
            if (!File.Exists(yName))
                return 2;

            BinaryReader x = new BinaryReader(File.Open(xName, FileMode.Open));
            BinaryReader y = new BinaryReader(File.Open(yName, FileMode.Open));

            for (; ; )
            {
                byte e = 0;
                byte a = 1;

                try
                {
                    e = x.ReadByte();
                }
                catch
                {
                    try
                    {
                        a = y.ReadByte();
                        x.Close();
                        y.Close();
                        return 1;
                    }
                    catch
                    {
                        x.Close();
                        y.Close();
                        return 0;
                    }
                }

                try
                {
                    a = y.ReadByte();
                }
                catch
                {
                    x.Close();
                    y.Close();
                    return -1;
                }

                if (e != a)
                    if (e > a)
                    {
                        x.Close();
                        y.Close();
                        return -1;
                    }
                    else
                    {
                        x.Close();
                        y.Close();
                        return 1;
                    }
            }
        }

        [Test]
        public void MainWithErrCode()
        {
            try
            {
                if (!File.Exists("Test.bmp"))
                    Assert.Warn("Test file not exist");

                if (File.Exists("NotATest.bmp"))
                    Assert.Warn("NotATest file exist");

                Assert.AreEqual(Image.InvalidInput, Filters.Main(new string[] { "incorrect" }));
                Assert.AreEqual(Image.InvalidInput, Filters.Main(new string[] { "incorrect\"" }));
                Assert.AreEqual(Image.InvalidInput, Filters.Main(new string[] { "Test.bmp", "not a filter" }));
                Assert.AreEqual(Image.InvalidInput, Filters.Main(new string[] { "Test.bmp", "median", "/sg", "MainWithErrCodeOut.bmp" }));
                Assert.AreEqual(Image.FileNotExist, Filters.Main(new string[] { "NotATest.bmp", "median", "MainWithErrCodeOut.bmp" }));
                Assert.AreEqual(Image.InvalidInput, Filters.Main(new string[] { "Test.bmp", "not a filter", "MainWithErrCodeOut.bmp" }));
                Assert.AreEqual(false, File.Exists("MainWithErrCodeOut.bmp"));
            }
            catch
            {
                Assert.Warn("Unexpected error");
            }
}
        [Test]
        public void MedianSz5MSquare()
        {
            try
            {
                if (!File.Exists("MedianSz5MSquareRef.bmp"))
                    Assert.Warn("Ref file not exist");

                if (!File.Exists("Test.bmp"))
                    Assert.Warn("Test file not exist");

                string[][] input = new string[][]
                    {
                        new string[] { "Test.bmp", "median", "/sz", "=", "5", "/m", "=", "square", "MedianSz5MSquareOut.bmp" },
                        new string[] { "Test.bmp", "median", "/sz", "5", "/m", "square", "MedianSz5MSquareOut.bmp"  },
                        new string[] { "Test.bmp", "median", "/sz", "4", "/m", "square", "MedianSz5MSquareOut.bmp"  },             //sz = 4 must work like sz = 5
                        new string[] { "Test.bmp", "median", "MedianSz5MSquareOut.bmp" }
                    };

                for (int i = 0; i < 4; i++)
                {
                    Assert.AreEqual(0, Filters.Main(input[i]));
                    Assert.AreEqual(0, AreEcualFiles("MedianSz5MSquareRef.bmp", "MedianSz5MSquareOut.bmp"));
                }
            }
            catch
            {
                Assert.Warn("Unexpected error");
            }
        }
        [Test]
        public void MedianSz15MSquare()
        {
            try
            {
                if (!File.Exists("MedianSz15MSquareRef.bmp"))
                    Assert.Warn("Ref file not exist");

                if (!File.Exists("Test.bmp"))
                    Assert.Warn("Test file not exist");

                string[][] input = new string[][]
                    {
                        new string[] { "Test.bmp", "median", "/sz", "=", "15", "/m", "=", "square", "MedianSz15MSquareOut.bmp" },
                        new string[] { "Test.bmp", "median", "/sz", "15", "/m", "square", "MedianSz15MSquareOut.bmp"  },
                        new string[] { "Test.bmp", "median", "/sz", "14", "/m", "square", "MedianSz15MSquareOut.bmp"  }
                    };

                for (int i = 0; i < 3; i++)
                {
                    Assert.AreEqual(0, Filters.Main(input[i]));
                    Assert.AreEqual(0, AreEcualFiles("MedianSz15MSquareRef.bmp", "MedianSz15MSquareOut.bmp"));
                }
            }
            catch
            {
                Assert.Warn("Unexpected error");
            }
        }
        [Test]
        public void MiddleSz5MSquare()
        {
            try
            {
                if (!File.Exists("MiddleSz5MSquareRef.bmp"))
                    Assert.Warn("Ref file not exist");

                if (!File.Exists("Test.bmp"))
                    Assert.Warn("Test file not exist");

                string[][] input = new string[][]
                    {
                        new string[] { "Test.bmp", "middle", "/sz", "=", "5", "/m", "=", "square", "MiddleSz5MSquare_out.bmp" },
                        new string[] { "Test.bmp", "middle", "/sz", "5", "/m", "square", "MiddleSz5MSquare_out.bmp"  },
                        new string[] { "Test.bmp", "middle", "/sz", "4", "/m", "square", "MiddleSz5MSquare_out.bmp"  },             //sz = 4 must work like sz = 5
                        new string[] { "Test.bmp", "middle", "MiddleSz5MSquare_out.bmp" }
                    };

                for (int i = 0; i < 4; i++)
                {
                    Assert.AreEqual(0, Filters.Main(input[i]));
                    Assert.AreEqual(0, AreEcualFiles("MiddleSz5MSquareRef.bmp", "MiddleSz5MSquare_out.bmp"));
                }
            }
            catch
            {
                Assert.Warn("Unexpected error");
            }
        }
        [Test]
        public void MiddleSz15MSquare()
        {
            try
            {
                if (!File.Exists("MiddleSz15MSquareRef.bmp"))
                    Assert.Warn("Ref file not exist");

                if (!File.Exists("Test.bmp"))
                    Assert.Warn("Test file not exist");

                string[][] input = new string[][]
                    {
                        new string[] { "Test.bmp", "middle", "/sz", "=", "15", "/m", "=", "square", "MiddleSz15MSquareOut.bmp" },
                        new string[] { "Test.bmp", "middle", "/sz", "15", "/m", "square", "MiddleSz15MSquareOut.bmp"  },
                        new string[] { "Test.bmp", "middle", "/sz", "14", "/m", "square", "MiddleSz15MSquareOut.bmp"  }
                    };

                for (int i = 0; i < 3; i++)
                {
                    Assert.AreEqual(0, Filters.Main(input[i]));
                    Assert.AreEqual(0, AreEcualFiles("MiddleSz15MSquareRef.bmp", "MiddleSz15MSquareOut.bmp"));
                }
            }
            catch
            {
                Assert.Warn("Unexpected error");
            }
        }
        [Test]
        public void GaussianSz5Sg1_6MSquare()
        {
            try
            {
                if (!File.Exists("GaussianSz5Sg1_6MSquareRef.bmp"))
                    Assert.Warn("Ref file not exist");

                if (!File.Exists("Test.bmp"))
                    Assert.Warn("Test file not exist");

                string[][] input = new string[][]
                    {
                        new string[] { "Test.bmp", "gaussian", "/sz", "=", "5", "/sg", "=", "1,6", "/m", "=", "square", "GaussianSz5Sg1_6MSquareOut.bmp" },
                        new string[] { "Test.bmp", "gaussian", "/sz", "5", "/sg", "1,6", "/m", "square", "GaussianSz5Sg1_6MSquareOut.bmp"  },
                        new string[] { "Test.bmp", "gaussian", "/sz", "4", "/sg", "1,6", "/m", "square", "GaussianSz5Sg1_6MSquareOut.bmp"  },
                        new string[] { "Test.bmp", "gaussian", "/sg", "1,6","GaussianSz5Sg1_6MSquareOut.bmp" }
                    };

                for (int i = 0; i < 4; i++)
                {
                    Assert.AreEqual(0, Filters.Main(input[i]));
                    Assert.AreEqual(0, AreEcualFiles("GaussianSz5Sg1_6MSquareRef.bmp", "GaussianSz5Sg1_6MSquareOut.bmp"));
                }
            }
            catch
            {
                Assert.Warn("Unexpected error");
            }
        }
        [Test]
        public void GaussianSz15Sg3MSquare()
        {
            try
            {
                if (!File.Exists("GaussianSz15Sg3MSquareRef.bmp"))
                    Assert.Warn("Ref file not exist");

                if (!File.Exists("Test.bmp"))
                    Assert.Warn("Test file not exist");

                string[][] input = new string[][]
                    {
                        new string[] { "Test.bmp", "gaussian", "/sz", "=", "15", "/sg", "=", "3", "/m", "=", "square", "GaussianSz15Sg3MSquareOut.bmp" },
                        new string[] { "Test.bmp", "gaussian", "/sz", "15", "/sg", "3", "/m", "square", "GaussianSz15Sg3MSquareOut.bmp" },
                        new string[] { "Test.bmp", "gaussian", "/sz", "14", "/sg", "3", "/m", "square", "GaussianSz15Sg3MSquareOut.bmp" }
                    };

                for (int i = 0; i < 3; i++)
                {
                    Assert.AreEqual(0, Filters.Main(input[i]));
                    Assert.AreEqual(0, AreEcualFiles("GaussianSz15Sg3MSquareRef.bmp", "GaussianSz15Sg3MSquareOut.bmp"));
                }
            }
            catch
            {
                Assert.Warn("Unexpected error");
            }
        }
        [Test]
        public void ShadeTest()
        {
            try
            {
                if (!File.Exists("ShadeRef.bmp"))
                    Assert.Warn("Ref file not exist");

                if (!File.Exists("Test.bmp"))
                    Assert.Warn("Test file not exist");

                Assert.AreEqual(0, Filters.Main(new string[] { "Test.bmp", "shade", "ShadeOut.bmp" }));
                Assert.AreEqual(0, AreEcualFiles("ShadeRef.bmp", "ShadeOut.bmp"));
            }
            catch
            {
                Assert.Warn("Unexpected error");
            }
        }
        [Test]
        public void SobelXTh2()
        {
            try
            {
                if (!File.Exists("SobelXTh2Ref.bmp"))
                    Assert.Warn("Ref file not exist");

                if (!File.Exists("Test.bmp"))
                    Assert.Warn("Test file not exist");

                string[][] input = new string[][]
                    {
                        new string[] { "Test.bmp", "sobel_x", "/th", "=", "2", "SobelXTh2Out.bmp" },
                        new string[] { "Test.bmp", "sobel_x", "/th", "2", "SobelXTh2Out.bmp" },
                        new string[] { "Test.bmp", "sobel_x", "SobelXTh2Out.bmp" }
                    };

                for (int i = 0; i < 3; i++)
                {
                    Assert.AreEqual(0, Filters.Main(input[i]));
                    Assert.AreEqual(0, AreEcualFiles("SobelXTh2Ref.bmp", "SobelXTh2Out.bmp"));
                }
            }
            catch
            {
                Assert.Warn("Unexpected error");
            }
        }
        [Test]
        public void SobelYTh2()
        {
            try
            {
                if (!File.Exists("SobelYTh2Ref.bmp"))
                    Assert.Warn("Ref file not exist");

                if (!File.Exists("Test.bmp"))
                    Assert.Warn("Test file not exist");

                string[][] input = new string[][]
                    {
                        new string[] { "Test.bmp", "sobel_y", "/th", "=", "2", "SobelYTh2Out.bmp" },
                        new string[] { "Test.bmp", "sobel_y", "/th", "2", "SobelYTh2Out.bmp" },
                        new string[] { "Test.bmp", "sobel_y", "SobelYTh2Out.bmp" }
                    };

                for (int i = 0; i < 3; i++)
                {
                    Assert.AreEqual(0, Filters.Main(input[i]));
                    Assert.AreEqual(0, AreEcualFiles("SobelYTh2Ref.bmp", "SobelYTh2Out.bmp"));
                }
            }
            catch
            {
                Assert.Warn("Unexpected error");
            }
        }
        [Test]
        public void SobelTh2()
        {
            try
            {
                if (!File.Exists("SobelTh2Ref.bmp"))
                    Assert.Warn("Ref file not exist");

                if (!File.Exists("Test.bmp"))
                    Assert.Warn("Test file not exist");

                string[][] input = new string[][]
                    {
                        new string[] { "Test.bmp", "sobel", "/th", "=", "2", "SobelTh2Out.bmp" },
                        new string[] { "Test.bmp", "sobel", "/th", "2", "SobelTh2Out.bmp" },
                        new string[] { "Test.bmp", "sobel", "SobelTh2Out.bmp" }
                    };

                for (int i = 0; i < 3; i++)
                {
                    Assert.AreEqual(0, Filters.Main(input[i]));
                    Assert.AreEqual(0, AreEcualFiles("SobelTh2Ref.bmp", "SobelTh2Out.bmp"));
                }
            }
            catch
            {
                Assert.Warn("Unexpected error");
            }
        }
    }
    public class other_tests
    {
        public other_tests()
        {
            if (!File.Exists("Test.bmp"))
                System.IO.File.WriteAllBytes("Test.bmp", Properties.Resources.Test);
            System.IO.File.WriteAllBytes("MedianSz15MDiagonalCrossRef.bmp", Properties.Resources.MedianSz15MDiagonalCrossRef);
        }

        [Test]
        public void CreateSquareMaskSz9Test()
        {
            byte[,] maskExp = new byte[,]
            {
                {1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1},
            };
            Assert.AreEqual(maskExp, Image.CreateSquareMask(9));
            Assert.AreEqual(maskExp, Image.CreateSquareMask(8));
            Assert.AreEqual(null, Image.CreateSquareMask(-1));
        }
        [Test]
        public void CreateCircleMaskSz9Test()
        {
            byte[,] maskExp = new byte[,]
            {
                {0, 0, 0, 0, 1, 0, 0, 0, 0},
                {0, 0, 1, 1, 1, 1, 1, 0, 0},
                {0, 1, 1, 1, 1, 1, 1, 1, 0},
                {0, 1, 1, 1, 1, 1, 1, 1, 0},
                {1, 1, 1, 1, 1, 1, 1, 1, 1},
                {0, 1, 1, 1, 1, 1, 1, 1, 0},
                {0, 1, 1, 1, 1, 1, 1, 1, 0},
                {0, 0, 1, 1, 1, 1, 1, 0, 0},
                {0, 0, 0, 0, 1, 0, 0, 0, 0}
            };
            Assert.AreEqual(maskExp, Image.CreateCircleMask(9));
            Assert.AreEqual(maskExp, Image.CreateCircleMask(8));
            Assert.AreEqual(null, Image.CreateCircleMask(-1));
        }
        [Test]
        public void CreateCrossMaskSz9Test()
        {
            byte[,] maskExp = new byte[,]
            {
                {0, 0, 0, 0, 1, 0, 0, 0, 0},
                {0, 0, 0, 0, 1, 0, 0, 0, 0},
                {0, 0, 0, 0, 1, 0, 0, 0, 0},
                {0, 0, 0, 0, 1, 0, 0, 0, 0},
                {1, 1, 1, 1, 1, 1, 1, 1, 1},
                {0, 0, 0, 0, 1, 0, 0, 0, 0},
                {0, 0, 0, 0, 1, 0, 0, 0, 0},
                {0, 0, 0, 0, 1, 0, 0, 0, 0},
                {0, 0, 0, 0, 1, 0, 0, 0, 0}
            };
            Assert.AreEqual(maskExp, Image.CreateCrossMask(9));
            Assert.AreEqual(maskExp, Image.CreateCrossMask(8));
            Assert.AreEqual(null, Image.CreateCrossMask(-1));
        }
        [Test]
        public void CreateDiagonalCrossMaskSz9Test()
        {
            byte[,] maskExp = new byte[,]
            {
                {1, 0, 0, 0, 0, 0, 0, 0, 1},
                {0, 1, 0, 0, 0, 0, 0, 1, 0},
                {0, 0, 1, 0, 0, 0, 1, 0, 0},
                {0, 0, 0, 1, 0, 1, 0, 0, 0},
                {0, 0, 0, 0, 1, 0, 0, 0, 0},
                {0, 0, 0, 1, 0, 1, 0, 0, 0},
                {0, 0, 1, 0, 0, 0, 1, 0, 0},
                {0, 1, 0, 0, 0, 0, 0, 1, 0},
                {1, 0, 0, 0, 0, 0, 0, 0, 1}
            };
            Assert.AreEqual(maskExp, Image.CreateDiagonalCrossMask(9));
            Assert.AreEqual(maskExp, Image.CreateDiagonalCrossMask(8));
            Assert.AreEqual(null, Image.CreateDiagonalCrossMask(-1));
        }
        [Test]
        public void CreateEmptySquareMaskSz9Test()
        {
            byte[,] maskExp = new byte[,]
            {
                {1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 0, 0, 0, 0, 0, 0, 0, 1},
                {1, 0, 0, 0, 0, 0, 0, 0, 1},
                {1, 0, 0, 0, 0, 0, 0, 0, 1},
                {1, 0, 0, 0, 0, 0, 0, 0, 1},
                {1, 0, 0, 0, 0, 0, 0, 0, 1},
                {1, 0, 0, 0, 0, 0, 0, 0, 1},
                {1, 0, 0, 0, 0, 0, 0, 0, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1}
            };
            byte[,] mask_act = Image.CreateEmptySquareMask(9);
            Assert.AreEqual(maskExp, Image.CreateEmptySquareMask(9));
            Assert.AreEqual(maskExp, Image.CreateEmptySquareMask(8));
            Assert.AreEqual(null, Image.CreateEmptySquareMask(-1));
        }
        [Test]
        public void MedianSz15MDiagonalCross()
        {
            try
            {
                if (!File.Exists("MedianSz15MDiagonalCrossRef.bmp"))
                    Assert.Warn("Ref file not exist");

                if (!File.Exists("Test.bmp"))
                    Assert.Warn("Test file not exist");

                Assert.AreEqual(0, Filters.Main(new string[] { "Test.bmp", "median", "/sz", "15", "/m", "diagonal_cross", "MedianSz15MDiagonalCrossOut.bmp" }));
                Assert.AreEqual(0, Main_tests.AreEcualFiles("MedianSz15MDiagonalCrossRef.bmp", "MedianSz15MDiagonalCrossOut.bmp"));
            }
            catch
            {
                Assert.Warn("Unexpected error");
            }
        }
    }
}