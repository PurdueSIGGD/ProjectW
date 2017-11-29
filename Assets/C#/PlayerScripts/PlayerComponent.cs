using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public abstract class PlayerComponent : NetworkBehaviour {
    /* Base class that stores a reference to the player movement */
    protected BasePlayer myBase;
    public PlayerComponent initialize(BasePlayer p) {
        myBase = p;
        return this;
    }

    // Script start execution is grumpy with me, so I am manually overriding this to make sure BasePlayer starts first
    public abstract void PlayerComponent_Start();
    public abstract void PlayerComponent_Update();
    bool hasStarted = false;
    public void Start() {
        if (myBase == null && !hasStarted) {
            hasStarted = false;
        } else {
            // Our actual start is here
            //print("actual start: " + this);
            //bufs.Callback = BufChanged;
            myComponents = this.GetComponents<PlayerComponent>();
            for (int i = 0; i < myComponents.Length; i++) {
                if (myComponents[i] == this) myComponentIndex = i;
            }
            PlayerComponent_Start();
            hasStarted = true;

        }
    }
    public void Update() {
        if (myBase != null) {
            if (!hasStarted) {
                Start(); // Try again
            }
            PlayerComponent_Update();
            //if (delegates.Count > 0) print(playerControllerId + ", delegates size " + delegates.Count);
        }
    }

    /**
     * ============================================
     * Methods to help with network synchronization
     * ============================================
     */
     public struct BufWrapper {
        public int index;
        public Buf buf;
    };
    public struct Buf {
        public string methodName;
        // Filled with all the data that you may need, feel free to expand if needed
        public int[] intList;
        public float[] floatList;
        public Vector3[] vectorList;
    };
    public class CallbackBufs : SyncListStruct<Buf> { }
    // Our syncvar, synchronized across all clients
    public Buf bufData;
    public delegate void OnClientNotify(Buf values);
    public delegate bool ServerVerifyClientNotification(Buf values);
    public Hashtable delegates = new Hashtable();
    public Hashtable verifications = new Hashtable();
    private PlayerComponent[] myComponents;
    private int myComponentIndex;

    /**
     * To be called at class initialization, same across server and all clients
     */
    public void ResgisterDelegate(string methodName, OnClientNotify method, ServerVerifyClientNotification verification) {
        delegates.Add(methodName, method);
        verifications.Add(methodName, verification);
        //print("resgistering delegate: " + methodName + " " + method + " " + delegates.Count);
    }
    public void ResgisterDelegate(string methodName, OnClientNotify method)
    {
        ServerVerifyClientNotification alwaysTrue = (values) => { return true; };
        ResgisterDelegate(methodName, method, alwaysTrue);
    }

    public void NotifyAllClientDelegates(Buf data) {
        // We can call the method directly if we are calling this from the player

        if (isLocalPlayer || myBase.myInput.isBot()) {
            /* Server verification method */
            if (((ServerVerifyClientNotification)verifications[data.methodName]).Invoke(data)) {
                BufWrapper bufW = new BufWrapper();
                bufW.buf = data;
                bufW.index = myComponentIndex;
                ((OnClientNotify)delegates[data.methodName]).Invoke(data);
                CmdNotifyAll(bufW);
            } else
            {
                Debug.LogError("Request was denied by the server. We should probably kick this player.");
            }
           
        } else {
            Debug.LogWarning("You are not the owner of this object! Not sending any data.");
        } 
    }

    [Command]
    public void CmdNotifyAll(BufWrapper data) {
        RpcAddBuf(data);
        
    }

    [ClientRpc]
    void RpcAddBuf(BufWrapper data) {
        //Debug.Log("adding buf at index " + data.index + " which is " + myComponents[data.index]);
        if (myBase != null)
        {
            for (int i = 0; i < myComponents.Length; i++)
            {
                if (i == data.index)
                {
                    myComponents[i].bufData = data.buf;
                    myComponents[i].BufChanged();
                }
            }
        }
    }
    /**
     * Clear the latest change, wait for the next operation
     */
    void BufChanged() {
        if (!isLocalPlayer) {
            Buf data = bufData;
            //print("buf now at size: " + bufs.Count);
            //(data.methodName + " " + delegates.Count + " " + delegates[data.methodName] + " " + this);
            ((OnClientNotify)delegates[data.methodName]).Invoke(data);
           
        }

    }
       

}
