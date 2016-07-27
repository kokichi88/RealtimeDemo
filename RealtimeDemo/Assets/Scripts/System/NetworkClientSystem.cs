using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkClientSystem : AbstractSystem {
	private World2WorldPipeline connector;
	public int lastServerFrame = 0;
	public NetworkClientSystem(World2WorldPipeline connector)
	{
		this.connector = connector;
	}
	protected override void OnAddedToWorld (World world)
	{

	}

	public override void DoFixedUpdate (float dt)
	{

	}

	public override void DoUpdate (float dt)
	{

	}

	public override void ProcessMessage (MessageList.Message message)
	{
		if(message.cmdId == MessageList.CMD_SEND_INPUT_2_SERVER)
		{
			connector.Send2Server(this.world, message);
		}
		if(world.mode == World.Mode.ON_LINE)
		{
			if(message.cmdId == MessageList.CMD_UPDATE_GAME_STATE)
			{
				MessageList.UpdateStateMessage updateStateMsg = message  as MessageList.UpdateStateMessage;
				if(lastServerFrame > updateStateMsg.serverFrame) return;
				lastServerFrame = updateStateMsg.serverFrame;
				List<MessageList.ActorData> gameState = updateStateMsg.content;
				for(int i = 0; i < world.players.Count; ++i)
				{
					ActorComponent actor = world.players[i].GetComponent<ActorComponent>();
					MessageList.ActorData actorPos = ClientInterpolationSystem.GetActorDataById(actor.id, gameState);
					if(actorPos != null && actor.id != world.ownerId)
					{
						MoveComponent moveComp = world.players[i].GetComponent<MoveComponent>();
						moveComp.savedPoses.Add(actorPos.pos);
						if(moveComp.savedPoses.Count > 2){
							moveComp.step = 0;
							moveComp.savedPoses.RemoveAt(0);
						}
					}
				}
			}
		}

	}


}
