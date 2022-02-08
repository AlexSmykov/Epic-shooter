using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMap : MonoBehaviour
{
    public GameObject Map;
    public GameObject FloorSpawner;

    void Start()
    {
        Map = GameObject.FindGameObjectWithTag("Map");
        Map.SetActive(false);
        FloorSpawner = GameObject.FindGameObjectWithTag("FloorSpawner");
        TogleMap();
    }

    public void TogleMap()
    {
        if (Map.activeSelf)
        {
            Map.SetActive(false);
            return;
        }
        Map.SetActive(true);
        UpdateMap();
    }

    public void UpdateMap()
    {
        for (int i = 0; i < Map.transform.childCount; i++)
        {
            Transform row = Map.transform.GetChild(Map.transform.childCount - i - 1);
            for (int j = 0; j < row.childCount; j++)
            {
                Transform tile = row.transform.GetChild(j);
                if (FloorSpawner.GetComponent<FloorMaker>().RoomsOnTheMap[j, i])
                {
                    if (FloorSpawner.GetComponent<FloorMaker>().RoomsChecked[j, i])
                    {
                        tile.GetComponent<SpriteRenderer>().color = new Color(200f / 255f, 200f / 255f, 200f / 255f, 200f / 255f);
                        string type = FloorSpawner.GetComponent<FloorMaker>().RoomTypeOnTheMap[j, i];
                        if (type == "Boss")
                        {
                            OffTilesType(tile);
                            tile.GetChild(0).gameObject.SetActive(true);
                        }
                        else if (type == "Chest")
                        {
                            OffTilesType(tile);
                            tile.GetChild(1).gameObject.SetActive(true);
                        }
                        else if (type == "Shop")
                        {
                            OffTilesType(tile);
                            tile.GetChild(2).gameObject.SetActive(true);
                        }
                        else if (type == "Workshop")
                        {
                            OffTilesType(tile);
                            tile.GetChild(3).gameObject.SetActive(true);
                        }
                        else if (type == "SuperBoss")
                        {
                            OffTilesType(tile);
                            tile.GetChild(4).gameObject.SetActive(true);
                        }
                        else
                        {
                            OffTilesType(tile);
                        }
                    }
                    if (GetComponent<Player>().Cords.x == j && GetComponent<Player>().Cords.y == i)
                    {
                        tile.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 200f / 255f);
                    }
                }
                else
                {
                    tile.GetComponent<SpriteRenderer>().color = new Color(200f / 255f, 200f / 255f, 200f / 255f, 0f / 255f);
                }
            }
        }
    }

    private void OffTilesType(Transform tile)
    {
        for(int i = 0; i < tile.childCount; i++)
        {
            tile.GetChild(i).gameObject.SetActive(false);
        }
    }
}
