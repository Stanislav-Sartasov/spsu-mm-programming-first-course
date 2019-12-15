#include <stdio.h>
#include <string.h>
#include "Header.h"

/*
	To use this program in Visual Studio, right click at the project(not the solution) and click Properties.
	A popup box should appear, choose Configuration Properties Debugging. In the Command Arguments section, fill in this line:
	"input.bmp" "#filterName" "#outputName"
	in which #filterName could be Average3x3, Gauss3x3, SobelX, SobelY or Grey
	and #outputName is the output file's name, it should contains ".bmp". For example:
	"input.bmp" "Average3x3" "Average3x3.bmp"
	(see https://imgur.com/a/IhcDufk), please DO NOT delete the file "input.bmp".
*/


// The main program starts here.
int main(unsigned int argc, char* argv[])
{
	if (argc != 4) {
		printf("The data you entered is incorrect.\nTry it again.\n");
		return(4);
	}

	FILE* fileInput;
	FILE* fileOutput;
	char* filterName;
	unsigned int flag = 0;

	// Open the files
	fopen_s(&fileInput, argv[1], "rb");
	fopen_s(&fileOutput, argv[3], "wb");
	filterName = argv[2];

	if (!fileInput) {
		printf("Cannot open the input file, please try again.\n");
		_fcloseall();
		return(3);
	}
	else {
		printf("Successfully opened the input file.\n");
	}

	if (!fileOutput) {
		printf("Cannot open the ouput file, please try again.\n");
		_fcloseall();
		return(-3);
	}
	else {
		printf("Successfully opened the ouput file.\n");
	}


	// Apply the filter
	if (!strcmp(filterName, "Average3x3")) {
		flag = useFilter(fileInput, 1, fileOutput);
	}
	else if (!strcmp(filterName, "Gauss3x3")) {
		flag = useFilter(fileInput, 2, fileOutput);
	}
	else if (!strcmp(filterName, "SobelX")) {
		flag = useFilter(fileInput, 3, fileOutput);
	}
	else if (!strcmp(filterName, "SobelY"))	{
		flag = useFilter(fileInput, 4, fileOutput);
	}
	else if (!strcmp(filterName, "Grey")) {
		flag = useFilter(fileInput, 5, fileOutput);
	}
	else {
		printf("Failed detecting filter, please check the filter name.\n");
		_fcloseall();
		return(2);
	}


	if (!flag) {
		printf("Failed to apply filter, please check the input and try again.\n");
		_fcloseall();
		return(-2);
	}
	else {
		printf("Successfully applied the filter, please check the output file %s.\n", argv[3]);
	}

	return(0);
}