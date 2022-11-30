using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer==6)
        {
            if (GameManager.instance.m_nEnemyID == 1)
            {
                SceneManager.LoadScene("MainScene");
            }
            else
            {
                SceneManager.LoadScene("InGameScene");
                PlayerPrefs.SetInt("EnemyID", GameManager.instance.m_nEnemyID + 1);
            }
        }
        
    }
}
