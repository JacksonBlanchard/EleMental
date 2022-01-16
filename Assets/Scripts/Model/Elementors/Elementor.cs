using System.Collections.Generic;
using UnityEngine;

public enum ElementorStateEnum { IDLE, PLANNING, READY1, READY2, SUBMITTED }

public class Elementor : MonoBehaviour
{
    [SerializeField] private ElementorData m_elementorData;
    private ActionData[] m_actions;

    private int m_currentHealth;
    private ElementorUI m_ui;
    private ElementorStateEnum m_state;

    /*
    // Long-term goal
    public bool ai;
    */

    public string Name
    {
        get { return m_elementorData.name; }
    }

    public int MaxHealth
    {
        get { return m_elementorData.health; }
    }

    public int CurrentHealth
    {
        get { return m_currentHealth; }
    }

    public int Speed
    {
        get { return m_elementorData.speed; }
    }

    public int Mobility
    {
        get { return m_elementorData.mobility; }
    }

    public ElementorUI UI
    { 
        get { return m_ui; } 
        set { m_ui = value; value.Init(this); } 
    }

    public ElementorStateEnum State
    {
        get { return UpdateElementorState(); }
        set { m_state = UpdateElementorState(); }
    }

    public List<Action> Actions { get; set; }
    public List<Action> SubmittedActions { get; set; }

    void Awake()
    {
        Actions = new List<Action>();
        SubmittedActions = new List<Action>();
        m_state = ElementorStateEnum.IDLE;
    }

    public ElementorStateEnum UpdateElementorState()
    {
        ElementorStateEnum s = ElementorStateEnum.IDLE;
        foreach(Action a in Actions)
        {
            if(a.State == ActionStateEnum.PLANNING)
            {
                s = ElementorStateEnum.PLANNING;
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
                        s = ElementorStateEnum.READY1;
                        break;
                    case 2:
                        s = ElementorStateEnum.READY2;
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
        // only plan the action if the elementor is not locked in and this action hasn't been submitted
        if(m_state != ElementorStateEnum.SUBMITTED && !SubmittedActions.Contains(action))
        {
            UpdateActionState(action, ActionStateEnum.PLANNING);
            m_state = UpdateElementorState();
        }
    }

    public void SubmitAction(Action action)
    {
        UpdateActionState(action, ActionStateEnum.READY);
        SubmittedActions.Add(action);
        m_state = UpdateElementorState();
    }

    public void CancelAction(Action action)
    {
        UpdateActionState(action, ActionStateEnum.IDLE);
        m_state = UpdateElementorState();
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

    private void UpdateUIState(ElementorStateEnum state)
    {
        UI.UpdateCharState(state);
        if (state == ElementorStateEnum.SUBMITTED)
        {
            UI.ActivateMenuPanel(false);
            UI.enabled = false; // doesn't work as expected
        }
    }

    public void SubmitTurn()
    {
        if (m_state != ElementorStateEnum.PLANNING)
        {
            m_state = ElementorStateEnum.SUBMITTED;
            UpdateUIState(m_state);
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
        if (m_currentHealth <= damage)
        {
            // disabled dead elementor
            m_currentHealth = 0;
            GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            m_currentHealth -= damage;
        }

        UI.UpdateHealth(m_currentHealth);
    }

    public void SendElementor()
    {
        GameObject.Find("GameManager").GetComponent<BattleSystem>().SetCurrElementor(this);
    }
}
