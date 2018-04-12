using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTrap : MonoBehaviour {

    private GameObject sourcePlayer;
    private PlayerStats sourcePlayerStats;
    private bool hitSameTeam;
    Ability_GrassTrapSpawner ability;
    public PlayerEffects.Effects effect;
    private float effectDuration = 3;
    private ArrayList hasHit;
    private bool justSpawned;

    public void StartGrassTrap(Ability_GrassTrapSpawner ability, float effectDuration, float cooldown, PlayerStats sourcePlayerStats, bool hitSameTeam)
    {
        this.sourcePlayerStats = sourcePlayerStats;
        sourcePlayer = sourcePlayerStats.gameObject;
        sourcePlayerStats.numberOfSummonedObjects++;
        this.ability = ability;
        this.effectDuration = effectDuration;
        this.hitSameTeam = hitSameTeam;
        hasHit = new ArrayList();
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
        if(justSpawned && sourcePlayerStats.numberOfSummonedObjects <= 1)
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
                if (ps && !hasHit.Contains(ps))
                {
                    if (!hitSameTeam && ps.teamIndex == sourcePlayer.GetComponent<PlayerStats>().teamIndex && ps.teamIndex != -1) return; // dont hit players on same team

                    hasHit.Add(ps);
                    HitManager.HitClientside(new HitArguments(r.GetComponentInParent<BasePlayer>().gameObject, sourcePlayer).withDamage(ability.damage).withEffect(effect).withEffectDuration(effectDuration).withHitSameTeam(hitSameTeam));
                    if(hasHit.Count <= 1)
                    {
                        Invoke("DestroyMe", effectDuration);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider col)
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
            if ((col.transform.GetComponent<Rigidbody>()) != null)
            {
                if (ps && hasHit.Contains(ps))
                {
                    Effect activeEffect;
                    if((activeEffect = ps.GetComponentInChildren<Effect>()) && activeEffect.effectType == effect)
                    {
                        activeEffect.Effect_End(ps.GetComponent<PlayerEffects>());
                    }
                }
            }
        }
    }
}
