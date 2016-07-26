using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World2WorldPipeline : MonoBehaviour {
	public float lag = 100f; // ms
	public List<Packet> inPackets = new List<Packet>();
	public List<Packet> outPackets = new List<Packet>();

	public List<World> clients = new List<World>();
	public World server;

	public void SetServer(World server)
	{
		this.server = server;
	}

	void Update()
	{
		for(int i = 0; i < inPackets.Count; ++i)
		{
			Packet p = inPackets[i];
			p.ping -= Time.deltaTime;
			if(p.ping <= 0)
			{
				ServerProcess(p);
				inPackets.RemoveAt(i);
				--i;
			}

		}

		for(int i = 0; i < outPackets.Count; ++i)
		{
			Packet p = outPackets[i];
			p.ping -= Time.deltaTime;
			if(p.ping <= 0)
			{
				ProcessSend(p);
				outPackets.RemoveAt(i);
				--i;
			}
			
		}
	}

	void ServerProcess(Packet p)
	{
		if(p.message.cmdId == MessageList.CMD_SEND_INPUT_2_SERVER){
			MessageList.MoveMessage mm = (MessageList.MoveMessage)p.message;
			server.ProcessServerMessage(mm);
		}
	}

	void ProcessSend (Packet p)
	{
		p.receiver.ProcessServerMessage(p.message);
	}

	public void AddClients(World world)
	{
		clients.Add(world);
	}

	public void Send2Server(World sender, MessageList.Message message)
	{
		Packet p = new Packet();
		p.sender = sender;
		p.ping = lag / 1000;
		p.message = message;
		inPackets.Add(p);
	}

	public void Send2Client(List<World> receivers, MessageList.Message message)
	{
		for(int i = 0; i < receivers.Count; ++i)
		{
			World receiver = receivers[i];
			Packet p = new Packet();
			p.receiver = receiver;
			p.ping = lag/1000;
			p.message = message;
			outPackets.Add(p);
		}

	}

	public class Packet
	{
		public static int GEN_ID = 0;
		public int id;
		public World sender;
		public World receiver;
		public float ping;
		public MessageList.Message message;

		public Packet()
		{
			id = GEN_ID;
			++GEN_ID;
		}
	}
}
