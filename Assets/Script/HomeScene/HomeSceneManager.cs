using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeSceneManager : MonoBehaviour
{

    [SerializeField] UnityEngine.UI.Text textScore = null;
    [SerializeField]AudioClip bgm = null;

    private void Start()
    {
        GtionProduction.GtionBGM.Play(bgm);
        textScore.text = ""+PlayerPrefs.GetInt("highscore", 25);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PlayEndlessGame() {
        // UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        GtionProduction.GtionLoading.openLayerLoading(() =>
        {
            GtionProduction.GtionLoading.startMoveScene("GameScene");
        });

    }
    public void PlayCampaign()
    {

    }

}
