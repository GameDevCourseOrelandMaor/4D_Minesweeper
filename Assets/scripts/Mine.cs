using UnityEngine;
using System.Collections;
using TMPro;

public class Mine : MonoBehaviour
{
    [Tooltip("Mine's position (only for display)")]
    [SerializeField] private float xPos;
    [SerializeField] private float yPos;
    [SerializeField] private float zPos;
    [SerializeField] private float wPos;

    //Mine's attributes:
    [SerializeField] private bool is_Opened;
    [SerializeField] private bool is_Flagged;
    [SerializeField] private bool is_Armed;
    [SerializeField] private int neighborCount;
    [SerializeField] private GameObject prefabMine;

    // Properties for private variables
    public float XPos
    {
        get { return xPos; }
        set { xPos = value; }
    }

    public float YPos
    {
        get { return yPos; }
        set { yPos = value; }
    }

    public float ZPos
    {
        get { return zPos; }
        set { zPos = value; }
    }

    public float WPos
    {
        get { return wPos; }
        set { wPos = value; }
    }

    public bool IsOpened
    {
        get { return is_Opened; }
        set { is_Opened = value; }
    }

    public bool IsFlagged
    {
        get { return is_Flagged; }
        set { is_Flagged = value; }
    }

    public bool IsArmed
    {
        get { return is_Armed; }
        set { is_Armed = value; }
    }
    public void putMine()
    {
       is_Armed = true;
        Debug.Log("in func putMine, armed " + xPos + ", " + yPos + ", " + zPos+
            " And location: " + GetComponent<Transform>().position.x + 
            ", " + GetComponent<Transform>().position.y + ", " + GetComponent<Transform>().position.z + ", ");
    }

    public int NeighborCount
    {
        get { return neighborCount; }
        set { neighborCount = value; }
    }

    public GameObject PrefabMine
    {
        get { return prefabMine; }
        set { prefabMine = value; }
    }

    //References to objects and components:
    [SerializeField] private Grid gridRef; // reference to the grid
    [SerializeField] private GameObject grid; // to attach the grid object in game
    [SerializeField] private GameObject mesh; // to attach the physical appearance of the mine
    [SerializeField] private GameObject numOfNeighbors; // to attach the physical appearance of the mine
    void Start()
    {
        //Debug.Log("Mine script is running");
        grid = GameObject.Find("Grid"); // find the grid object by name

        //Debug.Log("instanciating a mine");
       /* this.is_Opened = false;
        this.is_Flagged = false;
        this.is_Armed = false;
        this.neighborCount = 0;*/
        xPos = GetComponent<Transform>().position.x;
        yPos = GetComponent<Transform>().position.y;
        zPos = GetComponent<Transform>().position.z;

    }



   /*
    public bool isArmed()
    {
        return is_Armed;
    }
    public bool isOpened()
    {
        return this.is_Opened;
    }
    public bool isFlagged()
    {
        return this.is_Flagged;
    }*/

    public void flagMine()
    {
        this.is_Flagged = true;
    }

    public void incNeighborCount()
    {
        neighborCount++;
        //numOfNeighbors.GetComponent<Mine>().updateNeighborcountVisuals();
    }
    public int getNeighborCount()
    {
        return neighborCount;
    }
    public void openThisCell()
    {
        is_Opened = true;
        mesh.GetComponent<MeshRenderer>().enabled = false;
        numOfNeighbors.GetComponent<TextMeshPro>().text = getNeighborCount().ToString();
    }

    void OnMouseUp()
    {
        Debug.Log("Mouse clicked on Mine: " + GetComponent<Transform>().position.x
          + ", " + GetComponent<Transform>().position.y
          + ", " + GetComponent<Transform>().position.z);

        grid.GetComponent<Grid>().openCell((int)xPos, (int)yPos, (int)zPos, (int)wPos);

    }

    public void openMine() 
    {
        //mesh.GetComponent<MeshRenderer>().enabled = false;
    }

    public void updateNeighborcountVisuals() 
    {
        //numOfNeighbors.GetComponent<NumOfNeigbors>().SetNumber();
    }
    
    // Update is called once per frame
    /*   private void Update()
       {}
    */
}

