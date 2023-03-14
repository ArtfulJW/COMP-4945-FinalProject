using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Move();
            }
        }

        public void Move()
        {
            /*
             * Because we are the Server, we immediately move.
             * Don't RPC to the Server
             */
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetRandomPositionOnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            }
            /*
             * Because we are a Client, we RPC to the server.
             */
            else
            {   
                // [ServerRpc]
                SubmitPositionRequestServerRpc();
            }
        }

        [ServerRpc]
        // A client invoked remote procedure call received by and executed on the server-side.
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane();
            //Position.Value += new Vector3(1, 1, 0); 
            //if (Input.GetKeyDown(KeyCode.W))
            //{
            //    transform.position += new Vector3(0, (float)0.1, 0);
            //    Position.Value = transform.position;
            //}
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        void Update()
        {
            //SubmitPositionRequestServerRpc();
            transform.position = Position.Value;
        }
    }
}