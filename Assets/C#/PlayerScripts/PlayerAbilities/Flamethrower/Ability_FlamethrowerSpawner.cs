using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_FlamethrowerSpawner : Ability_ObjectSpawner {

    public GameObject FlameThrowerEffect;
    private GameObject flameSpawned;
    public float lifetime = 1;
    public PlayerEffects.Effects effect;

    private float lastUsage = -100; // Last time we used it, in seconds;

    public override void OnSpellSpawned(GameObject spawn)
    {
        Flamethrower f;
        if (f = spawn.GetComponent<Flamethrower>())
        {
            f.sourcePlayer = this.gameObject;
            f.initEffect(flameSpawned);
        }

    }

    public override void SpawnSpell(PlayerComponent.Buf data)
    {
        Vector3 spawnAngle = data.vectorList[0];
        Vector3 spawnPosition = data.vectorList[1];


        // Spawn our spell in the place the server told us
        // However if we are the client, we don't wait for that luxury.
        GameObject spawn = GameObject.Instantiate(itemToSpawn, spawnPosition + transform.TransformDirection(spawnOffset), transform.rotation);
        Rigidbody r;

        Quaternion aimAngle = Quaternion.LookRotation(spawnAngle);
        flameSpawned = GameObject.Instantiate(FlameThrowerEffect, spawnPosition + transform.TransformDirection(spawnOffset), aimAngle);
        if (r = spawn.GetComponent<Rigidbody>())
        {
            r.AddForce(spawnAngle * spawnSpeed);
        }
        OnSpellSpawned(spawn);

        Destroy(flameSpawned, lifetime);
        PlayerStats sourcePlayerStats = GetComponentInParent<PlayerStats>();
        HitManager.HitClientside(new HitArguments(sourcePlayerStats.gameObject, sourcePlayerStats.gameObject).withEffect(effect).withEffectDuration(cooldown));
    }
    public override void use()
    {
        // Clients should not worry about magic draw
        if (Time.time - lastUsage > cooldown && !myBase.myMovement.isInAir())
        {
            if (!isLocalPlayer || myBase.myStats.canUseMagic(magicDraw))
            {
                if (abilityIcon != null && cooldown > 0.2f)
                { // We don't let it go if it takes like no time
                    this.abilityIcon.myAnimator.SetFloat("CooldownSpeed", 1 / cooldown);
                    string extra = UnityEngine.Random.Range(0.0f, 1.0f) < 0.001 ? "1" : ""; // ;)
                    this.abilityIcon.myAnimator.SetTrigger("Cooldown" + extra);
                }
				myBase.myStats.changeMagic(-1 * magicDraw);
				lastUsage = Time.time;
				hasNotified = false;
				use_UseAbility();
				switch (animationTriggerType) {
				case AnimationType.Trigger:
					this.myBase.myAnimator.SetTrigger(CAST_STRING);
					break;
				case AnimationType.Bool:
					this.myBase.myAnimator.SetBool(CAST_STRING, true);
					break;
				default:
					break;
				}
            }
            else
            {
                // Not enough magic
            }

        }
    }
}
