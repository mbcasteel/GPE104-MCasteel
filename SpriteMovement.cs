using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Transform tf;//Create variable for our transform component
    public float minX = 0;

    public float maxX = 0;
   
    public float minY = 0;  

    public float maxY = 0;

    //use this for initialization 
    void Start()
    {
        //Load our transform component into our variable
        tf = GetComponent<Transform>();

        //start at the world origin
        tf.position = new Vector3(0, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        //If input key "W" is pressed function will be called
        if (Input.GetKeyDown(KeyCode.W))
        {
            //Pick a random x and y position within the defined range
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);

            //set new position to random point along x and y
            tf.position = new Vector3(randomX, randomY, 0f);
            //Print to console
            Debug.Log("slingshot engaged");
        }
    }
}
