using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] TMP_InputField nickname;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] Button button;

    void OnEnable()
    {
        score.text = "Score : " + (GameMgr.GetIns._Score).ToString();
    }

    void Update()
    {
        if (nickname.text.Length <= 0)
            button.interactable = false;
        else
            button.interactable = true;
    }

    // Start is called before the first frame update
    public void onSubmit()
    {
        GameMgr.GetIns._Nickname = nickname.text;
        GameMgr.GetIns.SaveData();
        SceneManager.LoadScene("RankingScene");
    }
}
