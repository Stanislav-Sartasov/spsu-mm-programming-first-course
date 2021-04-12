using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocols;
using System.Drawing;
using System.IO;

namespace FilterLibrary
{
	public class Echo : IFilter
    {
        public IFilter Use(byte[] image)
        {
            Echo filter = new Echo();
            filter.UseFilter(BitmapGetter.GetBitmap(image));
            return filter;
        }

        object dataLock = new object();
        double progress = 0;
        byte[] result = null;
        public string Name { get; } = "Echo";
        public double Progress
        {
            get
            {
                lock (dataLock)
                    return progress;
            }
        }
        public byte[] Result
        {
            get
            {
                lock (dataLock)
                    return result;
            }
        }
        void UseFilter(Bitmap image)
        {
            progress = 100;
            result = BitmapGetter.GetArray(image);
        }
        public void Abort() { }
	}
}
