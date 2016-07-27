using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClientInterpolationSystem : AbstractSystem {
	public static int MAX_STEP = 150;

	public override void DoFixedUpdate (float dt)
	{

	}

	public override void DoUpdate (float dt)
	{
		if(world.mode == World.Mode.ON_LINE)
		{
			for(int i = 0; i < world.players.Count; ++i)
			{
				ActorComponent actor = world.players[i].GetComponent<ActorComponent>();
				if(actor.id != world.ownerId)
				{
					MoveComponent moveComp = world.players[i].GetComponent<MoveComponent>();
					if(moveComp.savedPoses.Count >= 2)
					{
						moveComp.step += dt;
						moveComp.pos = Vector3.Lerp(moveComp.savedPoses[0], moveComp.savedPoses[1], moveComp.step/MAX_STEP/1000f);
					}
				}
			}
		}

	}

	public override void ProcessMessage (MessageList.Message message)
	{

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
