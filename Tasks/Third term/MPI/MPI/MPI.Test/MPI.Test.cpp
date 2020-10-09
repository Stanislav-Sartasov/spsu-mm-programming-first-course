#include "pch.h"
#include "CppUnitTest.h"
#include "../MPI/ParallelSort.h"
#include "../MPI/ParallelSort.cpp"
#include <fstream>

using namespace std;
using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace MPITest
{
	TEST_CLASS(MPITest)
	{
	public:
		
		TEST_METHOD(TestSort)
		{
			int* array = new int[10]{ 0, 9, 3, 5, 7, 1, 4, 6, 2, 8 };
			ofstream file_creator("data.in");
			for (int i = 0; i < 9; i++)
				file_creator << array[i] << " ";
			file_creator << array[9];
			file_creator.close();
			int* expected = new int[10]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			int count;
			int* actual = parallel_sort("data.in", count);
			
			bool isEqual = true;
			if (count != 10)
				isEqual = false;
			int i = 0;
			while (isEqual && i < 10)
			{
				if (expected[i] != actual[i])
					isEqual = false;
				i++;
			}
			remove("data.in");
			Assert::IsTrue(isEqual);
		}
	};
}
