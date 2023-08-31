using UnityEngine;
using Mirror;

public class NetworkStarter : MonoBehaviour
{
    private NetworkManager networkManager;

    [Header("Startup Options")]
    public bool startAsServer;
    public bool startAsHost;

    private void Start()
    {
        networkManager = FindObjectOfType<NetworkManager>();
        if (networkManager == null)
        {
            Debug.LogError("No NetworkManager found in the scene.");
            return;
        }

        if (startAsServer)
        {
            networkManager.StartServer();
        }
        else if (startAsHost)
        {
            networkManager.StartHost();
        }
    }
}
