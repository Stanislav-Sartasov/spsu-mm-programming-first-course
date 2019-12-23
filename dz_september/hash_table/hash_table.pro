TEMPLATE = app
CONFIG += console
CONFIG -= app_bundle
CONFIG -= qt

SOURCES += \
        main.c \
    ../mylib/functionToGo.c \
    hash_table.c

HEADERS += \
    ../mylib/functionToGo.h \
    hash_table.h
