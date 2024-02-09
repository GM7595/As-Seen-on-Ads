using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleScript : MonoBehaviour
{
    public Button start;
    public Button select;
    public Button quit;
    public Transform playerModel;
    public float rotateSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {
        start.onClick.AddListener(StartGame);
        select.onClick.AddListener(SelectStage);
        quit.onClick.AddListener(QuitGame);
    }

    private void FixedUpdate()
    {
        playerModel.Rotate(Vector3.up * 10);
    }

    void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void SelectStage()
    {

    }

    void QuitGame()
    {
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
