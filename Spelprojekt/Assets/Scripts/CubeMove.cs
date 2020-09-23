using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{

    public float myMovementSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move(Input.GetAxis("Horizontal"));
    }

    private void Move(float someInput)
    {
        transform.Translate(new Vector2(someInput * myMovementSpeed * Time.deltaTime, 0));
    }
}
