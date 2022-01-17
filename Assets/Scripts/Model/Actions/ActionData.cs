using UnityEngine;

[CreateAssetMenu(fileName = "New Action", menuName = "Action")]
public class ActionData : ScriptableObject
{
    public new string name;
    public ElementEnum element;
    public int damage;
    public PathAreaTypeEnum areaType;
    public DirectionEnum[] area;
    public DirectionEnum[] userMovement;
}
