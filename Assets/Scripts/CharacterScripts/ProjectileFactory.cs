using UnityEngine;
using System.Collections;

public class ProjectileFactory : MonoBehaviour {

    public GameObject projectile;

	// Use this for initialization
	void Start () {
        EventManager.instance.OnProjectileCreation.AddListener((ProjectileInfoPack information) =>
        {
            //Instantiate as Gameobject to alter projectile variables
            if(information.thisObject == this.gameObject)
            {
                GameObject thisProj = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
                thisProj.GetComponent<Projectile>().SetInformation(information);
                Debug.Log("Call Set Info");
                thisProj.GetComponent<Projectile>().StartManual();

                GetComponent<TwoDimensionalCharacterController>().myState = TwoDimensionalCharacterController.PlayerStates.psProjectileRecovery;
            }
        });
	}
}
