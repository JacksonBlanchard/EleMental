using System;
using System.Collections.Generic;
using UnityEngine;

public class Turn
{
    // ordered by speed
    public List<Elementor> Elementors { get; }

    public Turn()
    {
        Elementors = new List<Elementor>();
    }

    public void ResetTurn()
    {
        Elementors.Clear();
    }

    public void SortElementors()
    {
        Elementors.Sort((e1, e2) => e2.Speed.CompareTo(e1.Speed));
    }

    public void AddElementor(Elementor elementor)
    {
        if(!Elementors.Contains(elementor))
            Elementors.Add(elementor);

        // sort the elementors by speed every time a new one is added
        SortElementors();
    }

    public void RemoveElementor(Elementor elementor)
    {
        foreach(Elementor e in Elementors)
        {
            if(e == elementor)
            {
                Elementors.Remove(e);
            }
        }
    }

    public Action GetNextAction()
    {
        return null;
    }
}
