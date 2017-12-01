using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    //The abilities 2 and 3 are commented out because they have nothing attached to them yet.
    //Need to make a null check for them
    //Or something like an empty ability. As an ability.

    //Abilitiy tracking
    private float Ability1Cooldown = 0;
    private float Ability2Cooldown = 0;
    private float Ability3Cooldown = 0;
    private float AbilityDefensiveCooldown = 0;
    private float Ability1CooldownPercent = 0;
    private float Ability2CooldownPercent = 0;
    private float Ability3CooldownPercent = 0;
    private float AbilityDefensiveCooldownPercent = 0;


    //Gameobjects for the UI
    public GameObject HealthBarUI;
    public GameObject ShieldBarUI;
    public GameObject Ability1UI;
    public GameObject Ability2UI;
    public GameObject Ability3UI;
    public GameObject AbilityDefensiveUI;

    //Stat tracking, to get health and shield from
    private CharacterStats PlayerStats;
    private uint CurrentHealth;
    private uint MaxHealth;
    private uint CurrentShield;
    private uint MaxShield;
    

    // Use this for initialization
    void Start()
    {
        //Set start values for player
        PlayerStats = GetComponentInParent<CharacterStats>();
        UpdateUIInfo();

    }

    // Update is called once per frame
    void LateUpdate()
    {     
        UpdateUIInfo();

        //Could make this a function to call them all if you really wanted too
        UpdateText(HealthBarUI, CurrentHealth);
        UpdateText(ShieldBarUI, CurrentShield);
        //Have an if statement to change the text to just be " " if the CD == MaxCD Time.
        //That way it doesn't show a number when it's at max. Or we can change the text to == something else to help the player
        UpdateText(Ability1UI, (int)Ability1Cooldown);
        //UpdateText(Ability2UI, Ability2Cooldown);
        //UpdateText(Ability3UI, Ability3Cooldown);
        UpdateText(AbilityDefensiveUI, (int)AbilityDefensiveCooldown);

        //Could also make this a function as well.
        UpdateFillAmount(HealthBarUI, (float)CurrentHealth / (float)MaxHealth);
        UpdateFillAmount(ShieldBarUI, (float)CurrentShield / (float)MaxShield);
        UpdateFillAmount(Ability1UI,  Ability1CooldownPercent);
        //UpdateFillAmount(Ability2UI, Ability2CooldownPercent);
        //UpdateFillAmount(Ability3UI, Ability3CooldownPercent);
        UpdateFillAmount(AbilityDefensiveUI, AbilityDefensiveCooldownPercent);


    }

    //Created overrides to make it easier
    //Takes gameobject (UI Object), and a new text for it
    void UpdateText(GameObject Object, string NewText)
    {
        Object.GetComponentsInChildren<Text>()[0].text = NewText;
    }

    void UpdateText(GameObject Object, uint NewText)
    {
        Object.GetComponentsInChildren<Text>()[0].text = NewText.ToString();
    }

    void UpdateText(GameObject Object, int NewText)
    {
        Object.GetComponentsInChildren<Text>()[0].text = NewText.ToString();
    }

    void UpdateText(GameObject Object, float NewText)
    {
        Object.GetComponentsInChildren<Text>()[0].text = NewText.ToString();
    }


    //Updates/Refreshes the current stats, do this every frame to get accurate reading, helps reduce line clutter
    void UpdateUIInfo()
    {
        CurrentHealth = PlayerStats.Health;
        MaxHealth = PlayerStats.MaxHealth;

        CurrentShield = PlayerStats.Shield;
        MaxShield = PlayerStats.MaxShield;

        //Basic attack is index 0, Index 4 is knockback
        Ability1Cooldown = PlayerStats.Abilities[1].GetCoolDownTime();
        //Ability2Cooldown = PlayerStats.Abilities[2].GetCoolDownTime();
        //Ability3Cooldown = PlayerStats.Abilities[3].GetCoolDownTime();
        AbilityDefensiveCooldown = PlayerStats.Abilities[4].GetCoolDownTime();

        //Update the cooldown status for fill
        //Debug.Log(PlayerStats.Abilities[1].GetCoolDownTime());
        Ability1CooldownPercent = PlayerStats.Abilities[1].GetCoolDownTimePercent();
        //Ability2CooldownPercent = PlayerStats.Abilities[2].GetCoolDownTimePercent();
        //Ability3CooldownPercent = PlayerStats.Abilities[3].GetCoolDownTimePercent();
        AbilityDefensiveCooldownPercent = PlayerStats.Abilities[4].GetCoolDownTimePercent();

    }

    //Takes gameobject to access, and a percentage to fill the image/button
    void UpdateFillAmount(GameObject Object, float FillPercent)
    {
        Object.GetComponentInChildren<Image>().fillAmount = FillPercent;
    }

}
