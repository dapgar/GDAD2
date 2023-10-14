using UnityEngine;

[CreateAssetMenu]
public class DialogueAsset : ScriptableObject
{
    [TextArea]
    [Tooltip("Lines should be up to 115 characters in length")]
    public string[] dialogue;
}
