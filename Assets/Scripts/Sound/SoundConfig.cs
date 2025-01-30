using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundConfig", menuName = "ScriptableObjects/SoundConfig")]
public class SoundConfig : ScriptableObject
{
    public SoundData[] soundData;
}

[Serializable]
public class SoundData
{
    public SoundType soundType;
    public AudioClip soundClip;
}