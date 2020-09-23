namespace BMPfilters
{
    public class SobelFilter : KernelFilter
    {
        public SobelFilter(string format)
        {
            if (format == "X")
                Matrix = new double[,]
                {
                    
                    {  1,  2,  1 },
                    {  0,  0,  0 },
                    { -1, -2, -1 }
                };
            else if (format == "Y")
                Matrix = new double[,]
                {
                    { -1,  0,  1 },
                    { -2,  0,  2 },
                    { -1,  0,  1 }
                };
        }
    }

}
