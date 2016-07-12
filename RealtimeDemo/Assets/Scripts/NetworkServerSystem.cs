using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkServerSystem : AbstractSystem {
	private World2WorldPipeline connector;
	public NetworkServerSystem(World2WorldPipeline connector)
	{
		this.connector = connector;
	}
	
	protected override void OnAddedToWorld (World world)
	{
		
	}
	
	public override void DoFixedUpdate (float dt)
	{
		List<MessageList.ActorPos> players = new List<MessageList.ActorPos>();
		for(int i = 0; i < world.players.Count; ++i)
		{
			MessageList.ActorPos actorPos = new MessageList.ActorPos();
			Actor actor = world.players[i].GetComponent<Actor>();
			actorPos.id = actor.id;
			actorPos.pos = world.players[i].GetComponent<MoveComponent>().pos;
			players.Add(actorPos);
		}
		connector.Send2Client(connector.clients, new MessageList.UpdateStateMessage(players));
	}
	
	public override void DoUpdate (float dt)
	{
		
	}
	
	public override void ProcessMessage (World.Message message)
	{

	}
}