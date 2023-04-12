using StageGeneration.Stage;
using UnityEngine;

namespace StageGeneration.Rooms
{
    public class Door : MonoBehaviour
    {
        public int roomPosXOffset;
        public int roomPosZOffset;

        public bool hasNeighbour;

        [SerializeField]
        private StageHelper.roomDirections direction;

        public Cell cell;

        public StageHelper.roomDirections GetDirection()
        {
            return direction;
        }
    }
}
