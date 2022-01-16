using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject charUIprefab;
    public GameObject playerPanel;
    public GameObject opponentPanel;
    public GameObject textPanel;
    public TeamManager teamManager;

    // called by Play button
    public void SetupGameUI()
    {
        // start by clearing the UI
        ClearUI();

        switch(teamManager.NumTeams())
        {
            // hardcode for now
            case 2:
                // create UI for all Elementors in Team 0, and add to playerPanel
                foreach(Elementor e in teamManager.GetTeamElementors(0))
                {
                    // create new UI GameObject and add it to the playerPanel
                    CharUI charUI = Instantiate(charUIprefab, playerPanel.transform).GetComponentInChildren<CharUI>();
                    // store the CharUI component in the associated elementor
                    e.UI = charUI;
                }

                // create UI for all Elementors in Team 1, and add to opponentPanel
                foreach (Elementor e in teamManager.GetTeamElementors(1))
                {
                    // create new UI GameObject and add it to the opponentPanel
                    CharUI charUI = Instantiate(charUIprefab, opponentPanel.transform).GetComponentInChildren<CharUI>();
                    // store the CharUI component in the associated elementor
                    e.UI = charUI;
                }

                break;
            default:
                break;
        }
    }

    public void SetDialogue(string text)
    {
        textPanel.GetComponentInChildren<Text>().text = text;
    }

    public void ClearUI()
    {
        // destroy all player elementor UIs
        foreach (GameObject ui in playerPanel.transform)
        {
            Destroy(ui);
        }
        foreach(GameObject ui in opponentPanel.transform)
        {
            Destroy(ui);
        }
    }

    public void ResetAllCharState()
    {
        foreach(Team t in teamManager.GetTeams())
        {
            foreach(Elementor e in t.GetElementors())
            {
                e.UpdateElementorState();
            }
        }
    }
}
