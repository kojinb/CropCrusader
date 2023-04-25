using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChangeTileOnClick : MonoBehaviour
{
    [SerializeField]
    Tile accessibleTile;
    [SerializeField]
    Tile inaccessibleTile;
    [SerializeField]
    Tile hoverTile;
    [SerializeField]
    Tilemap accessibleMap;
    [SerializeField]
    Tilemap inaccessibleMap;

    private Vector3Int previousMousePos = new Vector3Int(); 

    // Update is called once per frame
    void Update()
    {
        Vector3Int mousePos = GetMousePosition();

        // Reset previous hovered over tile
        if (!mousePos.Equals(previousMousePos) && inaccessibleMap.HasTile(previousMousePos))
            inaccessibleMap.SetTile(previousMousePos, inaccessibleTile);

        // Highlight hovered over tile
        if (!mousePos.Equals(previousMousePos) && inaccessibleMap.HasTile(mousePos))
        {
            inaccessibleMap.SetTile(mousePos, null);
            inaccessibleMap.SetTile(mousePos, hoverTile);
            previousMousePos = mousePos;
        }

        if (Input.GetMouseButtonDown(0))
        {
            inaccessibleMap.SetTile(mousePos, null);
            accessibleMap.SetTile(mousePos, accessibleTile);
        }
    }

    Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return inaccessibleMap.WorldToCell(mouseWorldPos);
    }
}
