using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // Importa a biblioteca de UI

public class InventoryManagment : MonoBehaviour
{
    public List<WeaponController> weaponPlaces = new List<WeaponController>(6); // Essa lista tera 6 armas
    public int[] weaponIndex = new int[6]; 
    public List<Image> weaponUIImages = new List<Image>(6); // Essa lista tera 6 imagens de armas
    public List<PassiveItem> passiveItemsPlaces = new List<PassiveItem>(6); // Essa lista tera 6 itens passivos
    public int[] passiveItemsIndex = new int[6]; //Talvez seja melhor usar um dicionario para armazenar o index do item passivo(veremos)
    public List<Image> passiveItemUIImages = new List<Image>(6); // Essa lista tera 6 imagens de itens passivos

    [System.Serializable]
    public class WeaponUpgrade
    {
        public int weaponUpgradeIndex;
        public GameObject initialWeapon;
        public WeaponScriptableObject weaponStats;
    }

    [System.Serializable]
    public class PassiveItemUpgrade
    {
        public int passiveItemUpgradeIndex;
        public GameObject initialPassiveItem;
        public PassiveItemScriptableObject passiveItemData;

    }

    [System.Serializable]
    public class UpgradeUI
    {
        public Text upgradeNameDisplay;
        public Text upgradeDescriptionDisplay;
        public Image upgradeIconDisplay;
        public Button upgradeButton;

    }

    public List<WeaponUpgrade> weaponUpgradeOption = new List<WeaponUpgrade>();
    public List<PassiveItemUpgrade> passiveItemUpgradeOption = new List<PassiveItemUpgrade>();
    public List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>();

    PlayerStats player;

    void Start()
    {
        player = GetComponent<PlayerStats>();
    }

    // metodo para adicionar um armas ao inventario
    public void AddWeapon(int placeIndex, WeaponController weapon)
    {
        weaponPlaces[placeIndex] = weapon;
        weaponIndex[placeIndex] = weapon.weaponStats.Level; // Adiciona o level da arma ao index
        weaponUIImages[placeIndex].enabled = true; // Ativa a imagem da arma no UI
        weaponUIImages[placeIndex].sprite = weapon.weaponStats.Icon; // Atualiza a imagem da arma no UI

        if(GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();

        }
    }

