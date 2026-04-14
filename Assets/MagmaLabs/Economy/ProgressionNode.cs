
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MagmaLabs.Editor;
using MagmaLabs.Utilities.Primitives;
using MagmaLabs;

namespace MagmaLabs.Economy{
    [System.Serializable]
    [CreateAssetMenu(menuName = "MagmaLabs/Economy/ProgressionNode")]
    public class ProgressionNode : Savable
    {
        const int DEBUG_INFO_LEVEL = 2;
        public static List<ProgressionNode> allNodes = new List<ProgressionNode>();
        public string id { get; private set; }
        public NodeStatus status;
        [SerializeField] private bool usePrerequisitesInsteadOfContinuations = false;

        [ShowIf("usePrerequisitesInsteadOfContinuations", false)]
        public List<ProgressionNode> continuationNodes  = new List<ProgressionNode>();

        [ShowIf("usePrerequisitesInsteadOfContinuations", true)]
        public List<ProgressionNode> prerequisiteNodes  = new List<ProgressionNode>();

        public List<ProgressionNode> mutuallyExclusiveNodes  = new List<ProgressionNode>();
        public List<Savable> activationRewards = new List<Savable>();

         public string ToString()
        {
            return JsonUtility.ToJson(this);
        }
         public void FromString(string serialized)
        {
            JsonUtility.FromJsonOverwrite(serialized, this);
        }

    

    }
    [System.Serializable]
    public enum NodeStatus
    {
        Locked,
        Unlocked,
        Activated
    }

}