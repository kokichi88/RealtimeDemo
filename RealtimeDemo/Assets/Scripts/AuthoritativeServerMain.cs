using UnityEngine;
using System.Collections;

public class AuthoritativeServerMain : MonoBehaviour {
	public GameObject client_prefab;
	public GameObject server_prefab;

	void Start () {
		World2WorldPipeline connector = gameObject.GetComponent<World2WorldPipeline>();


		GameObject server = GameObject.Instantiate(server_prefab) as GameObject;
		World serverWorld = server.GetComponent<World>();
		serverWorld.AddSystem(new MoveSystem());
		serverWorld.AddSystem(new NetworkServerSystem(connector));

		serverWorld.AddPlayerToWorld();
		connector.SetServer(serverWorld);

		GameObject client = GameObject.Instantiate(client_prefab) as GameObject;
		World clientWorld = client.GetComponent<World>();
		clientWorld.AddSystem(new MoveSystem());
		clientWorld.AddSystem(new InputSystem());
		clientWorld.AddSystem(new NetworkClientSystem(connector));
		clientWorld.AddSystem(new ClientViewSystem());
		clientWorld.AddPlayerToWorld();
		connector.AddClients(clientWorld);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
