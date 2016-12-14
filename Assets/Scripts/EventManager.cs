using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ProjectileCreation : UnityEvent<ProjectileInfoPack>{
    ProjectileInfoPack information;
}

public class SuperMoveAbility : UnityEvent<GameObject>
{
    GameObject superMoveUser;
}

public class SpawnCharacters : UnityEvent<SpawningInformation> {
    SpawningInformation spawnInformation;
}


public class EventManager : MonoBehaviour {

    private static EventManager inst;
    public static EventManager instance
    {
        get
        {
            if (inst == null)
            {
                var newEventManager = new GameObject("EventManager");
                inst = newEventManager.AddComponent<EventManager>();
            }

            return inst; 
        }
    }

    public ProjectileCreation OnProjectileCreation = new ProjectileCreation();
    public SuperMoveAbility OnSuperMove = new SuperMoveAbility();
    public SpawnCharacters OnSpawnCharacter = new SpawnCharacters();

    void Awake()
    {
        if(inst != null)
        {
            DestroyImmediate(this);
            return;
        }

        inst = this;
    }
}
