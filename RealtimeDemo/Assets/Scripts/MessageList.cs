using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageList{
	public static int CMD_USER_INPUT = 1;
	public static int CMD_SEND_INPUT_2_SERVER = 2;
	public static int CMD_UPDATE_GAME_STATE = 3;
	public static int CMD_SERVER_SEND_UPDATE_TIME = 4;


	public class Message
	{
		public int cmdId;
		public int activeFrame;
	}

	public class InputMessage : Message
	{
		public List<MoveComponent.MoveInput> content;
		public InputMessage (List<MoveComponent.MoveInput> inputs)
		{
			cmdId = CMD_USER_INPUT;
			this.content = inputs; 
		}
	}

	public class SendInputMessage : Message
	{
		public List<MoveComponent.MoveInput> content;
		public SendInputMessage (List<MoveComponent.MoveInput> inputs)
		{
			cmdId = CMD_SEND_INPUT_2_SERVER;
			this.content = inputs; 
		}
	}

	public class UpdateStateMessage : Message
	{
		public List<ActorPos> content;
		public UpdateStateMessage(List<ActorPos> players)
		{
			cmdId = CMD_UPDATE_GAME_STATE;
			this.content = players;
		}
	}

	public class ChangeServerUpdateTime : Message
	{
		public float frameTime;
		public ChangeServerUpdateTime(float frameTime)
		{
			cmdId = CMD_SERVER_SEND_UPDATE_TIME;
			this.frameTime = frameTime;
		}
	}

	public class ActorPos
	{
		public int id;
		public Vector3 pos;
	}

}
