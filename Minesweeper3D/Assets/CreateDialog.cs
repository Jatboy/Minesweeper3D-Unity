using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Minesweeper;
using TMPro;

public class CreateDialog : MonoBehaviour {
    public GameObject MineCount;
    public GameObject TilePadding;
    public GameObject TileSize;
    public GameObject MinefieldWidth;
    public GameObject MinefieldHeight;
    public GameObject MinefieldDepth;
    public GameObject CreateButton;
    public Minefield _Minefield;
    
    public Color DefaultSelection;

    private const int MIN_MINES = 5;
    private const int MIN_PADDING = 3;
    private const int MIN_SIZE = 1;

    private int mineCount = 10, tilePadding = 3, tileSize = 1;
    private float width = 8, height = 8, depth = 8;
    private bool isValid = true;

    public void OnMineCount(string text) {
        TMP_Text field = MineCount.GetComponent<TMP_Text>();
        if (!text.Equals("") && text != null) {
            int value = int.Parse(text);
            if (value < MIN_MINES) {
                field.color = Color.red;
                isValid = false;
            }
            else {
                field.color = Color.white;
                isValid = true;
            }
            mineCount = value;
        } else
            field.color = Color.red;
    }

    public void OnTilePadding(string text) {
        TMP_Text field = TilePadding.GetComponent<TMP_Text>();
        if (!text.Equals("") && text != null) {
            int value = int.Parse(text);
            if (value < MIN_PADDING) {
                field.color = Color.red;
                isValid = false;
            } else {
                field.color = Color.white;
                isValid = true;
            }
            tilePadding = value;
        } else
            field.color = Color.red;
    }

    public void OnTileSize(string text) {
        TMP_Text field = TileSize.GetComponent<TMP_Text>();
        if (!text.Equals("") && text != null) {
            int value = int.Parse(text);
            if (value < MIN_SIZE) {
                isValid = false;
                field.color = Color.red;
            }
            else {
                field.color = Color.white;
                isValid = true;
            }
        }
        else
            field.color = Color.red;
    }

    public void SetMinefieldWidth(float value) {
        TMP_InputField field = MinefieldWidth.GetComponentInChildren<TMP_InputField>();
        field.text = value.ToString();
        width = value;
    }

    public void SetMinefieldHeight(float value) {
        TMP_InputField field = MinefieldHeight.GetComponentInChildren<TMP_InputField>();
        field.text = value.ToString();
        height = value;
    }

    public void SetMinefieldDepth(float value) {
        TMP_InputField field = MinefieldDepth.GetComponentInChildren<TMP_InputField>();
        field.text = value.ToString();
        depth = value;
    }

    public void Create() {
        gameObject.SetActive(false);
        _Minefield.SetValues(mineCount, tilePadding, tileSize, width, height, depth);
        _Minefield.CreateMinefield();
        CreateButton.SetActive(true);
    }

}
