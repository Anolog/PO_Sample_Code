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
    private float m_Ability1Cooldown = 0;
    private float m_Ability2Cooldown = 0;
    private float m_Ability3Cooldown = 0;
    private float m_AbilityDefensiveCooldown = 0;
    private float m_Ability1CooldownPercent = 0;
    private float m_Ability2CooldownPercent = 0;
    private float m_Ability3CooldownPercent = 0;
    private float m_AbilityDefensiveCooldownPercent = 0;


    //Gameobjects for the UI
    public GameObject HealthBarUI;
    public GameObject ShieldBarUI;
    public GameObject Ability1UI;
    public GameObject Ability2UI;
    public GameObject Ability3UI;
    public GameObject AbilityDefensiveUI;
    public GameObject Reticle;
    public GameObject Ability1UIBackground;
    public GameObject Ability2UIBackground;
    public GameObject Ability3UIBackground;
    public GameObject RevivePrompt;

    //Stat tracking, to get health and shield from
    private PlayerStats m_PlayerStats;
    private int m_CurrentHealth;
    private int m_MaxHealth;
    private int m_CurrentShield;
    private int m_MaxShield;

    //Colors for the Image change       Takes Hex values for RGB conversion
    private Color m_ChargeColor = new Color32(0x00, 0x00, 0xff, 0xff);
    private Color m_EnergyBombColor = new Color32(0x00, 0x80, 0x80, 0xff);
    private Color m_GroundShockColor = new Color32(0x80, 0x00, 0x80, 0xff);
    private Color m_WeaponSmashColor = new Color32(0xff, 0x00, 0x00, 0xff);
    private Color m_HealColor = new Color32(0x00, 0x80, 0x00, 0xff);
    private Color m_EnrageColor = new Color32(0xff, 0xa5, 0x00, 0xff);
    private Color m_ChainLightningColor = new Color32(0xfd, 0xd0, 0x23, 0xff);
    private Color m_VortexColor = new Color32(0x55, 0x1a, 0x8b, 0xff);
    private Color m_ProtectMeColor = new Color32(0x00, 0x34, 0x00, 0xff);

    //private Vector3 reticlePos;
    //private bool hasResized = false;

    // Use this for initialization
    void Start()
    {
        //Set start values for player
        m_PlayerStats = GetComponentInParent<PlayerStats>();
        UpdateUIInfo();

        //reticlePos = Reticle.transform.position;

        //Grab the ability name or something to represent it from PlayerStats.Abilitites
        //Use this to determine the color of the UI Icon
        for (int i = 1; i < 4; i++)
        {
            if (m_PlayerStats.Abilities[i].AbilityName == "Charge")
            {
                UpdateBackgroundColor(i, m_ChargeColor);
            }

            else if (m_PlayerStats.Abilities[i].AbilityName == "Energy Bomb")
            {
                UpdateBackgroundColor(i, m_EnergyBombColor);
            }

            else if (m_PlayerStats.Abilities[i].AbilityName == "Ground Shock")
            {
                UpdateBackgroundColor(i, m_GroundShockColor);
            }

            else if (m_PlayerStats.Abilities[i].AbilityName == "Weapon Smash")
            {
                UpdateBackgroundColor(i, m_WeaponSmashColor);
            }

            else if (m_PlayerStats.Abilities[i].AbilityName == "Heal")
            {
                UpdateBackgroundColor(i, m_HealColor);
            }

            else if (m_PlayerStats.Abilities[i].AbilityName == "Enrage")
            {
                UpdateBackgroundColor(i, m_EnrageColor);
            }

            else if (m_PlayerStats.Abilities[i].AbilityName == "Chain Lightning")
            {
                UpdateBackgroundColor(i, m_ChainLightningColor);
            }

            else if (m_PlayerStats.Abilities[i].AbilityName == "Vortex")
            {
                UpdateBackgroundColor(i, m_VortexColor);
            }
            else if (m_PlayerStats.Abilities[i].AbilityName == "Protect Me")
            {
                UpdateBackgroundColor(i, m_ProtectMeColor);
            }
        }


        
        //PlayerStats.Abilities[0].

    }

    // Update is called once per frame
    void LateUpdate()
    {     
        UpdateUIInfo();

        //Could make this a function to call them all if you really wanted too
        UpdateText(HealthBarUI, m_CurrentHealth);
        UpdateText(ShieldBarUI, m_CurrentShield);
        //Have an if statement to change the text to just be " " if the CD == MaxCD Time.
        //That way it doesn't show a number when it's at max. Or we can change the text to == something else to help the player
        UpdateText(Ability1UI, (int)m_Ability1Cooldown);
        UpdateText(Ability2UI, (int)m_Ability2Cooldown);
        UpdateText(Ability3UI, (int)m_Ability3Cooldown);
        UpdateText(AbilityDefensiveUI, (int)m_AbilityDefensiveCooldown);

        //Could also make this a function as well.
        UpdateFillAmount(HealthBarUI, (float)m_CurrentHealth / (float)m_MaxHealth);
        UpdateFillAmount(ShieldBarUI, (float)m_CurrentShield / (float)m_MaxShield);
        UpdateFillAmount(Ability1UI, m_Ability1CooldownPercent);
        UpdateFillAmount(Ability2UI, m_Ability2CooldownPercent);
        UpdateFillAmount(Ability3UI, m_Ability3CooldownPercent);
        UpdateFillAmount(AbilityDefensiveUI, m_AbilityDefensiveCooldownPercent);
    }

    //Created overrides to make it easier
    //Takes gameobject (UI Object), and a new text for it
    void UpdateText(GameObject Object, string newText)
    {
        Object.GetComponentsInChildren<Text>()[0].text = newText;
    }

    void UpdateText(GameObject Object, uint newText)
    {
        Object.GetComponentsInChildren<Text>()[0].text = newText.ToString();
    }

    void UpdateText(GameObject Object, int newText)
    {
        Object.GetComponentsInChildren<Text>()[0].text = newText.ToString();
    }

    void UpdateText(GameObject Object, float newText)
    {
        Object.GetComponentsInChildren<Text>()[0].text = newText.ToString();
    }


    //Updates/Refreshes the current stats, do this every frame to get accurate reading, helps reduce line clutter
    void UpdateUIInfo()
    {
        m_CurrentHealth = (int)m_PlayerStats.Health;
        m_MaxHealth = m_PlayerStats.MaxHealth;

        m_CurrentShield = m_PlayerStats.Shield;
        m_MaxShield = m_PlayerStats.MaxShield;

        //Basic attack is index 0, Index 4 is knockback
        m_Ability1Cooldown = m_PlayerStats.Abilities[1].GetCoolDownTime();
        m_Ability2Cooldown = m_PlayerStats.Abilities[2].GetCoolDownTime();
        m_Ability3Cooldown = m_PlayerStats.Abilities[3].GetCoolDownTime();
        m_AbilityDefensiveCooldown = m_PlayerStats.Abilities[4].GetCoolDownTime();

        //Update the cooldown status for fill
        //Debug.Log(PlayerStats.Abilities[1].GetCoolDownTime());
        m_Ability1CooldownPercent = m_PlayerStats.Abilities[1].GetCoolDownTimePercent();
        m_Ability2CooldownPercent = m_PlayerStats.Abilities[2].GetCoolDownTimePercent();
        m_Ability3CooldownPercent = m_PlayerStats.Abilities[3].GetCoolDownTimePercent();
        m_AbilityDefensiveCooldownPercent = m_PlayerStats.Abilities[4].GetCoolDownTimePercent();

    }

    //Takes gameobject to access, and a percentage to fill the image/button
    void UpdateFillAmount(GameObject Object, float fillPercent)
    {
        Object.GetComponentInChildren<Image>().fillAmount = fillPercent;
    }

    void UpdateBackgroundColor(int abilityNum, Color color)
    {
        if (abilityNum == 1)
        {
            Ability1UIBackground.GetComponent<Image>().color = color;
        }

        else if (abilityNum == 2)
        {
            Ability2UIBackground.GetComponent<Image>().color = color;
        }

        else if (abilityNum == 3)
        {
            Ability3UIBackground.GetComponent<Image>().color = color;
        }
    }
}
