using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BlueGravity.Core;
using BlueGravity.Tools;
using UnityEngine;

namespace BlueGravity.Sound
{
    public class SoundManager : Singleton<SoundManager>
    {
        public List<AudioClip> Sounds {get; private set;}

        private ObjectPool m_soundPool;
        private readonly Dictionary<string, AudioClip> m_nameToSound = new Dictionary<string, AudioClip>();

        private void Awake()
        {
            Sounds = Resources.LoadAll<AudioClip>("Audio").ToList();
            m_soundPool = GetComponent<ObjectPool>();
            m_soundPool.Initialize();
        }

        private void Start()
        {
            foreach (var sound in Sounds)
            {
                m_nameToSound.Add(sound.name, sound);
            }
        }

        public void AddSounds(List<AudioClip> soundsToAdd)
        {
            foreach (var sound in soundsToAdd)
            {
                m_nameToSound.Add(sound.name, sound);
            }
        }
        public void RemoveSounds(List<AudioClip> soundsToAdd)
        {
            foreach (var sound in soundsToAdd)
            {
                m_nameToSound.Remove(sound.name);
            }
        }
        public void PlaySound(AudioClip clip, Vector3 position, bool loop = false, bool randomPitch = false)
        {

            if (clip != null)
            {
                m_soundPool.GetObject().GetComponent<SoundFx>().Play(clip, position, loop, randomPitch);
            }
        }

        public void PlaySound(AudioClip clip, Vector3 position, bool loop = false, bool randomPitch = false, float volumeSound = 1)
        {



            if (clip != null)
            {

                m_soundPool.GetObject().GetComponent<SoundFx>().Play(clip, position, loop, randomPitch, volumeSound);
            }
        }

        public void PlaySound(string soundName, Vector3 position, bool loop = false, bool randomPitch = false, float volumeSound = 1)
        {
            var clip = m_nameToSound[soundName];
            if (clip != null)
            {
                PlaySound(clip, position, loop, randomPitch, volumeSound);
            }
        }

        public void PlaySFXSound(string soundName, Vector3 position, float volume = 1)
        {
            var clip = m_nameToSound[soundName];
            if (clip != null)
            {
                PlaySound(clip, position, false, true, volume);
            }
        }
        public void StopSound(string soundName)
        {
            foreach (var sound in m_soundPool.GetComponentsInChildren<SoundFx>())
            {
                if (sound.GetComponent<AudioSource>().clip == m_nameToSound[soundName])
                {
                    sound.GetComponent<PooledObject>().pool.ReturnObject(sound.gameObject);
                }
            }
        }
    }
}


