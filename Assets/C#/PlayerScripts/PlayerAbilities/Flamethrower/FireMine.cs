using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMine : MonoBehaviour {

    public GameObject explodeParticles;
    private GameObject sourcePlayer;
    private PlayerStats sourcePlayerStats;
    private Ability_FireMineSpawner ability;
    private bool justSpawned;

    public void StartFireMine(Ability_FireMineSpawner ability, float cooldown, PlayerStats sourcePlayerStats, bool hitSameTeam)
    {
        this.ability = ability;
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
        if (col.GetComponentInParent<IHittable>() == null)
            return;
        PlayerStats ps;
        if ((ps = col.GetComponentInParent<PlayerStats>()) != null && ps.gameObject == sourcePlayer.gameObject)
        {
            return;
        }
        else
        {
            Rigidbody r;
            if ((r = col.transform.GetComponent<Rigidbody>()) != null)
            {
                HitManager.HitClientside(new HitArguments(r.GetComponentInParent<BasePlayer>().gameObject, sourcePlayer).withDamage(ability.damage));
                if (explodeParticles)
                {
                    GameObject spawn = GameObject.Instantiate(explodeParticles, transform.position, Quaternion.identity);
                    Explosion e;
                    if ((e = spawn.GetComponent<Explosion>()))
                    {
                        e.sourcePlayer = sourcePlayer;
                    }
                }
                DestroyMe();
            }
        }
    }
}
