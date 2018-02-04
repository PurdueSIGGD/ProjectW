using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bamboo : MonoBehaviour {

    public GameObject sourcePlayer;
    Ability_BambooSpawner ability;
    public PlayerEffects.Effects effect;

    public void StartBamboo(Ability_BambooSpawner ability, float cooldown, GameObject sourcePlayer)
    {
        this.ability = ability;
        this.sourcePlayer = sourcePlayer;
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
                if (ps)
                {
                    HitManager.HitClientside(new HitArguments(r.GetComponentInParent<BasePlayer>().gameObject, sourcePlayer).withDamage(ability.damage).withEffect(effect));
                }
            }
        }
    }
}
