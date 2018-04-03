using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bamboo : MonoBehaviour {

    public GameObject sourcePlayer;
    Ability_BambooSpawner ability;
    public PlayerEffects.Effects effect;
    public float effectDuration = 3;
    private ArrayList hasHit;
    private bool hitSameTeam;

    public void StartBamboo(Ability_BambooSpawner ability, float cooldown, GameObject sourcePlayer, bool hitSameTeam)
    {
        this.ability = ability;
        this.sourcePlayer = sourcePlayer;
        this.hitSameTeam = hitSameTeam;
        hasHit = new ArrayList();
        Invoke("DestroyMe", cooldown);
    }

    void DestroyMe()
    {
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
                    HitManager.HitClientside(new HitArguments(r.GetComponentInParent<BasePlayer>().gameObject, sourcePlayer)
                        .withDamage(ability.damage)
                        .withEffect(effect)
                        .withEffectDuration(effectDuration)
                        .withHitSameTeam(hitSameTeam));
                }
            }
        }
    }
}
