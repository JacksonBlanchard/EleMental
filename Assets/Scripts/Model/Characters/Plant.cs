using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Character
{
    public string attack1name;
    public int attack1damage;
    public PathAreaTypeEnum attack1areaType;
    private int[] attack1area = new int[] { 0, 1, 0, 3, 0, 4, 0, 5, 0, 6, 0, 1, 0 };

    public string attack2name;
    public int attack2damage;
    public PathAreaTypeEnum attack2areaType;
    private int[] attack2area = new int[] { 0, 1, 0, 3, 0, 4, 0, 5, 0, 6, 0, 1, 0 }; // same area as attack 1;

    public string attack3name;
    public int attack3damage;
    public PathAreaTypeEnum attack3areaType;
    private int[] attack3area = new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 6, 0, 3, 2, 0, 5, 1, 0 };

    private void Start()
    {
        currentHealth = maxHealth;
        Actions = new List<Action>();

        // create attack 1
        Action attack1 = new Action(attack1name, attack1damage, element, attack1areaType, attack1area);
        Actions.Add(attack1);

        // create attack 2
        Action attack2 = new Action(attack2name, attack2damage, element, attack2areaType, attack2area);
        Actions.Add(attack2);

        // create attack 3
        Action attack3 = new Action(attack3name, attack3damage, element, attack3areaType, attack3area);
        Actions.Add(attack3);
    }
}
