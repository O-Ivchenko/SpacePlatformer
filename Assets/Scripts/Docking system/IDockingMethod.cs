using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDockingMethod
{
    void Setup(GameObject parent, Vector3 position);
    void DestroyObject();
}
