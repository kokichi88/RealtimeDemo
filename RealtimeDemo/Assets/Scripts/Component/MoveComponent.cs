using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveComponent : MonoBehaviour {
	public float speed;
	public Vector3 dir;
	public float friction;
	public List<MessageList.MoveMessage> lastProcessedMoves = new List<MessageList.MoveMessage>();
	public List<MessageList.MoveMessage> queueMoves = new List<MessageList.MoveMessage>();
	public List<Vector3> savedPoses = new List<Vector3>();
	public float step = 0;
	public float currSpeed;
	public Vector3 pos;

	public class MoveInput
	{
		public int actorId;
		public int inputId;
		public int activeFrame;
		public Vector3 dir;
	}
}
