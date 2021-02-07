using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GtionProduction
{
    public class GtionBGM : MonoBehaviour
    {
        static GtionBGM _bgm;
        public static GtionBGM bgm
        {
            get
            {
                if (_bgm == null)
                {
                    GameObject prefab = Resources.Load<GameObject>("General/CanvasEkstra");

                    GameObject temp = Instantiate(prefab);
                    _bgm = temp.GetComponent<GtionBGM>();
                    GtionLoading.loading = temp.GetComponent<GtionLoading>();

                    DontDestroyOnLoad(_bgm.gameObject);
                    //return _bgm;
                }
                return _bgm;
            }
            set
            {
                _bgm = value;
                //DontDestroyOnLoad(_loading.gameObject);
            }
        }

        const float Smooth = 2;

        [SerializeField] AudioSource audioSource = null;
        [SerializeField] AudioClip nextClip = null;
        [SerializeField] [Range(0,1)]float masterVolumes = 1;
        float maxVolume = 1;
        bool isStopped = false;

        float target = 0;
        float velo = 0;

        public static void Play(AudioClip clip, float maxVolume = 1, bool replayed = false, bool loop = true)
        {
            if (replayed || bgm.audioSource.clip != clip)
            {

                // jika yang akan di play sama
                bgm.maxVolume = maxVolume * bgm.masterVolumes;
                bgm.nextClip = clip;
                bgm.target = 0;
                bgm.audioSource.loop = loop;

            }
            bgm.isStopped = false;
        }
        public static void Play(string clipName, float maxVolume = 1, bool replayed = false)
        {
            AudioClip clip = Resources.Load<AudioClip>("Audio/" + clipName);
            Play(clip, maxVolume, replayed);
        }

        public static void Stop()
        {
            bgm.isStopped = true;
            bgm.target = 0;
        }

        public static void Pause()
        {
            bgm.audioSource.Pause();
        }

        public static void Resume()
        {
            bgm.audioSource.UnPause();
        }

        public static void Mute(bool isMute)
        {
            bgm.audioSource.mute = isMute;
        }

        public static void MasterVolume(float Value)
        {
            bgm.maxVolume = Value;
            bgm.audioSource.volume = bgm.maxVolume * bgm.masterVolumes;
        }



        private void Start()
        {
            //audioSource.transform.position = Vector2.zero;
        }
        // Update is called once per frame
        void Update()
        {
            if (target == 0)
            {
                audioSource.volume = Mathf.SmoothDamp(audioSource.volume, target, ref velo, Smooth / 2);
            }
            else
            {
                audioSource.volume = Mathf.SmoothDamp(audioSource.volume, target, ref velo, Smooth);
            }

            if (audioSource.volume < 0.03f && !isStopped)
            {
                target = maxVolume;
                audioSource.Stop();
                audioSource.clip = nextClip;
                audioSource.Play();
            }

        }
    }
}