using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkServerSystem : AbstractSystem {
	private World2WorldPipeline connector;
	public float sendUpdateTime;
	private float count;
	public NetworkServerSystem(World2WorldPipeline connector, float sendUpdateTime)
	{
		this.connector = connector;
		this.sendUpdateTime = sendUpdateTime;
	}
	
	protected override void OnAddedToWorld (World world)
	{
		
	}
	
	public override void DoFixedUpdate (float dt)
	{
		count -= dt;
		if(count <= 0)
		{
			count += sendUpdateTime;
			List<MessageList.ActorPos> players = new List<MessageList.ActorPos>();
			for(int i = 0; i < world.players.Count; ++i)
			{
				MessageList.ActorPos actorPos = new MessageList.ActorPos();
				ActorComponent actor = world.players[i].GetComponent<ActorComponent>();
				actorPos.id = actor.id;
				actorPos.pos = world.players[i].GetComponent<MoveComponent>().pos;
				actorPos.pos = new Vector3(actorPos.pos.x, actorPos.pos.y, actorPos.pos.z);
				players.Add(actorPos);
				//			Debug.Log(string.Format("{0} {1} y : {2}", this.world.currentFrame ,this.world.role, 
				//			                        world.players[i].GetComponent<MoveComponent>().pos.y));
				
			}
			connector.Send2Client(connector.clients, new MessageList.UpdateStateMessage(players));
		}

	}
	
	public override void DoUpdate (float dt)
	{
		
	}
	
	public override void ProcessMessage (MessageList.Message message)
	{
		if(message.cmdId == MessageList.CMD_SERVER_SEND_UPDATE_TIME)
		{
			sendUpdateTime = (message as MessageList.ChangeServerUpdateTime).frameTime;
			Debug.Log("update frame time " + sendUpdateTime);
		}
	}
}