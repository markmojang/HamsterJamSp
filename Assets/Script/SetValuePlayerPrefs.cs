using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetValuePlayerPrefs : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("PmaxHp")){
            PlayerPrefs.SetFloat("PmaxHp", 100f);
            PlayerPrefs.Save();
        }
        if(!PlayerPrefs.HasKey("PDamage")){
            PlayerPrefs.SetFloat("PDamage", 100f);
            PlayerPrefs.Save();
        }
        if(!PlayerPrefs.HasKey("PMoveSpeed")){
            PlayerPrefs.SetFloat("PMoveSpeed", 13f);
            PlayerPrefs.Save();
        }
        if(!PlayerPrefs.HasKey("PFirerate")){
            PlayerPrefs.SetFloat("PFirerate", 0.5f);
            PlayerPrefs.Save();
        }
        if(!PlayerPrefs.HasKey("PVelocity")){
            PlayerPrefs.SetFloat("PVelocity", 25f);
            PlayerPrefs.Save();
        }
        if(!PlayerPrefs.HasKey("PChips")){
            PlayerPrefs.SetInt("PChips", 0);
            PlayerPrefs.Save();
        }
        if(!PlayerPrefs.HasKey("UpgradeHp")){
            PlayerPrefs.SetInt("UpgradeHp", 10);
            PlayerPrefs.Save();
        }
        if(!PlayerPrefs.HasKey("UpgradeDmg")){
            PlayerPrefs.SetInt("UpgradeDmg", 10);
            PlayerPrefs.Save();
        }
        if(!PlayerPrefs.HasKey("UpgradeSpeed")){
            PlayerPrefs.SetInt("UpgradeSpeed", 10);
            PlayerPrefs.Save();
        }
        if(!PlayerPrefs.HasKey("UpgradeFirerate")){
            PlayerPrefs.SetInt("UpgradeFirerate", 10);
            PlayerPrefs.Save();
        }
    }


}
