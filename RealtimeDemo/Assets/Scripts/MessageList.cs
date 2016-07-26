using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageList{
	public const int CMD_USER_INPUT = 1;
	public const int CMD_SEND_INPUT_2_SERVER = 2;
	public const int CMD_UPDATE_GAME_STATE = 3;
	public const int CMD_SERVER_SEND_UPDATE_TIME = 4;
	public const int CMD_PREDICT_USER_INPUT = 5;
	

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

	public class MoveMessage : Message
	{
		public static int GEN_ID = 0;
		public int actorId;
		public int moveId;
		public Vector3 vec;
		public MoveMessage (Vector3 vec, int actorId)
		{
			cmdId = CMD_SEND_INPUT_2_SERVER;
			this.vec = vec;
			this.actorId = actorId;
			moveId = ++GEN_ID;
		}
	}

	public class PredictionInputMessage : Message
	{
		public int inputId;
		public MoveComponent.MoveInput content;
		public PredictionInputMessage (MoveComponent.MoveInput input)
		{
			cmdId = CMD_PREDICT_USER_INPUT;
			this.content = input; 
			this.content.inputId = this.inputId;
		}
	}

	public class UpdateStateMessage : Message
	{
		public List<ActorData> content;
		public int serverFrame;
		public UpdateStateMessage(List<ActorData> players, int serverFrame)
		{
			cmdId = CMD_UPDATE_GAME_STATE;
			this.content = players;
			this.serverFrame = serverFrame;
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
		public List<MessageList.MoveMessage> lastProcessedMoves;
		public Vector3 pos;
		public float currentSpeed;
		public Vector3 dir;
	}

}
