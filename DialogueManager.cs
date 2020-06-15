using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DialogueManager : MonoBehaviour
{
    [Tooltip("Used for Identifying this set of sentances within the game.")]
    public int BlockAddress;

    [Tooltip("Each Dialogue Node is for a given character, to simulate a conversation between multiple people.")]
    public List<DialogueNode> Conversation;

    public void AddDialogue()
    {
        //When called, add a dialogue node to the end of the array
        DialogueNode dn = new DialogueNode();

        dn.name = string.Format("Index {0}", Conversation.Count);
        Conversation.Add(dn);
        dn.Dialogue = new List<string>();
        dn.Dialogue.Add(string.Empty);
    }
    public void RemoveDialogue(Int32 index)
    {
        //When called, removes the dialogue node at the specified index
        Conversation.RemoveAt(index);
    }

    public System.Boolean CanRemoveCallback()
    {
        // If the conversation contains any elements at all, return true, meaning we can remove an item
        return Conversation.Any();
    }

}
