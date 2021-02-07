using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

namespace GtionProduction
{

    [System.Serializable]
    public class ContentLoading
    {
        public string title = "Boss";
        public string description = "The most biggest enemy";
        public Sprite icon = null;
    }

    public class GtionLoading : MonoBehaviour
    {

        static GtionLoading _loading;
        public static GtionLoading loading
        {
            get
            {
                if (_loading == null)
                {
                    GameObject prefab = Resources.Load<GameObject>("General/CanvasEkstra");

                    GameObject temp = Instantiate(prefab);
                    _loading = temp.GetComponent<GtionLoading>();
                    GtionBGM.bgm = temp.GetComponent<GtionBGM>();

                    DontDestroyOnLoad(_loading.gameObject);

                }
                return _loading;
            }
            set
            {
                _loading = value;
                //DontDestroyOnLoad(_loading.gameObject);
            }
        }

        public static bool isOpening
        {
            get { return loading.isOpen; }
        }

        

        /*[Header("Content")]
        public Text titleText = null;
        public Text descText = null;
        public Image iconImage = null;
        public ContentLoading[] contents = null;
        */

        [Header("Loading")]
        public Image loadingProgress = null;
        public Animator anim = null;

        bool isOpen = false;
        

        UnityEngine.Events.UnityAction responBack = null;

        public static void openLayerLoading(UnityEngine.Events.UnityAction responBack = null)
        {
            //loading.randomContent();
            //loading.randomContent();
            loading.anim.gameObject.SetActive(true);
            setAmountLoading(0);
            loading.anim.SetTrigger("OpenLoading"); //.Play("OpenLoading");

            loading.isOpen = true;

            if (responBack != null)
            {
                loading.InvokeResponBack(responBack, 0.7f);
            }
        }

        public static void hideLayerLoading(UnityEngine.Events.UnityAction responBack = null)
        {
            loading.anim.SetTrigger("HideLoading");
            setAmountLoading(1);
            loading.setInactiveLoadingObject();
            loading.isOpen = false;
            if (responBack != null)
            {
                loading.InvokeResponBack(responBack, 0.7f);
            }
        }

        public static void setAmountLoading(float Amount)
        {
            Amount = Mathf.Clamp(Amount, 0.02f, 1f);
            //Debug.Log(loading.loadingProgress.name);
            loading.loadingProgress.fillAmount = Amount;// .text = "Loading " + Mathf.FloorToInt(Amount * 100)+" %";
        }

        public void InvokeResponBack(UnityEngine.Events.UnityAction responBack, float time)
        {
            this.responBack = responBack;
            Invoke("runResponBack", time);
        }

        void runResponBack()
        {

            if (responBack != null)
            {
                responBack.Invoke();
                responBack = null;
            }
        }

        public void setInactiveLoadingObject()
        {
            Invoke("setInactive", 0.8f);
        }

        void setInactive()
        {
            anim.gameObject.SetActive(false);
        }

        private void Update()
        {
            
        }

        public string nextSceneName = "";

        public static void startMoveScene(string sceneName)
        {
            loading.nextSceneName = sceneName;
            loading.Invoke("startMoveScene", 0.8f);
        }

        void startMoveScene()
        {
            StartCoroutine(LoadYourAsyncScene(nextSceneName));
        }

        float currentProgress = 0;
        float velocityProgress = 0;

        IEnumerator LoadYourAsyncScene(string sceneName)
        {
            // The Application loads the Scene in the background as the current Scene runs.
            // This is particularly good for creating loading screens.
            // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
            // a sceneBuildIndex of 1 as shown in Build Settings.

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            velocityProgress = 0;
            currentProgress = 0;
            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                currentProgress = Mathf.SmoothDamp(currentProgress, asyncLoad.progress, ref velocityProgress, 0.5f);
                GtionLoading.setAmountLoading(currentProgress);
                yield return null;
            }
            GtionLoading.setAmountLoading(1);
            GtionLoading.hideLayerLoading();

        }

    }
}
