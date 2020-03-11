QT += testlib
QT -= gui

CONFIG += qt console warn_on depend_includepath testcase
CONFIG -= app_bundle

TEMPLATE = app

SOURCES +=  tst_testdirectfilters.cpp \
    ../image.cpp

HEADERS += \
    ../image.h

DISTFILES += \
    test/ga.bmp \
    test/gauss.bmp \
    test/gray.bmp \
    test/median.bmp \
    test/sobelAll.bmp \
    test/sobelX.bmp \
    test/sobelY.bmp
