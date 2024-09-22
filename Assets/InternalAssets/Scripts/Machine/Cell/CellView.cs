using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InternalAssets
{
    public class CellView: MonoBehaviour
    {
        [SerializeField] private TMP_Text itemName;
        [SerializeField] private Image itemIcon;
        [SerializeField] private Image itemRarity;

        public void UpdateVisual(Item item)
        {
            itemName.text = item.name;
            itemIcon.sprite = item.icon;
            itemRarity.color = ItemsDatabase.GetColorByRarity(item.rarity);
        }
    }
}