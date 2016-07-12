using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClientViewSystem : AbstractSystem {

	public override void DoFixedUpdate (float dt)
	{

	}

	public override void DoUpdate (float dt)
	{

	}

	public override void ProcessMessage (World.Message message)
	{
		if(message is MessageList.UpdateStateMessage)
		{
			List<MessageList.ActorPos> gameState = message.content as List<MessageList.ActorPos>;
			for(int i = 0; i < world.players.Count; ++i)
			{
				MessageList.ActorPos actorPos = GetActorPosById(world.ownerId, gameState);
				if(actorPos != null)
				{
					ActorComponent actor = world.players[i].GetComponent<ActorComponent>();
					world.players[i].GetComponent<MoveComponent>().pos = actorPos.pos;
				}
			}
		}
	}

	private MessageList.ActorPos GetActorPosById(int id, List<MessageList.ActorPos> poses)
	{
		for(int i = 0; i < poses.Count; ++i)
		{
			if(id == poses[i].id)
				return poses[i];
		}
		return null;
	}
}
