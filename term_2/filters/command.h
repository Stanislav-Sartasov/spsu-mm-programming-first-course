#ifndef COMMAND_H
#define COMMAND_H


class Command
{
private:
    int argc;
    char **argv;
    double indexX = 0, indexY = 0;
public:
    Command(int argc, char *argv[]);
    ~Command();
    void verify();
    void enterCommand();
};

#endif // COMMAND_H
