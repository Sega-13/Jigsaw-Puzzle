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
        button4.onClick.AddListener(() => { SetDifficulty(4); });
        button6.onClick.AddListener(() => { SetDifficulty(6); });
        button8.onClick.AddListener(() => { SetDifficulty(8); });
    }

    public void SetDifficulty(int difficulty)
    {
        this.difficulty = difficulty;
    }
    public int GetDifficulty()
    {
        return difficulty;
    }

}
