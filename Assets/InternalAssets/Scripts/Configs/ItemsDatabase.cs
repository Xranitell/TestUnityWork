using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsDatabase", menuName = "ScriptableObjects/Items Database")]
public class ItemsDatabase : ScriptableObject
{
    private static Dictionary<RarityType, Color> Rarity = new Dictionary<RarityType, Color> {
        { RarityType.Common, Color.white },
        { RarityType.Uncommon, Color.yellow },
        { RarityType.Rare, Color.green },
        { RarityType.Epic, Color.blue },
        { RarityType.Legendary, Color.magenta }
    };

    [SerializeField] private List<Item> Items = new List<Item>();

    public Item GetItemById(string id)
    {
        return Items.Find(x => x.id == id);
    }

    public List<string> GetItemsIdsListByChance(int count)
    {
        List<string> result = new List<string>();
        int summaryChances = Items.Sum(x => x.dropChance);

        for (int i = 0; i < count; i++)
        {
            int randomValue = UnityEngine.Random.Range(0, summaryChances);
            int currentSum = 0;

            foreach (var item in Items)
            {
                currentSum += item.dropChance;
                if (randomValue < currentSum)
                {
                    result.Add(item.id);
                    break;
                }
            }
        }

        return result.GetRange(0, count);
    }

    public static Color GetColorByRarity(RarityType type)
    {
        return Rarity[type];
    }
}

[Serializable]
public class Item
{
    public string id;
    public string name;
    public Sprite icon;
    public RarityType rarity;
    [Range(0, 100)] public int dropChance;
}

public enum RarityType
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}
