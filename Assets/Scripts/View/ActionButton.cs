using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    public Action action;

    public void SetAction(Action action)
    {
        this.action = action;
        GetComponentInChildren<Text>().text = action.Name;
    }
}
