using StageGeneration.Stage;
using UnityEngine;

namespace StageGeneration.Rooms
{
    public class Door : MonoBehaviour
    {
        public int roomPosXOffset;
        public int roomPosZOffset;

        public bool hasNeighbour;

        public Cell cell;

        [SerializeField]
        private StageHelper.RoomDirections direction;

        public StageHelper.RoomDirections GetDirection()
        {
            return direction;
        }
    }
}
