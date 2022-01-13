using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Character
{
    // Fireball
    public string action1name;
    public int action1damage;
    public PathAreaTypeEnum action1areaType;
    private int[] action1area = new int[] { 0, 1, 3, 4, 5, 6, 1 };

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

    private void Start()
    {
        currentHealth = maxHealth;
        Actions = new List<Action>();

        // create Fireball attack
        Action action1 = new Attack(action1name, action1damage, element, action1areaType, action1area);
        Actions.Add(action1);

        // create Combust combo
        movement2 = new Movement(2, element, PathAreaTypeEnum.SPOKES);
        Action action2 = new Combo(attack2name, attack2damage, movement2, element, attack2areaType, attack2area);
        Actions.Add(action2);

        // create Propulsive Blast combo
        movement3 = new Movement(3, element, PathAreaTypeEnum.SPOKES);
        Action action3 = new Combo(attack3name, attack3damage, movement3, element, attack3areaType, attack3area);
        Actions.Add(action3);

        // create Walk movement
        Action action4 = new Movement(action4name, mobility, element, action4areaType, action4area);
        Actions.Add(action4);
    }
}
