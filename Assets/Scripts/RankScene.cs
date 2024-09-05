using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RankScene : MonoBehaviour
{
    [SerializeField] Text[] _txtNums;
    //[SerializeField] Text _txtNumOther;

    [SerializeField] Button _btnExit;

    private void Start()
    {
        // _btnExit.onClick.AddListener(OnClicked_btnExit);
        Initialize();
    }

    public void Initialize()
    {
        //_txtNums[0].text = "";

        GameMgr.GetIns.LoadData();

        Debug.Log("ini" + GameMgr.GetIns._listData.Count);

        for (int i = 0; i < GameMgr.GetIns._listData.Count; i++)
        {
            PlayerData data = GameMgr.GetIns._listData[i];
            _txtNums[0].text += string.Format("No.{0} {1} {2}Á¡\n", i + 1, data._Nickname, data._Score);
        }
        /*
        int count = 0;
        if (GameMgr.GetIns._listData.Count < 3) count = GameMgr.GetIns._listData.Count;
        else count = 3;

        for (int i = 0; i < count; ++i)
        {
            PlayerData data = GameMgr.GetIns._listData[i];
            _txtNums[i].text = string.Format("{0} , Score {1}", data._Nickname, data._Score);
        }
        */
    }

    public void OnClicked_btnExit()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void onClick_PlayAgain()
    {
        SceneManager.LoadScene("GameScene");
    }
}
