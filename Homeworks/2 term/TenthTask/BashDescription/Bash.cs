using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenthTask.BashDescription
{
	class Bash
	{
		public void Start()
		{
			var parser = new Parser();
			while (true)
			{
				var str = Console.ReadLine();
				parser.Parse(str);
			}
		}
	}
}

//на русском пишу для себя
//дописать системный вызов, разделить команды и парсер на классы
//покрасивее оформить строки (через автосвойства)
//абстрактный класс?