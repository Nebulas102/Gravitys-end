using System.Collections;
using System.Collections.Generic;
using Core.UI;
using Core.UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHandler : MonoBehaviour
{
    EventSystem m_EventSystem;


    [SerializeField] GameObject inventoryMenuFirstSelected;
    [SerializeField] GameObject pauseMenuFirstSelected;

    private bool inventorySelected = false;
    private bool pauseMenuSelected = false;


    void OnEnable()
    {
        //Fetch the current EventSystem.
        m_EventSystem = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {
        // if the inventory is opened and it is not yet selected then select the first button
        if (Inventory.Instance.inventoryOpened && !inventorySelected)
        {
            m_EventSystem.SetSelectedGameObject(inventoryMenuFirstSelected);
            inventorySelected = true;
            pauseMenuSelected = false;
        }

        // if the game is paused and it is not yet selected then select the first button
        if (PauseMenu.instance.isPaused && !pauseMenuSelected)
        {
            m_EventSystem.SetSelectedGameObject(pauseMenuFirstSelected);
            inventorySelected = false;
            pauseMenuSelected = true;
        }

    }
}
