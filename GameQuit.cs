using UnityEngine;

public class GameQuitter : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
      if (Input.GetButtonDown ("quit")) {
        Application.Quit();
        }   
    }
}
