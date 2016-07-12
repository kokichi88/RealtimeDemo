using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PredictionSystem : MoveSystem {
	public override void DoFixedUpdate (float dt)
	{
		if(this.world.role == World.Role.CLIENT && this.world.mode == World.Mode.ON_LINE)
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


	public override void ProcessMessage (World.Message message)
	{
		if(message is MessageList.SendInputMessage)
		{
			ProcessMoveInput(message.content as List<MoveComponent.MoveInput>);
		}
	}
}
