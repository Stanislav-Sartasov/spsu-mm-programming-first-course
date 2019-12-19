TEMPLATE = app
CONFIG += console
CONFIG -= app_bundle
CONFIG -= qt

SOURCES += \
        main.c \
    ../mylib/functionToGo.c \
    hash-table.c

HEADERS += \
    ../mylib/functionToGo.h \
    hash-table.h
