using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class NumOfNeigbors : MonoBehaviour
{
    public void SetNumber(int newNumber)
    {
        GetComponent<TextMeshPro>().text = newNumber.ToString();
    }

}
