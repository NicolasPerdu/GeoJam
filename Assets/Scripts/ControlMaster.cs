using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlMaster : MonoBehaviour
{
    static public ControlMaster main;


    [HideInInspector]public CharacterController2D activePlayerCharacter = null;

    public List<CharacterController2D> playerCharacters = new List<CharacterController2D>();

    void Awake()
    {
        main = this;
    }

    void FixedUpdate()
    {
        // TODO: switch between player characters
        // make the active player character the first one in the list at the start of the scene
        activePlayerCharacter = playerCharacters[0];
    }

    // TODO: DIFFERENTIATE BETWEEN CHARACTER THAT'S ACTIVE
	public void OnMove(InputAction.CallbackContext input) => activePlayerCharacter.dPadInput = input.ReadValue<Vector2>();
    public void OnJump(InputAction.CallbackContext input) => activePlayerCharacter.jumpInput = input.ReadValueAsButton();
	public void OnAction(InputAction.CallbackContext input) => activePlayerCharacter.actionInput = input.ReadValueAsButton();
}
