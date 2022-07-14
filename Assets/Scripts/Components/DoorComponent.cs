using UnityEngine;

namespace EcsTest
{
    public struct DoorComponent
    {
        public uint TriggerId;
        public Vector3 ClosedPosition;
        public Vector3 OpenedPosition;
        public float OpeningTime;
    }
}