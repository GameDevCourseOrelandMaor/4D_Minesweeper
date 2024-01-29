using UnityEngine;
using System.Collections;



public class Grid : MonoBehaviour
{

    [SerializeField] private Mine prefabToSpawn;

    //Grid attributes:
    public GameObject[,,,] map;

    [Tooltip("The dimensions of the mine grid")]
    [SerializeField] private int xdim = 1, ydim = 1, zdim = 1, wdim = 1;

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

                        //map[x, y, z, w] = new Cell(x, y, z, w, prefabMine);
                        Debug.Log("created mine[ " + x + "," + y + "," + z + "," + w + " ]");
                    }
                }
            }
        }

        int numOfMines = (int)(map.GetLength(0) * map.GetLength(1) * map.GetLength(2) * map.GetLength(3) * 0.2);
        Debug.Log("number of mines: " + numOfMines);
        setMines(numOfMines);

    }

    public void setMines(int n)
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
            if (!map[x, y, z, w].GetComponent<Mine>().IsArmed)
            {
                Debug.Log("No, arming mine...");
                Mine tempRef =map[x, y, z, w].GetComponent<Mine>();
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
    }

    // opens the cell
    public void openCell(int x, int y, int z, int w)
    {
        Debug.Log("in openCell, opening mine: " + x + ", " + y + ", " + z + ", " + w);

        if (!(x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1) && z >= 0 && z < map.GetLength(2) && w >= 0 && w < map.GetLength(3)))
        {
            //System.out.println("invalid cordinates, please try again");
            Debug.Log("invalid cordinates, please try again");
            //GetComponent<Transform>().position = new Vector3(1, 1, 0);
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

    public void openCellRecursive(int x, int y, int z, int w)
    {
        // if Mine is out of map bounds then return
        if (!(x >= 0 && x < map.GetLength(0)
            && y >= 0 && y < map.GetLength(1)
            && z >= 0 && z < map.GetLength(2)
            && w >= 0 && w < map.GetLength(3)))
        {
            return;
        }
        if (map[x, y, z, w].GetComponent<Mine>().IsArmed)
            return;
        if (map[x, y, z, w].GetComponent<Mine>().getNeighborCount() != 0)
        {
            map[x, y, z, w].GetComponent<Mine>().openThisCell();
            return;
        }
        if (map[x, y, z, w].GetComponent<Mine>().IsOpened)
            return;
        //last case is that we hit a 0 mines cell:
        map[x, y, z, w].GetComponent<Mine>().openThisCell();
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
                                            openCellRecursive(xi, yi, zi, wi);
                                }
                        }
                }
        }
        return;
    }


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
                                        //TODO: REPLACE IF STATEMENT WITH DEC-NEIGHBOR
                                        if (!((xi == x) && (yi == y) && (zi == z) && (wi == w)))
                                            map[xi, yi, zi, wi].GetComponent<Mine>().incNeighborCount();

                                }
                        }
                }
        }

    }

}

/* public string toString()
  {
      string s = "";
      for (int x = 0; x < map.GetLength(0); x++)
      {
          s = s + "\n" + "cube number " + x.ToString() + "\n";
          for (int y = 0; y < map.GetLength(1); y++)
          {
              s = s + "\n"; //+"depth = "+Integer.toString(y)+"\n";

              for (int z = 0; z < map.GetLength(2); z++)
              {
                  for (int w = 0; w < map.GetLength(3); w++)
                  {
                      if (map[x, y, z, w].isArmed())
                          s = s + "X";
                      else { s = s + "#"; }
                  }
                  s = s + "\n";
              }
          }
          s = s + "\n";

      }
      return s;
  }*/

/* public string showMapDev()
 {
     string s = "";
     for (int x = 0; x < map.GetLength(0); x++)
     {
         s = s + "\n" + "cube number " + (x + 1).ToString() + "\n";
         for (int y = 0; y < map.GetLength(1); y++)
         {
             s = s + "\n"; //+"depth = "+Integer.toString(y)+"\n";

             for (int z = 0; z < map.GetLength(2); z++)
             {

                 for (int w = 0; w < map.GetLength(3); w++)
                 {
                     //	if(map[x][y][z][w].isOpened)
                     if (map[x, y, z, w].isArmed())
                         s = s + "X | ";

                     else { s = s + map[x, y, z, w].getNeighborCount().ToString() + " | "; }
                 }
                 s = s + "\n";
             }
         }
         s = s + "\n";

     }
     return s;

 }*/
/*  public string showMap()
  {
      string s = "";
      for (int x = 0; x < map.GetLength(0); x++)
      {
          s = s + "\n" + "cube number " + (x + 1).ToString() + "\n";
          for (int y = 0; y < map.GetLength(1); y++)
          {
              s = s + "\n"; //+"depth = "+Integer.toString(y)+"\n";

              for (int z = 0; z < map.GetLength(2); z++)
              {

                  for (int w = 0; w < map.GetLength(3); w++)
                  {
                      if (map[x, y, z, w].isOpened())
                      {
                          if (map[x, y, z, w].isArmed())
                              s = s + "X | ";


                          else { s = s + map[x, y, z, w].getNeighborCount().ToString() + " | "; }
                      }
                      else if (map[x, y, z, w].isFlagged())
                          s = s + "F" + " | ";
                      else { s = s + "#" + " | "; }
                  }
                  s = s + "\n";
              }
          }
          s = s + "\n";

      }
      return s;

  }*/



/*public void flagCell(int x, int y, int z, int w)
{
    map[x, y, z, w].flagMine();

}*/

/*   public void setMinesAtRandom()
   {
       for (int x = 0; x < map.GetLength(0); x++)
       {
           for (int y = 0; y < map.GetLength(1); y++)
           {
               for (int z = 0; z < map.GetLength(2); z++)
               {
                   for (int w = 0; w < map.GetLength(3); w++)
                   {
                       if (Random.Range(0, 1) > 0.9)
                       {
                           map[x, y, z, w].putMine();
                           //updates neighbors
                           updateNeighbors(x, y, z, w);


                       }

                   }
               }
           }
       }
   }*/