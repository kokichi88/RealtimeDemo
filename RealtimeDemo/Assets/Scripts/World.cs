using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {
	public enum Mode {
		ON_LINE,
		OFF_LINE
	}

	public enum Role {
		CLIENT,
		SERVER
	}

	public static float MAX_X = 10;
	public static float MAX_Y = 10;

	public float frameTime = 0.1f; // 10 fps
	public GameObject avatar_prefab;
	public GameObject map_prefab;
	public Vector3 mapOriginPos;
	public Vector3 startPos;
	public int currentFrame;
	public Mode mode;
	public Role role;

	public int ownerId = 0;
	public List<GameObject> players = new List<GameObject>();
	private GameObject map;
	private List<AbstractSystem> systems = new List<AbstractSystem>();
	private List<Message> messages = new List<Message>();
	private int genId = 0;
	private float lastTime;
	private float stackTime;

	void Start()
	{
		Init ();
	}

	public void Init()
	{
		map = GameObject.Instantiate(map_prefab, mapOriginPos, map_prefab.transform.rotation) as GameObject;
		map.name = this.gameObject.name + "_map";
		lastTime = Time.realtimeSinceStartup;
	}

	public void AddPlayerToWorld()
	{
		GameObject player = GameObject.Instantiate(avatar_prefab) as GameObject;
		player.name = gameObject.name + "_player_" + genId;;
		player.GetComponent<ActorComponent>().id = genId;
		player.GetComponent<ActorComponent>().name = player.name;

		players.Add(player);
		++genId;
	}

	public void AddSystem(AbstractSystem system)
	{
		systems.Add(system);
		system.OnAdd(this);
	}

	public void AddMessage(Message message)
	{
		messages.Add(message);
	}

	public void ProcessServerMessage(Message message)
	{
		for(int j = 0; j < systems.Count; ++j)
		{
			systems[j].ProcessMessage(message);
		}
	}

	void Update()
	{
		float curTime = Time.realtimeSinceStartup;
		stackTime += curTime - lastTime;
		lastTime = curTime;
		while(stackTime >= frameTime)
		{
			stackTime -= frameTime;
			DoUpdate(frameTime);
		}

		for(int i = 0; i < systems.Count; ++i)
		{
			systems[i].DoUpdate(Time.deltaTime);
		}


	}


	
	void DoUpdate (float dt)
	{
		for(int i = 0; i < systems.Count; ++i)
		{
			systems[i].DoFixedUpdate(dt);
		}

		for(int i = 0; i < messages.Count; ++i)
		{
			if(messages[i].activeFrame == currentFrame)
			{
				for(int j = 0; j < systems.Count; ++j)
				{
					systems[j].ProcessMessage(messages[i]);
				}
				messages.RemoveAt(i);
			}
		}
		++currentFrame;
	}


	public class Message
	{
		public int activeFrame;
		public object content;
	}
}
