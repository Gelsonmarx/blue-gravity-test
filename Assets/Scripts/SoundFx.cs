using System.Collections;
using System.Collections.Generic;
using BlueGravity.Core;
using UnityEngine;
using UnityEngine.Assertions;


namespace BlueGravity.Sound
{
    public class SoundFx : MonoBehaviour
    {
        private AudioSource m_audioSource;
        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
            Assert.IsTrue(m_audioSource != null);
        }
        public void Play(AudioClip clip, Vector3 position, bool loop = false, bool randomPitch = false, float volume = 1)
        {
            if (clip == null)
            {
                return;
            }
            float pitch = Random.Range(0.93f, 1.07f);
            m_audioSource.pitch = randomPitch ? pitch : 1;
            gameObject.transform.position = position;
            m_audioSource.volume = volume;
            m_audioSource.clip = clip;
            m_audioSource.loop = loop;
            m_audioSource.Play();
            Invoke("DisableSoundFx", clip.length + 0.1f);
        }
        private void DisableSoundFx()
        {
            GetComponent<PooledObject>().pool.ReturnObject(gameObject);
        }
    }
}

