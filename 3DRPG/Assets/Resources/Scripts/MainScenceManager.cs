using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScenceManager : MonoBehaviour
{


    public void GameStart()
    {
        SceneManager.LoadScene("InGameScence");
    }


    public void SetResolution()
    {
        int setWidth = 1280; // ����� ���� �ʺ�
        int setHeight = 720; // ����� ���� ����

        Screen.SetResolution(setWidth,setHeight,false);
    }

        // Start is called before the first frame update
        void Start()
    {
        SetResolution();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
