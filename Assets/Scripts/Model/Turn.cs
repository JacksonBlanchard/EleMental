using System;
using System.Collections.Generic;
using UnityEngine;

public class Turn
{
    // ordered by speed
    public List<Character> Characters { get; }

    public Turn()
    {
        Characters = new List<Character>();
    }

    public void ResetTurn()
    {
        Characters.Clear();
    }

    public void SortCharacters()
    {
        Characters.Sort((c1, c2) => c2.speed.CompareTo(c1.speed));
    }

    public void AddCharacter(Character character)
    {
        if(!Characters.Contains(character))
            Characters.Add(character);

        // sort the characters by speed every time a new one is added
        SortCharacters();
    }

    public void RemoveCharacter(Character character)
    {
        foreach(Character c in Characters)
        {
            if(c == character)
            {
                Characters.Remove(c);
            }
        }
    }

    public Action GetNextAction()
    {
        return null;
    }
}
