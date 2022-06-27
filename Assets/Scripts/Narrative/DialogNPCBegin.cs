using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogNPCBegin : DialogNPC {
    private bool finished = false;
    private SpriteRenderer rendererPlayer, npcRenderer;
    private NPC npc;
    private Transform playerPosition;
    private NPC npcPosition;
    private Image image;
    private string fullText;
    private string currentText;
    private int indexText;
    private float delay = 0.025f;
    private float startTime;
    private Text storyText;
    public GameObject selector;
    private GameObject select;
    private int indexCursor = 0;
    private List<Button> buttons;
    private float bufferChoice = 0.5f;
    private float limitTimeChoice = 0.5f;
    //public EventSystem eventSystem;


    public override void pushButton() {
        buttons[indexCursor].onClick.Invoke();
    }

    public override void updateCursor(float cursor) {
        if (bufferChoice >= limitTimeChoice) {
            if (cursor < 0) {
                indexCursor++;
                if (indexCursor >= buttons.Count) {
                    indexCursor = buttons.Count - 1;
                }
                bufferChoice = 0;
            } else if (cursor > 0) {
                indexCursor--;
                if (indexCursor < 0) {
                    indexCursor = 0;
                }
                bufferChoice = 0;
            }
            select.transform.localPosition = select.transform.localPosition = new Vector3(buttons[indexCursor].transform.localPosition.x - 100, buttons[indexCursor].transform.localPosition.y, 0);
        }
        bufferChoice = bufferChoice + Time.deltaTime; 
    }

    void Awake() {
        // Remove the default message
        //RemoveChildren();
        //StartStory();
    }

    // Creates a new Story object with the compiled story which we can then play!
    public override void StartStory(SpriteRenderer renderer, Transform pos, NPC posNPC) {
        rendererPlayer = renderer;
        npcRenderer = posNPC.GetComponent<SpriteRenderer>();
        playerPosition = pos;
        npcPosition = posNPC;
        npc = gameObject.GetComponent<NPC>();
        buttons = new List<Button>();

        if (!dialogEnabled) {
            RemoveChildren();
            story = new Story(inkJSONAsset.text);
            finished = false;
            RefreshView();
        }

        if (dialogEnabled) {
            if (finished) {
                RemoveChildren();
                dialogEnabled = false;
                finished = false;
                choicesGenerated = false;
            }
        } else {
            dialogEnabled = true;
        }
    }

    private void Update() {
        if (dialogEnabled) {
            float endTime = Time.time;
            if ((endTime - startTime) > delay) {
                if (indexText < (fullText.Length - 1)) {
                    indexText++;
                    currentText = currentText + fullText[indexText];
                    if(storyText != null) {
                        storyText.text = currentText;
                    }
                } else {
                    if (story.currentChoices.Count > 0 && !choicesGenerated) {
                        buttons.Clear();
                        for (int i = 0; i < story.currentChoices.Count; i++) {
                            Choice choice = story.currentChoices[i];
                            string txt = choice.text.Trim();
                            Button button = CreateChoiceView(txt);
                            // Tell the button what to do when we press it
                            button.onClick.AddListener(delegate {
                                OnClickChoiceButton(choice);
                            });
                            buttons.Add(button);
                        }
                        //create the selector
                        select = Instantiate(selector);
                        select.transform.SetParent(image.transform, false);
                        select.transform.localPosition = new Vector3(buttons[0].transform.localPosition.x - 10, buttons[0].transform.localPosition.y, 0);
                        select.transform.localScale = new Vector3(500, 500, 0);

                        choicesToGenerate = false;
                        choicesGenerated = true;
                        finished = true;
                    }
                }

                startTime = endTime;
            }
        }

        /*if(choicesGenerated) {
            if(Input.GetButtonUp("Submit")) {
                
                //ExecuteEvents.Execute(buttons[indexCursor].gameObject, new BaseEventData(eventSystem), ExecuteEvents.submitHandler);
            } 
        }*/
    }

    // This is the main function called every time the story changes. It does a few things:
    // Destroys all the old content and choices.
    // Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
    public override void RefreshView() {
        //Debug.Log("RefreshView");
        // Remove all the UI on screen
        RemoveChildren();
        choicesGenerated = false;

        // Read all the content until we can't continue any more
        // Replace while to if
        if (story.canContinue) {
            // Continue gets the next line of the story
            string text = story.Continue();
            bool player = false;

            if (!choicesGenerated) {
                string[] arr = text.Split(':');
                
                if (arr[0] == "Player") {
                    player = true;
                }

                if (arr.Length > 0 && text.Length >= 
                    (arr[0].Length + 2) ) {
                    text = text.Substring(arr[0].Length + 2);
                }   
            } else {
                player = true;
            }

            // This removes any white space from the text.
            fullText = TextCreator.addReturnText(text).Trim();
            indexText = 0;
            currentText = fullText.Substring(indexText, 1);
            startTime = Time.time;
            // Display the text on screen!
            CreateContentView(currentText, player);
        } else {
            dialogEnabled = false; 
        }
    }

    // When we click the choice button, tell the story to choose that choice!
    void OnClickChoiceButton(Choice choice) {
        story.ChooseChoiceIndex(choice.index);
        RefreshView();
    }

    // Creates a button showing the choice text
    void CreateContentView(string text, bool player) {
        image = Instantiate(imagePrefab) as Image;
        storyText = Instantiate(textPrefab) as Text;

        //Debug.Log("CreateContentView");

        // get the size of the text
        TextGenerator textGen = new TextGenerator();
        TextGenerationSettings generationSettings = storyText.GetGenerationSettings(new Vector2(500, 200));

        float widthtext = textGen.GetPreferredWidth(fullText, generationSettings);
        float heightText = textGen.GetPreferredHeight(fullText, generationSettings);

        //Debug.Log("height text before : " + heightText);
        // Add the choices size
        widthtext = widthtext + 20;
        for (int i = 0; i < story.currentChoices.Count; i++) {
            heightText = heightText + 30;
        }
        // Debug.Log("height text after : " + heightText);

        storyText.text = text;

        //Debug.Log("CreateContentView text: " + storyText.text);
        Transform trans = canvas.transform;

        // if the player is speaking
        if (player) {
            Vector3 newPos = playerPosition.position;
            //newPos.x = newPos.x;
            newPos.y = newPos.y + rendererPlayer.bounds.size.y / 2f + heightText * canvas.transform.lossyScale.x / 2f;

            trans.SetPositionAndRotation(newPos, playerPosition.rotation);
            image.transform.SetParent(trans, false);
            image.rectTransform.sizeDelta = new Vector2(widthtext, heightText + 10);

            storyText.transform.SetParent(image.transform, false);

        } else {
            Vector3 newPos = npcPosition.transform.position;
            //newPos.x = newPos.x;
            newPos.y = newPos.y + npcRenderer.bounds.size.y / 2f + heightText * canvas.transform.lossyScale.y / 2f;

            trans.SetPositionAndRotation(newPos, npcPosition.transform.rotation);
            image.transform.SetParent(trans, false);
            image.rectTransform.sizeDelta = new Vector2(widthtext, heightText + 10);

            storyText.transform.SetParent(image.transform, false);
        }

        //Debug.Log("CreateContentView text2: " + storyText.text);
    }

    // Creates a button showing the choice text
    Button CreateChoiceView(string text) {
        // Creates the button from a prefab
        Button choice = Instantiate(buttonPrefab) as Button;
        choice.transform.SetParent(image.transform, false);

        // Gets the text from the button prefab
        Text choiceText = choice.GetComponentInChildren<Text>();
        choiceText.text = text;

        // Make the button expand to fit the text
        HorizontalLayoutGroup layoutGroup = choice.GetComponent<HorizontalLayoutGroup>();
        layoutGroup.childForceExpandHeight = false;

        return choice;
    }

    // Destroys all the children of this gameobject (all the UI)
    void RemoveChildren() {
        int childCount = canvas.transform.childCount;
        for (int i = childCount - 1; i >= 0; --i) {
            //Debug.Log("RemoveChildren: " + canvas.transform.GetChild(i).gameObject.name);
            GameObject.Destroy(canvas.transform.GetChild(i).gameObject);
        }
    }

    [SerializeField]
    private TextAsset inkJSONAsset;

    [SerializeField]
    private Canvas canvas;

    // UI Prefabs
    [SerializeField]
    private Image imagePrefab;
    [SerializeField]
    private Text textPrefab;
    [SerializeField]
    private Button buttonPrefab;
}

