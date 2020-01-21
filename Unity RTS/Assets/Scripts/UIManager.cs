using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public struct PanelInfo
{
    public string name;
    public GameObject panel;
}

public class UIManager : MonoBehaviour
{
    [Header("Money And Supply Variables")]
    #region MoneyAndSupplyVariables
    public TextMeshProUGUI mineralText;
    public TextMeshProUGUI gasText;
    public TextMeshProUGUI supplyText;
    #endregion  

    [Header("Single Unit Panel Variables")]
    #region SingleUnitPanelVariables
    public Image unitImage;
    public TextMeshProUGUI hpText;

    public TextMeshProUGUI unitNameText;

    public Image armorUpgradeImage;
    public Image weaponUpgradeImage;

    public TextMeshProUGUI armorClassAndAttackTypeText;
    #endregion

    [Header("Panel Objects")]
    #region panels
    public Dictionary<string, GameObject> UIPanels;
    public List<PanelInfo> panels;
    #endregion

    [Header("Grid Buttons")]
    public List<Button> gridButtons;
    [SerializeField]
    private List<Image> gridButtonImages;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gridButtonImages = new List<Image>();

        foreach(Button button in gridButtons)
        {
            gridButtonImages.Add(button.GetComponentsInChildren<Image>()[1]);
        }

        foreach(PanelInfo panelInfo in panels)
        {
            UIPanels.Add(panelInfo.name, panelInfo.panel);
        }

        Player player = GameController.Instance.GetPlayer();

        mineralText.SetText(player.mineralCount.ToString());
        gasText.SetText(player.gasCount.ToString());
        supplyText.SetText(player.currentCapacity.ToString());

    }

    public void SetSingleSelectionPanel(RTSObject rtsObject)
    {
        DisablePanels();

        if (rtsObject is Building)
        {
            if(!rtsObject.GetComponent<Building>().CompletedBuilding)
            {
                SetBuildingProgressPanel();
            }
        }
        else
        {
            UIPanels["single unit"].SetActive(true);

            unitImage.sprite = rtsObject.uiSprite;
            hpText.SetText($"{rtsObject.health.currentHealth}/{rtsObject.health.maxHealth}");

            hpText.color = rtsObject.health.HealthToColor();

            unitNameText.SetText(rtsObject.name);

            //Upgrade stuff

            armorClassAndAttackTypeText.SetText($"{Enum.GetName(typeof(ArmorClass), rtsObject.armorClass)}");

            SetSingleSelectionPanelGrid(rtsObject);
        }
    }

    private void SetSingleSelectionPanelGrid(RTSObject rtsObject)
    {
        ClearButtonListeners();

        if(rtsObject is Unit)
        {
            //Define the default behavior for stuff
        }
        else if(rtsObject is Building)
        {
            if(rtsObject is TrainingBuilding)
            {
                TrainingBuilding trainingBuilding = rtsObject.GetComponent<TrainingBuilding>();
                for (int i = 0; i < trainingBuilding.producableUnits.Count; i++)
                {
                    int unitIndex = i;
                    gridButtonImages[i].sprite = trainingBuilding.producableUnits[i].uiSprite;
                    gridButtons[i].onClick.AddListener(() => { trainingBuilding.AddUnitToQueue(trainingBuilding.producableUnits[unitIndex]); });
                }
            }
        }
    }

    private void ClearButtonListeners()
    {
        foreach(Button button in gridButtons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    public void UpdateResourcesText()
    {
        Player player = GameController.Instance.GetPlayer();

        mineralText.SetText(player.mineralCount.ToString());
        gasText.SetText(player.gasCount.ToString());
        supplyText.SetText(player.currentCapacity.ToString());
    }

    public void SetBuildingProgressPanel()
    {
        UIPanels["building progress"].SetActive(true);

        Debug.Log("Ya yeet");
    }

    public void DisablePanels()
    { 
        foreach(var panel in UIPanels)
        {
            panel.Value.SetActive(false);
        }
    }
}
