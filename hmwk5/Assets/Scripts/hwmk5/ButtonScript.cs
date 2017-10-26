using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    using UnityEngine.SceneManagement;
public class ButtonScript : MonoBehaviour {
    public livesManager livesMan;
    void Start()
    {
        Time.timeScale = 0f;
    }
    
    public void StartGame()
    {
        Time.timeScale = 1f;
        Destroy(this.gameObject);
    }
}
