﻿using UnityEngine;



public class MyGrid : MonoBehaviour
{
    // MyGrid Attributes:
    [SerializeField] private Mine prefabToSpawn;
    [Tooltip("when filling up the map, the amount of generated mines depends on the precentage")]
    [SerializeField] private float precentageOfMines = 0.2f;
    [SerializeField] private bool isMapArmed = false; // for controlling that we only arm the map after the first mine click by the player
    public GameObject[,,,] map;

    [Tooltip("The dimensions of the mine grid")]
    [SerializeField] private int xdim = 1, ydim = 1, zdim = 1; // wdim = 1; - for future work

    void Start()
    {
        Debug.Log("start is working on Grid class");

        this.map = new GameObject[xdim, ydim, zdim, 1];

        fillMap();
    }


    public void fillMap()
    {

        //arms the grid with mines
        Debug.Log("creating a gridmap");

        for (int w = 0; w < map.GetLength(3); w++)
        {
            for (int z = 0; z < map.GetLength(2); z++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    for (int x = 0; x < map.GetLength(0); x++)
                    {
                        map[x, y, z, w] = Instantiate(prefabToSpawn.gameObject, new Vector3(x, y, z), Quaternion.identity);
                        Debug.Log("created mine[ " + x + "," + y + "," + z + "," + w + " ]");
                    }
                }
            }
        }
        //for future work:
        /*int numOfMines = (int)(map.GetLength(0) * map.GetLength(1) * map.GetLength(2) * map.GetLength(3) * 0.2);
        Debug.Log("number of mines: " + numOfMines);*/
    }

    //arming the mines, and not arming the clicked mine that called this function, noted by its dimentions:
    public void setMines(int n, int xCurrMine, int yCurrMine, int zCurrMine, int wCurrMine)
    {
        Debug.Log("in setMines: ");
        int x;
        int y;
        int z;
        int w;
        int amountOfArmedMines = 0;

        for (int i = 0; i < n; i++)
        {
            x = (int)(Random.Range(0, map.GetLength(0)));
            y = (int)(Random.Range(0, map.GetLength(1)));
            z = (int)(Random.Range(0, map.GetLength(2)));
            w = (int)(Random.Range(0, map.GetLength(3)));

            Debug.Log("is mine [" + x + "," + y + "," + z + "," + w + "] armed? ");

            //if isn't armed and isnt the first clicked mine in the game:
            if (!map[x, y, z, w].GetComponent<Mine>().IsArmed
                && !(x == xCurrMine && y == yCurrMine && z == zCurrMine && w == wCurrMine))
            {
                Debug.Log("No, arming mine...");
                Mine tempRef = map[x, y, z, w].GetComponent<Mine>();
                Debug.Log("changing is_armed value of mine(xyzPos): " + tempRef.XPos + ", " + tempRef.YPos + ", " + tempRef.ZPos + ", ");
                map[x, y, z, w].GetComponent<Mine>().putMine();
                tempRef.putMine();
                amountOfArmedMines++;
                //updates neighbors
                updateNeighbors(x, y, z, w);
            }
            else { Debug.Log("yes, not arming"); }
        }
        Debug.Log("amount of armed mines: " + amountOfArmedMines);
        isMapArmed = true;

    }

    // opens the cell
    public void openCell(int x, int y, int z, int w)
    {
        if (!isMapArmed) { setMines((int)(xdim * ydim * zdim * precentageOfMines), x, y, z, w); }


        Debug.Log("in openCell, opening mine: " + x + ", " + y + ", " + z + ", " + w);

        if (IsMineOutOfBounds(x, y, z, w))
        {
            Debug.Log("invalid cordinates, please try again");
            return;
        }
        if (map[x, y, z, w].GetComponent<Mine>().IsArmed)
        {
            //map[x, y, z, w].is_Opened = true;
            Debug.Log("OPENED AN ARMED MINE! LOST!");
            return;
        }
        openCellRecursive(x, y, z, w);
    }

    //recursively keeps opening all adjecant neighbors until reaching a none 0-neighboring mine:
    public void openCellRecursive(int x, int y, int z, int w)
    {
        // if Mine is out of map bounds then return
        if (IsMineOutOfBounds(x, y, z, w))
        {
            return;
        }

        //if a mine is armed then return
        if (map[x, y, z, w].GetComponent<Mine>().IsArmed)
            return;

        //if a mine has more than 0 neighboring armed mines then open it and return
        if (map[x, y, z, w].GetComponent<Mine>().getNeighborCount() != 0)
        {
            map[x, y, z, w].GetComponent<Mine>().openThisCell();
            return;
        }

        //if mine is already open then return
        if (map[x, y, z, w].GetComponent<Mine>().IsOpened)
            return;

        //last case is that we hit a 0 armed neighboring mines cell:
        map[x, y, z, w].GetComponent<Mine>().openThisCell();

        //recursively calls all the function again on all it's neighbors:
        for (int xj = x - 1; xj < x + 2; xj++)
        {
            if (xj >= 0 && xj < map.GetLength(0))
                for (int yj = y - 1; yj < y + 2; yj++)
                {
                    if (yj >= 0 && yj < map.GetLength(1))
                        for (int zj = z - 1; zj < z + 2; zj++)
                        {
                            if (zj >= 0 && zj < map.GetLength(2))
                                for (int wj = w - 1; wj < w + 2; wj++)
                                {
                                    if (wj >= 0 && wj < map.GetLength(3))
                                        if (!((xj == x) && (yj == y) && (zj == z) && (wj == w)))
                                            openCellRecursive(xj, yj, zj, wj);
                                }
                        }
                }
        }
        return;
    }


    // armed mine updates all its neighbors about itself being armed:
    public void updateNeighbors(int x, int y, int z, int w)
    {

        for (int xi = x - 1; xi < x + 2; xi++)
        {
            if (xi >= 0 && xi < map.GetLength(0))
                for (int yi = y - 1; yi < y + 2; yi++)
                {
                    if (yi >= 0 && yi < map.GetLength(1))
                        for (int zi = z - 1; zi < z + 2; zi++)
                        {
                            if (zi >= 0 && zi < map.GetLength(2))
                                for (int wi = w - 1; wi < w + 2; wi++)
                                {
                                    if (wi >= 0 && wi < map.GetLength(3))
                                        if (!((xi == x) && (yi == y) && (zi == z) && (wi == w)))
                                            map[xi, yi, zi, wi].GetComponent<Mine>().incNeighborCount();

                                }
                        }
                }
        }

    }

    private bool IsMineOutOfBounds(int x, int y, int z, int w)
    {
        if ((x >= 0 && x < map.GetLength(0)
                && y >= 0 && y < map.GetLength(1)
                && z >= 0 && z < map.GetLength(2)
                && w >= 0 && w < map.GetLength(3)))
            return false;
        return true;
    }

}