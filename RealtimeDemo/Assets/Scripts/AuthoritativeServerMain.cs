using UnityEngine;
using System.Collections;

public class AuthoritativeServerMain : MonoBehaviour {
	public GameObject client_prefab;
	public float frameTime = 0.02f;
	public int updateStateTimesPerSecond = 10;
	public GameObject server_prefab;

	protected World serverWorld;

	protected World clientWorld;

	void Start () {
		World2WorldPipeline connector = gameObject.GetComponent<World2WorldPipeline>();


		GameObject server = GameObject.Instantiate(server_prefab) as GameObject;
		serverWorld = server.GetComponent<World> ();
		serverWorld.AddSystem(new MoveSystem());
		serverWorld.AddSystem(new NetworkServerSystem(connector, 1f/updateStateTimesPerSecond));

		serverWorld.AddPlayerToWorld();
		connector.SetServer(serverWorld);

		GameObject client = GameObject.Instantiate(client_prefab) as GameObject;
		clientWorld = client.GetComponent<World> ();
		clientWorld.AddSystem(new MoveSystem());
		clientWorld.AddSystem(new InputSystem());
		clientWorld.AddSystem(new NetworkClientSystem(connector));
		clientWorld.AddSystem(new ClientViewSystem());
		clientWorld.AddPlayerToWorld();
		connector.AddClients(clientWorld);

	}
	
	// Update is called once per frame
	void Update () {
		serverWorld.frameTime = frameTime;
		clientWorld.frameTime = frameTime;

		if(updateStateTimesPerSecond > 0)
		{
			MessageList.Message msg = new MessageList.ChangeServerUpdateTime(1f/ updateStateTimesPerSecond);
			msg.activeFrame = serverWorld.currentFrame + 1;
			serverWorld.AddMessage(msg);
		}
	}
}
