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

	public override void ProcessMessage (MessageList.Message message)
	{
		if(world.mode == World.Mode.ON_LINE)
		{
			if(message.cmdId == MessageList.CMD_UPDATE_GAME_STATE)
			{
				MessageList.UpdateStateMessage updateStateMsg = message  as MessageList.UpdateStateMessage;
				List<MessageList.ActorData> gameState = updateStateMsg.content;
				for(int i = 0; i < world.players.Count; ++i)
				{
					ActorComponent actor = world.players[i].GetComponent<ActorComponent>();
					MessageList.ActorData actorPos = GetActorDataById(actor.id, gameState);
					if(actorPos != null && actor.id != world.ownerId)
					{
						world.players[i].GetComponent<MoveComponent>().pos = actorPos.pos;
					}
				}
			}
		}
	}

	public static MessageList.ActorData GetActorDataById(int id, List<MessageList.ActorData> actorDatas)
	{
		for(int i = 0; i < actorDatas.Count; ++i)
		{
			if(id == actorDatas[i].id)
				return actorDatas[i];
		}
		return null;
	}
}
