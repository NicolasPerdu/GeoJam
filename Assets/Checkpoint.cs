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

    private float timeDeactivate = 0;
    private float timeActivate = 0;

    Transform poleObj;

    private void Awake() {
        if (Active) {
            meshRenderer.material = activeMaterial;

        }
        else {
            meshRenderer.material = inActiveMaterial;
        }

        poleObj = transform.Find("Pole");
    }

    public void Activate() {
        foreach(Checkpoint cp in FindObjectsOfType<Checkpoint>()) {
            cp.DeActivate();
        }

        if (!Active)  
            timeActivate = Time.timeSinceLevelLoad;
        Active = true;
        meshRenderer.material = activeMaterial;
    }

    public void DeActivate() {
        if (Active)  
            timeDeactivate = Time.timeSinceLevelLoad;
        Active = false;
        meshRenderer.material = inActiveMaterial;
    }

    private void Update() {
        if (Active) {
            if (Input.GetKey(respawnButton))
                aboutToRespawn = true;
            
            poleObj.localScale = Vector3.Lerp(poleObj.localScale, Vector3.one * 1.15F + Vector3.up * 3.5F, (Time.timeSinceLevelLoad - timeActivate) * Time.deltaTime * 50);
            poleObj.localPosition = Vector3.Lerp(poleObj.localPosition, Vector3.down / 2, (Time.timeSinceLevelLoad - timeActivate) * Time.deltaTime * 50);
            
            float rotator = poleObj.localRotation.eulerAngles.y + Time.deltaTime * 32;

            poleObj.localRotation = Quaternion.Euler(0, rotator, 0);
        }
        else
        {
            poleObj.localScale = Vector3.Lerp(poleObj.localScale, Vector3.one * 1.25F + Vector3.up * 2, (Time.timeSinceLevelLoad - timeActivate) * Time.deltaTime * 100);
            poleObj.localPosition = Vector3.Lerp(poleObj.localPosition, Vector3.down, (Time.timeSinceLevelLoad - timeActivate) * Time.deltaTime * 100);

            poleObj.localRotation = Quaternion.Lerp(poleObj.localRotation, Quaternion.identity, (Time.timeSinceLevelLoad - timeActivate) * Time.deltaTime * 100);
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

        foreach(PlayerType playerChr in FindObjectsOfType<PlayerType>()) {
            if (playerChr.avatar.playable)
            {
                playerChr.controller.ResetValues();
                playerChr.controller._groundLayer = layer;
                playerChr.transform.root.position = spawnPoints[spawnIndex].position;
                spawnIndex++;
            }
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
