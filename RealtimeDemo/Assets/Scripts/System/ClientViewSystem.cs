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
					MessageList.ActorData actorPos = GetActorPosById(world.ownerId, gameState);
					if(actorPos != null)
					{
						ActorComponent actor = world.players[i].GetComponent<ActorComponent>();
						world.players[i].GetComponent<MoveComponent>().pos = actorPos.pos;
//						Debug.Log(string.Format("{0} {1} y : {2}", Time.realtimeSinceStartup,this.world.role, actorPos.pos.y));
					}
				}
			}
		}
	}

	private MessageList.ActorData GetActorPosById(int id, List<MessageList.ActorData> poses)
	{
		for(int i = 0; i < poses.Count; ++i)
		{
			if(id == poses[i].id)
				return poses[i];
		}
		return null;
	}
}
