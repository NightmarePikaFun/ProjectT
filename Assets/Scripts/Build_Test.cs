using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build_Test : MonoBehaviour
{
    [SerializeField]
    private float gridSize = 1.0f;
    [SerializeField]
    private GameObject ghostBlock;

    private Vector3 worldPosition;
    Plane plane = new Plane(Vector3.up, 0);

    private bool canBuild;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            canBuild = !canBuild;
        }
        if(canBuild)
        {
            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(plane.Raycast(ray, out distance))
            {
                worldPosition = ray.GetPoint(distance);
            }
            //TODO add height calc
            worldPosition = VectorRound(worldPosition);
            worldPosition.y = GenerateChank.Instance.GetHeight(new Vector2Int((int)worldPosition.x, (int)worldPosition.z))+0.5f;
            ghostBlock.transform.position = worldPosition;
            if(Input.GetKeyDown(KeyCode.H)) 
            {
                GenerateChank.Instance.UpHeight(new Vector2Int((int)worldPosition.x, (int)worldPosition.z));
            }

        }
    }

    private Vector3 VectorRound(Vector3 inputVector)
    {

        inputVector.x = Mathf.Round(inputVector.x / gridSize) * gridSize - 0.5f;
        inputVector.z = Mathf.Round(inputVector.z / gridSize) * gridSize - 0.5f;
        return inputVector;
    }
}
