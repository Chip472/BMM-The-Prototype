using UnityEngine;

public class IngredientTransition : MonoBehaviour
{
    public GameObject[] ingredient;

    public void IngredientOff()
    {
        for (int i = 0; i < ingredient.Length; i++)
        {
            if (ingredient[i].activeSelf)
            {
                ingredient[i].GetComponent<Collider2D>().enabled = false;
                ingredient[i].GetComponent<SpriteRenderer>().sortingLayerName = "DraggingItem";
            }
        }
        ingredient[0].GetComponent<DragAndDropScaleScript>().enabled = false;
        ingredient[3].GetComponent<DragAndDropScaleScript>().enabled = false;
    }

    public void IngredientOn()
    {
        for (int i = 0; i < ingredient.Length; i++)
        {
            if (ingredient[i].activeSelf)
            {
                ingredient[i].GetComponent<Collider2D>().enabled = true;
                ingredient[i].GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            }
        }
        ingredient[0].GetComponent<DragAndDropScaleScript>().enabled = true;
        ingredient[3].GetComponent<DragAndDropScaleScript>().enabled = true;
    }

}
