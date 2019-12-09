using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class StartButton : MonoBehaviour
{
    Button button;
    Image image;
    GameObject instructions;
    [SerializeField]
        Object scene;

    void Start()
    {
        button = GetComponent<Button>();
        //button = this.gameObject.GetComponentsInChildren<Button>()[0];
        button.onClick.AddListener(delegate { GoToScene(scene); });
        //RectTransform rect = button.GetComponent<RectTransform>();
        //rect.sizeDelta = new Vector3(Screen.width / 4, Screen.height / 4);

    }

    void GoToScene(Object scene)
    {
        SceneManager.LoadScene(scene.name);
        Debug.Log(scene.name);
    }

}
