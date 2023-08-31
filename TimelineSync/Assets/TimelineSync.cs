using Mirror;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineSync : NetworkBehaviour
{
    [Header("Settings")]
    public PlayableDirector director;
    public float syncMasterInterval = 0.5f; // 0.5秒ごとに再生位置を同期

    private float nextSyncTime;
    private bool isDirectorSet = true; // PlayableDirectorがセットされているかのフラグ

    private void Start()
    {
        // PlayableDirectorのチェック
        if (director == null)
        {
            Debug.LogWarning("PlayableDirector is not set on TimelineSync. Update process will be stopped.");
            isDirectorSet = false;
        }
    }

    private void Update()
    {
        // マスター側でのみ実行 & PlayableDirectorがセットされている場合のみ実行
        if (!isServer || !isDirectorSet) return;

        if (Time.time >= nextSyncTime)
        {
            nextSyncTime = Time.time + syncMasterInterval;
            CmdSyncTimelinePosition((float)director.time);
        }
    }

    [Command]
    public void CmdPlayTimeline()
    {
        director.Play();
        RpcPlayTimeline();
    }

    [Command]
    public void CmdPauseTimeline()
    {
        director.Pause();
        RpcPauseTimeline();
    }

    [Command]
    public void CmdStopTimeline()
    {
        director.Stop();
        RpcStopTimeline();
    }

    [Command]
    private void CmdSyncTimelinePosition(float time)
    {
        RpcSyncTimelinePosition(time);
    }

    [ClientRpc]
    private void RpcPlayTimeline()
    {
        director.Play();
    }

    [ClientRpc]
    private void RpcPauseTimeline()
    {
        director.Pause();
    }

    [ClientRpc]
    private void RpcStopTimeline()
    {
        director.Stop();
    }

    [ClientRpc]
    private void RpcSyncTimelinePosition(float time)
    {
        director.time = time;
    }
}
