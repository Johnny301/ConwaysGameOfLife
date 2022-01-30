using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    Game game;
    
    private void Start() {
        game = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Game>();
    }


}
