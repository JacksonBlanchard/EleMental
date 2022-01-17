using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GroundEnum { DEEP_WATER, SHALLOW_WATER, EARTH }
public enum SurfaceEnum { EMPTY, FIRE, PLANTS }
public enum AtmosphereEnum { WIND_E, WIND_SE, WIND_SW, WIND_W, WIND_NW, WIND_NE }

public class HexTile : MonoBehaviour
{
    public int m_x;
    public int m_y;
    public int m_z;

    public GroundEnum m_ground;
    public SurfaceEnum m_surface;
    public AtmosphereEnum m_atmosphere;
    public int m_temperature;
    public int m_luminance;

    public ElementEnum currElement;
    public ElementEnum tempElement;
    public ElementEnum atkElement;

    private Elementor m_elementor;
    public ElementManager m_elementManager;

    public void SetCoordinates(int row, int col)
    {

        SetWithRowCol(row, col);

    }

    public void InitWithName()
    {
        string name = this.gameObject.name;
        int row = Int32.Parse(name.Substring(4, name.IndexOf(",") - 4)); // from end of 'Tile' to comma
        int col = Int32.Parse(name.Substring(name.IndexOf(",") + 1)); // from comma to end
        SetWithRowCol(row, col);

        currElement = ElementEnum.NEUTRAL;
        atkElement = ElementEnum.NEUTRAL;
        m_elementManager = GameObject.Find("ElementManager").GetComponent<ElementManager>();
    }

    public bool HasElementor()
    {
        return m_elementor != null;
    }

    public Elementor GetElementor()
    {
        return m_elementor;
    }

    public void SetElementor(GameObject elementor)
    {
        m_elementor = elementor.GetComponent<Elementor>();
        elementor.transform.position = transform.position;
    }

    public void SetWithXYZ(int x, int y, int z)
    {
        m_x = x;
        m_y = y;
        m_z = z;
    }

    public void SetWithRowCol(int row, int col)
    {
        // -row because my row coordinate system is opposite
        // -y and -z because it makes more sense
        m_x = col - (-row - (-row & 1)) / 2; // col - (row - (row & 1)) / 2
        m_z = row;                           // -(-row)
        m_y = m_x - m_z;                         // -(-x - (-z))
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
        SetShader(m_elementManager.GetColor(element1), m_elementManager.GetColor(element2), speed);
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
        return m_x == x && m_y == y && m_z == z;
    }
}

