using System;

namespace Task1
{
    public class Filters
    {
        public static int Main(string[] args)
        {
            const int nMask = 0;
            const int nSize = 5;
            const double nSigma = 0.6;
            const double nThreshold = 2;
            int errCode = 0;
            bool auto = false;

            int InvalidInput()
            {
                Console.WriteLine(" > invalid input");
                if (auto)
                    return -1;
                return Image.InvalidInput;
            }
            
            for (; ; )
            {
                if (args.Length == 0 && auto == false)
                {
                    Console.WriteLine("\n\tthis program allows you to use certain filters for bmp-24 and bmp-32 images");
                    Console.WriteLine("\tinput format: <input file name> <filter with modificators> <output file name>");
                    Console.WriteLine("\tenter <help> for details");
                    Console.WriteLine("\tenter <exit> for finish\n");
                    auto = true;
                }

                errCode = 0;
                if (auto)
                {
                    while (Console.KeyAvailable)
                        Console.ReadKey(true);

                    Console.Write(" > ");
                    string str = Console.ReadLine().Trim();

                    int a = -1;
                    for (int i = 0; i < str.Length; i++)
                        if (str[i] == '"')
                            a *= -1;
                        else if (a < 0 && str[i] == ' ')
                            str = str.Substring(0, i) + "\"" + str.Substring(i + 1);

                    if (a > 0)
                    {
                        if ((errCode = InvalidInput()) > 0)
                            return errCode;
                    }
                    else
                        args = str.Split("\"", StringSplitOptions.RemoveEmptyEntries);
                }

                if (errCode < 0)
                    continue;

                if (args.Length < 3)
                {
                    if (args.Length == 1)
                    {
                        if (args[0] == "help")
                        {
                            Console.WriteLine("\n\tfilters supported:");
                            Console.WriteLine("\n\t<median>");
                            Console.WriteLine("\t\t/sz - matrix size");
                            Console.WriteLine("\t\t/m - matrix type");
                            Console.WriteLine("\n\t<middle>");
                            Console.WriteLine("\t\t/sz - matrix size");
                            Console.WriteLine("\t\t/m - matrix type");
                            Console.WriteLine("\n\t<gaussian>");
                            Console.WriteLine("\t\t/sz - matrix size");
                            Console.WriteLine("\t\t/sg - sigma");
                            Console.WriteLine("\t\t/m - matrix type");
                            Console.WriteLine("\n\t<sobel> <Sobel_x> <Sobel_y>");
                            Console.WriteLine("\t\t/th - threshold, pixels in shades of gray from (255 / th) is white, the rest are black");
                            Console.WriteLine("\n\t<shade>");
                            Console.WriteLine("\n\t</m> types:");
                            Console.WriteLine("\t\tsquare");
                            Console.WriteLine("\t\tcircle");
                            Console.WriteLine("\t\tcross");
                            Console.WriteLine("\t\tdiagonal_cross");
                            Console.WriteLine("\t\tempty_square");
                            Console.WriteLine("\n\tyou can use modificators like <gaussian /sz = 5>");
                            Console.WriteLine("\n\twithout modifiers standard values will be taken:");
                            Console.WriteLine("\n\t\tsz = " + nSize + "\n\t\tsg = " + nSigma + "\n\t\tm = square\n\t\tth = " + nThreshold);
                            Console.WriteLine();
                            if (auto == false)
                                return 0;
                            else
                                continue;
                        }
                        else if (args[0] == "exit")
                            return 0;
                        else
                            if ((errCode = InvalidInput()) > 0)
                                return errCode;
                    }
                    else
                        if ((errCode = InvalidInput()) > 0)
                            return errCode;
                }

                if (errCode < 0)
                    continue;

                int filter = -1;
                int SetFilter(int type)
                {
                    if (filter >= 0)
                        return InvalidInput();
                    filter = type;
                    return 0;
                }

                int mask = -1;
                int SetMask(int type)
                {
                    if (mask >= 0)
                        return InvalidInput();
                    mask = type;
                    return 0;
                }
                int size = -1;
                double sigma = -1;
                double threshold = -1;

                try
                {
                    for (int i = 1; i < args.Length - 1; i++)
                    {
                        switch (args[i])
                        {
                            case "median":
                                errCode = SetFilter(0);
                                break;
                            case "middle":
                                errCode = SetFilter(1);
                                break;
                            case "gaussian":
                                errCode = SetFilter(2);
                                break;
                            case "shade":
                                errCode = SetFilter(3);
                                break;
                            case "sobel_x":
                                errCode = SetFilter(4);
                                break;
                            case "sobel_y":
                                errCode = SetFilter(5);
                                break;
                            case "sobel":
                                errCode = SetFilter(6);
                                break;
                            case "/m":
                                if (++i >= args.Length - 1)
                                {
                                    errCode = InvalidInput();
                                    break;
                                }
                                if (args[i] == "=")
                                    if (++i >= args.Length - 1)
                                    {
                                        errCode = InvalidInput();
                                        break;
                                    }
                                switch (args[i])
                                {
                                    case "square":
                                        errCode = SetMask(0);
                                        break;
                                    case "circle":
                                        errCode = SetMask(1);
                                        break;
                                    case "cross":
                                        errCode = SetMask(2);
                                        break;
                                    case "diagonal_cross":
                                        errCode = SetMask(3);
                                        break;
                                    case "empty_square":
                                        errCode = SetMask(4);
                                        break;
                                    default:
                                        errCode = InvalidInput();
                                        break;
                                }
                                break;
                            case "/sz":
                                if (++i >= args.Length - 1)
                                {
                                    errCode = InvalidInput();
                                    break;
                                }
                                if (args[i] == "=")
                                    if (++i >= args.Length - 1)
                                    {
                                        errCode = InvalidInput();
                                        break;
                                    }
                                if (size >= 0 || (size = int.Parse(args[i])) <= 0)
                                    errCode = InvalidInput();
                                break;
                            case "/sg":
                                if (filter != 2 || ++i >= args.Length - 1)
                                {
                                    errCode = InvalidInput();
                                    break;
                                }
                                if (args[i] == "=")
                                    if (++i >= args.Length - 1)
                                    {
                                        errCode = InvalidInput();
                                        break;
                                    }
                                if (sigma >= 0 || (sigma = double.Parse(args[i])) <= 0)
                                    errCode = InvalidInput();
                                break;
                            case "/th":
                                if (filter < 4 || ++i >= args.Length - 1)
                                {
                                    errCode = InvalidInput();
                                    break;
                                }
                                if (args[i] == "=")
                                    if (++i >= args.Length - 1)
                                    {
                                        errCode = InvalidInput();
                                        break;
                                    }
                                if (threshold >= 0 || (threshold = double.Parse(args[i])) <= 0)
                                    errCode = InvalidInput();
                                break;
                            default:
                                errCode = InvalidInput();
                                break;
                        }
                        if (errCode != 0)
                            break;
                    }
                    if (errCode < 0)
                        continue;
                    if (errCode > 0)
                        return errCode;
                }
                catch
                {
                    if ((errCode = InvalidInput()) > 0)
                        return errCode;
                    else
                        continue;
                }

                if (filter == -1)
                    if ((errCode = InvalidInput()) > 0)
                        return errCode;
                    else
                        continue;

                Image paint = new Image();

                if ((errCode = Image.WriteErrorName(paint.GetFromFile(args[0]))) != 0)
                    if (auto)
                        continue;
                    else
                        return errCode;

                if (mask == -1)
                    mask = nMask;
                if (size == -1)
                    size = nSize;
                if (sigma == -1)
                    sigma = nSigma;
                if (threshold == -1)
                    threshold = nThreshold;

                if ((errCode = Image.WriteErrorName(paint.FilterByCode(filter, size, mask, sigma, threshold))) != 0)
                    if (auto)
                        continue;
                    else
                        return errCode;

                if ((errCode = Image.WriteErrorName(paint.PutInFile(args[args.Length - 1]))) != 0)
                    if (auto)
                        continue;
                    else
                        return errCode;

                if (!auto)
                    return 0;
            }
        }
    }
}