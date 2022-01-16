using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Elementor
{
    public string attack1name;
    public int attack1damage;
    public PathAreaTypeEnum attack1areaType;
    private int[] attack1area = new int[] { 0, 1, 3, 4, 5, 6, 1 };

    public string attack2name;
    public int attack2damage;
    public int attack2distance;
    public PathAreaTypeEnum attack2areaType;
    private int[] attack2area = new int[] { 0 };

    public string attack3name;
    public int attack3damage;
    public PathAreaTypeEnum attack3areaType;
    private int[] attack3area = new int[] { 1 };

    public string a4name;
    public PathAreaTypeEnum a4AreaType;
    private int[] a4area = new int[] { 1 };
}
