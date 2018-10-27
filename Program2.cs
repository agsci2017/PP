using System;
using System.Threading;

public class Program
{

	static Random Rand = new Random();

	static int MaxPackets = 3;

	static Semaphore mutex = new Semaphore (1, 1);
	static Semaphore customers = new Semaphore (0, MaxPackets);
	static Semaphore barber = new Semaphore (1, 1);


	static int counter=0;

	static void Barber()
	{

		while(true){

			Console.WriteLine("B wait for customers");
			customers.WaitOne ();

			mutex.WaitOne ();
				Console.WriteLine("B customer arrived");
				Thread.Sleep (Rand.Next (1, 3) * 100);
				counter--;

				barber.Release ();

			mutex.Release ();


			Console.WriteLine("B ready");

			Thread.Sleep (Rand.Next (1, 3) * 100);
		}

	}

	static void Customer(Object id)
	{
		int ID = (int)id;

		while (true) {
			mutex.WaitOne ();
			if (counter < MaxPackets) {
				counter++;
				Console.WriteLine("C{0} add",ID);


				customers.Release ();

				mutex.Release ();

				barber.WaitOne ();
				Console.WriteLine("C{0} haircut",ID);

			} else {
				Console.WriteLine("C{0} skip",ID);
				mutex.Release ();
			}

			Thread.Sleep (Rand.Next (1, 3) * 40);

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



		Thread ProcessorThread = new Thread(Barber);
		ProcessorThread.Start();

		Thread[] Packets = new Thread[MaxPackets];

		for (int i = 0; i < MaxPackets; i++)
		{
			Packets[i] = new Thread(new ParameterizedThreadStart(Customer));
			Packets[i].Start(i);
		}

		for (int i = 0; i < MaxPackets; i++)
		{
			Packets[i].Join();
		}



		ProcessorThread.Join();

	}
}
