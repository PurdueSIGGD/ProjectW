using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTrap : MonoBehaviour {

    public GameObject sourcePlayer;
    Ability_GrassTrapSpawner ability;
    public PlayerEffects.Effects effect;
    private float effectDuration = 3;
    private ArrayList hasHit;

    public void StartGrassTrap(Ability_GrassTrapSpawner ability, float effectDuration, float cooldown, GameObject sourcePlayer)
    {
        this.ability = ability;
        this.sourcePlayer = sourcePlayer;
        this.effectDuration = effectDuration;
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
                    hasHit.Add(ps);
                    HitManager.HitClientside(new HitArguments(r.GetComponentInParent<BasePlayer>().gameObject, sourcePlayer).withDamage(ability.damage).withEffect(effect).withEffectDuration(effectDuration));
                    if(hasHit.Count <= 1)
                    {
                        Invoke("DestroyMe", effectDuration);
                    }
                }
            }
        }
    }
}
