using UnityEngine;
using UnityEngine.SceneManagement;


public class TowerTransitionManager : MonoBehaviour
{
    public string nextSceneName = "Level_02";
    //Hier kommt der Name von der Parkour Szene rein


    // So weiﬂ er welcher Turm welcher ist
    public Transform leftScale;
    public Transform rightScale;

    public void TransferTowerAndLoadScene()
    {
        GameObject leftContainer = new GameObject("SavedTower_Left");
        GameObject rightContainer = new GameObject("SavedTower_Right");


        GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("TowerBlock");

        foreach (GameObject block in allBlocks)
        {
            float distToLeft = Vector3.Distance(block.transform.position, leftScale.position);
            float distToRight = Vector3.Distance(block.transform.position, rightScale.position);

            if (distToLeft < distToRight)
            {
                block.transform.SetParent(leftContainer.transform, true);
            }
            else
            {
                block.transform.SetParent(rightContainer.transform, true);
            }
            Rigidbody rb = block.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Er beh‰lt sein Gewicht, bewegt sich aber nicht mehr von selbst.
               
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        rightContainer.tag = "PlayerTower";

        DontDestroyOnLoad(leftContainer);
        DontDestroyOnLoad(rightContainer);

        //SceneManager.LoadScene(nextSceneName);
    }

}
