using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetButtonDown("Action") || Input.GetButtonDown("Jump")) {
            SceneManager.LoadScene("2DWorld");
        }
    }
}
