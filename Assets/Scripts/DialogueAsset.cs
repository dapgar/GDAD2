using UnityEngine;

[CreateAssetMenu]
public class DialogueAsset : ScriptableObject
{
    [TextArea]
    [Tooltip("Lines should be up to 264 characters in length")]
    public string[] dialogue;
    [Tooltip("Speed of the corresponding line, If you want to leave it at the default speed set the value to 0")]
    public int[] textLineSpeed;
}
