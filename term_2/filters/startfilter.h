#ifndef STARTFILTER_H
#define STARTFILTER_H

#include "command.h"
#include <cstring>
#include "image.h"

class StartFilter
{
private:
    Command a;

public:
    StartFilter(int argc, char* argv[]);
    StartFilter();
    ~StartFilter();
    void getCommand(int argc, char* argv[]);
    void goFilter();
};

#endif // STARTFILTER_H
