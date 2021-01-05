using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureManager : MonoBehaviour
{
    public static CreatureManager SharedInstance;

    private List<EnemyBase> CreatureList = new List<EnemyBase>();
    
    void Awake()
    {
        SharedInstance = this;
    }
    
    void Update()
    {
        
    }

    public void RetargetCreaturesByUsername(string username, Transform target)
    {
        for (int i = 0; i < CreatureList.Count; i++)
        {
            EnemyBase creature = CreatureList[i];
            if (creature.PlayerName == username)
            {
                creature.SetAttackTarget(target);
            }
        }
    }

    public void AddCreature(EnemyBase creature)
    {
        CreatureList.Add(creature);
    }

    public void RemoveCreature(EnemyBase creature)
    {
        CreatureList.Remove(creature);
    }
}
