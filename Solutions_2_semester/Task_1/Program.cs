using System;

namespace Task_1
{
    public class Program
    {
        public static int Main(string[] args)
        {
            const int n_mask = 0;
            const int n_size = 5;
            const double n_sigma = 0.6;
            const double n_threshold = 2;
            int err_code = 0;
            bool auto;

            int invalid_input()
            {
                Console.WriteLine(" > invalid input");
                if (auto)
                    return -1;
                return 4;
            }

            if (args.Length == 0)
                auto = true;
            else
                auto = false;
            for (;;)
            {
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
                            Console.WriteLine("help here");
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

                image paint = new image();

                if ((err_code = image.write_error_name(paint.get_from_file(args[0]))) != 0)
                    if (auto)
                        continue;                
                    else
                        return err_code;

                try
                {
                    for (int i = 1; i < args.Length - 1; i++)
                    {
                        switch (args[i])
                        {
                            case "median":
                                if ((err_code = set_filter(0)) > 0)
                                    return err_code;
                                break;
                            case "middle":
                                if ((err_code = set_filter(1)) > 0)
                                    return err_code;
                                break;
                            case "gaussian":
                                if ((err_code = set_filter(2)) > 0)
                                    return err_code;
                                break;
                            case "shade":
                                if ((err_code = set_filter(3)) > 0)
                                    return err_code;
                                break;
                            case "sobel_x":
                                if ((err_code = set_filter(4)) > 0)
                                    return err_code;
                                break;
                            case "sobel_y":
                                if ((err_code = set_filter(5)) > 0)
                                    return err_code;
                                break;
                            case "sobel":
                                if ((err_code = set_filter(6)) > 0)
                                    return err_code;
                                break;
                            case "/m":
                                if (++i >= args.Length - 1)
                                    if ((err_code = invalid_input()) > 0)
                                        return err_code;
                                    else
                                        break;
                                if (args[i] == "=")
                                    if (++i >= args.Length - 1)
                                        if ((err_code = invalid_input()) > 0)
                                            return err_code;
                                        else
                                            break;
                                switch (args[i])
                                {
                                    case "square":
                                        if ((err_code = set_mask(0)) > 0)
                                            return err_code;
                                        break;
                                    case "circle":
                                        if ((err_code = set_mask(1)) > 0)
                                            return err_code;
                                        break;
                                    case "cross":
                                        if ((err_code = set_mask(2)) > 0)
                                            return err_code;
                                        break;
                                    case "diagonal_cross":
                                        if ((err_code = set_mask(3)) > 0)
                                            return err_code;
                                        break;
                                    case "empty_square":
                                        if ((err_code = set_mask(4)) > 0)
                                            return err_code;
                                        break;
                                    default:
                                        if ((err_code = invalid_input()) > 0)
                                            return err_code;
                                        break;
                                }
                                break;
                            case "/sz":
                                if (++i >= args.Length - 1)
                                    if ((err_code = invalid_input()) > 0)
                                        return err_code;
                                    else
                                        break;
                                if (args[i] == "=")
                                    if (++i >= args.Length - 1)
                                        if ((err_code = invalid_input()) > 0)
                                            return err_code;
                                        else
                                            break;
                                if (size >= 0 || (size = int.Parse(args[i])) <= 0)
                                    if ((err_code = invalid_input()) > 0)
                                        return err_code;
                                    else
                                        break;
                                break;
                            case "/sg":
                                if (filter != 2 || ++i >= args.Length - 1)
                                    if ((err_code = invalid_input()) > 0)
                                        return err_code;
                                    else
                                        break;
                                if (args[i] == "=")
                                    if (++i >= args.Length - 1)
                                        if ((err_code = invalid_input()) > 0)
                                            return err_code;
                                        else
                                            break;
                                if (sigma >= 0 || (sigma = double.Parse(args[i])) <= 0)
                                    if ((err_code = invalid_input()) > 0)
                                        return err_code;
                                break;
                            case "/th":
                                if (filter < 4 || ++i >= args.Length - 1)
                                    if ((err_code = invalid_input()) > 0)
                                        return err_code;
                                    else
                                        break;
                                if (args[i] == "=")
                                    if (++i >= args.Length - 1)
                                        if ((err_code = invalid_input()) > 0)
                                            return err_code;
                                        else
                                            break;
                                if (sigma >= 0 || (threshold = double.Parse(args[i])) <= 0)
                                    if ((err_code = invalid_input()) > 0)
                                        return err_code;
                                break;
                        }
                        if (err_code < 0)
                            break;
                    }
                    if (err_code < 0)
                        continue;
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

                switch (filter)
                {
                    case 0:
                    case 1:
                    case 2:
                        if (mask == -1)
                            mask = n_mask;
                        if (size == -1)
                            size = n_size;
                        if (filter == 2 && sigma == -1)
                            sigma = n_sigma;
                        break;
                    case 3:
                        break;
                    default:
                        if (threshold == -1)
                            threshold = n_threshold;
                        break;
                }

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

                paint = null;

                if (!auto)
                    return 0;
            }
        }
    }
}