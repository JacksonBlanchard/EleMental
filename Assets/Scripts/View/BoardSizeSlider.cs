using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardSizeSlider : MonoBehaviour
{
    public Slider sizeSlider;
    public GameObject board;

    public void Start()
    {
        //Adds a listener to the main slider and invokes a method when the value changes.
        sizeSlider.onValueChanged.AddListener(delegate { UpdateBoardSize(); });
    }

    void UpdateBoardSize()
    {
        board.GetComponent<Board>().SetRadius((int)sizeSlider.value);
    }
}
