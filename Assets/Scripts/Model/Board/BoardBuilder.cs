using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardBuilder : MonoBehaviour
{
    public Board m_board;

    public Text m_radiusText;
    public Text m_heightText;
    public Text m_widthText;
    public Slider m_radiusSlider;
    public Slider m_heightSlider;
    public Slider m_widthSlider;

    public Button m_deleteButton;
    public Button m_saveButton;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize slider min and max values
        m_radiusSlider.minValue = 1;
        m_radiusSlider.maxValue = 10;
        m_heightSlider.minValue = 1;
        m_heightSlider.maxValue = 20;
        m_widthSlider.minValue = 1;
        m_widthSlider.maxValue = 20;
        m_radiusText.text = "Radius: " + m_radiusSlider.value;
        m_heightText.text = "Height: " + m_heightSlider.value;
        m_widthText.text = "Width: " + m_widthSlider.value;

        // Set slider onValueChanged listeners
        m_radiusSlider.onValueChanged.AddListener(delegate { SetBoardRadius(); });
        m_heightSlider.onValueChanged.AddListener(delegate { SetBoardDimensions(); });
        m_widthSlider.onValueChanged.AddListener(delegate { SetBoardDimensions(); });

        // Set button onClick listeners
        m_deleteButton.onClick.AddListener(delegate { DeleteTile(); });
        m_saveButton.onClick.AddListener(delegate { SaveBoard(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetBoardRadius()
    {
        m_radiusText.text = "Radius: " + m_radiusSlider.value;
        m_board.GenerateCircularBoard((int)m_radiusSlider.value);
    }

    void SetBoardDimensions()
    {
        m_heightText.text = "Height: " + m_heightSlider.value;
        m_widthText.text = "Width: " + m_widthSlider.value;
        m_board.SetDimensions((int)m_heightSlider.value, (int)m_widthSlider.value);
    }

    void DeleteTile()
    {
        // not sure how to select a specific tile, or even groups
        // maybe have two input fields for the row and column?
    }

    void SaveBoard()
    {
        m_board.SaveBoard();
    }
}
