using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverButton : MonoBehaviour
{
    Button btn;
    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(Again);
    }

    private void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    // Update is called once per frame
    void Again()
    {

        SceneManager.LoadScene("SampleScene");
    }
}
