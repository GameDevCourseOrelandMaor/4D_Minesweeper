// using UnityEngine;
// using System.Collections;

// using MinesNameSpace;


// public class mineGen : MonoBehaviour
// {
//     public GameObject mine; // Reference to the prefab or object you want to instantiate

//     void Start()
//     {
// 				Debug.Log("start is working on mineGen class");

//         // Instantiate the object at a specific position and rotation
//         GameObject newMine = (GameObject)Instantiate(mine, new Vector3(0f, 0f, 0f), Quaternion.identity);

//         // Optionally, you can modify properties of the instantiated object
//         // For example, you can access and modify its transform, renderer, or any other component attached to it

//         // Example: Change the scale of the new object
//         newMine.transform.localScale = new Vector3(2f, 2f, 2f);
//     }
// }
