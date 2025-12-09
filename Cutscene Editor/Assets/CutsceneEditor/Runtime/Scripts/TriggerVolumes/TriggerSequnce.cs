using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Collider))]
public class TriggerSequence : MonoBehaviour
{
    [SerializeField]
    protected SequenceAsset sequence;

    [SerializeField]
    protected List<CinemachineVirtualCamera> virtualCameras = new List<CinemachineVirtualCamera>();

    [SerializeField]
    protected PlayableDirector playableDirector;

    public SequenceAsset GetSequenceAsset()
    {
        return sequence;
    }

    public List<CinemachineVirtualCamera> GetVirtualCameras()
    {
        return virtualCameras;
    }

    public PlayableDirector GetPlayableDirector()
    {
        return playableDirector;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Player"))
        {
            return;
        }
        
        SequenceRunner.Instance.TryRunSequence(sequence, this, other.gameObject);
    }
}
