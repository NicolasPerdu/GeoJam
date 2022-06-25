using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {
    public Image actorImage;
    public Text actorName;
    public Text messageText;
    //will be used to animate our dialogbox
    public RectTransform backgroundBox;
    public static bool isActive = false;

    private Message[] currentMessages;
    private Actor[] currentActors;
    private int activeMessage = 0;
    private const float animationDuration = 0.5f;
    private Vector3 originalDialogBoxSize = new Vector3(34, 4, 1);

    //used to display messages
    public void OpenDialogue(Message[] messages, Actor[] actors) {
        currentMessages = messages;
        currentActors = actors;
        activeMessage = 0;
        isActive = true;
        Debug.Log("Started conv" + messages.Length);
        DisplayMessage();
        // used to resize the dialog box
        backgroundBox.LeanScale(originalDialogBoxSize, animationDuration).setEaseInOutExpo();
    }

    private void AnimateTextColor() {
        //make text transparent
        LeanTween.textAlpha(messageText.rectTransform, 0, 0);
        //make text visible
        LeanTween.textAlpha(messageText.rectTransform, 1, 0.5f);
    }

    private void DisplayMessage() {
        Message messageToDisplay = currentMessages[activeMessage];
        messageText.text = messageToDisplay.message;

        Actor actorToDisplay = currentActors[messageToDisplay.actorID];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.actorImage;
        AnimateTextColor();
    }

    private void NextMessage() {
        activeMessage++;
        if (activeMessage < currentMessages.Length) {
            DisplayMessage();
        } else {
            Debug.Log("Conversation ended");
            isActive = false;
            backgroundBox.LeanScale(Vector3.zero, animationDuration).setEaseInOutExpo();
        }
    }

    // Start is called before the first frame update
    void Start() {
        backgroundBox.transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update() {
        //left alt is configured for the moment
        if (Input.GetButtonDown("Fire2") && isActive) {
            NextMessage();
        }
    }
}
