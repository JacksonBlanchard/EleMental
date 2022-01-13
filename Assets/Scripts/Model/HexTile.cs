using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileState { BURNING, FLOODED, OVERGROWN, CHARGED, ICEY, SNOWY, SHROUDED, ILLUMINATED}

public class HexTile : MonoBehaviour
{
    public int x;
    public int y;
    public int z;

    public TileState state;

    public ElementEnum currElement;
    public ElementEnum tempElement;
    public ElementEnum atkElement;

    private Character character;
    public ElementManager elementManager;

    public void InitWithName()
    {
        string name = this.gameObject.name;
        int row = Int32.Parse(name.Substring(4, name.IndexOf(",") - 4)); // from end of 'Tile' to comma
        int col = Int32.Parse(name.Substring(name.IndexOf(",") + 1)); // from comma to end
        SetWithRowCol(row, col);

        currElement = ElementEnum.NEUTRAL;
        atkElement = ElementEnum.NEUTRAL;
        elementManager = GameObject.Find("ElementManager").GetComponent<ElementManager>();
    }

    public bool HasCharacter()
    {
        return character != null;
    }

    public Character GetCharacter()
    {
        return character;
    }

    public void SetCharacter(GameObject character)
    {
        this.character = character.GetComponent<Character>();
        character.transform.position = transform.position;
    }

    public void SetWithXYZ(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public void SetWithRowCol(int row, int col)
    {
        // -row because my row coordinate system is opposite
        // -y and -z because it makes more sense
        this.x = col - (-row - (-row & 1)) / 2; // col - (row - (row & 1)) / 2
        this.z = row;                           // -(-row)
        this.y = x - z;                         // -(-x - (-z))
    }

    // Reset both shader colors to currType
    public void ResetType()
    {
        SetShader(currElement, currElement, 0);
    }

    // Set temporary select shader colors
    public void SetTempElement(ElementEnum tempElement)
    {
        this.tempElement = tempElement;
        SetShader(currElement, tempElement, 4f);
    }
    
    // Reset the tempType
    public void ResetTempElement()
    {
        SetShader(currElement, atkElement, 2f);
    }

    // Set shader colors to currType and atkingType respectively
    public void SetAtkElement()
    {
        atkElement = tempElement;
        SetShader(currElement, atkElement, 2f);
    }

    public void ChangeToAtkElement()
    {
        currElement = atkElement;
        SetShader(currElement, currElement, 0);
    }

    // Set shader colors to the new type
    public void ChangeElement(ElementEnum newElement)
    {
        atkElement = newElement; // TODO: test this // reset atkType so it doesnt linger through turns
        currElement = newElement;
        SetShader(currElement, currElement, 0);
    }

    // Set shader colors
    private void SetShader(ElementEnum element1, ElementEnum element2, float speed)
    {
        SetShader(elementManager.GetColor(element1), elementManager.GetColor(element2), speed);
    }

    private void SetShader(Color color1, Color color2, float speed)
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        rend.material.SetColor("_Color1", color1);
        rend.material.SetColor("_Color2", color2);
        rend.material.SetFloat("_Speed", speed);
    }

    public bool Equal(int x, int y, int z)
    {
        return this.x == x && this.y == y && this.z == z;
    }
}
