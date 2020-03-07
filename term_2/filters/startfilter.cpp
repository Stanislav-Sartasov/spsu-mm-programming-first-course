#include "startfilter.h"
#include <cstring>

StartFilter::StartFilter(int argc, char* argv[])
{
    this->argc = argc;
    this->argv = new char* [argc];
    for (int i = 0; i < argc; ++i)
    {
        this->argv[i] = new char [strlen(argv[i])];
        strcpy(this->argv[i], argv[i]);
    }
}

StartFilter::~StartFilter()
{
    for (int i = 0; i < argc; i++)
    {
        delete [] argv[i];
    }
    delete [] argv;
}

void StartFilter::goFilter()
{

}
