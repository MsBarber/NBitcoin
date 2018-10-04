using NBitcoin;
using NBitcoin.Protocol;
using System;
using System.Threading;

namespace CFTester
{
	class Program
	{
		private static Node node;

		static void Main(string[] args)
		{

			Console.WriteLine("Hello World!");
			ConnectNode();
			Console.WriteLine("Tap to send some Cfilter stuff");
			Console.ReadLine();
			node.SendMessage(new GetCompactFilterHeadersPayload(FilterType.Basic, 299000u, new uint256("000000000000000082ccf8f1557c5d40b21edabb18d2d691cfbf87118bac7254")), default(CancellationToken));
			node.SendMessage(new GetCompactFilterCheckPointPayload(FilterType.Basic, new uint256("000000000000000082ccf8f1557c5d40b21edabb18d2d691cfbf87118bac7254")), default(CancellationToken));
			node.SendMessage(new GetCompactFiltersPayload(FilterType.Basic, 299900u, new uint256("000000000000000082ccf8f1557c5d40b21edabb18d2d691cfbf87118bac7254")), default(CancellationToken));
		}


		private static void ConnectNode()
		{
			node = Node.Connect(Network.Main, "138.197.200.132", null, true, default(CancellationToken));

			node.Disconnected += Node_Disconnected;
			node.MessageReceived += Node_MessageReceived;
			node.VersionHandshake(default(CancellationToken));
		}

		private static void Node_MessageReceived(Node node, IncomingMessage message)
		{
			Console.WriteLine(message.Message.Command);
			bool flag = message.Message.Command == "cfilter";
			if (flag)
			{
				CompactFilterPayload x = message.Message.Payload as CompactFilterPayload;
				Console.WriteLine(x.ToString());
			}
			else
			{
				bool flag2 = message.Message.Command == "cfheaders";
				if (flag2)
				{
					CompactFilterHeadersPayload x2 = message.Message.Payload as CompactFilterHeadersPayload;
					Console.WriteLine(x2.ToString());
				}
				else
				{
					bool flag3 = message.Message.Command == "cfcheckpt";
					if (flag3)
					{
						CompactFilterCheckPointPayload x3 = message.Message.Payload as CompactFilterCheckPointPayload;
						Console.WriteLine(x3.ToString());
					}
				}
			}
		}

		private static void Node_Disconnected(Node node)
		{
			Console.WriteLine(node.DisconnectReason.Reason);
		}
	}
}
