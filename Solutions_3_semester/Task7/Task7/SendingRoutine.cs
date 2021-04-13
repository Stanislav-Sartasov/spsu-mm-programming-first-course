using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Protocols;
using System.Windows;

namespace Task7
{
	public class SendingRoutine
	{
		public SendingRoutine(Action<double> ProgressBarChange, Action<Exception> MessageBoxShow, Action<MemoryStream> SetImage, Action FinishedRoutine)
		{
			this.ProgressBarChange = ProgressBarChange;
			this.MessageBoxShow = MessageBoxShow;
			this.SetImage = SetImage;
			this.FinishedRoutine = FinishedRoutine;
		}

		volatile object abortLock = new object();
		volatile bool abort = false;
		volatile bool finished = false;
		public bool Finished
		{
			get
			{
				return finished;
			}
		}
		event Action<double> ProgressBarChange;
		event Action<Exception> MessageBoxShow;
		event Action<MemoryStream> SetImage;
		event Action FinishedRoutine;
		public static SendingRoutine GetFinished()
		{
			return new SendingRoutine(null, null, null, null)
			{
				abort = true,
				finished = true
			};
		}
		public void Send(byte[] image, IPEndPoint endPoint, string filter)
		{
			if (Finished || abort)
				return;

			Task.Run(() =>
			{

				TcpClient client = null;
				try
				{
					client = new TcpClient();
					client.Connect(endPoint);

					new Message(SendingProtocols.BitMapSending, filter).SendToStream(client, 10000);
					new Message(image).SendToStream(client, 10000);

					while (true)
					{
						Message reply = Message.GetFromStream(client, 10000);

						lock (abortLock)
							if (abort)
							{
								client.Close();
								finished = true;
								return;
							}

						if (reply.Flag < 100)
							ProgressBarChange(reply.Flag);
						else
						{
							new Message(SendingProtocols.Check).SendToStream(client, 10000);
							SetImage(new MemoryStream(reply.ContentByteArray));
							FinishedRoutine();
							client.Close();
							finished = true;
							return;
						}
					}

				}
				catch (Exception ex)
				{
					lock (abortLock)
						if (!abort)
						{
							MessageBoxShow(ex);
							FinishedRoutine();
						}
					try
					{
						client.Close();
					}
					catch { }
					finished = true;
				}
			});
		}
		public void Abort()
		{
			lock (abortLock)
				abort = true;
			finished = true;
		}

		~SendingRoutine()
		{
			Abort();
		}
	}
}
