using UnityEngine;

public class Controller : MonoBehaviour
{   public Pawn pawn; // Reference to the Pawn

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Initialization logic here
      
    }

    // Update is called once per frame
    void Update()
    { if (Input.GetKey(KeyCode.W))
        {
            pawn.MoveForward();
        }
        if (Input.GetKey(KeyCode.S))
        {
            pawn.MoveBackward();
        }

        if (Input.GetKey(KeyCode.A))
        {
            pawn.RotateLeft();
        }   
        if (Input.GetKey(KeyCode.D))
        {
            pawn.RotateRight();
        }   
        // Example logic to avoid empty Update
        // You can replace this with your actual update logic
        if (Input.anyKey)
        {
            Debug.Log("A key was pressed.");
        }
    }
}
