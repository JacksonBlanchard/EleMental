using UnityEngine;
using UnityEngine.UI;

public class ElementorUI : MonoBehaviour
{
    public Text elementorName;
    public Slider healthBar;
    public Button actionStatusBtn;
    public Button submitBtn;
    public Button actionBtnPrefab;
    public GameObject menuPanel;
    public GameObject attackBtnParent;
    public GameObject movementBtnParent;

    public void Init(Elementor elementor)
    {
        name = elementor.Name + "_UI";
        elementorName.text = elementor.Name;
        healthBar.maxValue = elementor.MaxHealth;
        healthBar.value = elementor.CurrentHealth;

        // attach submit method to button
        submitBtn.onClick.AddListener(() => { elementor.SubmitTurn(); });

        // actionStatusBtn activates
        actionStatusBtn.onClick.AddListener(() => { ToggleMenuPanel(); });
        // init ActionStatusBtn text
        UpdateCharState(ElementorStateEnum.IDLE);

        // set actions
        foreach (Action action in elementor.Actions)
        {
            Button btn;
            if (action.GetType().Equals(typeof(Movement)))
            {
                btn = Instantiate(actionBtnPrefab, movementBtnParent.transform);
            }
            else
            {
                btn = Instantiate(actionBtnPrefab, attackBtnParent.transform);
            }

            // set Action and listeners for action buttons
            btn.name = action.Name + " Btn";
            btn.GetComponent<ActionButton>().SetAction(action);
            btn.onClick.AddListener(() => { elementor.PlanAction(action); elementor.SendElementor(); });
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

    public void UpdateCharState(ElementorStateEnum state)
    {
        actionStatusBtn.GetComponentInChildren<Text>().text = state.ToString();
    }

    public void UpdateHealth(int health)
    {
        healthBar.value = health;
        if (health == 0)
        {
            enabled = false;
        }
    }
}