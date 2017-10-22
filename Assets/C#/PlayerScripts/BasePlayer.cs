using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(PlayerNetworking))]
[RequireComponent(typeof(PlayerGUI))]
[RequireComponent(typeof(PlayerEffects))]
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
    public Collider myNoFrictionCollider;
    [HideInInspector]
    public PlayerAbility[] myAbilities;
    [HideInInspector]
    public NetworkIdentity myNetworkIdentity;
    [HideInInspector]
    public PlayerNetworking myNetworking;
    [HideInInspector]
    public PlayerGUI myGUI;
    [HideInInspector]
    public PlayerEffects myEffects;
    [HideInInspector]
    [SyncVar]
    public string playerId;

    // Use this for initialization
    void Start () {
        

        /* every component that is a PlayerComponent must be initialized with the base player */
        myInput = (PlayerInput)GetComponent<PlayerInput>().initialize(this);
        myMovement = (PlayerMovement)GetComponent<PlayerMovement>().initialize(this);
        myStats = (PlayerStats)GetComponent<PlayerStats>().initialize(this);
        myNetworking = (PlayerNetworking)GetComponent<PlayerNetworking>().initialize(this);
        myGUI = (PlayerGUI)GetComponent<PlayerGUI>().initialize(this);
        myEffects = (PlayerEffects)GetComponent<PlayerEffects>().initialize(this);
        PlayerAbility[] abilityCandidates = GetComponents<PlayerAbility>();
        myAbilities = new PlayerAbility[abilityCandidates.Length];
        int myAbilityIndex = 0;
        foreach (PlayerAbility playerAbility in abilityCandidates) {
            myAbilities[myAbilityIndex] = (PlayerAbility)playerAbility.initialize(this);
            myAbilityIndex++;
        }
        

        myAnimator = transform.GetChild(0).GetComponent<Animator>();
        myRigid = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
        myNoFrictionCollider = transform.Find("NoFrictionSides").GetComponent<Collider>();
        myNetworkIdentity = this.GetComponent<NetworkIdentity>();

        if (isServer) {
            playerId = Guid.NewGuid().ToString();
        }
    }
	
}
