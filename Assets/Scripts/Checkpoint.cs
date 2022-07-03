using System;
using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private Material inActiveMaterial;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private KeyCode respawnButton;
    [SerializeField] private LayerMask layer;
    [SerializeField] public PlayerController San;
    [SerializeField] public bool Active;

    private bool aboutToRespawn;
    private void Awake() {
        if (Active) {
            meshRenderer.material = activeMaterial;
        }
        else {
            meshRenderer.material = inActiveMaterial;
        }
    }

    public void Activate() {
        foreach(Checkpoint cp in FindObjectsOfType<Checkpoint>()) {
            cp.DeActivate();
        }

        Active = true;
        meshRenderer.material = activeMaterial;
    }

    public void DeActivate() {
        Active = false;
        meshRenderer.material = inActiveMaterial;
    }

    private void Update() {
        if (Active && Input.GetKey(respawnButton)) {
            aboutToRespawn = true;
        }
    }

    private void FixedUpdate() {
        if (aboutToRespawn) {
            Respawn();
            aboutToRespawn = false;
        }
    }

    public void Respawn()
    {
        int spawnIndex = 0;

        foreach(PlayerController currPlayer in FindObjectsOfType<PlayerController>()) {
            currPlayer._groundLayer = layer;
            currPlayer.transform.position = spawnPoints[spawnIndex].position;
            spawnIndex++;
        }
    }

    public void RespawnSan()
    {
        San._groundLayer = layer;
        San.transform.position = spawnPoints[0].position;
        San.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") ){
            Activate();
        }
    }
}
