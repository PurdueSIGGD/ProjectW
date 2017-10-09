using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NetworkIdentity))]
public class BasePlayer : NetworkBehaviour {
    [HideInInspector]
    public PlayerInput myInput;
    [HideInInspector]
    public PlayerMovement myMovement;
    [HideInInspector]
    public PlayerStats myStats;
    [HideInInspector]
    public Animator myAnimator;
    [HideInInspector]
    public Rigidbody myRigid;
    [HideInInspector]
    public Collider myCollider;
    [HideInInspector]
    public PlayerAbility[] myAbilities;
    [HideInInspector]
    public NetworkIdentity myNetworkIdentity;
    [HideInInspector]
    public PlayerNetworking myNetworking;
    [HideInInspector]
    public PlayerGUI myGUI;
    
	// Use this for initialization
	void Start () {
        /* every component that is a PlayerComponent must be initialized with the base player */
        myInput = (PlayerInput)GetComponent<PlayerInput>().initialize(this);
        myMovement = (PlayerMovement)GetComponent<PlayerMovement>().initialize(this);
        myStats = (PlayerStats)GetComponent<PlayerStats>().initialize(this);
        myNetworking = (PlayerNetworking)GetComponent<PlayerNetworking>().initialize(this);
        myGUI = (PlayerGUI)GetComponent<PlayerGUI>().initialize(this);
        PlayerAbility[] abilityCandidates = GetComponents<PlayerAbility>();
        myAbilities = new PlayerAbility[abilityCandidates.Length];
        int myAbilityIndex = 0;
        bool failed = false;
        foreach (PlayerAbility playerAbility in abilityCandidates) {
            foreach (PlayerAbility myAbility in myAbilities) {
                if (myAbility != null && playerAbility.GetType() == myAbility.GetType()) {
                    Debug.LogError("You cannot add two of the same abilities. Talk to Andrew if you want more information on the subject.");
                    Destroy(playerAbility);
                    /* note to Andrew: look at PlayerAbility, and the odd ramifications of sending messages across the network to components */
                    failed = true;
                    break;
                }
            }
            if (!failed) {

                myAbilities[myAbilityIndex] = (PlayerAbility)playerAbility.initialize(this);
                myAbilityIndex++; 
               
            }
            failed = false;
        }
        

        myAnimator = transform.GetChild(0).GetComponent<Animator>();
        myRigid = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
        myNetworkIdentity = this.GetComponent<NetworkIdentity>();
    }
	
}
