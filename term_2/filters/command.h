#ifndef COMMAND_H
#define COMMAND_H

#include <cstring>
#include <iostream>
#include <fstream>

struct cmd
{
    char fileIn[80];
    char fileOut[80];
    char mod[20];
    double indexX = 0, indexY = 0;
};

class Command
{
private:
    int argc;
    char **argv;
    double indexX = 0, indexY = 0;
    bool ready;
public:
    Command(int argc, char *argv[]);
    ~Command();
    void verify();
    void enterCommand();
    struct cmd putGommand();
    bool status()
    {
        return ready;
    }
};

#endif // COMMAND_H
