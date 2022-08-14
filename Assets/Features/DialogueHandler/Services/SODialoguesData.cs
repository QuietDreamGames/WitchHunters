using System;
using Features.Character.Services;
using Features.DialogueHandler.Components;
using Features.DialogueHandler.Systems;
using UnityEngine;

namespace Features.DialogueHandler.Services
{
    [CreateAssetMenu(fileName = "DialoguesData", menuName = "Dialogues")]
    public class SODialoguesData : ScriptableObject
    {
        public DialogueInfo[] DialogueDatas;
    }

    [Serializable]
    public class DialogueInfo
    {
        public int PhraseKey;
        public CharacterType SpeakerCharacter;
    }
}