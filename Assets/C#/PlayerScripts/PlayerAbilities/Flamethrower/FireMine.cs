using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMine : MonoBehaviour {

    public GameObject sourcePlayer;
    private Ability_FireMineSpawner ability;

    public void StartFireMine(Ability_FireMineSpawner ability, float cooldown, GameObject sourcePlayer)
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
                HitManager.HitClientside(new HitArguments(r.GetComponentInParent<BasePlayer>().gameObject, sourcePlayer).withDamage(ability.damage));
                DestroyMe();
            }
        }
    }
}
