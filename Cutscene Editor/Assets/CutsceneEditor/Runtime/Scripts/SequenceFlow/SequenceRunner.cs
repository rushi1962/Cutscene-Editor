using System.Collections;
using UnityEngine;

public class SequenceRunner : MonoBehaviour
{
    public static SequenceRunner Instance { get; private set; }

    private bool isRunning = false;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void TryRunSequence(SequenceAsset asset, TriggerSequence triggerVolume, GameObject instigator)
    {
        if (isRunning) return;

        StartCoroutine(Run(asset, triggerVolume, instigator));
    }

    public IEnumerator Run(SequenceAsset asset, TriggerSequence triggerVolume, GameObject instigator)
    {
        var context = new SequenceContext(
            runner: this,
            triggerVolume: triggerVolume,
            instigator: instigator,
            audioSource: instigator.GetComponent<AudioSource>()
        );

        SequenceNode current = asset.GetNode(asset.GetRootNode());

        while (current != null)
        {
            yield return StartCoroutine(current.Execute(context));

            // find next node
            var connection = asset.GetConnectionWithFromGUID(current.Guid);
            if (connection == null)
                break;

            current = asset.GetNode(connection.toNodeGuid);
        }
    }
}
