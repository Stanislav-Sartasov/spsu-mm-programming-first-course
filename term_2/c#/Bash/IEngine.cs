using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bash
{
    public interface IEngine
    {
        void InitInput(List<Message> start);
        bool StartCommand();
    }
}
