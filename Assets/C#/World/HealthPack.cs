using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthPack : MonoBehaviour{
    private ArrayList toDamage;
    public HitArguments.DamageType type;
    public float Health;
    public float respawnTimer;
    private bool active;
    /*
    TODO: When player joins while health pack is despawned, client side timers are off.
    */
    void Start()
    {
        toDamage = new ArrayList();
        active = true;
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
                //Test if health pack is there to give health
                if (active)
                {
                    //Hit target with negative damage
                    h.Hit(new HitArguments(this.gameObject, ((Component)h).gameObject)
                        .withDamage(-1 * Health)
                        .withDamageType(type));
                    //Remove health pack
                    active = false;
                    GetComponent<MeshRenderer>().enabled = false;
                    //Start respawning it
                    StartCoroutine(RespawnBehavior());
                }
                
                yield return new WaitForSeconds(0);
            }
        } while (h != null && toDamage.Contains(h));
    }
    
    public IEnumerator RespawnBehavior()
    {
        do {
            yield return new WaitForSeconds(respawnTimer);
            //Respawn health pack
            active = true;
            GetComponent<MeshRenderer>().enabled = true;
        } while (active == false);
    }
}
