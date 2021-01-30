
using UnityEngine;

namespace ImageEffect
{
    [ExecuteInEditMode]
    public class ChromaticAberration : MonoBehaviour
    {
        public static ChromaticAberration MAIN = null;

        Vector2 redOffset;
        Vector2 greenOffset;
        Vector2 blueOffset;

        [SerializeField]Material material;

        float actualAbrationTime = 0;
        float desireAmount = 0;
        float abrationTime = 0;

        // Use this for initialization
        void Start()
        {
            MAIN = this;
            AbrationCam(0);
            //material = new Material(Shader.Find("Hidden/ChromaticAberration"));
        }

        public static void AbrationCam(float amount , float duration)
        {// NOTE : sugested amount 0.005
            if (MAIN == null)
                return;

            MAIN.AbrationCam(amount);
            MAIN.desireAmount = amount;
            MAIN.abrationTime = duration;
            MAIN.actualAbrationTime = duration;
        }

        void AbrationCam(float amount) {
            MAIN.redOffset.x = amount;
            MAIN.blueOffset.x = -amount;
            MAIN.material.SetVector("u_redOffset", MAIN.redOffset);
            MAIN.material.SetVector("u_greenOffset", MAIN.greenOffset);
            MAIN.material.SetVector("u_blueOffset", MAIN.blueOffset);
        }
        
        private void Update()
        {
            if (actualAbrationTime > 0)
            {
                actualAbrationTime -= Time.unscaledDeltaTime;
                float amount = Mathf.Lerp(0, desireAmount, actualAbrationTime / abrationTime);
                AbrationCam(amount);
            }
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            
            Graphics.Blit(source, destination, material);
        }
    }
}
