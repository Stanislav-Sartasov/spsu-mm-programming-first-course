#include "command.h"
#include <cstring>
#include <iostream>

using namespace std;

Command::Command(int argc, char *argv[])
{
    this->argc = argc;
    this->argv = new char* [argc];
    for (int i = 0; i < argc; ++i)
    {
        this->argv[i] = new char [strlen(argv[i])];
        strcpy(this->argv[i], argv[i]);
    }
}

Command::~Command()
{
    for (int i = 0; i < argc; i++)
    {
        delete [] argv[i];
    }
    delete [] argv;
}

void Command::verify()
{


    if (argc < 4 ||
            !(strcmp(argv[2], "median") == 0 || strcmp(argv[2], "gauss") == 0 || strcmp(argv[2], "sobelX") == 0
            || strcmp(argv[2], "sobelY") == 0 || strcmp(argv[2], "bwInvert") == 0 || strcmp(argv[2], "sobelAll") == 0
            || strcmp(argv[2], "greenGray") == 0 || strcmp(argv[2], "blueGray") == 0 || strcmp(argv[2], "redGray") == 0 || strcmp(argv[2], "gray") == 0))
        {
            if ((argc == 2) && (strcmp(argv[1], "/help") == 0))
                cout << "Comands: median, gray, gauss, sobelX, sobelY, sobelAll\npatern of input: <filein.bmp> <comand> <fileout.bmp>\nExample of input: in.bmp gauss out.bmp\n";
            else
            {
                cout << "mumble command.\n try to type \"/help\"\n";
                if (argc == 1)
                {
                    void enterCommand();
                    void verify();
                }
            }
        }
    if (argc > 4)
    {
        indexX = atof(argv[4]);
    }
    if (argc > 5)
    {
        indexY = atof(argv[5]);
    }
}

void Command::enterCommand()
{
    for (int i = 0; i < argc; i++)
    {
        delete [] argv[i];
    }
    delete [] argv;

    argc = 4;
    argv = new char* [argc];
    char temp[100];
    cin >> temp;
    argv[1] = new char [strlen(temp)];
    strncpy(argv[1], temp, strlen(temp));

}
