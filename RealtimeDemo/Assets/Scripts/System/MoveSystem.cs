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
				for(int j = 0; j < moveComp.queueMoves.Count; ++j)
				{
					moveComp.pos += moveComp.queueMoves[j].vec * dt;
				}
				moveComp.lastProcessedMoves.AddRange(moveComp.queueMoves);
				moveComp.queueMoves.Clear();
//				moveComp.currSpeed -= moveComp.friction * dt;
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
		switch(message.cmdId)
		{
		case MessageList.CMD_USER_INPUT:
			ProcessMoveInput((message as MessageList.InputMessage).content);
			break;
		case MessageList.CMD_SEND_INPUT_2_SERVER:
			MessageList.MoveMessage moveMsg = message as MessageList.MoveMessage;
			GameObject player = this.world.GetPlayerById(moveMsg.actorId);
			MoveComponent moveComp = player.GetComponent<MoveComponent>();
			moveComp.queueMoves.Add(moveMsg);
			break;
		}
	}

	protected void ProcessMoveInput(MoveComponent.MoveInput input)
	{
		GameObject player = this.world.GetPlayerById(input.actorId);
		if(player != null)
		{
			MoveComponent moveComp = player.GetComponent<MoveComponent>();
			moveComp.dir = input.dir;
			moveComp.currSpeed = moveComp.speed;

		}
	}

}
