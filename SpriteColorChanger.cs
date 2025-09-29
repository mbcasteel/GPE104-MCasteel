using UnityEditor.Experimental;
using UnityEditor;
using UnityEngine;

public class SpriteColorChanger: MonoBehaviour
{
    // Declare our variables
        private SpriteRenderer theRenderer; // Variable for our renderer
        public Color spriteColor; //variable for our color

    // Use this for initialization
    void Start()
    {
        //Load the SpriteRenderer component form the same object this component is on.
        theRenderer = gameObject.GetComponent<SpriteRenderer>();
        //Change the sprite color from our color picker so that the alpha is 1
        spriteColor.a = 1.0f;
        // As long as theRenderer has been set
        if (theRenderer != null)
            //Change the "color" value of the SpriteRenderer component to our new color
            theRenderer.color = spriteColor;
    
    
    }

    // Update is called once per frame
    void Update()
    {
        

    }

}
