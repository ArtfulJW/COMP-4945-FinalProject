using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RPCTest : NetworkBehaviour
{

    public override void OnNetworkSpawn()
    {   
        // ONLY send an RPC to the server on the client that owns the NetworkObject that owns THIS NetworkBehaviour Instance.
        // Check if the spawned NetworkBehaviour.NetworkObject is NOT A SERVER and IS THE OWNER of the local player
        if(!IsServer && IsOwner) 
        {
            // Determined that we are a Client, Send a ServerRpc.
            TestServerRpc(0, NetworkObjectId);
        } else
        {
            // This NetworkBehaviour.NetworkObject is a Server, Send ClientRpc.
            TestClientRpc(0, NetworkObjectId);
        }
    }

    [ClientRpc]
    void TestClientRpc(int value, ulong sourceNetworkObjectId)
    {
        Debug.LogError($"Client Received the RPC #{value} on NetworkObject #{sourceNetworkObjectId}");
        
        // Only send and RPC to the server as a client that owns the NetworkObject that owns THIS NetworkBehaviour Instance.
        if (IsOwner)
        {
            TestServerRpc(value + 1, sourceNetworkObjectId);
        }
    }

    [ServerRpc]
    void TestServerRpc(int value, ulong sourceNetworkObjectId)
    {
        Debug.LogError($"Server Received the RPC #{value} on NetworkObject #{sourceNetworkObjectId}");
        TestClientRpc(value, sourceNetworkObjectId);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            // Is a Client
            if (Input.GetKeyDown(KeyCode.W))
            {
               
            }
        }
    }
}
