using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class ChatBox : NetworkBehaviour {

    NetworkClient client;
    const short chatMsgId = 1000;

    [SerializeField]
    private SyncListString chatList = new SyncListString(); // list of chat so far

    public InputField inputText;    // input Text for chatbox
    public Text chatLog;        // display of chat log

    public override void OnStartClient()
    {
        // make chatList aware when there is update
        chatList.Callback = OnChatUpdated;
    }

    // Use this for initialization
    void Start () {

        // get current client
        client = NetworkManager.singleton.client;

        // make server respond to chatMsg
        NetworkServer.RegisterHandler(chatMsgId, OnServerPostChatMessage);

        // handle when user finish editing
        inputText.onEndEdit.AddListener(delegate { PostChatMessage(inputText.text); });
	}
	
	
    /**
     * Call when user finish typing
     */ 
    [Client]
    public void PostChatMessage(string message)
    {
        // make a network message
        if (message.Length == 0) return;
        var msg = new StringMessage(message);

        // send to server, msg Type: chatMsg
        if (client == null)
        {
            Debug.Log("Client: null");
            return;
        }
        client.Send(chatMsgId, msg);
        Debug.Log(message);

        // reset input
        inputText.text = "";
        inputText.ActivateInputField();
        inputText.Select();
    }

    /**
     * Call when server receive msg
     */
    void OnServerPostChatMessage(NetworkMessage netMsg)
    {
        string message = netMsg.ReadMessage<StringMessage>().value;
        chatList.Add(message);
    }

    /**
     * Call when server update chat
     * Helps to update chat log display
     */ 
    private void OnChatUpdated(SyncListString.Operation op, int index)
    {
        chatLog.text += chatList[chatList.Count - 1] + "\n";
    }
}
