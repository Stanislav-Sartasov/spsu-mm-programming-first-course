#ifndef STARTFILTER_H
#define STARTFILTER_H


class StartFilter
{
private:
    int argc;
    char **argv;

public:
    StartFilter(int argc, char* argv[]);
    ~StartFilter();

    void goFilter();
};

#endif // STARTFILTER_H
