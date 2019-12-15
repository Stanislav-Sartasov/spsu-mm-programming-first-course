TEMPLATE = app
CONFIG += console
CONFIG -= app_bundle
CONFIG -= qt

SOURCES += \
        main.c \
    sys/mman.c

HEADERS += \
    sys/mman.h
