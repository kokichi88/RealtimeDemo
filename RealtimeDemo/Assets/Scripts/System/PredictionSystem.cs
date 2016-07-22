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
				if(moveComp.currSpeed != 0)
					Debug.Log(string.Format("[Prediction system] frame {0} has pos {1} and speed {2}", world.currentFrame, 
					                        moveComp.pos, moveComp.currSpeed));
			}
		}
		
	}


	public override void ProcessMessage (MessageList.Message message)
	{
		if(message.cmdId ==  MessageList.CMD_SEND_INPUT_2_SERVER)
		{
			MessageList.SendInputMessage sim = message as MessageList.SendInputMessage;
			GameObject player = this.world.GetPlayerById(sim.content.actorId);
			if(player != null)
			{
				MoveComponent moveComp = player.GetComponent<MoveComponent>();

			}
			ProcessMoveInput(sim.content);
			Debug.Log(string.Format("[Client predict input {0}] Process input frame {1} has dir {2}",sim.inputId, world.currentFrame, sim.content.dir));
		}
	}
}
