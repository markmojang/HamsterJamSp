using UnityEngine;
using TMPro;

public class PlayerUpgrades : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerShooter playerShooter;
    [SerializeField] private TextMeshProUGUI currencyText;

    private int currency = 0;
    private int maxHPUpgradeCost = 10;
    private int damageUpgradeCost = 10;
    private int moveSpeedUpgradeCost = 10;
    private int bulletSpeedUpgradeCost = 10;

    private const int costIncrement = 5; // Increment amount for each upgrade

    void Start()
    {
        UpdateCurrencyDisplay();
    }

    public void IncreaseMaxHP()
    {
        if (currency >= maxHPUpgradeCost)
        {
            playerController.maxhp += 50;
            playerController.health = playerController.maxhp; // Restore health to new max
            currency -= maxHPUpgradeCost;
            UpdateCurrencyDisplay();
            maxHPUpgradeCost += costIncrement; // Increase cost for this specific upgrade
        }
    }

    public void IncreaseDamage()
    {
        if (currency >= damageUpgradeCost)
        {
            playerController.Damage += 20;
            currency -= damageUpgradeCost;
            UpdateCurrencyDisplay();
            damageUpgradeCost += costIncrement; // Increase cost for this specific upgrade
        }
    }

    public void IncreaseMoveSpeed()
    {
        if (currency >= moveSpeedUpgradeCost)
        {
            playerController.moveSpeed += 1;
            currency -= moveSpeedUpgradeCost;
            UpdateCurrencyDisplay();
            moveSpeedUpgradeCost += costIncrement; // Increase cost for this specific upgrade
        }
    }

    public void IncreaseBulletSpeed()
    {
        if (currency >= bulletSpeedUpgradeCost)
        {
            playerShooter.bulletSpeed += 2;
            playerShooter.fireRate = Mathf.Max(0.1f, playerShooter.fireRate - 0.05f); // Ensure fireRate doesn't go below 0.1
            currency -= bulletSpeedUpgradeCost;
            UpdateCurrencyDisplay();
            bulletSpeedUpgradeCost += costIncrement; // Increase cost for this specific upgrade
        }
    }

    public void AddCurrency(int amount)
    {
        currency += amount;
        UpdateCurrencyDisplay();
    }

    private void UpdateCurrencyDisplay()
    {
        currencyText.text = "Gold : " + currency.ToString();
    }
}
