using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthPack : NetworkBehaviour {
    private ArrayList toDamage;
    public HitArguments.DamageType type;
    public float Health;
    public float respawnTimer;
    [SyncVar]
    private bool active = true;
    
    /*
    TODO: When player joins while health pack is despawned, client side timers are off.
    */
    void Start()
    {
        toDamage = new ArrayList();
        //active = true;
    }
    private void Update() {
        GetComponent<MeshRenderer>().enabled = active; // should update with server
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.isTrigger) return;
        IHittable h;
        if ((h = col.GetComponent<IHittable>()) != null)
        {
            toDamage.Add(h);
            StartCoroutine(DamagingBehavior(h));
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.isTrigger) return;
        IHittable h;
        if ((h = col.GetComponent<IHittable>()) != null)
        {
            toDamage.Remove(h);
        }
    }

    public IEnumerator DamagingBehavior(IHittable h)
    {
        do
        {
            // Local damage only
            if ((Component)h != null && ((Component)h).gameObject != null)
            {
                //Test if health pack is there to give health and that the player isn't at full health
                if (active && ((Component)h).GetComponentInParent<PlayerStats>().health != ((Component)h).GetComponentInParent<PlayerStats>().healthMax)
                {
                    //Hit target with negative damage
                    h.Hit(new HitArguments(this.gameObject, ((Component)h).gameObject)
                        .withDamage(-1 * Health)
                        .withDamageType(type));
                    //Remove health pack
                    CmdSetActive(false);
                }
                
                yield return new WaitForSeconds(0);
            }
        } while (h != null && toDamage.Contains(h));
    }
    [Command]
    void CmdSetActive(bool desired) {
        active = desired;
        RpcActiveSet(desired);
        //Start respawning it
        if (!desired) StartCoroutine(RespawnBehavior());
    }
    [ClientRpc]
    void RpcActiveSet(bool desired) {
        // idk is this needed?
    }
    public IEnumerator RespawnBehavior()
    {
        do {
            yield return new WaitForSeconds(respawnTimer);
            //Respawn health pack
            CmdSetActive(true);
        } while (active == false);
    }
}
