using UnityEngine;

[CreateAssetMenu(fileName = "New Elementor", menuName = "Elementor")]
public class ElementorData : ScriptableObject
{
    public new string name;
    public int health; // max
    public int speed; // determines turn order
    public int mobility; // determines movement
    public ActionData[] actions;
}
