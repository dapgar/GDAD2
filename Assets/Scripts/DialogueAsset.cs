using UnityEngine;

[CreateAssetMenu]
public class DialogueAsset : ScriptableObject
{
    [TextArea]
    [Tooltip("Lines should be up to 150 characters in length")]
    public string[] dialogue;
}
