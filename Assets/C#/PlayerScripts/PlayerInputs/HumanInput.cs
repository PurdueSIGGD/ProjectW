using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanInput : PlayerInput {
    public float sensitivityy = 0.05f;
	public bool AI = true;
	public bool multTeams = false;
	private int[,] botHorizontals;

	public override void PlayerComponent_Start() {
		base.PlayerComponent_Start ();
		if (isServer) {
			ProjectWGameManager manager = FindObjectOfType<ProjectWGameManager> ();
			botHorizontals = new int[manager.networkManager.botItems.Length,2];
			if (manager.teams.Length > 1) {
				multTeams = true;
			}
		}
	}

	public override PlayerInput.InputData getData() {
		if (isBot()) {
			return AIData ();

		} else {
			InputData data = new InputData ();
			data.horizontal = Input.GetAxis ("Horizontal");
			data.vertical = Input.GetAxis ("Vertical");
			data.mouseX = Input.GetAxis ("Mouse X") * sensitivityy;
			data.mouseY = Input.GetAxis ("Mouse Y") * sensitivityy;
			data.pause = Input.GetAxis ("Pause") > 0;
			data.jump = Input.GetAxis ("Jump") > 0;
			data.scoreboard = Input.GetAxis ("Scoreboard") > 0;
			data.useAbilities = new bool[ABILITY_INPUTS];
			// ability list starts at 1
			for (int i = 1; i <= PlayerInput.ABILITY_INPUTS; i++) {
				data.useAbilities [i - 1] = Input.GetAxis ("Ability" + i) > 0;
			}
			data.melee = Input.GetAxis ("Melee") > 0;
			return data;
		}
    }

	public PlayerInput.InputData AIData()
	{
		
			InputData data = MoveData ();
			data.scoreboard = false;
			data.useAbilities = triggerAttack ();
			data.melee = false;
			
			return data;

	}

	public PlayerInput.InputData MoveData()
	{
		InputData data = new InputData();
		data.horizontal = 0;
		data.vertical = 0;
		data.mouseX = 0f * sensitivityy;
		data.mouseY = 0 * sensitivityy;
		data.pause = false;
		data.jump = false;

		Collider[] closeby = Physics.OverlapSphere (cameraSlider.position, 20); //detect nearby enemy
		Vector3? targetPosition = null;
		foreach (Collider c in closeby) {
			PlayerStats tmpSts;
			if (tmpSts = c.transform.GetComponentInParent<PlayerStats> ()) {
				if (ignorePlayer (tmpSts))
					continue;



				if (targetPosition.HasValue) {
					if (Vector3.Distance(cameraSlider.position, (Vector3)targetPosition) > Vector3.Distance(cameraSlider.position, c.transform.position)) {
						targetPosition = c.transform.position;
					}
				}
				else {
					targetPosition = c.transform.position;
				}
			}
		}

		if (targetPosition.HasValue) {
			
			data.mouseX = -40 * Vector3.SignedAngle ((Vector3)targetPosition - cameraSlider.position, cameraSlider.forward, Vector3.up) * sensitivityy;

			data.mouseY = 10 * Vector3.SignedAngle ((Vector3)targetPosition - cameraSlider.position, cameraSlider.forward, Vector3.Cross (cameraSlider.forward, Vector3.up)) * sensitivityy;
			double dist = Vector3.Distance (cameraSlider.position, (Vector3)targetPosition);
			if (dist > 4) {
				data.vertical = 1f;
			} else if (dist < 3) {
				data.vertical = -1f;
			}
			System.Random rnd = new System.Random ();
			botHorizontals [getBot () - 1,0] += rnd.Next (-1, 2);
			if (botHorizontals [getBot () - 1,0] > 10) {
				botHorizontals [getBot () - 1,0] = 10;

			} else if (botHorizontals [getBot () - 1,0] < -10) {
				botHorizontals [getBot () - 1,0] = -10;
			}
				

			if (botHorizontals [getBot () - 1,0] > 7) {
				botHorizontals [getBot () - 1,1] = 1;

			} else if (botHorizontals [getBot () - 1,0] < -7) {
				botHorizontals [getBot () - 1,1] = -1;

			} else if (botHorizontals [getBot () - 1,0] > -3 && botHorizontals [getBot () - 1,0] < 3) {
				botHorizontals [getBot () - 1,1] = 0;
			}
			data.horizontal = botHorizontals [getBot () - 1,1];

		}


		if (Physics.SphereCast(new Ray(transform.position + Vector3.up * .15f, transform.forward), .1f, 4, 1<<11))
		{
			if (!Physics.SphereCast (new Ray (transform.position + Vector3.up * 2f, transform.forward), .1f, 4f, 1 << 11)) { 
				data.jump = true;//ledge detected
			}
		}
		return data;
	}

	public bool[] triggerAttack()
	{
		bool[] useAbilities = new bool[ABILITY_INPUTS];
		// ability list starts at 1
		for (int i = 1; i <= PlayerInput.ABILITY_INPUTS; i++) {
			useAbilities[i - 1] = false;
		}

		//Difficulty check






		Collider[] closeby = Physics.OverlapSphere (cameraSlider.position, 3.5f); //detect nearby enemy and flamefart
		foreach (Collider c in closeby) {
			PlayerStats tmpSts;
			if (tmpSts = c.transform.GetComponentInParent<PlayerStats> ()) {
				if (ignorePlayer (tmpSts))
					continue;

				//useAbilities[1] = true;
				break;
			}
		}

		RaycastHit[] hits = Physics.RaycastAll (cameraSlider.position, cameraSlider.forward, 15); // detect enemy in front and fireball
		foreach (RaycastHit h in hits) {
			if (h.collider.isTrigger) {
				continue;
			}
			if (h.transform.gameObject.layer == 1 << 11) {
				break;
			}
			PlayerStats tmpSts;
			if (tmpSts = h.transform.GetComponentInParent<PlayerStats> ()) {
				if (ignorePlayer (tmpSts))
					continue;

				//if (tmpSts.classIndex != 1) {//will fireball if target is not a time wizard
					useAbilities [0] = true;
				//}


			} else {
				continue;
			}


			//print ("overriding with object: " + h.transform);

			break;
		}

		return useAbilities;

	}

	private bool ignorePlayer(PlayerStats tmpSts)
	{
		return tmpSts.gameObject == this.gameObject
			|| tmpSts.death 
			|| (multTeams && tmpSts.teamColor == this.myBase.myStats.teamColor);
	}
}
