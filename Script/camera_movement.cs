using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_movement : MonoBehaviour
{
    // Start is called before the first frame update
    
    public GameObject mainCharacter;
    private Vector3 cameraPosition;
    private Vector3 characterPosition;
    private Vector3 distance;

    void Start()
    {
        cameraPosition = this.transform.position;
        characterPosition = mainCharacter.transform.position;
        distance = characterPosition - cameraPosition;
        //Debug.Log("Distance" + distance);

    }

    // Update is called once per frame
    void Update()
    {
       

    }

    private void LateUpdate()
    {
        //Debug.Log("lateupdate");
        characterPosition = mainCharacter.transform.position; //har lahze posiotion update she
        cameraPosition = characterPosition - distance;
        this.transform.position = cameraPosition;
    }
}
