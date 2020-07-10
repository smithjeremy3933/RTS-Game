using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    void MoveTo(Vector3 destination, float speedFraction);
    bool IsSelected();
}