    // metodo para adicionar um item passivo ao inventario
    public void AddPassiveItem(int placeIndex, PassiveItem passiveItem)
    {
        passiveItemsPlaces[placeIndex] = passiveItem;
        passiveItemsIndex[placeIndex] = passiveItem.passiveItemData.Level; // Adiciona o level do item passivo ao index
        passiveItemUIImages[placeIndex].enabled = true; // Ativa a imagem do item passivo no UI
        passiveItemUIImages[placeIndex].sprite = passiveItem.passiveItemData.Icon; // Atualiza a imagem do item passivo no UI

        if(GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void UpgradeWeapon(int placeIndex, int upgradeIndex)
    {
        if(weaponPlaces.Count > placeIndex)
        {
            WeaponController weapon = weaponPlaces[placeIndex];
            if(!weapon.weaponStats.NextLevelPrefab) // Se nao houver prox nivel, retorna
            {
                Debug.LogError("Nao ha prox nivel para " +weapon.name); 
                return; 
            } 
            GameObject upgradedWeapon = Instantiate(weapon.weaponStats.NextLevelPrefab, transform.position, Quaternion.identity); // Instancia a arma melhorada
            upgradedWeapon.transform.SetParent(transform); // Coloca a arma como filha do player
            AddWeapon(placeIndex, upgradedWeapon.GetComponent<WeaponController>()); // Adiciona a arma ao inventário
            Destroy(weapon.gameObject); // Destroi a arma antiga
            weaponIndex[placeIndex] = upgradedWeapon.GetComponent<WeaponController>().weaponStats.Level; // Atualiza o level da arma no index

            weaponUpgradeOption[upgradeIndex].weaponStats = upgradedWeapon.GetComponent<WeaponController>().weaponStats; // Atualiza a arma no index de upgrade

            if(GameManager.instance != null && GameManager.instance.choosingUpgrade)
            {
                GameManager.instance.EndLevelUp();
            }
        }   
    }

    public void UpgradePassiveItem(int placeIndex, int upgradeIndex)
    {
        if(passiveItemsPlaces.Count > placeIndex)
        {
            PassiveItem passiveItem = passiveItemsPlaces[placeIndex];
            if(!passiveItem.passiveItemData.NextLevelPrefab) // Se nao houver prox nivel, retorna
            {
                Debug.LogError("Nao ha prox nivel para " +passiveItem.name); 
                return; 
            } 
            GameObject upgradedPassiveItem = Instantiate(passiveItem.passiveItemData.NextLevelPrefab, transform.position, Quaternion.identity); // Instancia o item passivo melhorado
            upgradedPassiveItem.transform.SetParent(transform); // Coloca o item passivo como filho do player
            AddPassiveItem(placeIndex, upgradedPassiveItem.GetComponent<PassiveItem>()); // Adiciona item passivo ao inventário
            Destroy(passiveItem.gameObject); // Destroi a arma antiga
            passiveItemsIndex[placeIndex] = upgradedPassiveItem.GetComponent<PassiveItem>().passiveItemData.Level; // Atualiza o level do item passivo no index

            passiveItemUpgradeOption[upgradeIndex].passiveItemData = upgradedPassiveItem.GetComponent<PassiveItem>().passiveItemData; // Atualiza o item passivo no index de upgrade

            if(GameManager.instance != null && GameManager.instance.choosingUpgrade)
            {
                GameManager.instance.EndLevelUp();
            }
        }
    }

    void ApplyUpgradeOptions() 
    {
        List<WeaponUpgrade> availableWeaponUpgrades = new List<WeaponUpgrade>(weaponUpgradeOption);
        List<PassiveItemUpgrade> availablePassiveItemUpgrades = new List<PassiveItemUpgrade>(passiveItemUpgradeOption);

        foreach(var upgradeOption in upgradeUIOptions)
        {

            
            if(availableWeaponUpgrades.Count == 0 && availablePassiveItemUpgrades.Count == 0)
            {
                return;
            }
            int upgradeType;
            if(availableWeaponUpgrades.Count == 0)
            {
                upgradeType = 2;
            }
            else if(availablePassiveItemUpgrades.Count == 0)
            {
                upgradeType = 1;
            }
            else
            {
                upgradeType = Random.Range(1, 3);
            }

            if (upgradeType == 1)
            {
                WeaponUpgrade chosenWeaponUpgrade = availableWeaponUpgrades[Random.Range(0, availableWeaponUpgrades.Count)]; // Escolhe uma arma aleatoria

                availableWeaponUpgrades.Remove(chosenWeaponUpgrade); // Remove a arma escolhida da lista de armas disponiveis

                if(chosenWeaponUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);

                   bool newWeapon = false;
                   for(int i = 0; i < weaponPlaces.Count; i++)
                   {
                       if(weaponPlaces[i] != null && weaponPlaces[i].weaponStats == chosenWeaponUpgrade.weaponStats)
                       {
                           newWeapon = false;
                           if(!newWeapon)
                           {
                                if(!chosenWeaponUpgrade.weaponStats.NextLevelPrefab) // Se nao houver prox nivel, retorna
                                {
                                    DisableUpgradeUI(upgradeOption);
                                    break;
                                }

                               upgradeOption.upgradeButton.onClick.AddListener(() => UpgradeWeapon(i, chosenWeaponUpgrade.weaponUpgradeIndex));
                               upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponStats.NextLevelPrefab.GetComponent<WeaponController>().weaponStats.Description;
                               upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponStats.NextLevelPrefab.GetComponent<WeaponController>().weaponStats.Name;
                           }
                           break;
                       }
                       else
                       {
                            newWeapon = true;
                       }
                   }
                   if(newWeapon)
                   {
                       upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnWeapon(chosenWeaponUpgrade.initialWeapon));
                       upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponStats.Description;
                       upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponStats.Name;
                   }

                   upgradeOption.upgradeIconDisplay.sprite = chosenWeaponUpgrade.weaponStats.Icon;
                }
            }
            else if (upgradeType == 2)
            {
                PassiveItemUpgrade chosenPassiveItemUpgrade = availablePassiveItemUpgrades[Random.Range(0, availablePassiveItemUpgrades.Count)]; // Escolhe um item passivo aleatorio

                availablePassiveItemUpgrades.Remove(chosenPassiveItemUpgrade); // Remove o item passivo escolhido da lista de itens passivos disponiveis

                if(chosenPassiveItemUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);

                   bool newPassiveItem = false;
                   for(int i = 0; i < passiveItemsPlaces.Count; i++)
                   {
                       if(passiveItemsPlaces[i] != null && passiveItemsPlaces[i].passiveItemData == chosenPassiveItemUpgrade.passiveItemData)
                       {
                           newPassiveItem = false;
                           if(!newPassiveItem)
                           {
                                if(!chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab) // Se nao houver prox nivel, retorna
                                {
                                    DisableUpgradeUI(upgradeOption);
                                    break;
                                }

                               upgradeOption.upgradeButton.onClick.AddListener(() => UpgradePassiveItem(i, chosenPassiveItemUpgrade.passiveItemUpgradeIndex));
                               upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Description;
                               upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Name;
                           }
                           break;
                       }
                       else
                       {
                            newPassiveItem = true;
                       }
                   }
                   if(newPassiveItem == true)
                   {
                       upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnPassiveItem(chosenPassiveItemUpgrade.initialPassiveItem));
                       upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Description;
                       upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Name;
                   }

                   upgradeOption.upgradeIconDisplay.sprite = chosenPassiveItemUpgrade.passiveItemData.Icon;
                }
            }
        }
    }

    void RemoveUpgradeOptions()
    {
        foreach(var upgradeOption in upgradeUIOptions)
        {
            upgradeOption.upgradeButton.onClick.RemoveAllListeners();
            DisableUpgradeUI(upgradeOption);
        }
    }

    public void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }

    void DisableUpgradeUI( UpgradeUI upgradeUI)
    {
        upgradeUI.upgradeNameDisplay.transform.parent.gameObject.SetActive(false);
    }

    void EnableUpgradeUI(UpgradeUI upgradeUI)
    {
        upgradeUI.upgradeNameDisplay.transform.parent.gameObject.SetActive(true);
    }

    
}
