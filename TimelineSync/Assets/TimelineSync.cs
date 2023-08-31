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
    public bool startAsClient;  // 新しいチェックボックス

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
        if (isServer) return; // If already a server, skip the startup options

        if (startAsServer)
        {
            NetworkManager.singleton.StartServer();
        }
        else if (startAsHost)
        {
            NetworkManager.singleton.StartHost();
        }
        else if (startAsClient)
        {
            NetworkManager.singleton.StartClient();  // クライアントとして起動
        }
    }

    private void Update()
    {
        if (!isDirectorSet) return;

        if (isServer)
        {
            if (Time.time >= nextSyncTime)
            {
                nextSyncTime = Time.time + timelineSyncInterval;  // 名前を変更
                Debug.Log("Server: Sending sync signal.");  // サーバーとして同期信号を送信した時のログ
                RpcSyncTimelinePosition((float)director.time);
            }
        }
        else if (isClient)
        {
            // For clients, always sync at regular intervals
            if (Time.time >= nextSyncTime)
            {
                nextSyncTime = Time.time + timelineSyncInterval;  // 名前を変更
                CmdSyncTimelinePosition((float)director.time);
            }
        }
    }


    // Commands (Client -> Server)
    [Command]
    private void CmdSyncTimelinePosition(float time)
    {
        RpcSyncTimelinePosition(time);
    }

    // RPCs (Server -> Clients)
    [ClientRpc]
    public void RpcSyncTimelinePosition(float time)
    {
        Debug.Log("Client: Received sync signal.");  // クライアントとして同期信号を受信した時のログ
        director.time = time;
    }
}
