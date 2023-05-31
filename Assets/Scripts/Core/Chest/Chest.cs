using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] GameObject openedChestGameObject;

    private ChestOpening _ChestOpening;
    private bool chestOpeningInput;

    public float detectionRadius = 2f; // The radius to detect chests
    public LayerMask playerLayermask; // The layer that the chests are on

    private bool chestOpened = false;

    private void Awake()
    {
        _ChestOpening = new ChestOpening();
        playerLayermask = LayerMask.GetMask("Entity");  //Layer the player is on
    }

    private void OnEnable()
    {
        _ChestOpening.Enable();
        _ChestOpening.Chest.OpenChest.performed += ctx => chestOpeningInput = true;
    }

    private void OnDisable()
    {
        _ChestOpening.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        DetectNearbyPlayer();
        chestOpeningInput = false;
    }

    public void ShowControls()
    {
        // When the chest isn't opened yet, the player can open the chest.
        if (chestOpeningInput && !chestOpened)
        {
            DropLoot();
        }
    }

    public void DropLoot()
    {
        OpenChest();

        //TODO: Loot drop on ground
    }

    private void OpenChest()
    {
        chestOpened = true;

        // Code for changing the chest state to open
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Instantiate the serialized GameObject as a child of the parent GameObject
        GameObject newChild = Instantiate(openedChestGameObject, transform);
        newChild.transform.localPosition = Vector3.zero;

        SoundEffectsManager.instance.PlaySoundEffect(SoundEffectsManager.SoundEffect.ChestOpening);
    }

    private void DetectNearbyPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayermask);

        if (hitColliders.Length > 0)
        {
            // A chest is nearby, do something
            ShowControls();
        }
    }
}
