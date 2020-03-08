#include <iostream>
#include "startfilter.h"

using namespace std;



int main(int argc, char* argv[])
{
    StartFilter f(argc, argv);
    f.goFilter();
    return 0;
}
