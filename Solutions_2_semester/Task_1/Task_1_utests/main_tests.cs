using NUnit.Framework;
using NUnit;
using System.IO;

namespace Task_1_utests
{
    public class main_tests
    {
        [Test]
        public void median_sz_5_m_square_test()
        {
            int err_code = Task_1.Program.Main(new string[] { "test.bmp", "median", "/sz", "5", "median_sz_5_m_square_out.bmp" });
            Assert.AreEqual(0, err_code);

            BinaryReader file_exp = new BinaryReader(File.Open("median_sz_5_m_square_ref.bmp", FileMode.Open));
            BinaryReader file_act = new BinaryReader(File.Open("median_sz_5_m_square_out.bmp", FileMode.Open));

            Assert.AreEqual(file_exp.ToString(), file_act.ToString());
        }
    }
}