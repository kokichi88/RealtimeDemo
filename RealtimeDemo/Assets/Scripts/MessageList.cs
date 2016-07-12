using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageList{
	public class InputMessage : World.Message
	{
		public InputMessage (List<MoveComponent.MoveInput> inputs)
		{
			this.content = inputs; 
		}
	}

	public class SendInputMessage : World.Message
	{
		public SendInputMessage (List<MoveComponent.MoveInput> inputs)
		{
			this.content = inputs; 
		}
	}

	public class UpdateStateMessage : World.Message
	{
		public UpdateStateMessage(List<ActorPos> players)
		{
			this.content = players;
		}
	}

	public class ActorPos
	{
		public int id;
		public Vector3 pos;
	}

}
