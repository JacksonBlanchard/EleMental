using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Action
{
    public int damage { get; }

    public Attack(string name, int damage, ElementEnum element, PathAreaTypeEnum pathAreaType, int[] area) : 
        base(name, element, pathAreaType, area)
    {
        this.damage = damage;
    }

    public Attack(string name, int damage, int distance, ElementEnum element, PathAreaTypeEnum pathAreaType, int[] area) :
        base(name, distance, element, pathAreaType, area)
    {
        this.damage = damage;
    }
}
