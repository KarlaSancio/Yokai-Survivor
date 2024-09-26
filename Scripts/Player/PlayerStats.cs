using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;

    // Variáveis de status
    float currentHealth;
    float currentMight;
    float currentRecoveryItem;
    float currentProjectileSpeed;
    float currentMoveSpeed;
    float currentMagnet;


    #region Status Atuais
    public float CurrentHealth 
    {
        get { return currentHealth; }
        set 
        { 
            if(currentHealth != value)
            {
                currentHealth = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentHealthText.text = "Health: " + currentHealth;
                }
            }
        }
    }
    public float CurrentMight 
    {
        get { return currentMight; }
        set 
        { 
            if(currentMight != value)
            {
                currentMight = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentMightText.text = "Might: " + currentMight;
                }
            }
        }
    }

    public float CurrentRecoveryItem 
    {
        get { return currentRecoveryItem; }
        set 
        { 
            if(currentRecoveryItem != value)
            {
                currentRecoveryItem = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryItemText.text = "Recovery Item: " + currentRecoveryItem;
                }
            }
        }
    }

    public float CurrentProjectileSpeed 
    {
        get { return currentProjectileSpeed; }
        set 
        { 
            if(currentProjectileSpeed != value)
            {
                currentProjectileSpeed = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedText.text = "Projectile Speed: " + currentProjectileSpeed;
                }
            }
        }
    }   

    public float CurrentMoveSpeed 
    {
        get { return currentMoveSpeed; }
        set 
        { 
            if(currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedText.text = "Move Speed: " + currentMoveSpeed;
                }
            }
        }
    }

    public float CurrentMagnet 
    {
        get { return currentMagnet; }
        set 
        { 
            if(currentMagnet != value)
            {
                currentMagnet = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentMagnetText.text = "Magnet: " + currentMagnet;
                }
            }
        }
    }
    #endregion
   
    // Controle de xp e level
    [Header("XP/Level")]
    public int xp = 0;
    public int level = 1;
    public int xpCap;

    [System.Serializable] // Permite que a classe seja serializada pela Unity (apareça no inspector)
    public class LevelSpan 
    {
        public int startLevel;
        public int endLevel;
        public int xpCapIncrease;

    }

    // I-Frames -> É um tempo em que o player não pode ser atingido após ser atingido
    [Header("I-Frames")]
    public float invincibilityTime;
    float invincibilityTimer;
    bool isInvincible;


    public List<LevelSpan> levelSpans; // Armazena os diferentes intervalos de level e seus aumentos de xpCap

    InventoryManagment inventory;
    public int weaponIndex;
    public int passiveItemIndex;

    [Header("UI")]
    public Image healthBar;
    public Image xpBar;
    public Text levelText;




    public GameObject secondWeaponTest;
    public GameObject firstPassiveItemTest, secondPassiveItemTest;



    void Awake()
    {

        characterData = CharacterPicker.GetCharacterData(); // Pega os dados do personagem escolhido
        CharacterPicker.instance.DestroySingleton(); // Destrói o singleton do CharacterPicker

        inventory = GetComponent<InventoryManagment>();


        CurrentHealth = characterData.MaxH;
        CurrentMight = characterData.Might;
        CurrentRecoveryItem = characterData.RecoveryItem;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentMagnet = characterData.Magnet;

        // Instancia a arma inicial
        SpawnWeapon(characterData.StartWeapon);
        //SpawnWeapon(secondWeaponTest);
        //SpawnPassiveItem(firstPassiveItemTest);
        SpawnPassiveItem(secondPassiveItemTest);
    }


    void Start()
    {
        xpCap = levelSpans[0].xpCapIncrease; // Inicializa o xpCap com o valor do primeiro level span (0-1)

        // atualiza os textos de status (assim por enquanto, acho que tem uma maneira melhor de fazer isso)
        GameManager.instance.currentHealthText.text = "Health: " + currentHealth;
        GameManager.instance.currentMightText.text = "Might: " + currentMight;
        GameManager.instance.currentRecoveryItemText.text = "Recovery Item: " + currentRecoveryItem;
        GameManager.instance.currentProjectileSpeedText.text = "Projectile Speed: " + currentProjectileSpeed;
        GameManager.instance.currentMoveSpeedText.text = "Move Speed: " + currentMoveSpeed;
        GameManager.instance.currentMagnetText.text = "Magnet: " + currentMagnet;

        GameManager.instance.AssignChoosenCharacterUI(characterData);

        UpdateHealthBar();
        UpdateExperienceBar();
        UpdateLevelText();

    }

    void Update()
    {
        if(invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else if(isInvincible) // se o timer acabou e o player ainda está invencível, desativa a invencibilidade
        {
            isInvincible = false;
        }

        RecoverHealth();
    }

    public void AddXP(int xpToAdd)
    {
        xp += xpToAdd;
        LevelUpCheck();
        UpdateExperienceBar();
    }

    void LevelUpCheck()
    {
        if (xp >= xpCap)
        {
            level++;
            xp -= xpCap;

            int newXPcap = 0;
            foreach (LevelSpan span in levelSpans)
            {
                if (level >= span.startLevel && level <= span.endLevel)
                {
                    newXPcap = span.xpCapIncrease;
                    break;
                }
            }
            xpCap += newXPcap;

            UpdateLevelText();

            GameManager.instance.StartLevelUp();

        }
    }

    void UpdateExperienceBar()
    {
        xpBar.fillAmount = (float)xp / xpCap;
    }

    void UpdateLevelText()
    {
        levelText.text = "Level: " + level.ToString();
    }

    public void DamagePlayer(float damage)
    {
        if (!isInvincible)
        {
            CurrentHealth -= damage;

            invincibilityTimer = invincibilityTime;
            isInvincible = true;

            if (CurrentHealth <= 0)
            {
                KillPlayer();
            }

            UpdateHealthBar();
        }
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = CurrentHealth / characterData.MaxH;
    }

    public void KillPlayer()
    {
        if(!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.AssignChoosenWeaponsAndPassiveItensUI(inventory.weaponUIImages, inventory.passiveItemUIImages); // ver se ta certo
            GameManager.instance.GameOver();
        }
    }

    public void CureHealth(float qtd)
    {
        if(CurrentHealth < characterData.MaxH) // Se a vida atual for menor que a vida máxima, cura
        {
            CurrentHealth += qtd;

            if(CurrentHealth > characterData.MaxH) // Se a vida atual for maior que a vida máxima, seta a vida atual para a vida máxima
            {
                CurrentHealth = characterData.MaxH;
            }
        }
    }

    void RecoverHealth()
    {
        if(CurrentHealth < characterData.MaxH) // Se a vida atual for menor que a vida máxima, cura
        {
            CurrentHealth += CurrentRecoveryItem * Time.deltaTime;

            if(CurrentHealth > characterData.MaxH) // Se a vida atual for maior que a vida máxima, seta a vida atual para a vida máxima
            {
                CurrentHealth = characterData.MaxH;
            }
        }
    }

    public void SpawnWeapon(GameObject weapon)
    {
        if(weaponIndex >= inventory.weaponPlaces.Count - 1) // Se o index da arma for maior ou igual ao tamanho da lista de armas, nao adiciona
        {
            Debug.LogError("Inventario cheio");
            return;
        }

        // Instancia a arma e a coloca como filha do player
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform); 
        inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>()); // Adiciona a arma ao inventário

        weaponIndex++; // Precisa ser incrementado para que a proxima arma seja adicionada em outro slot (sem overlap)
    }

    public void SpawnPassiveItem(GameObject passiveItem)
    {
        if(passiveItemIndex >= inventory.passiveItemsPlaces.Count - 1) // Se o index da arma for maior ou igual ao tamanho da lista de armas, nao adiciona
        {
            Debug.LogError("Inventario cheio");
            return;
        }

        // Instancia o item passivo e o coloca como filho do player
        GameObject spawnedItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedItem.transform.SetParent(transform); 
        inventory.AddPassiveItem(passiveItemIndex, spawnedItem.GetComponent<PassiveItem>()); // Adiciona item passivo ao inventário

        passiveItemIndex++; // Precisa ser incrementado para que o proximo item seja adicionado em outro slot (sem overlap)
    }
    
}
