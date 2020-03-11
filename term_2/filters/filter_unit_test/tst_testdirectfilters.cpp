#include <QtTest>
#include "../image.h"
using namespace std;

// add necessary includes here

bool file_compare(char *result, char *test)
{
    ofstream logF("../filter_unit_test/test/log.txt");
    ifstream fileIn(result, ios::binary);
    fileIn.seekg(0, ios_base::end);
    int len = fileIn.tellg();
    fileIn.seekg(0);

    ifstream fileTest(test, ios::binary);
    fileTest.seekg(0, ios_base::end);
    int lenTest = fileTest.tellg();
    fileTest.seekg(0);

    if (len != lenTest)
    {
        throw std::error_code();
    }
    char *testBuffer, *buffer;
    testBuffer = new char [len];
    buffer = new char [len];
    fileTest.read(testBuffer, len);
    fileIn.read(buffer, len);

    for (int i = 0; i < len; ++i)
        if (testBuffer[i] != buffer[i])
        {
            logF << (int)testBuffer[i] << " " << (int)buffer[i] << " " << i <<  "t b i\n";
            throw  std::error_code();
        }

    delete [] testBuffer;
    delete [] buffer;
    fileIn.close();
    fileTest.close();
    logF.close();
    return 0;
}


class testDirectFilters : public QObject
{
    Q_OBJECT

public:
    testDirectFilters();
    ~testDirectFilters();

private slots:
    void test_case1();
    void test_case2();
    void test_case3();
    void test_case4();
    void test_case5();
    void test_case6();

};

testDirectFilters::testDirectFilters()
{

}

testDirectFilters::~testDirectFilters()
{

}

void testDirectFilters::test_case1()
{
    Image im;
    char str[100] = "../filter_unit_test/test/ga.bmp", str2[100] = "../filter_unit_test/test/gray.bmp", str3[100] = "../filter_unit_test/test/gray1.bmp";

    im.openBMP(str);
    im.gary();
    im.closeBMP(str3);

    file_compare(str3, str2);
}

void testDirectFilters::test_case2()
{
    Image im;
    char str[100] = "../filter_unit_test/test/ga.bmp", str2[100] = "../filter_unit_test/test/gauss.bmp", str3[100] = "../filter_unit_test/test/gauss1.bmp";

    im.openBMP(str);
    im.gauss();
    im.closeBMP(str3);

    file_compare(str3, str2);
}

void testDirectFilters::test_case3()
{
    Image im;
    char str[100] = "../filter_unit_test/test/ga.bmp", str2[100] = "../filter_unit_test/test/median.bmp", str3[100] = "../filter_unit_test/test/median1.bmp";

    im.openBMP(str);
    im.median();
    im.closeBMP(str3);

    file_compare(str3, str2);
}

void testDirectFilters::test_case4()
{
    Image im;
    char str[100] = "../filter_unit_test/test/ga.bmp", str2[100] = "../filter_unit_test/test/sobelX.bmp", str3[100] = "../filter_unit_test/test/sobelX1.bmp";

    im.openBMP(str);
    im.sobelX();
    im.closeBMP(str3);
    file_compare(str3, str2);
}

void testDirectFilters::test_case5()
{
    Image im;
    char str[100] = "../filter_unit_test/test/ga.bmp", str2[100] = "../filter_unit_test/test/sobelY.bmp", str3[100] = "../filter_unit_test/test/sobelY1.bmp";

    im.openBMP(str);
    im.sobelY();
    im.closeBMP(str3);

    file_compare(str3, str2);
}

void testDirectFilters::test_case6()
{
    Image im;
    char str[100] = "../filter_unit_test/test/ga.bmp", str2[100] = "../filter_unit_test/test/sobelAll.bmp", str3[100] = "../filter_unit_test/test/sobelAll1.bmp";

    im.openBMP(str);
    im.sobelAll();
    im.closeBMP(str3);

    file_compare(str3, str2);
}


QTEST_APPLESS_MAIN(testDirectFilters)

#include "tst_testdirectfilters.moc"
