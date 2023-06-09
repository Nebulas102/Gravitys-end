using System.Collections;
using System.Collections.Generic;
using UI;
using UI.Runtime;
using UnityEngine;

public class MapUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Map;

    public static MapUIManager instance { get; private set; }
    
    private InputManager _inputManager;

    public bool mapIsActive
    {
        get => _mapIsActive;
        set
        {
            _mapIsActive = value;
            OnMapToggled?.Invoke(value);
            Map.SetActive(value);

            if (value)
                InventoryOverlayBehaviour.instance.inventoryOpened = false;
        }
    }

    private bool _mapIsActive;

    public delegate void MapToggled(bool mapOpened);
    public static event MapToggled OnMapToggled;

    private void Awake()
    {
        // Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        _inputManager = new InputManager();
    }

    void Update()
    {
        _inputManager.UI.Enable();
        if (_inputManager.UI.ToggleMap.triggered && !(DialogueManager.Instance.dialogueActive || PauseMenu.instance.isPaused))
        {
            ToggleMap();
        } 
    }

    private void ToggleMap()
    {
        mapIsActive = !mapIsActive;
        Time.timeScale = mapIsActive ? 0f : 1f;
    }
}
