using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class BulletDestroySystem : ComponentSystem
{
    struct BulletDestroyGroup
    {
        public readonly int Length;
        public ComponentDataArray<BulletDestroy> bulletDestroyData;
        public EntityArray entities;

    }
    [Inject] BulletDestroyGroup group;
    protected override void OnUpdate()
    {

        List<Entity> needToDestroy = new List<Entity>();
        for (int i = 0; i < group.Length; i++)
        {

            float destroyTimer = group.bulletDestroyData[i].Value;
            destroyTimer -= Time.deltaTime;
            group.bulletDestroyData[i] = new BulletDestroy { Value = destroyTimer };
            if (group.bulletDestroyData[i].Value <= 0)
            {
                needToDestroy.Add(group.entities[i]);
            }
        }

        for (int i = 0; i < needToDestroy.Count; i++)
        {
            EntityManager entityManager = World.Active.GetOrCreateManager<EntityManager>();
            entityManager.DestroyEntity(needToDestroy[i]);
        }

    }
}
