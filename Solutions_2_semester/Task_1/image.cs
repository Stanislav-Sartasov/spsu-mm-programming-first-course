using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Task_1
{
    public class image
    {
        public const int file_not_exist = 1;
        public const int file_read_error = 2;
        public const int unexpected_error = 3;
        public const int invalid_input = 4;
        public const int image_not_created = 5;

        public static int write_error_name(int code)
        {
            switch (code)
            {
                case 0:
                    return 0;
                case 1:
                    Console.WriteLine(" > file_not_exist");
                    return 1;
                case 2:
                    Console.WriteLine(" > file_read_error");
                    return 2;
                case 3:
                    Console.WriteLine(" > unexpected_error");
                    return 3;
                case 4:
                    Console.WriteLine(" > invalid_input");
                    return 4;
                case 5:
                    Console.WriteLine(" > image_not_created");
                    return 5;
                default:
                    return -1;
            }
        }

        int file_size;
        short reserved_1;
        short reserved_2;
        int offset_bits;
        int head_size;
        int width;
        int height;
        short planes;
        short bit_count;    //3 or 4
        int compression;
        int image_size;
        int xpels_per_meter;
        int ypels_per_meter;
        int colors_used;
        int colors_important;

        int[] palette;
        byte[,,] bit_map;
        bool is_created = false;
        double[] shade_coefficient = { 0.299, 0.587, 0.114 };

        public byte get_from_file(string path)
        {
            if (!File.Exists(path))
                return file_not_exist;
            BinaryReader file = new BinaryReader(File.Open(path, FileMode.Open));
            if (!((char)file.ReadByte() == 'B' && (char)file.ReadByte() == 'M'))
            {
                file.Close();
                return file_read_error;
            }
            try
            {
                file_size = file.ReadInt32();
                reserved_1 = file.ReadInt16();
                reserved_2 = file.ReadInt16();
                offset_bits = file.ReadInt32();
                head_size = file.ReadInt32();
                width = file.ReadInt32();
                height = file.ReadInt32();
                planes = file.ReadInt16();
                bit_count = file.ReadInt16();
                compression = file.ReadInt32();
                image_size = file.ReadInt32();
                xpels_per_meter = file.ReadInt32();
                ypels_per_meter = file.ReadInt32();
                colors_used = file.ReadInt32();
                colors_important = file.ReadInt32();

                if (offset_bits - 54 > 0)
                {
                    palette = new int[(offset_bits - 54) / 4];
                    for (int i = 0; i < palette.Length; i++)
                        palette[i] = file.ReadInt32();
                }

                if (bit_count == 32)
                {
                    bit_count = 4;
                    bit_map = new byte[width, height, 4];
                }
                else if (bit_count == 24)
                {
                    bit_count = 3;
                    bit_map = new byte[width, height, 3];
                }
                else
                {
                    file.Close();
                    return file_read_error;
                }

                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        for (int c = 2; c >= 0; c--)
                            bit_map[i, j, c] = file.ReadByte();
                        if (bit_count == 4)
                            bit_map[i, j, 3] = file.ReadByte();
                    }
                    for (int i = 0; i < (4 - (width * bit_count) % 4) % 4; i++)
                        file.ReadByte();
                }
                file.Close();
                is_created = true;
                return 0;
            }
            catch
            {
                file.Close();
                return file_read_error;
            }
        }
        public byte put_in_file(string path)
        {
            if (!is_created)
                return image_not_created;
            BinaryWriter file = new BinaryWriter(File.Open(path, FileMode.Create));
            try
            {
                file.Write('B');
                file.Write('M');

                file.Write(file_size);
                file.Write(reserved_1);
                file.Write(reserved_2);
                file.Write(offset_bits);
                file.Write(head_size);
                file.Write(width);
                file.Write(height);
                file.Write(planes);
                file.Write((short)(bit_count * 8));
                file.Write(compression);
                file.Write(image_size);
                file.Write(xpels_per_meter);
                file.Write(ypels_per_meter);
                file.Write(colors_used);
                file.Write(colors_important);

                if (palette != null)
                    for (int i = 0; i < palette.Length; i++)
                        file.Write(palette[i]);

                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        for (int c = 2; c >= 0; c--)
                            file.Write(bit_map[i, j, c]);
                        if (bit_count == 4)
                            file.Write(bit_map[i, j, 3]);
                    }
                    for (int i = 0; i < (4 - (width * bit_count) % 4) % 4; i++)
                        file.Write((byte)0);
                }

                file.Close();
                return 0;
            }
            catch
            {
                return unexpected_error;
            }
        }
        public byte create(int x_width, int y_height, short count)
        {
            try
            {
                if (x_width <= 0 || y_height <= 0 || count <= 0)
                    return invalid_input;

                if (count == 32 || count == 4)
                {
                    bit_count = 32;
                    count = 4;
                }
                else if (count == 24 || count == 3)
                {
                    bit_count = 24;
                    count = 3;
                }
                else
                    return invalid_input;

                image_size = (x_width * count + ((4 - (x_width * count) % 4) % 4)) * y_height;
                file_size = 50 + image_size;
                reserved_1 = 0;
                reserved_2 = 0;
                offset_bits = 50;
                head_size = 40;
                width = x_width;
                height = y_height;
                planes = 1;
                compression = 0;
                xpels_per_meter = 0;
                ypels_per_meter = 0;
                colors_used = 0;
                colors_important = 0;
                palette = null;
                bit_map = new byte[width, height, count];
                for (int j = 0; j < height; j++)
                    for (int i = 0; i < width; i++)
                        for (int c = 3; c >= 0; c--)
                            bit_map[i, j, c] = 0;
                is_created = true;
                return 0;
            }
            catch
            {
                return unexpected_error;
            }
        }
        public byte clear_head()
        {
            try
            {
                if (!is_created)
                    return image_not_created;

                image_size = (width * bit_count + ((4 - (width * bit_count) % 4) % 4)) * height;
                file_size = 50 + image_size;
                reserved_1 = 0;
                reserved_2 = 0;
                offset_bits = 50;
                head_size = 40;
                planes = 1;
                compression = 0;
                xpels_per_meter = 0;
                ypels_per_meter = 0;
                colors_used = 0;
                colors_important = 0;
                palette = null;

                return 0;
            }
            catch
            {
                return unexpected_error;
            }
        }
        static byte[,] create_mask(int size, Func<int, int, int> func)
        {
            if (size <= 0)
                return null;
            if (size % 2 == 0)
                size++;

            byte[,] mask = new byte[size, size];
            int m = size / 2;

            for (int j = -m; j <= m; j++)
                for (int i = -m; i <= m; i++)
                    if (func(i, j) > 0)
                        mask[i + m, j + m] = 1;
                    else
                        mask[i + m, j + m] = 0;

            return mask;
        }
        public static byte[,] create_square_mask(int size)
        {
            int func(int i, int j)
            {
                return 1;
            }

            return create_mask(size, func);
        }
        public static byte[,] create_circle_mask(int size)
        {
            int func(int i, int j)
            {
                if (Math.Pow(i, 2) + Math.Pow(j, 2) <= Math.Pow(size / 2, 2))
                    return 1;
                else
                    return 0;
            }

            return create_mask(size, func);
        }
        public static byte[,] create_cross_mask(int size)
        {
            int func(int i, int j)
            {
                if (i == 0 || j == 0)
                    return 1;
                else
                    return 0;
            }

            return create_mask(size, func);
        }
        public static byte[,] create_diagonal_cross_mask(int size)
        {
            int func(int i, int j)
            {
                if (Math.Abs(i) == Math.Abs(j))
                    return 1;
                else
                    return 0;
            }

            return create_mask(size, func);
        }
        public static byte[,] create_empty_square_mask(int size)
        {
            int func(int i, int j)
            {
                if (Math.Abs(i) == size / 2 || Math.Abs(j) == size / 2)
                    return 1;
                else
                    return 0;
            }

            return create_mask(size, func);
        }
        public static byte[,] create_mask_by_code(int size, int code)
        {
            switch (code)
            {
                case 0:
                    return create_square_mask(size);
                case 1:
                    return create_circle_mask(size);
                case 2:
                    return create_cross_mask(size);
                case 3:
                    return create_diagonal_cross_mask(size);
                case 4:
                    return create_empty_square_mask(size);
                default:
                    return null;
            }
        }
        double use_mask(int x, int y, int c, double[,] mask, double max_sum)
        {
            int m_x = mask.GetLength(0) / 2;
            int m_y = mask.GetLength(1) / 2;
            double sum = 0;
            for (int j = -m_y; j <= m_y; j++)
                for (int i = -m_x; i <= m_x; i++)
                    if ((y + j) >= 0 && (y + j) < height && (x + i) >= 0 && (x + i) < width)
                        sum += bit_map[x + i, y + j, c] * mask[i + m_x, j + m_y];
                    else
                        max_sum -= mask[i + m_x, j + m_y];
            if (max_sum == 0)
                return -1;
            return sum / max_sum;
        }
        public byte median(byte[,] mask_map)
        {
            try
            {
                if (!is_created)
                    return image_not_created;
                if (mask_map.GetLength(0) % 2 == 0 || mask_map.GetLength(1) % 2 == 0)
                    return invalid_input;

                byte[] arr = new byte[mask_map.Length];
                int len;

                byte[,,] new_bit_map = new byte[width, height, bit_count];

                int m_x = mask_map.GetLength(0) / 2;
                int m_y = mask_map.GetLength(1) / 2;
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                        for (int c = 0; c <= 2; c++)
                        {
                            len = 0;
                            for (int j = -m_y; j <= m_y; j++)
                                for (int i = -m_x; i <= m_x; i++)
                                    if ((y + j) >= 0 && (y + j) < height && (x + i) >= 0 && (x + i) < width && mask_map[i + m_x, j + m_y] != 0)
                                    {
                                        arr[len] = bit_map[x + i, y + j, c];
                                        len++;
                                    }
                            if (len == 0)
                                return invalid_input;
                            Array.Sort(arr, 0, len);
                            new_bit_map[x, y, c] = arr[len / 2];
                        }
                bit_map = new_bit_map;

                return 0;
            }
            catch
            {
                return unexpected_error;
            }
        }
        public byte median(int size)
        {
            try
            {
                if (size <= 0)
                    return invalid_input;

                return median(create_square_mask(size));
            }
            catch
            {
                return unexpected_error;
            }
        }
        public byte middle(byte[,] mask_map)
        {
            try
            {
                if (!is_created)
                    return image_not_created;
                if (mask_map.GetLength(0) % 2 == 0 || mask_map.GetLength(1) % 2 == 0)
                    return invalid_input;

                int count = 0;

                double[,] mask = new double[mask_map.GetLength(0), mask_map.GetLength(1)];

                for (int j = 0; j < mask_map.GetLength(1); j++)
                    for (int i = 0; i < mask_map.GetLength(0); i++)
                        if (mask_map[i, j] != 0)
                        {
                            mask[i, j] = 1;
                            count++;
                        }
                        else
                            mask[i, j] = 0;

                byte[,,] new_bit_map = new byte[width, height, bit_count];
                double code;

                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                        for (int c = 0; c <= 2; c++)
                        {
                            code = use_mask(x, y, c, mask, count);
                            if (code >= 0)
                                new_bit_map[x, y, c] = (byte)code;
                            else
                                return invalid_input;
                        }

                bit_map = new_bit_map;

                return 0;
            }
            catch
            {
                return unexpected_error;
            }
        }
        public byte middle(int size)
        {
            try
            {
                if (size <= 0)
                    return invalid_input;

                return middle(create_square_mask(size));
            }
            catch
            {
                return unexpected_error;
            }
        }
        public byte gaussian(byte[,] mask_map, double sigma)
        {
            if (sigma <= 0)
                return invalid_input;
            try
            {
                if (!is_created)
                    return image_not_created;
                if (mask_map.GetLength(0) % 2 == 0 || mask_map.GetLength(1) % 2 == 0)
                    return invalid_input;

                int m_x = mask_map.GetLength(0);
                int m_y = mask_map.GetLength(1);
                double[,] kern = new double[m_x, m_y];
                m_x = m_x / 2;
                m_y = m_y / 2;
                double sum_kern = 0;
                for (int j = -m_y; j <= m_y; j++)
                    for (int i = -m_x; i <= m_x; i++)
                        if (mask_map[i + m_x, j + m_y] != 0)
                        {
                            kern[i + m_x, j + m_y] = Math.Exp(-(Math.Pow(i, 2.0) + Math.Pow(j, 2.0)) / (2 * Math.Pow(sigma, 2.0))) / (2 * Math.PI * Math.Pow(sigma, 2.0));
                            sum_kern += kern[i + m_x, j + m_y];
                        }
                        else
                            kern[i + m_x, j + m_y] = 0;
                if (sum_kern == 0)
                    return invalid_input;

                byte[,,] new_bit_map = new byte[width, height, bit_count];
                double code;

                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                        for (int c = 0; c <= 2; c++)
                        {
                            code = use_mask(x, y, c, kern, sum_kern);
                            if (code >= 0)
                                new_bit_map[x, y, c] = (byte)code;
                            else
                                return invalid_input;
                        }

                bit_map = new_bit_map;

                return 0;
            }
            catch
            {
                return unexpected_error;
            }
        }
        public byte gaussian(int size, double sigma)
        {
            try
            {
                if (size <= 0 || sigma <= 0)
                    return invalid_input;

                return gaussian(create_square_mask(size), sigma);
            }
            catch
            {
                return unexpected_error;
            }
        }
        public byte shade()
        {
            try
            {
                double shade;
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                    {
                        shade = 0;
                        for (int c = 0; c <= 2; c++)
                            shade += bit_map[x, y, c] * shade_coefficient[c];
                        for (int c = 0; c <= 2; c++)
                            bit_map[x, y, c] = (byte)shade;
                    }

                return 0;
            }
            catch
            {
                return unexpected_error;
            }
        }
        public byte sobel(char type, double threshold)
        {
            try
            {
                if ((type != 'x' && type != 'y' && type != 'c') || threshold <= 0)
                    return invalid_input;

                double g_x = 0;
                double g_y = 0;
                double[,] g_x_mask = new double[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
                double[,] g_y_mask = new double[,] { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };
                double shade;

                byte[,,] new_bit_map = new byte[width, height, bit_count];

                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                        if (y > 0 && y < height - 1 && x > 0 && x < width - 1)
                        {
                            shade = 0;
                            for (int c = 0; c <= 2; c++)
                            {
                                if (type != 'y')
                                    g_x = Math.Abs(use_mask(x, y, c, g_x_mask, 1));
                                if (type != 'x')
                                    g_y = Math.Abs(use_mask(x, y, c, g_y_mask, 1));

                                if (g_x < 0 || g_y < 0)
                                    return unexpected_error;

                                if (type == 'x')
                                    shade += g_x * shade_coefficient[c];
                                else if (type == 'y')
                                    shade += g_y * shade_coefficient[c];
                                else
                                    shade += Math.Sqrt(Math.Pow(g_x, 2) + Math.Pow(g_y, 2)) * shade_coefficient[c];
                            }
                            if (shade > 255 / threshold)
                                shade = 255;
                            else
                                shade = 0;
                            for (int c = 0; c <= 2; c++)
                                new_bit_map[x, y, c] = (byte)shade;
                        }

                bit_map = new_bit_map;

                return 0;
            }
            catch
            {
                return unexpected_error;
            }
        }
        public byte filter_by_code(int filter_type, int size, int mask_type, double sigma, double threshold)
        {
            try
            {
                switch (filter_type)
                {
                    case 0:
                        return median(create_mask_by_code(size, mask_type));
                    case 1:
                        return middle(create_mask_by_code(size, mask_type));
                    case 2:
                        return gaussian(create_mask_by_code(size, mask_type), sigma);
                    case 3:
                        return shade();
                    case 4:
                        return sobel('x', threshold);
                    case 5:
                        return sobel('y', threshold);
                    case 6:
                        return sobel('c', threshold);
                    default:
                        return invalid_input;
                }
            }
            catch
            {
                return unexpected_error;
            }
        }
    }
}