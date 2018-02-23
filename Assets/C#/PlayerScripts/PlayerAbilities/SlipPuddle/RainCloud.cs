using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainCloud : MagicObject{

   
    public float damage;    // damage amount
    public float heal;      // heal amount

    public float dmgRate;   // in seconds
    public float healRate;  // in seconds
    public HitArguments.DamageType damageType;


    private int teamIndex;    // team index
    private ArrayList toAffect; // list of players to affect: damage, heal


    private void Start()
    {
        // get team number
        if (sourcePlayer == null)
        {
            Debug.LogWarning(gameObject.name + ": sourcePlayer is null.");
        }
        teamIndex = sourcePlayer.GetComponent<PlayerStats>().teamIndex;
        toAffect = new ArrayList();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.isTrigger) return;

        // if can be hit
        IHittable h;
        int colTeam;
        if ( (h = col.GetComponent<IHittable>()) != null)
        {
            // get col Team
            toAffect.Add(h);
            colTeam = col.GetComponent<PlayerStats>().teamIndex;

            // if same team
            if (teamIndex == colTeam)
                StartCoroutine(RoutineHeal(h));

            // if diff team
            else
                StartCoroutine(RoutineDamage(h));
        }

    }


    private IEnumerator RoutineDamage(IHittable h)
    {
        do
        {
            HitManager.HitClientside(HitManager.HitVerificationMethod.melee,
                new HitArguments(((Component)h).gameObject, sourcePlayer.GetComponentInParent<PlayerStats>().gameObject)
              .withDamage(damage)
              .withDamageType(damageType));

            yield return new WaitForSeconds(dmgRate);
        }
        while (h != null && toAffect.Contains(h));
        
    }

    private IEnumerator RoutineHeal(IHittable h)
    {
        do
        {
            HitManager.HitClientside(HitManager.HitVerificationMethod.melee,
                new HitArguments(((Component)h).gameObject, sourcePlayer.GetComponentInParent<PlayerStats>().gameObject)
              .withDamage( -1 * heal )
              .withDamageType(damageType));

            yield return new WaitForSeconds(healRate);
        }
        while (h != null && toAffect.Contains(h));

    }
}
