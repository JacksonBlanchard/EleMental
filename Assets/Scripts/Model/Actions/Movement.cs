using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : Action
{
    // nameless movements are "forces"
    public Movement(int mobility, ElementEnum element, PathAreaTypeEnum pathAreaType)
    {
        Distance = mobility;
        Element = element;
        PathAreaType = pathAreaType;
        State = ActionStateEnum.IDLE;
    }

    public Movement(string name, int mobility, ElementEnum element, PathAreaTypeEnum pathAreaType, int[] pathArea)
    {
        Name = name;
        Distance = mobility;
        Element = element;
        PathAreaType = pathAreaType;
        PathArea = pathArea;
        State = ActionStateEnum.IDLE;
    }
}
