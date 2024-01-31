using TMPro;
using UnityEngine;


// for visualizing the number of neighboring armed mines in game:
[RequireComponent(typeof(TextMeshPro))]
public class NumOfNeigbors : MonoBehaviour
{
    public void SetNumber(int newNumber)
    {
        GetComponent<TextMeshPro>().text = newNumber.ToString();
    }
}
