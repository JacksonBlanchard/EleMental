using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo : Action
{
    public int damage { get; set; }
    public Movement movement { get; set; }

    public Combo(string name, int damage, Movement movement, ElementEnum element, PathAreaTypeEnum pathAreaType, int[] area)
    {
        Name = name;
        this.damage = damage;
        this.movement = movement;
        Element = element;
        PathAreaType = pathAreaType;
        PathArea = area;
        State = ActionStateEnum.IDLE;
    }
}
