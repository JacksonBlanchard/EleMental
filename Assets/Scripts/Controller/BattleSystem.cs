using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum BattleState { START, PLANNING, EXECUTING, WIN, LOSE }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    private LayerMask boardLayer;

    public Board board;
    public UIManager uiManager;
    public TeamManager teamManager;

    private Turn currTurn;
    public Elementor CurrElementor { get; set; }
    public Action CurrAction { get; set; }

    private HexTile hoverTile;
    private List<HexTile> pathAreaPreview;

    void Start()
    {
        state = BattleState.START;
        boardLayer = LayerMask.GetMask("Board");

        currTurn = new Turn();
        pathAreaPreview = new List<HexTile>();
    }

    void Update()
    {
        // update the battle state every frame
        UpdateBattleState();

        if(state == BattleState.PLANNING)
        {
            // if the player hovers over the board
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit rayHit, Mathf.Infinity, boardLayer))
            {
                // adjust the Action using the hover tile
                hoverTile = rayHit.collider.gameObject.GetComponent<HexTile>();
                board.AdjustAction(CurrElementor, hoverTile);

                // get the actual path area for color viewing
                pathAreaPreview = board.GetActionArea(CurrElementor.GetPlanningAction());
                board.ViewTiles(pathAreaPreview, CurrAction.Element);
            }

            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                SubmitAction();
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                CancelAction();
            }
        }
        else if(state == BattleState.EXECUTING)
        {
            ExecuteTurn();
        }
        // handle other battle states here if necessary
    }

    void DoubleClick()
    {
        /*
        bool one_click = false;
        bool timer_running;
        float timer_for_double_click;

        //this is how long in seconds to allow for a double click
        float delay;
        if (Input.GetMouseButtonDown(0))
        {
            if (!one_click) // first click no previous clicks
            {
                one_click = true;
                timer_for_double_click = Time.time; // save the current time

                // do single click things;
            }
            else
            {
                one_click = false; // found a double click, now reset

                // do double click things
            }
        }
        if (one_click)
        {
            // if the time now is delay seconds more than when the first click started. 
            //if ((Time.time - timer_for_double_click) > delay)
            {

                //basically if thats true its been too long and we want to reset so the next click is simply a single click and not a double click.

                one_click = false;

            }
        }
        */
    }

    void UpdateBattleState()
    {
        switch(teamManager.GetActionState())
        {
            case ActionStateEnum.IDLE:
                state = BattleState.START;
                break;
            case ActionStateEnum.PLANNING:
                state = BattleState.PLANNING;
                break;
            case ActionStateEnum.READY:
                state = BattleState.EXECUTING;
                break;
        }
    }

    public void SetCurrElementor(Elementor c)
    {
        // store current elementor and action
        CurrElementor = c;
        CurrAction = CurrElementor.GetPlanningAction();

        // clear the previous Action preview
        board.UnviewTiles(pathAreaPreview);
        pathAreaPreview.Clear();

        // initialize start tile as the Elementor Tile for preview
        CurrAction.StartTile = board.GetCharTile(c);
        pathAreaPreview = board.GetActionArea(CurrAction);
        board.ViewTiles(pathAreaPreview, CurrAction.Element);
    }

    public void SubmitAction()
    {
        // update the Action and Elementor state
        CurrElementor.SubmitAction(CurrAction);

        // display the selected tiles and clear the preview list
        board.SubmitTiles(pathAreaPreview);
        pathAreaPreview.Clear();

        // add the Elementor to the turn
        currTurn.AddElementor(CurrElementor);
    }

    public void CancelAction()
    {
        // reset the Action and Elementor state
        if(CurrAction != null)
            CurrElementor.CancelAction(CurrAction);

        // clear the action preview and preview list
        board.UnviewTiles(pathAreaPreview);
        pathAreaPreview.Clear();

        // TODO: Remove elementor from turn if they have no SubmittedActions
    }

    void ExecuteTurn()
    {
        // reset planning preview
        board.ResetAllTiles();

        // loop over each elementor
        foreach (Elementor elementor in currTurn.Elementors)
        {
            foreach(Action action in elementor.SubmittedActions)
            {
                board.ExecuteAction(elementor, action);
            }
            // reset elementor when their turn is complete
            elementor.ResetSubmittedActions();
        }
        // reset turn when it has completed
        currTurn.ResetTurn();

        uiManager.ResetAllCharState();
    }
}
