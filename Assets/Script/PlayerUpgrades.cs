using UnityEngine;
using TMPro;
using UnityEngine.UI; // Include this for Button and UI management

public class PlayerUpgrades : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerShooter playerShooter;
    [SerializeField] private TextMeshProUGUI currencyText;

    [SerializeField] private Button maxHPUpgradeButton;
    [SerializeField] private TextMeshProUGUI maxHPUpgradeButtonText;
    [SerializeField] private Button damageUpgradeButton;
    [SerializeField] private TextMeshProUGUI damageUpgradeButtonText;
    [SerializeField] private Button moveSpeedUpgradeButton;
    [SerializeField] private TextMeshProUGUI moveSpeedUpgradeButtonText;
    [SerializeField] private Button bulletSpeedUpgradeButton;
    [SerializeField] private TextMeshProUGUI bulletSpeedUpgradeButtonText;

    private int currency = 0;
    private int maxHPUpgradeCost = 10;
    private int damageUpgradeCost = 10;
    private int moveSpeedUpgradeCost = 10;
    private int bulletSpeedUpgradeCost = 10;

    private const int costIncrement = 5; // Increment amount for each upgrade

    void Start()
    {
        currency = PlayerPrefs.GetInt("PChips");
            
        maxHPUpgradeCost = PlayerPrefs.GetInt("UpgradeHp");
        damageUpgradeCost = PlayerPrefs.GetInt("UpgradeDmg");
        moveSpeedUpgradeCost = PlayerPrefs.GetInt("UpgradeSpeed");
        bulletSpeedUpgradeCost = PlayerPrefs.GetInt("UpgradeFirerate");
        UpdateCurrencyDisplay();
        UpdateButtonLabels();
    }

    public void IncreaseMaxHP()
    {
        if (currency >= maxHPUpgradeCost)
        {
            playerController.maxhp += 25;
            playerController.health = playerController.maxhp; // Restore health to new max
            currency -= maxHPUpgradeCost;
            PlayerPrefs.SetInt("PChips", currency);
            PlayerPrefs.SetFloat("PmaxHp", playerController.maxhp);
            maxHPUpgradeCost += costIncrement; // Increase cost for this specific upgrade
            PlayerPrefs.SetInt("UpgradeHp", maxHPUpgradeCost);
            UpdateCurrencyDisplay();
            UpdateButtonLabels();
        }
    }

    public void IncreaseDamage()
    {
        if (currency >= damageUpgradeCost)
        {
            playerController.Damage += 20;
            currency -= damageUpgradeCost;
            PlayerPrefs.SetInt("PChips", currency);
            PlayerPrefs.SetFloat("PDamage", playerController.Damage);
            damageUpgradeCost += costIncrement; // Increase cost for this specific upgrade
            PlayerPrefs.SetInt("UpgradeDmg", damageUpgradeCost);
            UpdateCurrencyDisplay();
            UpdateButtonLabels();
        }
    }

    public void IncreaseMoveSpeed()
    {
        if (currency >= moveSpeedUpgradeCost)
        {
            playerController.moveSpeed += 1;
            currency -= moveSpeedUpgradeCost;
            PlayerPrefs.SetInt("PChips", currency);
            PlayerPrefs.SetFloat("PMoveSpeed", playerController.moveSpeed);
            moveSpeedUpgradeCost += costIncrement; // Increase cost for this specific upgrade
            PlayerPrefs.SetInt("UpgradeSpeed", moveSpeedUpgradeCost);
            UpdateCurrencyDisplay();
            UpdateButtonLabels();
        }
    }

    public void IncreaseBulletSpeed()
    {
        if (currency >= bulletSpeedUpgradeCost)
        {
            playerShooter.bulletSpeed += 2;
            playerShooter.fireRate = Mathf.Max(0.1f, playerShooter.fireRate - 0.05f); // Ensure fireRate doesn't go below 0.1
            currency -= bulletSpeedUpgradeCost;
            PlayerPrefs.SetFloat("PFirerate", playerShooter.fireRate);
            PlayerPrefs.SetFloat("PVelocity", playerShooter.bulletSpeed);
            PlayerPrefs.SetInt("PChips", currency);
            bulletSpeedUpgradeCost += costIncrement; // Increase cost for this specific upgrade
            PlayerPrefs.SetInt("UpgradeFirerate", bulletSpeedUpgradeCost);
            UpdateCurrencyDisplay();
            UpdateButtonLabels();
        }
    }

    public void AddCurrency(int amount)
    {
        currency += amount;
        PlayerPrefs.SetInt("PChips", currency);
        UpdateCurrencyDisplay();
    }

    private void UpdateCurrencyDisplay()
    {
        currencyText.text = "Chips: " + currency.ToString();
    }

    private void UpdateButtonLabels()
    {
        maxHPUpgradeButtonText.text = maxHPUpgradeCost.ToString();
        damageUpgradeButtonText.text = damageUpgradeCost.ToString();
        moveSpeedUpgradeButtonText.text = moveSpeedUpgradeCost.ToString();
        bulletSpeedUpgradeButtonText.text = bulletSpeedUpgradeCost.ToString();
    }
}