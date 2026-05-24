using UnityEngine;

public class WeightScaleManager : MonoBehaviour
{
    // These create empty slots in the Unity Inspector. 
    // We will drag our LeftSide and RightSide objects into these slots.
    public ScaleSide leftSide;
    public ScaleSide rightSide;

    // It keeps track of whether the game is already won.
    private bool hasWon = false;
    // Update is called once every single frame of the game.
    void Update()
    {
        if (hasWon == true)
        {
            return;
        }
        float left = leftSide.currentWeight;
        float right = rightSide.currentWeight;
        // We look at the 'currentWeight' variable from the scripts on both sides.
        if (left > right)
        {
            Debug.Log("The LEFT side is heavier!");
        }
        else if (right > left)
        {
            Debug.Log("The RIGHT side is heavier!");
        }
        else // If left is NOT greater than right, and right is NOT greater than left... they must be equal!
        {
            // 2. Are they equal because they are perfectly balanced, or because they are empty?
            if (left > 0 && right > 0)
            {
                TriggerWinCondition();
            }
            else
            {
                Debug.Log("The scale is empty.");
            }
        }
        void TriggerWinCondition()
        {
            // 3. Lock the game so this function can't be called again
            hasWon = true;

            // 4. Celebrate!
            Debug.Log(" YOU WIN! The scale is perfectly balanced! ");

            // Later, you can add code right here to play a victory sound, 
            // load the next level, or show a UI screen.
        }
    }
}

