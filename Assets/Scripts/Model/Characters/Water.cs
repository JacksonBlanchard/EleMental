using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Character
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

    private void Start()
    {
        currentHealth = maxHealth;
        Actions = new List<Action>();

        // create action 1
        Action a1 = new Attack(attack1name, attack1damage, element, attack1areaType, attack1area);
        Actions.Add(a1);

        // create attack 2
        Action a2 = new Attack(attack2name, attack2damage, attack2distance, element, attack2areaType, attack2area);
        Actions.Add(a2);

        // create action 3
        Action a3 = new Attack(attack3name, attack3damage, ElementEnum.ICE, attack3areaType, attack3area);
        Actions.Add(a3);

        // create action 4
        Action a4 = new Movement(a4name, mobility, element, a4AreaType, a4area);
        Actions.Add(a4);
    }
}
