using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImageEffect
{
    public class RipplePospProcessor : MonoBehaviour
    {
        public static RipplePospProcessor MAIN = null;
        public Material RippleMaterial;
        public float MaxAmount = 50f;

        [Range(0, 1)]
        public float Friction = .9f;

        private float Amount = 0f;

        private void Start()
        {
            MAIN = this;
        }

        void Update()
        {
            this.RippleMaterial.SetFloat("_Amount", this.Amount);
            this.Amount *= this.Friction;
        }

        public static void RippleCam(Vector2 posWorld)
        {
            MAIN.Amount = MAIN.MaxAmount;

            Vector2 pos = Camera.main.WorldToScreenPoint(posWorld);

            RippleCamScreen(pos);

        }

        public static void RippleCamCustom(Vector2 posWorld, int maxAmount)
        {
            MAIN.Amount = MAIN.MaxAmount;

            Vector2 pos = Camera.main.WorldToScreenPoint(posWorld);

            RippleCamScreenCustom(pos  , maxAmount);

        }

        public static void RippleCamScreen(Vector2 pos)
        {
            MAIN.Amount = MAIN.MaxAmount;

            MAIN.RippleMaterial.SetFloat("_CenterX", pos.x);
            MAIN.RippleMaterial.SetFloat("_CenterY", pos.y);
        }

        public static void RippleCamScreenCustom(Vector2 posWorld, int maxAmount)
        {
            MAIN.Amount = maxAmount;

            Vector2 pos = Camera.main.WorldToScreenPoint(posWorld);

            MAIN.RippleMaterial.SetFloat("_CenterX", pos.x);
            MAIN.RippleMaterial.SetFloat("_CenterY", pos.y);
        }




        void OnRenderImage(RenderTexture src, RenderTexture dst)
        {
            //Debug.Log("renderImage");
            Graphics.Blit(src, dst, this.RippleMaterial);
        }
    }
}
