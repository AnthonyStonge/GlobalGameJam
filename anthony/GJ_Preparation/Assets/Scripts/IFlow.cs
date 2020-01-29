using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFlow 
{
    void PreInitialize();
    void Initialize();
    void Refresh();
    void PhysicsRefresh();
    void LateRefresh();
    void EndFlow();
}
