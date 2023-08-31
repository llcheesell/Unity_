using Mirror;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineSyncClient : NetworkBehaviour
{
    [Header("Settings")]
    public PlayableDirector director;

    // クライアント側での操作は基本的にマスターからの指示に従うので、
    // コマンドは不要です。代わりに、マスターからのRPCを受け取るメソッドを実装します。

    [ClientRpc]
    public void RpcPlayTimeline()
    {
        director.Play();
    }

    [ClientRpc]
    public void RpcPauseTimeline()
    {
        director.Pause();
    }

    [ClientRpc]
    public void RpcStopTimeline()
    {
        director.Stop();
    }

    [ClientRpc]
    public void RpcSyncTimelinePosition(float time)
    {
        director.time = time;
    }
}
