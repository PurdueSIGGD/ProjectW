using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class PlayerAbility : PlayerComponent {
    /*
     * For all abilities a player can have, we add them like a component to the player
     * Whenever the user wants to use them, we call the method "use"
     * In that use, you can do whatever you want. Call upon other components through base player, etc.
     * 
     * In order to preserve network behavior, used and hasUsed track the amount of times that it has been called
     * If a client notices used has changed, they execute the method and try again.
     */
    public float magicDraw; // Has to be handled in your player ability class. Can be total amt, can be magica per second, etc.
    private PlayerAbility[] myAbilities;
    public override void PlayerComponent_Start() {
        // We gather abilities here. If you want to add a new ability at runtime, you must run RepopulateAbilities()
        RepopulateAbilities();
        ability_Start();
    }
    public void RepopulateAbilities() {
        myAbilities = this.GetComponents<PlayerAbility>();
    }
    [SyncVar]
    int used;
    //public String abilityGUID = Guid.NewGuid().ToString(); // Unique ID assigned by server for component communication
    int hasUsed;
    /* it's a pain in the ass because there's a bug where two components of the same type can't get a command referenced to a specific one */
    /* so we pass the string of the GUID... because you can't pass abstract parameters like a player component */
    [Command]
    public void CmdUse(int index)
    {
        //print("player is telling us to use ability " + index + " " + myAbilities[index].ToString() + " " + myAbilities[index].magicDraw);
        myAbilities[index].useAbility();
        /*foreach (PlayerAbility myP in myAbilities) {
            if (myP.abilityGUID == p) {
                myP.useAbility();
            }
        }*/
    }
   
    public void useAbility() {
        used++;
    }
    public override void PlayerComponent_Update() {
        if (used != hasUsed) {
            //print(" I (" + this.ToString() + ") am using ability");
            //print("server has told us to use");
            hasUsed++;
            use();
        }
        //print(this + "used: " + used + " hasUsed: " + hasUsed);
        ability_Update();
    }
    void Death() {
        used = 0;
        hasUsed = 0;
    }
    /* these are the other methods you must implement. Can be empty, there for your own benefit */
    public abstract void ability_Start(); // Called when the object is alive
    public abstract void ability_Update(); // Called once every frame
    public abstract void use();  // Called when either your input or the server tells you to use these

}
