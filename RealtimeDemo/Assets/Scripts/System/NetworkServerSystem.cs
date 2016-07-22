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
			List<MessageList.ActorData> players = new List<MessageList.ActorData>();
			for(int i = 0; i < world.players.Count; ++i)
			{
				MessageList.ActorData actorData = new MessageList.ActorData();
				ActorComponent actor = world.players[i].GetComponent<ActorComponent>();
				actorData.id = actor.id;
				actorData.pos = world.players[i].GetComponent<MoveComponent>().pos + Vector3.zero;
				actorData.dir = world.players[i].GetComponent<MoveComponent>().dir;
				actorData.currentSpeed = world.players[i].GetComponent<MoveComponent>().currSpeed;
				actorData.lastProcessedInputs = new List<int>(world.players[i].GetComponent<MoveComponent>().lastProcessedInputs);
				world.players[i].GetComponent<MoveComponent>().lastProcessedInputs.Clear();
				players.Add(actorData);
				//			Debug.Log(string.Format("{0} {1} y : {2}", this.world.currentFrame ,this.world.role, 
				//			                        world.players[i].GetComponent<MoveComponent>().pos.y));
				
			}
			connector.Send2Client(connector.clients, new MessageList.UpdateStateMessage(players, this.world.currentFrame));
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
		}
	}
}