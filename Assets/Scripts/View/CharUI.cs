using UnityEngine;
using UnityEngine.UI;

public class CharUI : MonoBehaviour
{
    public Text charName;
    public Slider healthBar;
    public Button actionStatusBtn;
    public Button submitBtn;
    public Button actionBtnPrefab;
    public GameObject menuPanel;
    public GameObject attackBtnParent;
    public GameObject movementBtnParent;

    public void Init(Character character)
    {
        name = character.name + "_UI";
        charName.text = character.name;
        healthBar.maxValue = character.maxHealth;
        healthBar.value = character.currentHealth;

        // attach submit method to button
        submitBtn.onClick.AddListener(() => { character.SubmitTurn(); });

        // actionStatusBtn activates
        actionStatusBtn.onClick.AddListener(() => { ToggleMenuPanel(); });
        // init ActionStatusBtn text
        UpdateCharState(CharacterStateEnum.IDLE);

        // set actions
        foreach (Action action in character.Actions)
        {
            Button btn;
            if (action.GetType().Equals(typeof(Movement)))
                btn = Instantiate(actionBtnPrefab, movementBtnParent.transform);
            else
                btn = Instantiate(actionBtnPrefab, attackBtnParent.transform);

            // set Action and listeners for action buttons
            btn.name = action.Name + " Btn";
            btn.GetComponent<ActionButton>().SetAction(action);
            btn.onClick.AddListener(() => { character.PlanAction(action); character.SendCharacter(); });
        }
    }

    public void ToggleMenuPanel()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
    }

    public void ActivateMenuPanel(bool active)
    {
        menuPanel.SetActive(active);
    }

    public void UpdateCharState(CharacterStateEnum state)
    {
        actionStatusBtn.GetComponentInChildren<Text>().text = state.ToString();
    }

    public void UpdateHealth(int health)
    {
        healthBar.value = health;
        if (health == 0)
            enabled = false;
    }
}