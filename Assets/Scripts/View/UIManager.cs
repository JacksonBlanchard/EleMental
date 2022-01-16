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

    // Called by Play button
    public void SetupGameUI()
    {
        // Start by clearing the UI
        ClearUI();

        switch(teamManager.NumTeams())
        {
            // hardcode for now
            case 2:
                // create UI for all Elementors in Team 0, and add to playerPanel
                foreach(Elementor e in teamManager.GetTeamElementors(0))
                {
                    // Create a new UI GameObject and add it to the playerPanel.
                    // Then store the ElementorUI component in the associated Elementor.
                    e.UI = Instantiate(charUIprefab, playerPanel.transform).GetComponentInChildren<ElementorUI>();
                }
                // create UI for all Elementors in Team 1, and add to opponentPanel
                foreach(Elementor e in teamManager.GetTeamElementors(1))
                {
                    // Create a new UI GameObject and add it to the opponentPanel.
                    // Then store the ElementorUI component in the associated Elementor.
                    e.UI = Instantiate(charUIprefab, opponentPanel.transform).GetComponentInChildren<ElementorUI>();
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
        // Destroy all ElementorUIs
        foreach(GameObject ui in playerPanel.transform)
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
