using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Elementor
{
    // Combust
    public string attack2name;
    public int attack2damage;
    public PathAreaTypeEnum attack2areaType;
    int[] attack2area = new int[] { 0, 1, 3, 4, 5, 6, 1, 1, 2, 3, 3, 4, 4, 5, 5, 6, 6, 1, 1, 2 };
    Movement movement2;

    // Propulsive Blast
    public string attack3name;
    public int attack3damage;
    public PathAreaTypeEnum attack3areaType;
    int[] attack3area = new int[] { 1, 1, 1, 2, 11, 6, 9, 1, 2, 11, 6, 9, 1, 2, 11, 6, 9 };
    Movement movement3;

    // Walk
    public string action4name;
    public PathAreaTypeEnum action4areaType;
    int[] action4area = new int[] { -1 };
}
