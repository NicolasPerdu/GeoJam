using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    private DialogNPCBegin dialog;
    public SpriteRenderer renderer2;
    public NPC npc;


    void Start() {
        dialog = GetComponent<DialogNPCBegin>();
        renderer2 = GetComponent<SpriteRenderer>();
        npc = GetComponent<NPC>();
    }

    void OnTriggerEnter(Collider col) {
        if (!dialog.dialogEnabled) {
            dialog.StartStory(renderer2, transform, npc);
        } else {
            Destroy(npc);
            Destroy(dialog);
        }
    }
}
