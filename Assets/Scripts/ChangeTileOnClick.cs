using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private Vector3Int previousCenterTile = new Vector3Int();

    // Update is called once per frame
    void Update()
    {
        Vector3Int mousePos = GetMousePosition();
        Vector3Int centerTile = ObtainCenterTile(mousePos.x, mousePos.y);

        // Reset previous hovered over tile
        if (!centerTile.Equals(previousCenterTile) && inaccessibleMap.HasTile(previousCenterTile))
        {
            SetGrid(previousCenterTile, inaccessibleTile);
        }

        // Highlight hovered over tile
        if (!centerTile.Equals(previousCenterTile) && inaccessibleMap.HasTile(centerTile))
        {
            SetGrid(centerTile, hoverTile);
            previousCenterTile = centerTile;
        }

        if (Input.GetMouseButtonDown(0) && PlayerManager.Instance.landCost <= PlayerManager.Instance.gold)
        {
            PlayerManager.Instance.PurchaseLand();
            SetGrid(centerTile, accessibleTile);
        }
    }

    void SetGrid(Vector3Int centerTile, Tile tile)
    {
        for (int x = centerTile.x - 1; x <= centerTile.x + 1; x++)
        {
            for (int y = centerTile.y - 1; y <= centerTile.y + 1; y++)
            {
                Vector3Int coord = new Vector3Int(x, y, 0);
                inaccessibleMap.SetTile(coord, null);

                if (tile == inaccessibleTile || tile == hoverTile)
                {
                    inaccessibleMap.SetTile(coord, tile);
                }
                else if (tile == accessibleTile)
                {
                    accessibleMap.SetTile(coord, tile);
                }
            }
        }
    }

    Vector3Int ObtainCenterTile(int x, int y)
    {
        int x_result = ObtainCenter(x);
        int y_result = ObtainCenter(y);

        return new Vector3Int(x_result, y_result, 0);
    }

    int ObtainCenter(int val)
    {
        int mod_result = val % 3;
        switch (mod_result)
        {
            case 2:
                return val - 1;
            case 0:
                return val + 1;
            case -1:
                return val - 1;
            default:
                return val;
        }
    }

    Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return inaccessibleMap.WorldToCell(mouseWorldPos);
    }
}
