using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum DraftState { START, PLAYERTURN, OPPONENTTURN, END }

public class TeamManager : MonoBehaviour
{
    private int numPicks;
    private int numPicksMade;
    private DraftState state;
    private string playersChoice;

    private int numTypes;
    //private int[] availableTypes;

    public Text turnText;
    public GameObject elements;

    public GameObject fireCharPrefab;
    public GameObject waterCharPrefab;
    public GameObject plantCharPrefab;
    public GameObject lightningCharPrefab;

    public ElementManager elementManager;

    void Start()
    {
        numPicksMade = 0;
        numTypes = elementManager.NumElements;
        //availableTypes = new int[typeManager.NumberOfTypes];

        /* hardcode teams for now */
        List<GameObject> team1chars = new List<GameObject>() { fireCharPrefab };
        CreateTeam(team1chars);
        List<GameObject> team2chars = new List<GameObject>() { waterCharPrefab };
        CreateTeam(team2chars);
        
        // have board set character positions
        GameObject.Find("Board").GetComponent<Board>().SetCharPositions();

        //turnText = GameObject.Find("TurnText").GetComponent<Text>();
        ResetText();

        /*
        for (int i = 0; i < elements.transform.childCount; i++)
        {
            if (elements.transform.GetChild(i).GetComponent<Button>().interactable)
                availableTypes[i] = 1; // make type available
        }
        */

        state = DraftState.START;
    }

    public void CreateTeam(List<GameObject> charPrefabs)
    {
        // create team name based on number of teams
        string name = "Team " + (transform.childCount + 1);
        // create team GameObject and add Team script component
        GameObject team = new GameObject(name, typeof(Team));
        team.transform.parent = transform;
        // add the specified list of characters to the team
        foreach(GameObject charPrefab in charPrefabs)
            team.GetComponent<Team>().AddCharacter(charPrefab);
    }

    public Team GetTeam(int index)
    {
        return transform.GetChild(index).GetComponent<Team>();
    }

    public List<Character> GetTeamCharacters(int teamNum)
    {
        return GetTeam(teamNum).GetCharacters();
    }

    public List<Team> GetTeams()
    {
        // loop over transform children and add them to a new list
        List<Team> teams = new List<Team>();
        foreach (Transform t in transform)
            teams.Add(t.GetComponent<Team>());
        return teams;
    }

    public int NumTeams()
    {
        return transform.childCount;
    }

    public void ResetDraft()
    {
        numPicksMade = 0;

        ResetTeams();
        ResetText();

        // TODO: rewrite this so that it doesn't make locked types interactable
        for (int i = 0; i < elements.transform.childCount; i++)
        {
            elements.transform.GetChild(i).GetComponent<Button>().interactable = true;
        }

        state = DraftState.START;
    }

    public void ResetTeams()
    {
        foreach (GameObject team in transform)
        {
            Destroy(team);
        }
    }

    public void ResetText()
    {
        turnText.text = "Pick " + numPicks + " character";
        if (numPicks != 1)
            turnText.text += "s";
        turnText.text += ".";
    }

    public void PlayerChoose()
    {
        playersChoice = EventSystem.current.currentSelectedGameObject.name;
        if (state == DraftState.START || state == DraftState.PLAYERTURN)
        {
            int index;
            GameObject tempCharPrefab;
            switch (playersChoice)
            {
                case "Fire":
                    index = 0;
                    tempCharPrefab = fireCharPrefab;
                    break;
                case "Water":
                    index = 1;
                    tempCharPrefab = waterCharPrefab;
                    break;
                case "Plant":
                    index = 2;
                    tempCharPrefab = plantCharPrefab;
                    break;
                case "Lightning":
                    index = 3;
                    tempCharPrefab = lightningCharPrefab;
                    break;
                default:
                    index = 0;
                    tempCharPrefab = fireCharPrefab;
                    break;
            }
            transform.GetChild(0).GetComponent<Team>().AddCharacter(tempCharPrefab);
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;
            //availableTypes[index] = 0; // make type unavailable

            state = DraftState.OPPONENTTURN;
            turnText.text = "Opponent's pick...";
            OpponentChoose(index);
        }
    }

    private void OpponentChoose(int playersChoice)
    {
        string opponentsChoice;
        int random = Random.Range(1, 101);
        int x_index = 0;
        float total = 0;
        float count = 0;

        /*
        for (int i = 0; i < typeManager.NumberOfTypes; i++)
        {
            if (availableTypes[i] == 1)
                total += typeManager.GetChange(i, x_index) * typeManager.GetChange(i, x_index);
        }

        for (int i = 0; i < typeManager.NumberOfTypes; i++)
        {
            if (availableTypes[i] == 1)
            {
                count += typeManager.GetChange(i, x_index) * typeManager.GetChange(i, x_index);
                float temp = count / total * 100f;
                if (random < temp)
                {
                    GameObject tempCharPrefab;
                    switch (i)
                    {
                        case 0:
                            tempCharPrefab = fireCharPrefab;
                            opponentsChoice = "Fire";
                            break;
                        case 1:
                            tempCharPrefab = waterCharPrefab;
                            opponentsChoice = "Water";
                            break;
                        case 2:
                            tempCharPrefab = plantCharPrefab;
                            opponentsChoice = "Plant";
                            break;
                        case 3:
                            tempCharPrefab = lightningCharPrefab;
                            opponentsChoice = "Lightning";
                            break;
                        default:
                            tempCharPrefab = fireCharPrefab;
                            opponentsChoice = "Fire";
                            break;
                    }
                    teams[1].GetComponent<Team>().AddCharacter(tempCharPrefab);
                    GameObject.Find(opponentsChoice).GetComponent<Button>().interactable = false;
                    break;
                }
            }
        }
        */

        numPicksMade += 1;

        if (numPicks - numPicksMade <= 0)
        {
            state = DraftState.END;
            turnText.text = "The teams are set!";
            GameObject.Find("Play!").GetComponent<Button>().interactable = true;
        }
        else
        {
            state = DraftState.PLAYERTURN;
            turnText.text = "Your turn!";
        }
    }

    public List<Team> GetReadyTeams()
    {
        List<Team> readyTeams = new List<Team>();
        foreach(Transform t in transform)
        {
            Team team = t.GetComponent<Team>();
            if (team.GetActionState() == ActionStateEnum.READY)
                readyTeams.Add(team);
        }
        return readyTeams;
    }

    public ActionStateEnum GetActionState()
    {
        ActionStateEnum state = ActionStateEnum.READY;
        foreach (Transform t in transform)
        {
            Team team = t.GetComponent<Team>();
            // if all player characters in the Team are ready then generate AI actions
            //if(team.GetPlayerActionState() == ActionStateEnum.READY)
            //    team.ChooseAIActions();

            ActionStateEnum teamState = team.GetActionState();
            if (teamState == ActionStateEnum.PLANNING) // if anyone is planning then return planning
                return teamState;
            else if (teamState < state) // otherwise if somebody is idle return idle
                state = teamState;
        }
        return state; // return ready only when everyone is ready
    }
}
