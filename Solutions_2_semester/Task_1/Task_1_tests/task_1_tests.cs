using NUnit.Framework;
using Task_1;
using System.IO;
using System.Drawing;

namespace Task_1_tests
{
    public class main_tests
    {
        public main_tests()
        {
            System.IO.File.WriteAllBytes("test.bmp", Properties.Resources.test);
            System.IO.File.WriteAllBytes("median_sz_5_m_square_ref.bmp", Properties.Resources.median_sz_5_m_square_ref);
            System.IO.File.WriteAllBytes("median_sz_15_m_square_ref.bmp", Properties.Resources.median_sz_15_m_square_ref);
            System.IO.File.WriteAllBytes("middle_sz_5_m_square_ref.bmp", Properties.Resources.middle_sz_5_m_square_ref);
            System.IO.File.WriteAllBytes("middle_sz_15_m_square_ref.bmp", Properties.Resources.middle_sz_15_m_square_ref);
            System.IO.File.WriteAllBytes("gaussian_sz_5_sg_1.6_m_square_ref.bmp", Properties.Resources.gaussian_sz_5_sg_1_6_m_square_ref);
            System.IO.File.WriteAllBytes("gaussian_sz_15_sg_3_m_square_ref.bmp", Properties.Resources.gaussian_sz_15_sg_3_m_square_ref);
            System.IO.File.WriteAllBytes("shade_ref.bmp", Properties.Resources.shade_ref);
            System.IO.File.WriteAllBytes("sobel_x_th_2_ref.bmp", Properties.Resources.sobel_x_th_2_ref);
            System.IO.File.WriteAllBytes("sobel_y_th_2_ref.bmp", Properties.Resources.sobel_y_th_2_ref);
            System.IO.File.WriteAllBytes("sobel_th_2_ref.bmp", Properties.Resources.sobel_th_2_ref);
        }

