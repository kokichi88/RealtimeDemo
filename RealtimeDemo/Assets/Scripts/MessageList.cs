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

		public MoveComponent.MoveInput content;
		public int inputId;
		public InputMessage (MoveComponent.MoveInput input)
		{
			cmdId = CMD_USER_INPUT;
			this.content = input; 
		}
	}

	public class SendInputMessage : Message
	{
		public static int GEN_ID = 0;
		public int inputId;
		public MoveComponent.MoveInput content;
		public SendInputMessage (MoveComponent.MoveInput input)
		{
			cmdId = CMD_SEND_INPUT_2_SERVER;
			inputId = ++GEN_ID;
			this.content = input; 
			this.content.inputId = this.inputId;
		}
	}

	public class UpdateStateMessage : Message
	{
		public List<ActorData> content;
		public int lastInputProcessed;
		public UpdateStateMessage(List<ActorData> players)
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

	public class ActorData
	{
		public int id;
		public int lastProcessedInput;
		public Vector3 pos;
	}

}
