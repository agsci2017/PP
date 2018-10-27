using System;
using System.Threading;

public class Program
{

	static Random Rand = new Random();

	static ReaderWriterLockSlim orderLock = new ReaderWriterLockSlim();

	static int MaxPackets = 3;

	static void Writer()
	{

		while(true){

			Console.WriteLine("W ready");
			orderLock.EnterWriteLock();

			Console.WriteLine("W write");
			Thread.Sleep (Rand.Next (1, 3) * 100);

			orderLock.ExitWriteLock();
			Console.WriteLine("W leave");
		}

	}

	static void Reader(Object id)
	{
		int ID = (int)id;

		while (true) {
			Console.WriteLine("R{0} ready",ID);
			orderLock.EnterReadLock();

			Console.WriteLine("R{0} read",ID);
			Thread.Sleep (Rand.Next (1, 3) * 100);

			orderLock.ExitReadLock();
			Console.WriteLine("R{0} leave",ID);
			//packetReady.ReleaseMutex ();

			//Console.WriteLine('packet ready');
		}


//		protectSlots.WaitOne();
//
//		if (freeSlots > 0) {
//
//			Console.WriteLine("packet");
//
//				freeSlots -= 1;
//
//			packetReady.Release();
//
//			processorReady.WaitOne();
//
//			protectSlots.Release();
//
//		} else {
//			
//			protectSlots.Release();
//		}
//
//		Console.WriteLine("Packet {0} leaves for the Processor shop", Number);
//		Thread.Sleep(Rand.Next(1, 5) * 100);
//
//		Console.WriteLine("Packet {0} has arrived.", Number);
//		freeSlots.WaitOne();
//
//		Console.WriteLine("Packet {0} entering waiting room", Number);
//		ProcessorChair.WaitOne();
//
//		freeSlots.Release();
//		Console.WriteLine("Processor, Packet {0} wishes to wake you up!", Number);
//
//		packetReady.Release();
//
//		chair.WaitOne();
//		ProcessorChair.Release();
//		Console.WriteLine("Packet {0} leaves the Processor shop.", Number);
	}


	public static void Main()
	{



		Thread ProcessorThread = new Thread(Writer);
		ProcessorThread.Start();

		Thread[] Packets = new Thread[MaxPackets];

		for (int i = 0; i < MaxPackets; i++)
		{
			Packets[i] = new Thread(new ParameterizedThreadStart(Reader));
			Packets[i].Start(i);
		}

		for (int i = 0; i < MaxPackets; i++)
		{
			Packets[i].Join();
		}



		ProcessorThread.Join();

	}
}
