using TMPro;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private GameObject[] itemObjects;
    [SerializeField] private TextMeshProUGUI[] qtyTexts;

    private void Update()
    {
        for (int i = 0; i < itemObjects.Length; i++)
        {
            int qty = gameManager.GetItemQuantity(i);

            itemObjects[i].SetActive(qty > 0);

            if (qty > 0)
            {
                qtyTexts[i].text = qty + "X";
            }
        }
    }
}