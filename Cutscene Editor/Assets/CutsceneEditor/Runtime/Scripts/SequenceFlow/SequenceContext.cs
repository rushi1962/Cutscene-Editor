using UnityEngine;

public class SequenceContext
{
    public MonoBehaviour Runner;
    public TriggerSequence TriggerVolume;
    public GameObject Instigator; // e.g. player entering trigger
    public AudioSource AudioSource;

    public SequenceContext(MonoBehaviour runner, TriggerSequence triggerVolume, GameObject instigator, AudioSource audioSource)
    {
        Runner = runner;
        TriggerVolume = triggerVolume;
        Instigator = instigator;
        AudioSource = audioSource;
    }
}
