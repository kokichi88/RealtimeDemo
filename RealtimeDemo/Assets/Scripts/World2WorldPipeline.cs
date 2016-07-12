using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World2WorldPipeline : MonoBehaviour {
	public float ping = 100f; // ms
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
			if(p.ping < 0)
			{
				ServerProcess(p);
				inPackets.RemoveAt(i);
			}

		}

		for(int i = 0; i < outPackets.Count; ++i)
		{
			Packet p = outPackets[i];
			p.ping -= Time.deltaTime;
			if(p.ping < 0)
			{
				ProcessSend(p);
				outPackets.RemoveAt(i);
			}
			
		}
	}

	void ServerProcess(Packet p)
	{
		if(p.message is MessageList.SendInputMessage){
			MessageList.SendInputMessage sim = (MessageList.SendInputMessage)p.message;
			MessageList.InputMessage message = new MessageList.InputMessage(sim.content as List<MoveComponent.MoveInput>);
			message.activeFrame = server.currentFrame + 1;
			server.AddMessage(message);
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

	public void Send2Server(World sender, World.Message message)
	{
		Packet p = new Packet();
		p.sender = sender;
		p.ping = ping / 1000;
		p.message = message;
		inPackets.Add(p);
	}

	public void Send2Client(List<World> receivers, World.Message message)
	{
		for(int i = 0; i < receivers.Count; ++i)
		{
			World receiver = receivers[i];
			Packet p = new Packet();
			p.receiver = receiver;
			p.ping = ping/1000;
			p.message = message;
			outPackets.Add(p);
		}

	}

	public class Packet
	{
		public World sender;
		public World receiver;
		public float ping;
		public World.Message message;
	}
}
