﻿using System;
using Unity.Collections;

namespace Features.DialogueHandler.Services
{
    [Serializable]
    public struct DialogueData
    {
        public int PhraseKey;
        public int SpeakerCharacter;
    }
}