using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INavigation
{
    public void InitAgent();
    public void Stop();
    public void MoveTowards(Vector3 newPos);
}
