using System.Collections.Generic;
using UnityEngine;

public enum CharacterStateEnum { IDLE, PLANNING, READY1, READY2, SUBMITTED }

public class Character : MonoBehaviour
{
    public bool ai;
    public int maxHealth;
    public int currentHealth;
    public int speed; // determines which character goes first
    public int mobility; // determines how much a character can move
    public ElementEnum element;
    public CharacterStateEnum state;
    private CharUI ui;

    public CharacterStateEnum State {
        get { return UpdateCharacterState(); }
        set { state = UpdateCharacterState(); }
    }
    public CharUI UI { 
        get { return ui; } 
        set { ui = value; value.Init(this); } 
    }
    public List<Action> Actions { get; set; }
    public List<Action> SubmittedActions { get; set; }

    void Awake()
    {
        Actions = new List<Action>();
        SubmittedActions = new List<Action>();
        state = CharacterStateEnum.IDLE;
    }

    public CharacterStateEnum UpdateCharacterState()
    {
        CharacterStateEnum s = CharacterStateEnum.IDLE;
        foreach(Action a in Actions)
        {
            if(a.State == ActionStateEnum.PLANNING)
            {
                s = CharacterStateEnum.PLANNING;
                break;
            }
            else if(a.State == ActionStateEnum.READY)
            {
                switch(SubmittedActions.Count)
                {
                    case 0:
                        // should never get here
                        break;
                    case 1:
                        s = CharacterStateEnum.READY1;
                        break;
                    case 2:
                        s = CharacterStateEnum.READY2;
                        break;
                    default:
                        // should never get here either
                        break;
                }
            }
        }
        UpdateUIState(s);
        return s;
    }

    public Action GetPlanningAction()
    {
        foreach(Action action in Actions)
            if (action.State == ActionStateEnum.PLANNING)
                return action;
        return null;
    }

    public void PlanAction(Action action)
    {
        // only plan the action if the character is not locked in and this action hasn't been submitted
        if(state != CharacterStateEnum.SUBMITTED && !SubmittedActions.Contains(action))
        {
            UpdateActionState(action, ActionStateEnum.PLANNING);
            state = UpdateCharacterState();
        }
    }

    public void SubmitAction(Action action)
    {
        UpdateActionState(action, ActionStateEnum.READY);
        SubmittedActions.Add(action);
        state = UpdateCharacterState();
    }

    public void CancelAction(Action action)
    {
        UpdateActionState(action, ActionStateEnum.IDLE);
        state = UpdateCharacterState();
        SubmittedActions.Remove(action);
    }

    public void ResetSubmittedActions()
    {
        foreach (Action action in SubmittedActions)
        {
            CancelAction(action);
        }
    }

    public void UpdateActionState(Action action, ActionStateEnum actionState)
    {
        foreach(Action a in Actions)
        {
            // update the chosen action's state to the actionState param
            if(a == action)
            {
                a.State = actionState;
            }
            // otherwise reset the chosen action to IDLE
            else if(a.State == actionState)
            {
                a.State = ActionStateEnum.IDLE;
            }
        }
    }

    private void UpdateUIState(CharacterStateEnum state)
    {
        UI.UpdateCharState(state);
        if (state == CharacterStateEnum.SUBMITTED)
        {
            UI.ActivateMenuPanel(false);
            UI.enabled = false; // doesn't work as expected
        }
    }

    public void SubmitTurn()
    {
        if (state != CharacterStateEnum.PLANNING)
        {
            state = CharacterStateEnum.SUBMITTED;
            UpdateUIState(state);
        }
    }

    public void ChooseRandomAction()
    {
        var random = new System.Random();
        int rand = random.Next(0, Actions.Count);
        Actions[rand].State = ActionStateEnum.READY;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= damage)
        {
            // disabled dead character
            currentHealth = 0;
            GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            currentHealth -= damage;
        }

        UI.UpdateHealth(currentHealth);
    }

    public void SendCharacter()
    {
        GameObject.Find("GameManager").GetComponent<BattleSystem>().SetCurrCharacter(this);
    }
}
