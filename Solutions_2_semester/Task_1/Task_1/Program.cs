using System;

namespace Task_1
{
    public class filters
    {
        public static int Main(string[] args)
        {
            const int n_mask = 0;
            const int n_size = 5;
            const double n_sigma = 0.6;
            const double n_threshold = 2;
            int err_code = 0;
            bool auto = false;

            int invalid_input()
            {
                Console.WriteLine(" > invalid input");
                if (auto)
                    return -1;
                return image.invalid_input;
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

                err_code = 0;
                if (auto)
                {
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
                        if ((err_code = invalid_input()) > 0)
                            return err_code;
                    }
                    else
                        args = str.Split("\"", StringSplitOptions.RemoveEmptyEntries);
                }

                if (err_code < 0)
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
                            Console.WriteLine("\n\t<sobel> <sobel_x> <sobel_y>");
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
                            Console.WriteLine("\n\t\tsz = " + n_size + "\n\t\tsg = " + n_sigma + "\n\t\tm = square\n\t\tth = " + n_threshold);
                            Console.WriteLine();
                            continue;
                        }
                        else if (args[0] == "exit")
                            return 0;
                        else
                            if ((err_code = invalid_input()) > 0)
                                return err_code;
                    }
                    else
                        if ((err_code = invalid_input()) > 0)
                            return err_code;
                }

                if (err_code < 0)
                    continue;

                int filter = -1;
                int set_filter(int type)
                {
                    if (filter >= 0)
                        return invalid_input();
                    filter = type;
                    return 0;
                }

                int mask = -1;
                int set_mask(int type)
                {
                    if (mask >= 0)
                        return invalid_input();
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
                                err_code = set_filter(0);
                                break;
                            case "middle":
                                err_code = set_filter(1);
                                break;
                            case "gaussian":
                                err_code = set_filter(2);
                                break;
                            case "shade":
                                err_code = set_filter(3);
                                break;
                            case "sobel_x":
                                err_code = set_filter(4);
                                break;
                            case "sobel_y":
                                err_code = set_filter(5);
                                break;
                            case "sobel":
                                err_code = set_filter(6);
                                break;
                            case "/m":
                                if (++i >= args.Length - 1)
                                {
                                    err_code = invalid_input();
                                    break;
                                }
                                if (args[i] == "=")
                                    if (++i >= args.Length - 1)
                                    {
                                        err_code = invalid_input();
                                        break;
                                    }
                                switch (args[i])
                                {
                                    case "square":
                                        err_code = set_mask(0);
                                        break;
                                    case "circle":
                                        err_code = set_mask(1);
                                        break;
                                    case "cross":
                                        err_code = set_mask(2);
                                        break;
                                    case "diagonal_cross":
                                        err_code = set_mask(3);
                                        break;
                                    case "empty_square":
                                        err_code = set_mask(4);
                                        break;
                                    default:
                                        err_code = invalid_input();
                                        break;
                                }
                                break;
                            case "/sz":
                                if (++i >= args.Length - 1)
                                {
                                    err_code = invalid_input();
                                    break;
                                }
                                if (args[i] == "=")
                                    if (++i >= args.Length - 1)
                                    {
                                        err_code = invalid_input();
                                        break;
                                    }
                                if (size >= 0 || (size = int.Parse(args[i])) <= 0)
                                    err_code = invalid_input();
                                break;
                            case "/sg":
                                if (filter != 2 || ++i >= args.Length - 1)
                                {
                                    err_code = invalid_input();
                                    break;
                                }
                                if (args[i] == "=")
                                    if (++i >= args.Length - 1)
                                    {
                                        err_code = invalid_input();
                                        break;
                                    }
                                if (sigma >= 0 || (sigma = double.Parse(args[i])) <= 0)
                                    err_code = invalid_input();
                                break;
                            case "/th":
                                if (filter < 4 || ++i >= args.Length - 1)
                                {
                                    err_code = invalid_input();
                                    break;
                                }
                                if (args[i] == "=")
                                    if (++i >= args.Length - 1)
                                    {
                                        err_code = invalid_input();
                                        break;
                                    }
                                if (threshold >= 0 || (threshold = double.Parse(args[i])) <= 0)
                                    err_code = invalid_input();
                                break;
                            default:
                                err_code = invalid_input();
                                break;
                        }
                        if (err_code != 0)
                            break;
                    }
                    if (err_code < 0)
                        continue;
                    if (err_code > 0)
                        return err_code;
                }
                catch
                {
                    if ((err_code = invalid_input()) > 0)
                        return err_code;
                    else
                        continue;
                }

                if (filter == -1)
                    if ((err_code = invalid_input()) > 0)
                        return err_code;
                    else
                        continue;

                image paint = new image();

                if ((err_code = image.write_error_name(paint.get_from_file(args[0]))) != 0)
                    if (auto)
                        continue;
                    else
                        return err_code;

                if (mask == -1)
                    mask = n_mask;
                if (size == -1)
                    size = n_size;
                if (sigma == -1)
                    sigma = n_sigma;
                if (threshold == -1)
                    threshold = n_threshold;

                if ((err_code = image.write_error_name(paint.filter_by_code(filter, size, mask, sigma, threshold))) != 0)
                    if (auto)
                        continue;
                    else
                        return err_code;

                if ((err_code = image.write_error_name(paint.put_in_file(args[args.Length - 1]))) != 0)
                    if (auto)
                        continue;
                    else
                        return err_code;

                if (!auto)
                    return 0;
            }
        }
    }
}