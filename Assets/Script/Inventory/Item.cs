using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public bool isEquipable; // Can the item be equipped?
    
    // Stats that the item modifies
    public float maxHpModifier;
    public float damageModifier;
    public float moveSpeedModifier;
    public float frameDurationModifier;
    public float bulletSpeedModifier;
    public float fireRateModifier;
    public float bulletLifespanModifier;
}
