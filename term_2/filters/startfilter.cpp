#include "command.h"
#include "startfilter.h"
#include <cstring>

StartFilter::StartFilter(int argc, char* argv[]) : a(argc, argv)
{

}

StartFilter::StartFilter(): a(0, 0)
{

}

StartFilter::~StartFilter()
{
    a.~Command();
}

void StartFilter::getCommand(int argc, char *argv[])
{
    a.~Command();
    a = Command(argc, argv);
}

void StartFilter::goFilter()
{

}
