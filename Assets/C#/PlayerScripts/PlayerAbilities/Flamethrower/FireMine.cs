using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMine : MonoBehaviour {

    public GameObject explodeParticles;
    private GameObject sourcePlayer;
    private PlayerStats sourcePlayerStats;
    private bool justSpawned;
    private bool hasHit;

    public void StartFireMine(float cooldown, PlayerStats sourcePlayerStats, bool hitSameTeam)
    {
        this.sourcePlayerStats = sourcePlayerStats;
        this.sourcePlayer = sourcePlayerStats.gameObject;
        sourcePlayerStats.numberOfSummonedObjects++;
        justSpawned = true;
    }

    void Update()
    {
        if (sourcePlayerStats.numberOfSummonedObjects > 1 || sourcePlayerStats.death)
        {
            if (!justSpawned)
            {
                Invoke("DestroyMe", 0);
            }
        }
        if (justSpawned && sourcePlayerStats.numberOfSummonedObjects <= 1)
        {
            justSpawned = false;
        }
    }

    void DestroyMe()
    {
        sourcePlayerStats.numberOfSummonedObjects--;
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (hasHit) { return; }
        PlayerStats ps;
        if ((ps = col.GetComponentInParent<PlayerStats>()) != null && ps.gameObject == sourcePlayer.gameObject)
        {
            return;
        }
        if (col.transform.GetComponent<Rigidbody>() != null && col.GetComponentInParent<IHittable>() != null)
        {
            hasHit = true;
            sourcePlayerStats.numberOfSummonedObjects--;
            if (explodeParticles)
            {
                GameObject spawn = GameObject.Instantiate(explodeParticles, transform.position, Quaternion.identity);
                Explosion e;
                if ((e = spawn.GetComponent<Explosion>()))
                {
                    e.sourcePlayer = sourcePlayer;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
