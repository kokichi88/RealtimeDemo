using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveSystem : AbstractSystem {

	public override void DoFixedUpdate (float dt)
	{
		if(this.world.role == World.Role.SERVER || this.world.mode == World.Mode.OFF_LINE)
		{
			for(int i = 0; i < this.world.players.Count; ++i)
			{
				GameObject player = this.world.players[i];
				MoveComponent moveComp = player.GetComponent<MoveComponent>();
				moveComp.pos += moveComp.dir * moveComp.currSpeed * dt;
				moveComp.currSpeed -= moveComp.friction * dt;
				if(moveComp.currSpeed < 0) moveComp.currSpeed = 0;
				if(moveComp.pos.x < 0) moveComp.pos.x = 0;
				if(moveComp.pos.x > World.MAX_X) moveComp.pos.x = World.MAX_X;
				if(moveComp.pos.y < 0) moveComp.pos.y = 0;
				if(moveComp.pos.y > World.MAX_Y) moveComp.pos.y = World.MAX_Y;
			}
		}

	}

	public override void DoUpdate (float dt)
	{
		for(int i = 0; i < this.world.players.Count; ++i)
		{
			GameObject player = this.world.players[i];
			MoveComponent moveComp = player.GetComponent<MoveComponent>();
			player.transform.position = moveComp.pos + this.world.mapOriginPos;
		}
	}

	public override void ProcessMessage (MessageList.Message message)
	{
		if(message.cmdId ==  MessageList.CMD_USER_INPUT)
		{
			ProcessMoveInput((message as MessageList.InputMessage).content);
		}
	}

	protected void ProcessMoveInput(MoveComponent.MoveInput input)
	{
	
		GameObject player = this.world.GetPlayerById(input.actorId);
		if(player != null)
		{
			MoveComponent moveComp = player.GetComponent<MoveComponent>();
			ActorComponent actorComp = player.GetComponent<ActorComponent>();
			moveComp.dir = input.dir;
			moveComp.currSpeed = moveComp.speed;
			moveComp.lastInputProcessed = input.inputId;
		}
	}

}
