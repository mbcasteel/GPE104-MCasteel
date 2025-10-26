using UnityEngine;


public class ColorChanger : MonoBehaviour
{
    // Create a variable to hold our sprite renderer component
    private SpriteRenderer spriteRenderer;
    public Color spriteColor = Color.yellow;

    // Awake runs when an object is created
    private void Awake()
    {
        // Load our sprite renderer into that variable 
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("Error! Sprite Renderer is null");
        }
        else
        {
            // Change the color of our sprite to variable color
            spriteRenderer.color = spriteColor;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Do this the first frame that the P key is pressed down
            // Change to a Random Color
            spriteRenderer.color = GetRandomColor();
        }
    }

    private Color GetRandomColor()
    {
        Color color;
        color.r = Random.Range(0.0f, 1.0f);
        color.g = Random.Range(0.0f, 1.0f);
        color.b = Random.Range(0.0f, 1.0f);
        color.a = 1.0f;

        return color;
    }
}