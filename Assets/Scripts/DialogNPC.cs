using UnityEngine;
using Ink.Runtime;


public abstract class DialogNPC : MonoBehaviour {
    public bool dialogEnabled = false;
    public bool choicesGenerated = false;
    public bool choicesToGenerate = false;
    public Story story = null;

    //start the story
    public abstract void RefreshView();
    public abstract void StartStory(SpriteRenderer renderer, Transform pos, NPC posNPC);

    public abstract void updateCursor(float axis);
    public abstract void pushButton(); 

    }