        static internal int are_ecual_files(string x_name, string y_name)
        {
            if (!File.Exists(x_name))
                return 2;
            if (!File.Exists(y_name))
                return 2;

            BinaryReader x = new BinaryReader(File.Open(x_name, FileMode.Open));
            BinaryReader y = new BinaryReader(File.Open(y_name, FileMode.Open));

            int err_code;

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
        public void main_with_err_code()
        {
            if (!File.Exists("test.bmp"))
                Assert.Warn("test file not exist");

            if (File.Exists("not_a_test.bmp"))
                Assert.Warn("not_a_test file exist");

            Assert.AreEqual(image.invalid_input, filters.Main(new string[] { "incorrect"}));
            Assert.AreEqual(image.invalid_input, filters.Main(new string[] { "incorrect\"" }));
            Assert.AreEqual(image.invalid_input, filters.Main(new string[] { "test.bmp", "not a filter" }));
            Assert.AreEqual(image.invalid_input, filters.Main(new string[] { "test.bmp", "median", "/sg", "main_with_err_cod_out.bmp" }));
            Assert.AreEqual(image.file_not_exist, filters.Main(new string[] { "not_a_test.bmp", "median", "main_with_err_cod_out.bmp" }));
            Assert.AreEqual(image.invalid_input, filters.Main(new string[] { "test.bmp", "not a filter", "main_with_err_cod_out.bmp" }));
            Assert.AreEqual(false, File.Exists("main_with_err_cod_out.bmp"));
        }
        [Test]
        public void median_sz_5_m_square()
        {
            try
            {
                if (!File.Exists("median_sz_5_m_square_ref.bmp"))
                    Assert.Warn("ref file not exist");

                if (!File.Exists("test.bmp"))
                    Assert.Warn("test file not exist");

                string[][] input = new string[][]
                    {
                        new string[] { "test.bmp", "median", "/sz", "=", "5", "/m", "=", "square", "median_sz_5_m_square_out.bmp" },
                        new string[] { "test.bmp", "median", "/sz", "5", "/m", "square", "median_sz_5_m_square_out.bmp"  },
                        new string[] { "test.bmp", "median", "/sz", "4", "/m", "square", "median_sz_5_m_square_out.bmp"  },             //sz = 4 must work like sz = 5
                        new string[] { "test.bmp", "median", "median_sz_5_m_square_out.bmp" }
                    };

                for (int i = 0; i < 4; i++)
                {
                    Assert.AreEqual(0, filters.Main(input[i]));
                    Assert.AreEqual(0, are_ecual_files("median_sz_5_m_square_ref.bmp", "median_sz_5_m_square_out.bmp"));
                }
            }
            catch
            {
                Assert.Warn("unexpected error");
            }
        }
        [Test]
        public void median_sz_15_m_square()
        {
            try
            {
                if (!File.Exists("median_sz_15_m_square_ref.bmp"))
                    Assert.Warn("ref file not exist");

                if (!File.Exists("test.bmp"))
                    Assert.Warn("test file not exist");

                string[][] input = new string[][]
                    {
                        new string[] { "test.bmp", "median", "/sz", "=", "15", "/m", "=", "square", "median_sz_15_m_square_out.bmp" },
                        new string[] { "test.bmp", "median", "/sz", "15", "/m", "square", "median_sz_15_m_square_out.bmp"  },
                        new string[] { "test.bmp", "median", "/sz", "14", "/m", "square", "median_sz_15_m_square_out.bmp"  }
                    };

                for (int i = 0; i < 3; i++)
                {
                    Assert.AreEqual(0, filters.Main(input[i]));
                    Assert.AreEqual(0, are_ecual_files("median_sz_15_m_square_ref.bmp", "median_sz_15_m_square_out.bmp"));
                }
            }
            catch
            {
                Assert.Warn("unexpected error");
            }
        }
        [Test]
        public void middle_sz_5_m_square()
        {
            try
            {
                if (!File.Exists("middle_sz_5_m_square_ref.bmp"))
                    Assert.Warn("ref file not exist");

                if (!File.Exists("test.bmp"))
                    Assert.Warn("test file not exist");

                string[][] input = new string[][]
                    {
                        new string[] { "test.bmp", "middle", "/sz", "=", "5", "/m", "=", "square", "middle_sz_5_m_square_out.bmp" },
                        new string[] { "test.bmp", "middle", "/sz", "5", "/m", "square", "middle_sz_5_m_square_out.bmp"  },
                        new string[] { "test.bmp", "middle", "/sz", "4", "/m", "square", "middle_sz_5_m_square_out.bmp"  },             //sz = 4 must work like sz = 5
                        new string[] { "test.bmp", "middle", "middle_sz_5_m_square_out.bmp" }
                    };

                for (int i = 0; i < 4; i++)
                {
                    Assert.AreEqual(0, filters.Main(input[i]));
                    Assert.AreEqual(0, are_ecual_files("middle_sz_5_m_square_ref.bmp", "middle_sz_5_m_square_out.bmp"));
                }
            }
            catch
            {
                Assert.Warn("unexpected error");
            }
        }
        [Test]
        public void middle_sz_15_m_square()
        {
            try
            {
                if (!File.Exists("middle_sz_15_m_square_ref.bmp"))
                    Assert.Warn("ref file not exist");

                if (!File.Exists("test.bmp"))
                    Assert.Warn("test file not exist");

                string[][] input = new string[][]
                    {
                        new string[] { "test.bmp", "middle", "/sz", "=", "15", "/m", "=", "square", "middle_sz_15_m_square_out.bmp" },
                        new string[] { "test.bmp", "middle", "/sz", "15", "/m", "square", "middle_sz_15_m_square_out.bmp"  },
                        new string[] { "test.bmp", "middle", "/sz", "14", "/m", "square", "middle_sz_15_m_square_out.bmp"  }
                    };

                for (int i = 0; i < 3; i++)
                {
                    Assert.AreEqual(0, filters.Main(input[i]));
                    Assert.AreEqual(0, are_ecual_files("middle_sz_15_m_square_ref.bmp", "middle_sz_15_m_square_out.bmp"));
                }
            }
            catch
            {
                Assert.Warn("unexpected error");
            }
        }
        [Test]
        public void gaussian_sz_5_sg_1_6_m_square()
        {
            try
            {
                if (!File.Exists("gaussian_sz_5_sg_1.6_m_square_ref.bmp"))
                    Assert.Warn("ref file not exist");

                if (!File.Exists("test.bmp"))
                    Assert.Warn("test file not exist");

                string[][] input = new string[][]
                    {
                        new string[] { "test.bmp", "gaussian", "/sz", "=", "5", "/sg", "=", "1,6", "/m", "=", "square", "gaussian_sz_5_sg_1.6_m_square_out.bmp" },
                        new string[] { "test.bmp", "gaussian", "/sz", "5", "/sg", "1,6", "/m", "square", "gaussian_sz_5_sg_1.6_m_square_out.bmp"  },
                        new string[] { "test.bmp", "gaussian", "/sz", "4", "/sg", "1,6", "/m", "square", "gaussian_sz_5_sg_1.6_m_square_out.bmp"  },
                        new string[] { "test.bmp", "gaussian", "/sg", "1,6","gaussian_sz_5_sg_1.6_m_square_out.bmp" }
                    };

                for (int i = 0; i < 4; i++)
                {
                    Assert.AreEqual(0, filters.Main(input[i]));
                    Assert.AreEqual(0, are_ecual_files("gaussian_sz_5_sg_1.6_m_square_ref.bmp", "gaussian_sz_5_sg_1.6_m_square_out.bmp"));
                }
            }
            catch
            {
                Assert.Warn("unexpected error");
            }
        }
        [Test]
        public void gaussian_sz_15_sg_3_m_square()
        {
            try
            {
                if (!File.Exists("gaussian_sz_15_sg_3_m_square_ref.bmp"))
                    Assert.Warn("ref file not exist");

                if (!File.Exists("test.bmp"))
                    Assert.Warn("test file not exist");

                string[][] input = new string[][]
                    {
                        new string[] { "test.bmp", "gaussian", "/sz", "=", "15", "/sg", "=", "3", "/m", "=", "square", "gaussian_sz_15_sg_3_m_square_out.bmp" },
                        new string[] { "test.bmp", "gaussian", "/sz", "15", "/sg", "3", "/m", "square", "gaussian_sz_15_sg_3_m_square_out.bmp" },
                        new string[] { "test.bmp", "gaussian", "/sz", "14", "/sg", "3", "/m", "square", "gaussian_sz_15_sg_3_m_square_out.bmp" }
                    };

                for (int i = 0; i < 3; i++)
                {
                    Assert.AreEqual(0, filters.Main(input[i]));
                    Assert.AreEqual(0, are_ecual_files("gaussian_sz_15_sg_3_m_square_ref.bmp", "gaussian_sz_15_sg_3_m_square_out.bmp"));
                }
            }
            catch
            {
                Assert.Warn("unexpected error");
            }
        }
        [Test]
        public void shade_test()
        {
            try
            {
                if (!File.Exists("shade_ref.bmp"))
                    Assert.Warn("ref file not exist");

                if (!File.Exists("test.bmp"))
                    Assert.Warn("test file not exist");

                Assert.AreEqual(0, filters.Main(new string[] { "test.bmp", "shade", "shade_out.bmp" }));
                Assert.AreEqual(0, are_ecual_files("shade_ref.bmp", "shade_out.bmp"));
            }
            catch
            {
                Assert.Warn("unexpected error");
            }
        }
        [Test]
        public void sobel_x_th_2()
        {
            try
            {
                if (!File.Exists("sobel_x_th_2_ref.bmp"))
                    Assert.Warn("ref file not exist");

                if (!File.Exists("test.bmp"))
                    Assert.Warn("test file not exist");

                string[][] input = new string[][]
                    {
                        new string[] { "test.bmp", "sobel_x", "/th", "=", "2", "sobel_x_th_2_out.bmp" },
                        new string[] { "test.bmp", "sobel_x", "/th", "2", "sobel_x_th_2_out.bmp" },
                        new string[] { "test.bmp", "sobel_x", "sobel_x_th_2_out.bmp" }
                    };

                for (int i = 0; i < 3; i++)
                {
                    Assert.AreEqual(0, filters.Main(input[i]));
                    Assert.AreEqual(0, are_ecual_files("sobel_x_th_2_ref.bmp", "sobel_x_th_2_out.bmp"));
                }
            }
            catch
            {
                Assert.Warn("unexpected error");
            }
        }
        [Test]
        public void sobel_y_th_2()
        {
            try
            {
                if (!File.Exists("sobel_y_th_2_ref.bmp"))
                    Assert.Warn("ref file not exist");

                if (!File.Exists("test.bmp"))
                    Assert.Warn("test file not exist");

                string[][] input = new string[][]
                    {
                        new string[] { "test.bmp", "sobel_y", "/th", "=", "2", "sobel_y_th_2_out.bmp" },
                        new string[] { "test.bmp", "sobel_y", "/th", "2", "sobel_y_th_2_out.bmp" },
                        new string[] { "test.bmp", "sobel_y", "sobel_y_th_2_out.bmp" }
                    };

                for (int i = 0; i < 3; i++)
                {
                    Assert.AreEqual(0, filters.Main(input[i]));
                    Assert.AreEqual(0, are_ecual_files("sobel_y_th_2_ref.bmp", "sobel_y_th_2_out.bmp"));
                }
            }
            catch
            {
                Assert.Warn("unexpected error");
            }
        }
        [Test]
        public void sobel_th_2()
        {
            try
            {
                if (!File.Exists("sobel_th_2_ref.bmp"))
                    Assert.Warn("ref file not exist");

                if (!File.Exists("test.bmp"))
                    Assert.Warn("test file not exist");

                string[][] input = new string[][]
                    {
                        new string[] { "test.bmp", "sobel", "/th", "=", "2", "sobel_th_2_out.bmp" },
                        new string[] { "test.bmp", "sobel", "/th", "2", "sobel_th_2_out.bmp" },
                        new string[] { "test.bmp", "sobel", "sobel_th_2_out.bmp" }
                    };

                for (int i = 0; i < 3; i++)
                {
                    Assert.AreEqual(0, filters.Main(input[i]));
                    Assert.AreEqual(0, are_ecual_files("sobel_th_2_ref.bmp", "sobel_th_2_out.bmp"));
                }
            }
            catch
            {
                Assert.Warn("unexpected error");
            }
        }
    }
    public class other_tests
    {
        public other_tests()
        {
            if (!File.Exists("test.bmp"))
                System.IO.File.WriteAllBytes("test.bmp", Properties.Resources.test);
            System.IO.File.WriteAllBytes("median_sz_15_m_diagonal_cross_ref.bmp", Properties.Resources.median_sz_15_m_diagonal_cross_ref);
        }

