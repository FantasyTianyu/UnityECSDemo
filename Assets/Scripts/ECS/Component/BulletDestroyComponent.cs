using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[Serializable]
public struct BulletDestroy : IComponentData
{
    public float Value;
}

public class BulletDestroyComponent : ComponentDataWrapper<BulletDestroy>
{

}
