using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReconciliationSystem : AbstractSystem {


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
		GameObject player = this.world.GetPlayerById(this.world.ownerId);
		MoveComponent moveComp = player.GetComponent<MoveComponent>();
		List<MessageList.MoveMessage> queueMoves = moveComp.queueMoves;
		switch(message.cmdId)
		{
		case MessageList.CMD_SEND_INPUT_2_SERVER:
//			queueInputs.Add((message as MessageList.SendInputMessage).content);
			break;
		case MessageList.CMD_UPDATE_GAME_STATE:
			if(world.mode == World.Mode.ON_LINE)
			{
				MessageList.UpdateStateMessage updateStateMsg = message  as MessageList.UpdateStateMessage;
				MessageList.ActorData actorData = ClientInterpolationSystem.GetActorDataById(this.world.ownerId, updateStateMsg.content);
				if(actorData != null && player != null)
				{
					moveComp.pos = actorData.pos;
//					moveComp.dir = actorData.dir;
//					moveComp.currSpeed = actorData.currentSpeed;
					int lastInputProcessedFrame = -1;
					for(int i = 0; i < queueMoves.Count; ++i)
					{
						for(int j = 0; j < actorData.lastProcessedMoves.Count; ++j)
						{
							if(queueMoves[i].moveId <= actorData.lastProcessedMoves[j].moveId)
							{
//								Debug.Log(string.Format("[Remove processed input {0}] server's frame {1} - client's frame {2} ",queueMoves[i].moveId, updateStateMsg.serverFrame,
//								                        world.currentFrame));
								lastInputProcessedFrame = updateStateMsg.serverFrame;
								moveComp.queueMoves.RemoveAt(i);
								actorData.lastProcessedMoves.RemoveAt(j);
								--i;
								--j;
								if( i < 0)
									break;
							}
						}

					}

//					Debug.Log(string.Format("[Update game state] server's frame {0} - client's frame {1} has pos {2} and speed {3}", updateStateMsg.serverFrame,
//					                        world.currentFrame, moveComp.pos, moveComp.currSpeed));

					
//					if(queueMoves.Count <= 0)
//					{
//						int frameCount = (int)(2 * world.lag  / 1000f / world.frameTime) + 2;
//						for(int frame = 0; frame < frameCount ; ++frame)
//						{
//							ReapplyMove(moveComp, world);
//						}
//						Debug.Log(string.Format("[queueInputs === 0] server's frame {0} - client's frame {1} has pos {2} and speed {3}", updateStateMsg.serverFrame,
//						                        world.currentFrame, moveComp.pos, moveComp.currSpeed));
//					}else
					{
//						int extraFrame = (int)(1 * world.lag  / 1000f / world.frameTime) + 2;
//						int frameCount = extraFrame;
//						if( lastInputProcessedFrame > 0)
//						{
//							frameCount = queueInputs[0].activeFrame + extraFrame - lastInputProcessedFrame;
//						}
//						for(int frame = 0; frame < frameCount ; ++frame)
//						{
//							ReapplyMove(moveComp, world);
//						}
//
						for(int i = 0; i < queueMoves.Count; ++i)
						{
							moveComp.pos += queueMoves[i].vec * world.frameTime;
							if(moveComp.pos.x < 0) moveComp.pos.x = 0;
							if(moveComp.pos.x > World.MAX_X) moveComp.pos.x = World.MAX_X;
							if(moveComp.pos.y < 0) moveComp.pos.y = 0;
							if(moveComp.pos.y > World.MAX_Y) moveComp.pos.y = World.MAX_Y;
						}
//						Debug.Log(string.Format("[queueInputs have {0} units] server's frame {1} - client's frame {2} has pos {3} and speed {4}",queueInputs.Count, updateStateMsg.serverFrame,
//						                        world.currentFrame, moveComp.pos, moveComp.currSpeed));
					}


				}
			}
			break;
		}

	}

	public static void ReapplyInput (MoveComponent.MoveInput input, MoveComponent moveComp)
	{
		moveComp.dir = input.dir;
		moveComp.currSpeed = moveComp.speed;
	}

	public static void ReapplyMove (MoveComponent moveComp, World world)
	{
		moveComp.pos += moveComp.dir * moveComp.currSpeed * world.frameTime;
		moveComp.currSpeed -= moveComp.friction * world.frameTime;
		if(moveComp.currSpeed < 0) moveComp.currSpeed = 0;
		if(moveComp.pos.x < 0) moveComp.pos.x = 0;
		if(moveComp.pos.x > World.MAX_X) moveComp.pos.x = World.MAX_X;
		if(moveComp.pos.y < 0) moveComp.pos.y = 0;
		if(moveComp.pos.y > World.MAX_Y) moveComp.pos.y = World.MAX_Y;
	}
}