        [Test]
        public void create_square_mask_sz_9_test()
        {
            byte[,] mask_exp = new byte[,]
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
            Assert.AreEqual(mask_exp, image.create_square_mask(9));
            Assert.AreEqual(mask_exp, image.create_square_mask(8));
            Assert.AreEqual(null, image.create_square_mask(-1));
        }
        [Test]
        public void create_circle_mask_sz_9_test()
        {
            byte[,] mask_exp = new byte[,]
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
            Assert.AreEqual(mask_exp, image.create_circle_mask(9));
            Assert.AreEqual(mask_exp, image.create_circle_mask(8));
            Assert.AreEqual(null, image.create_circle_mask(-1));
        }
        [Test]
        public void create_cross_mask_sz_9_test()
        {
            byte[,] mask_exp = new byte[,]
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
            Assert.AreEqual(mask_exp, image.create_cross_mask(9));
            Assert.AreEqual(mask_exp, image.create_cross_mask(8));
            Assert.AreEqual(null, image.create_cross_mask(-1));
        }
        [Test]
        public void create_diagonal_cross_mask_sz_9_test()
        {
            byte[,] mask_exp = new byte[,]
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
            Assert.AreEqual(mask_exp, image.create_diagonal_cross_mask(9));
            Assert.AreEqual(mask_exp, image.create_diagonal_cross_mask(8));
            Assert.AreEqual(null, image.create_diagonal_cross_mask(-1));
        }
        [Test]
        public void create_empty_square_mask_sz_9_test()
        {
            byte[,] mask_exp = new byte[,]
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
            byte[,] mask_act = image.create_empty_square_mask(9);
            Assert.AreEqual(mask_exp, image.create_empty_square_mask(9));
            Assert.AreEqual(mask_exp, image.create_empty_square_mask(8));
            Assert.AreEqual(null, image.create_empty_square_mask(-1));
        }
        [Test]
        public void median_sz_15_m_diagonal_cross()
        {
            try
            {
                if (!File.Exists("median_sz_15_m_diagonal_cross_ref.bmp"))
                    Assert.Warn("ref file not exist");

                if (!File.Exists("test.bmp"))
                    Assert.Warn("test file not exist");

                Assert.AreEqual(0, filters.Main(new string[] { "test.bmp", "median", "/sz", "15", "/m", "diagonal_cross", "median_sz_15_m_diagonal_cross_out.bmp" }));
                Assert.AreEqual(0, main_tests.are_ecual_files("median_sz_15_m_diagonal_cross_ref.bmp", "median_sz_15_m_diagonal_cross_out.bmp"));
            }
            catch
            {
                Assert.Warn("unexpected error");
            }
        }
    }
}