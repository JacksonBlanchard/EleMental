using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    private ActionStateEnum state;

    public void Awake()
    {
        state = ActionStateEnum.IDLE;
    }

    public void AddElementor(GameObject charPrefab)
    {
        Instantiate(charPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
    }

    public GameObject GetElementorGO(int index)
    {
        return transform.GetChild(index).gameObject;
    }

    public GameObject GetElementorGO(string name)
    {
        foreach(Transform c in transform)
        {
            if(c.name == name)
                return c.gameObject;
        }
        return null;
    }

    public List<Elementor> GetElementors()
    {
        // loop over transform children and add them to a new list
        List<Elementor> chars = new List<Elementor>();
        foreach(Transform t in transform)
            chars.Add(t.GetComponent<Elementor>());
        return chars;
    }

    public int Size()
    {
        return transform.childCount;
    }

    public void ResetTeam()
    {
        foreach(GameObject elementor in transform)
            Destroy(elementor.gameObject);
    }

    public ActionStateEnum GetActionState()
    {
        ActionStateEnum teamState = ActionStateEnum.READY;
        foreach(Transform t in transform)
        {
            ElementorStateEnum charState = t.GetComponent<Elementor>().State;
            if (charState == ElementorStateEnum.PLANNING)
                return ActionStateEnum.PLANNING;
            else if (charState == ElementorStateEnum.SUBMITTED)
                teamState = ActionStateEnum.READY;
            else
                teamState = ActionStateEnum.IDLE;
        }
        return teamState;
    }

    // Long-term goal
    public void ChooseAIActions()
    {
        /*
        foreach(Transform t in transform)
        {
            Elementor c = t.GetComponent<Elementor>();
            if(c.ai)
                c.ChooseRandomAction();
        }
        */
    }
}
