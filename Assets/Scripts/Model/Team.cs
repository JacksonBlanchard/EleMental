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

    public void AddCharacter(GameObject charPrefab)
    {
        Instantiate(charPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
    }

    public GameObject GetCharacterGO(int index)
    {
        return transform.GetChild(index).gameObject;
    }

    public GameObject GetCharacterGO(string name)
    {
        foreach(Transform c in transform)
        {
            if(c.name == name)
                return c.gameObject;
        }
        return null;
    }

    public List<Character> GetCharacters()
    {
        // loop over transform children and add them to a new list
        List<Character> chars = new List<Character>();
        foreach(Transform t in transform)
            chars.Add(t.GetComponent<Character>());
        return chars;
    }

    public int Size()
    {
        return transform.childCount;
    }

    public void ResetTeam()
    {
        foreach(GameObject character in transform)
            Destroy(character.gameObject);
    }

    public ActionStateEnum GetActionState()
    {
        ActionStateEnum teamState = ActionStateEnum.READY;
        foreach(Transform t in transform)
        {
            CharacterStateEnum charState = t.GetComponent<Character>().state;
            if (charState == CharacterStateEnum.PLANNING)
                return ActionStateEnum.PLANNING;
            else if (charState == CharacterStateEnum.SUBMITTED)
                teamState = ActionStateEnum.READY;
            else
                teamState = ActionStateEnum.IDLE;
        }
        return teamState;
    }

    public void ChooseAIActions()
    {
        foreach(Transform t in transform)
        {
            Character c = t.GetComponent<Character>();
            if(c.ai)
                c.ChooseRandomAction();
        }
    }
}
