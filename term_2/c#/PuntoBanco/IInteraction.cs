using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuntoBanco
{
	public interface IInteraction
	{
		int GetInt();

		bool Ready();

		//void doBet(ref SomeBet betNow, int moneyMoment);
		SomeBet DoBet(SomeBet betNow, int moneyMoment);
	}
}
