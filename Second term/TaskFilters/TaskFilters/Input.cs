using System;

namespace TaskFilters
{
    public class Input
    {
        public static bool CorrectInput(string[] args)
        {
            if ((args.Length != 3) || (String.Compare(args[1], "Averaging") != 0) && (String.Compare(args[1], "Gauss3") != 0)
                && (String.Compare(args[1], "Gauss5") != 0) && (String.Compare(args[1], "SobelX") != 0) &&
                (String.Compare(args[1], "SobelY") != 0) && (String.Compare(args[1], "Grayscale") != 0))
            {
                Console.WriteLine("Использование: Program.exe [-input] [-name] [-output]");
                Console.WriteLine("Параметры:\n-input\t\t Получает картинку формата .bmp с указанным названием\n" +
                    "-name\t\t Название используемого фильтра (одного из Averaging, Gauss3, Gauss5, SobelX, SobelY, Grayscale)\n" +
                    "-output\t\t Создает картинку формата .bmp с указанным названием");
                return false;
            }
            return true;
        }
    }
}
