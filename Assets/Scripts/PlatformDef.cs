using System;
using UnityEngine;

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlatformDef", order = 1)]
    [Serializable]
    public class PlatformDef : ScriptableObject
    {
        public bool collidesWithPlayer = true;
        public bool collidesWithEntities = true;
        public int decay = -1;
        public Color color;
        public bool canMove = false;
    }
