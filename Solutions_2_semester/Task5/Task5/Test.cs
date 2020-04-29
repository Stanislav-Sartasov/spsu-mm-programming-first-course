using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task5
{
    class Test
    {
        public async void VoiseFromVoid()
        {
            await Task.Run(() => { Thread.Sleep(20000); Console.WriteLine("Voise from void"); });
        }
    }
}
