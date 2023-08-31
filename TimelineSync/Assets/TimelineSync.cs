using Mirror;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineSync : NetworkBehaviour
{
    [Header("Settings")]
    public PlayableDirector director;
    public float timelineSyncInterval = 0.5f;

    [Header("Startup Options")]
    public bool startAsServer;
    public bool startAsHost;
    public bool startAsClient;

    private float nextSyncTime;
    private bool isDirectorSet = true;

    private void Start()
    {
        if (director == null)
        {
            Debug.LogWarning("PlayableDirector is not set on TimelineSync. Update process will be stopped.");
            isDirectorSet = false;
            return;
        }

        // Network start options
        if (isServer) return;

        if (startAsServer)
        {
            NetworkManager.singleton.StartServer();
            Debug.Log("Server started");
        }
        else if (startAsHost)
        {
            NetworkManager.singleton.StartHost();
            Debug.Log("Host started");
        }
        else if (startAsClient)
        {
            NetworkManager.singleton.StartClient();
            Debug.Log("Client started");
        }
    }

    private void Update()
    {
        if (!isDirectorSet) return;

        if (isServer)
        {
            if (Time.time >= nextSyncTime)
            {
                nextSyncTime = Time.time + timelineSyncInterval;
                RpcSyncTimelinePosition((float)director.time);
                Debug.Log("Timeline position synced to clients");
            }
        }
    }


    // RPCs (Server -> Clients)
    [ClientRpc]
    public void RpcSyncTimelinePosition(float time)
    {
        director.time = time;
        Debug.Log("Timeline position synced from server");
    }
}
