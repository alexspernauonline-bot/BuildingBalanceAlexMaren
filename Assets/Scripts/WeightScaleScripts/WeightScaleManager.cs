using UnityEngine;

public class WeightScaleManager : MonoBehaviour
{
    // These create empty slots in the Unity Inspector. 
    // We will drag our LeftSide and RightSide objects into these slots.
    public ScaleSide leftSide;
    public ScaleSide rightSide;

    // Update is called once every single frame of the game.
    void Update()
    {
        // We look at the 'currentWeight' variable from the scripts on both sides.
        if (leftSide.currentWeight > rightSide.currentWeight)
        {
            Debug.Log("The LEFT side is heavier!");

            // In the future, you can add code here to visually rotate or move the scale down on the left.
        }
        else if (rightSide.currentWeight > leftSide.currentWeight)
        {
            Debug.Log("The RIGHT side is heavier!");

            // In the future, you can add code here to visually rotate or move the scale down on the right.
        }
        else
        {
            Debug.Log("The scale is perfectly BALANCED.");
        }
    }
}

