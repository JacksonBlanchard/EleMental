using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionStateEnum { IDLE, PLANNING, READY }
public enum PathAreaTypeEnum { FIXED, FIXED_RANDOM, SELF, SPOKES, STRAIGHT_LINE_SOLID, STRAIGHT_LINE_SPACED, FREE_FORM, ROTATE_AROUND, TYPED_GLOBAL, TRUE_GLOBAL }
public enum ActsOnEnum { SELF, ALLY, ENEMY }

public class Action
{
    public string Name { get; set; }
    public int Distance { get; set; }
    public ElementEnum Element { get; set; }
    public PathAreaTypeEnum PathAreaType { get; set; }
    public int[] PathArea { get; set; }
    public HexTile StartTile { get; set; }
    public ActionStateEnum State { get; set; }

    // Constructors //
    protected Action() { }

    public Action(string name, ElementEnum element, PathAreaTypeEnum pathAreaType, int[] area)
    {
        Name = name;
        Element = element;
        PathAreaType = pathAreaType;
        PathArea = area;
        State = ActionStateEnum.IDLE;
    }

    public Action(string name, int distance, ElementEnum element, PathAreaTypeEnum pathAreaType, int[] area)
    {
        Name = name;
        Distance = distance;
        Element = element;
        PathAreaType = pathAreaType;
        PathArea = area;
        State = ActionStateEnum.IDLE;
    }

    // MIGHT NOT WORK WITH NEW DirectionEnums
    public void RotatePathArea(DirectionEnum dir)
    {
        // only rotate if needed
        if (dir != DirectionEnum.SELF)
        {
            int offset = 0;

            for (int i = 0; i < PathArea.Length; i++)
            {
                // init offset with first non-zero Direction
                if (PathArea[i] != 0)
                {
                    offset = (int)dir - PathArea[i];
                    break;
                }
            }

            for (int i = 0; i < PathArea.Length; i++)
            {
                // add the offset to every Direction in the path
                if (PathArea[i] != 0)
                {
                    PathArea[i] += offset;
                    if (PathArea[i] <= 0)
                        PathArea[i] += 6;
                    if (PathArea[i] > 6)
                        PathArea[i] -= 6;
                }
            }
        }
    }

    public void AddToPathArea(DirectionEnum dir)
    {
        // dont add extra 0s
        if (PathArea.Length > 0 && PathArea[PathArea.Length-1] == 0 && (int)dir == 0)
            return;
        // create longer array and copy contents over
        int[] newPathArea = new int[PathArea.Length + 1];
        for(int i = 0; i < PathArea.Length; i++)
            newPathArea[i] = PathArea[i];
        newPathArea[PathArea.Length] = (int)dir;
        PathArea = newPathArea;
    }

    public void ShrinkPathArea()
    {
        if (PathArea.Length == 1)
            return;

        int[] newPathArea = new int[PathArea.Length - 1];
        for(int i = 0; i < PathArea.Length - 1; i++)
            newPathArea[i] = PathArea[i];
        PathArea = newPathArea;
    }

    public void ReplaceLastDir(DirectionEnum dir)
    {
        if (PathArea.Length > 0)
            PathArea[PathArea.Length - 1] = (int)dir;
    }

    public void FixedRandomPathArea()
    {
        // TODO: implement
    }

    // remove all but first direction
    public void ResetPathArea()
    {
        PathArea = new int[1] { PathArea[0] };
    }

    public bool CanGoFurther()
    {
        return PathArea.Length <= Distance;
    }
}
