using UnityEngine;

public class DiskBluebwwy : MonoBehaviour
{
    public GameObject blueberryPrefab1;

    public Animator decorAnim;

    public Transform blueberryParent;

    public GameObject blue1;

    //bool check = false;

    private void OnMouseDown()
    {
        if (!blue1.activeSelf)
        {
            InstantiateBlueberry(blueberryPrefab1);
        }
        //else
        //{
        //    if (!blue2.activeSelf)
        //    {
        //        InstantiateBlueberry(blueberryPrefab1);
        //    }
        //}
    }

    //private void Update()
    //{
    //    if (!check && blue1.activeSelf)
    //    {
    //        check = true;
    //        decorAnim.SetBool("final", true);
    //    }
    //}

    void InstantiateBlueberry(GameObject prefab)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        GameObject newBlueberry = Instantiate(prefab, worldPos, Quaternion.identity, blueberryParent);

        BluebwwyDragAndDrop dragScript = newBlueberry.GetComponent<BluebwwyDragAndDrop>();
        if (dragScript != null)
        {
            dragScript.BeginDrag(worldPos);
        }
    }


}
