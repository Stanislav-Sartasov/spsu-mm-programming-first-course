#include "startfilter.h"

using namespace std;

StartFilter::StartFilter(int argc, char* argv[]) : a(argc, argv)
{

}

StartFilter::StartFilter(): a(0, 0)
{

}

StartFilter::~StartFilter()
{
}

void StartFilter::getCommand(int argc, char *argv[])
{
    a.~Command();
    a = Command(argc, argv);
}

void StartFilter::goFilter()
{
    a.verify();
    if (!a.status())
    {
        cout << "error in command\n";
        return;
    }
    Image im;
    struct cmd load = a.putGommand();
    im.openBMP(load.fileIn);

    if (strcmp(load.mod, "gray") == 0)
    {
        im.gary();
        im.closeBMP(load.fileOut);
        cout << load.mod << " is done good\n";
        return;
    }
    if (strcmp(load.mod, "median") == 0)
    {
        im.median();
        im.closeBMP(load.fileOut);
        cout << load.mod << " is done good\n";
        return;
    }
    if (strcmp(load.mod, "gauss") == 0)
    {
        im.gauss();
        im.closeBMP(load.fileOut);
        cout << load.mod << " is done good\n";
        return;
    }
    if (strcmp(load.mod, "sobelX") == 0)
    {
        im.sobelX();
        im.closeBMP(load.fileOut);
        cout << load.mod << " is done good\n";
        return;
    }
    if (strcmp(load.mod, "sobelY") == 0)
    {
        im.sobelY();
        im.closeBMP(load.fileOut);
        cout << load.mod << " is done good\n";
        return;
    }
    if (strcmp(load.mod, "sobelAll") == 0)
    {
        im.sobelAll();
        im.closeBMP(load.fileOut);
        cout << load.mod << " is done good\n";
        return;
    }
//    if (strcmp(load.mod, "blueGray") == 0)
//    {
//        blueGray(image, indexX, indexY);
//        return;
//    }
//    if (strcmp(load.mod, "redGray") == 0)
//    {
//        redGray(image, indexX, indexY);
//        return;
//    }
//    if (strcmp(load.mod, "greenGray") == 0)
//    {
//        greenGray(image, indexX, indexY);
//        return;
//    }
    //im.gary();
    //im.gauss();
    //im.sobelY();
    //im.median();
    //im.sobelAll();
}
