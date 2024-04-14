using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DifficultyLevelButton : MonoBehaviour
{
    public static DifficultyLevelButton instance;
    public static DifficultyLevelButton Instance {  get { return instance; } }
    [SerializeField] private Button button4;
    [SerializeField] private Button button6;
    [SerializeField] private Button button8;
    string buttonName;
    int difficulty;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        //GetButtonName();
        button4.onClick.AddListener(() => { SetDifficulty(4); });
        button6.onClick.AddListener(() => { SetDifficulty(6); });
        button8.onClick.AddListener(() => { SetDifficulty(8); });
        Debug.Log("Hello everyone");
    }

    public void SetDifficulty(int difficulty)
    {
        this.difficulty = difficulty;
    }
    public int GetDifficulty()
    {
        return difficulty;
    }

    /*public void GetButtonName()
    {
        buttonName = EventSystem.current.currentSelectedGameObject.name;
        
        Debug.Log(buttonName);
        SetDifficultyLevel(buttonName);
    }
    public void SetDifficultyLevel(string buttonName)
    {
        switch(buttonName)
        {
            case "Button4":
                SetDifficulty(4);
                break;
            case "Button6":
                SetDifficulty(6);
                break;
            case "Button8":
                SetDifficulty(8);
                break;
        }

    }
    */
    void Update()
    {
        
       
    }
}
