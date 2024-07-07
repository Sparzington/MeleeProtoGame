using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFighter
{
    public void LightAttack(bool bash);
    public void HeavyAttack();
    public void Block();

}
