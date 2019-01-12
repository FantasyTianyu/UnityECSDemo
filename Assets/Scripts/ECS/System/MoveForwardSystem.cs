using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class MoveForwardSystem : ComponentSystem
{
    struct Group
    {
        public readonly int Length;
        public ComponentDataArray<Position> positionData;
        public ComponentDataArray<Rotation> rotationData;
    }
    [Inject] Group group;
    protected override void OnUpdate()
    {
        for (int i = 0; i < group.Length; i++)
        {
            float3 position = group.positionData[i].Value;
            position += Time.deltaTime * 20 * math.forward(group.rotationData[i].Value);
            group.positionData[i] = new Position { Value = position };
        }

    }
}
