using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ElementorSelection : MonoBehaviour
{
    public GameObject fireCharPrefab;
    public GameObject waterCharPrefab;
    public GameObject plantCharPrefab;
    public GameObject lightningCharPrefab;

    public ElementManager elementManager;


    public void SetPlayerChar(ElementEnum element)
    {
        switch(element)
        {
            default:
                Debug.Log("Hit empty switch case in ElementorSelection::SetPlayerChar()");
                break;
        }
    }

    public void SetEnemyChar(ElementEnum element)
    {
        switch(element)
        {
            default:
                Debug.Log("Hit empty switch case in ElementorSelection::SetEnemyChar()");
                break;
        }
    }

    /*
    private int numPicks;
    private int numPicksMade;
    private DraftState state;
    private string playersChoice;

    private int[] availableTypes;

    private int numTeams;
    private List<GameObject> teams;

    private Text turnText;
    public GameObject elements;

    

    void Start()
    {
        typeManager = GameObject.Find("TypeManager").GetComponent<TypeManager>();
        numPicksMade = 0;
        availableTypes = new int[typeManager.NumberOfTypes];

        numTeams = 2;
        teams = new List<GameObject>();
        for (int i = 0; i < numTeams; i++)
        {
            GameObject team = new GameObject();
            team.AddComponent<Team>();
            team.GetComponent<Team>().Init("Team " + (i + 1), false);
            teams.Add(team);
        }
        teams[1].GetComponent<Team>().aiTeam = true;

        turnText = GameObject.Find("TurnText").GetComponent<Text>();
        ResetText();

        for (int i = 0; i < elements.transform.childCount; i++)
        {
            if (elements.transform.GetChild(i).GetComponent<Button>().interactable)
                availableTypes[i] = 1; // make type available
        }

        state = DraftState.START;
    }

    public void ResetDraft()
    {
        numPicksMade = 0;

        foreach (GameObject team in teams)
        {
            team.GetComponent<Team>().ResetTeam();
        }

        ResetText();

        // TODO: rewrite this so that it doesn't make locked types interactable
        for (int i = 0; i < elements.transform.childCount; i++)
        {
            elements.transform.GetChild(i).GetComponent<Button>().interactable = true;
        }

        state = DraftState.START;
    }

    public void ResetText()
    {
        turnText.text = "Pick " + numPicks + " elementor";
        if (numPicks != 1)
            turnText.text += "s";
        turnText.text += ".";
    }

    public void PlayGame()
    {
        GameObject.Find("Board").GetComponent<Board>().SetCharPositions(teams);

        // Load Game scene
        SceneManager.LoadScene("Game");
    }

    public void SetNumPicks(string level)
    {
        // if (num < numUnlocked / 2)
        //numPicks = num;
        switch (level)
        {
            case "level1":
                numPicks = 1;
                break;
            case "level2":
                numPicks = 2;
                break;
            case "level3":
                numPicks = 3;
                break;
            default:
                numPicks = 1;
                break;
        }
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
            teams[0].GetComponent<Team>().AddElementor(tempCharPrefab);
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;
            availableTypes[index] = 0; // make type unavailable

            state = DraftState.OPPONENTTURN;
            turnText.text = "Opponent's pick...";
            StartCoroutine(OpponentChoose(index));
        }
    }

    private IEnumerator OpponentChoose(int playersChoice)
    {
        string opponentsChoice;
        int random = Random.Range(1, 101);
        int x_index = 0;
        float total = 0;
        float count = 0;

        yield return new WaitForSeconds(1.5f);

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
                    teams[1].GetComponent<Team>().AddElementor(tempCharPrefab);
                    GameObject.Find(opponentsChoice).GetComponent<Button>().interactable = false;
                    break;
                }
            }
        }

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
    */
}
