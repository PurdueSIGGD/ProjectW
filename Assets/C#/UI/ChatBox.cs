using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class ChatBox : NetworkBehaviour {

    [SerializeField]
    private SyncListString chatList = new SyncListString(); // list of chat so far

    public InputField inputText;    // input Text for chatbox
    public Text chatLog;        // display of chat log


    // Use this for initialization
    void Start () {
            
        // handle when user finish editing
        inputText.onEndEdit.AddListener(delegate { CmdPostChatMessage(inputText.text); });
	}
	
	
    /**
     * Call when user finish typing
     * Tell server to update chatlog
     */ 
    [Command]
    public void CmdPostChatMessage(string message)
    {
        // make a network message
        if (message.Length == 0) return;
        
        // tell server: post this msg
        RpcServerPostChatMsg(message);

        // reset input
        inputText.text = "";
        inputText.ActivateInputField();
        inputText.Select();
    }

    /**
     * Called by server, make every client to update their chat log
     */ 
    [ClientRpc]
    void RpcServerPostChatMsg(string message)
    {
        Debug.Log("Server: " + message);
        chatLog.text += "\n" + message;
    }
  
}
