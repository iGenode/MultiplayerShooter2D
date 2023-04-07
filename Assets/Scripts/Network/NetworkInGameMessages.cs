using Fusion;
using UnityEngine;

public class NetworkInGameMessages : NetworkBehaviour
{
    //[SerializeField]
    //private InGameNotificationsHandler _inGameNotificationHandler;

    //public void SendInGameRPCMessage(string nickname, string message)
    //{
    //    RPC_InGameMessage($"<b>{nickname}</b> {message}");
    //}

    //// Message from the host to all clients
    //[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    //private void RPC_InGameMessage(string message, RpcInfo info = default)
    //{
    //    Debug.Log($"[RPC] InGameMessage {message}");

    //    if (_inGameNotificationHandler != null)
    //    {
    //        // TODO: use an event
    //        _inGameNotificationHandler.OnMessageReceived(message);
    //    }
    //}
}
