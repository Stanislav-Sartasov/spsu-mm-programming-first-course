using Protocols;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Task7;
using Task7Server;

namespace Task7Test
{
	class SenderHandler
	{
		volatile bool finished = false;
		public bool Finished
		{
			get
			{
				return finished;
			}
		}
		public long ElapsedMilliseconds { get; private set; } = -1;
		public void Work(byte[] image, string filter, IPEndPoint server)
		{
			Task.Run(() =>
			{
				int error = 0;

				Stopwatch stopwatch = new Stopwatch();

				stopwatch.Start();

				var sending = new SendingRoutine(
						x => { return; },
						(x) => Interlocked.Increment(ref error),
						x => { return; },
						() => { return; });

				sending.Send(image, server, filter);

				while (!sending.Finished)
				{
					if (error != 0)
					{
						sending.Abort();
						stopwatch.Stop();
						finished = true;
						return;
					}
					Thread.Sleep(100);
				}

				stopwatch.Stop();

				if (error != 0)
				{
					sending.Abort();
					stopwatch.Stop();
					finished = true;
					return;
				}

				ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
				finished = true;
			});
		}
	}
}
