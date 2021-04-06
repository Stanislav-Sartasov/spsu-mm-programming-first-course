TEMPLATE = app
CONFIG += console c++11
CONFIG -= app_bundle
CONFIG -= qt
CONFIG = release
QMAKE_LFLAGS += -static

SOURCES += \
        command.cpp \
        image.cpp \
        main.cpp \
        startfilter.cpp

HEADERS += \
    command.h \
    image.h \
    startfilter.h
