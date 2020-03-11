#include "command.h"

using namespace std;

Command::Command(int argc, char *argv[])
{
    this->argc = argc;
    this->argv = new char* [argc];
    for (int i = 0; i < argc; ++i)
    {
        this->argv[i] = new char [strlen(argv[i]) + 1];
        strncpy(this->argv[i], argv[i], strlen(argv[i]));
        this->argv[i][strlen(argv[i])] = '\0';
        //this->argv[i] = new char [strlen(argv[i])];
        //strcpy(this->argv[i], argv[i]);
    }

}

Command::~Command()
{
    int first = 0;
    if (argv[0] == nullptr)
        first = 1;

    for (int i = first; i < argc; ++i)
    {
        delete [] this->argv[i];
    }
}

void Command::verify()
{

    bool good = 1;
    this->ready = 0;
    if (argc < 4 ||
            !(strcmp(argv[2], "median") == 0 || strcmp(argv[2], "gauss") == 0 || strcmp(argv[2], "sobelX") == 0
            || strcmp(argv[2], "sobelY") == 0 || strcmp(argv[2], "bwInvert") == 0 || strcmp(argv[2], "sobelAll") == 0
            || strcmp(argv[2], "greenGray") == 0 || strcmp(argv[2], "blueGray") == 0 || strcmp(argv[2], "redGray") == 0 || strcmp(argv[2], "gray") == 0))
        {
            good = 0;
            if ((argc == 2) && (strcmp(argv[1], "/help") == 0))
                cout << "Comands: median, gray, gauss, sobelX, sobelY, sobelAll\npatern of input: <filein.bmp> <comand> <fileout.bmp>\nExample of input: in.bmp gauss out.bmp\n";
            else
            {
                if ((argc == 0) || (argc == 1))
                {
                    cout << "Comands: median, gray, gauss, sobelX, sobelY, sobelAll\npatern of input: <filein.bmp> <comand> <fileout.bmp>\nExample of input: in.bmp gauss out.bmp\n";
                    enterCommand();
                    verify();//my be infinite cycle
                }
                else
                {
                    cout << " Mumble command.\n Try to type \"/help\"\n" << argc << "\n";
                }
            }
            return;
        }
    if (argc > 4)
    {
        indexX = atof(argv[4]);
    }
    if (argc > 5)
    {
        indexY = atof(argv[5]);
    }
    if (good)
    {
        ofstream out(argv[3]);
        ifstream in(argv[1]);
        if (!out.is_open())
        {
            cout << "error to open OUTPUT FILE\n";
            good = 0;
        }
        if (!in.is_open())
        {
            cout << "error to open INPUT FILE\n";
            good = 0;
        }
        in.close();
        out.close();
    }
    if (!this->ready)
    {
        this->ready = good;
    }
}

void Command::enterCommand()
{
    int first = 0;
    if (argv[0] == nullptr)
        first = 1;
    for (int i = first; i < argc; i++)
    {
        delete [] argv[i];
    }

    argc = 6;
    argv = new char* [argc];
    argv[0] = nullptr;
    char temp[100];

    cout << "enter input file: ";
    cin >> temp;
    argv[1] = new char [strlen(temp) + 1];
    strncpy(argv[1], temp, strlen(temp));
    argv[1][strlen(temp)] = '\0';

    cout << "enter command: ";
    cin >> temp;
    argv[2] = new char [strlen(temp) + 1];
    strncpy(argv[2], temp, strlen(temp));
    argv[2][strlen(temp)] = '\0';

    cout << "enter output file: ";
    cin >> temp;
    argv[3] = new char [strlen(temp) + 1];
    strncpy(argv[3], temp, strlen(temp));
    argv[3][strlen(temp)] = '\0';

    cout << "indexX: ";
    cin >> temp;
    argv[4] = new char [strlen(temp) + 1];
    strncpy(argv[4], temp, strlen(temp));
    argv[4][strlen(temp)] = '\0';

    cout << "indexY: ";
    cin >> temp;
    argv[5] = new char [strlen(temp) + 1];
    strncpy(argv[5], temp, strlen(temp));
    argv[5][strlen(temp)] = '\0';
}

cmd Command::putGommand()
{
    struct cmd temp;
    temp.indexX = indexX;
    temp.indexY = indexY;
    strcpy(temp.fileIn, argv[1]);
    strcpy(temp.fileOut, argv[3]);
    strcpy(temp.mod, argv[2]);
    return temp;
}
