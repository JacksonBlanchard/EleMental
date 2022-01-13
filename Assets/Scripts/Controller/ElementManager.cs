using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementEnum {
    FIRE, WATER, EARTH, AIR,
    LIGHTNING, ICE, METAL, PLANT, POISON,
    LIGHT, DARK, MIND, SPIRIT,
    NEUTRAL // must be last because type chart is indexed by 
}

public enum ChangeEnum { 
    NEUTRAL, WEAK, STRONG,
    NEUTRALIZE1, SHRINK1, SPREAD1,
    NEUTRALIZE2, SHRINK2, SPREAD2,
    BLOCK, CONTIGUOUS
}

public class ElementManager : MonoBehaviour
{
    // TODO: Use relative file pathing
    //string path = Resources.Load<("Resources/Element_Matchup_Chart.csv");
    string path = @"D:\Unity\EleMental\Resources\Element_Matchup_Chart.csv";
    public int[,] elementMatchupChart = new int[13, 13]; // hardcode for now;

    public Color playerColor = Color.grey;
    public Color neutral = Color.white;
    public Color fire = Color.red;
    public Color water = Color.blue;
    public Color plant = Color.green;
    public Color lightning = Color.yellow;

    private void Start()
    {
        // throw exception if we can't find the type matchup file
        if (!File.Exists(path))
            throw new System.Exception("CANNOT FIND " + path);

        // open and parse the file for the ChangeEnums
        using (StreamReader sr = File.OpenText(path))
        {
            string s;
            int line_num = 0;
            while ((s = sr.ReadLine()) != null)
            {
                string[] changes = s.Split(',');
                for (int i = 0; i < changes.Length; i++)
                {
                    switch (changes[i])
                    {
                        case "Neutral":
                            elementMatchupChart[line_num, i] = (int)ChangeEnum.NEUTRAL;
                            break;
                        case "Neutralize 1":
                            elementMatchupChart[line_num, i] = (int)ChangeEnum.NEUTRALIZE1;
                            break;
                        case "Neutralize 2":
                            elementMatchupChart[line_num, i] = (int)ChangeEnum.NEUTRALIZE2;
                            break;
                        case "Weak":
                            elementMatchupChart[line_num, i] = (int)ChangeEnum.WEAK;
                            break;
                        case "Shrink 1":
                            elementMatchupChart[line_num, i] = (int)ChangeEnum.SHRINK1;
                            break;
                        case "Shrink 2":
                            elementMatchupChart[line_num, i] = (int)ChangeEnum.SHRINK2;
                            break;
                        case "Strong":
                            elementMatchupChart[line_num, i] = (int)ChangeEnum.STRONG;
                            break;
                        case "Spread 1":
                            elementMatchupChart[line_num, i] = (int)ChangeEnum.SPREAD1;
                            break;
                        case "Spread 2":
                            elementMatchupChart[line_num, i] = (int)ChangeEnum.SPREAD2;
                            break;
                        case "Block":
                            elementMatchupChart[line_num, i] = (int)ChangeEnum.BLOCK;
                            break;
                        case "Contiguous":
                            elementMatchupChart[line_num, i] = (int)ChangeEnum.CONTIGUOUS;
                            break;
                        default:
                            throw new System.Exception("Unrecognized type matchup " + changes[i] + " at " + line_num + "," + i);
                    }
                }
                line_num++;
            }
        }
    }

    public int NumElements { get { return elementMatchupChart.Length; } }

    public Color GetColor(ElementEnum element)
    {
        // TODO: change these to raw color rgb values? (if the variables aren't used anywhere else)
        switch(element)
        {
            case ElementEnum.FIRE:
                return fire;
            case ElementEnum.WATER:
                return water;
            case ElementEnum.PLANT:
                return plant;
            case ElementEnum.LIGHTNING:
                return lightning;
            default:
                return neutral;
        }
    }

    public ChangeEnum GetChange(ElementEnum currType, ElementEnum atkType)
    {
        if (currType == ElementEnum.NEUTRAL)
            return ChangeEnum.STRONG;
        else
            return (ChangeEnum)elementMatchupChart[(int)atkType,(int)currType];
    }

    // TODO: redo the use of this method in CharacterSelection
    public int GetChange(int atkType, int defType)
    {
        if ((ElementEnum)defType == ElementEnum.NEUTRAL)
            return 2;
        else
            return elementMatchupChart[atkType, defType];
    }
}
