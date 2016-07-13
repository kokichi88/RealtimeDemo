using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkClientSystem : AbstractSystem {
	private World2WorldPipeline connector;
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


	}


}
