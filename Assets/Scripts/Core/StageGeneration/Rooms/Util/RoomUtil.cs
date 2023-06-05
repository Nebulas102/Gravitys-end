using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.StageGeneration.Rooms.Util
{
    public static class RoomUtil
    {
        public static Dictionary<string, float> CalculateSizeBasedOnChildren(GameObject parent)
        {
            var sizes = new Dictionary<string, float>();

            float x = 0;
            float y = 0;
            float z = 0;

            float floorY = 0;

            var childrenFloor = parent.GetComponentsInChildren<Transform>().Where(g => g.CompareTag("Floor")).ToArray();
            var childrenWalls = parent.GetComponentsInChildren<Transform>().Where(g => g.CompareTag("Wall")).ToArray();

            x = childrenFloor.Max(g => g.transform.localScale.x);
            z = childrenFloor.Max(g => g.transform.localScale.z);
            floorY = childrenFloor.Max(g => g.transform.localScale.y);

            foreach (Transform t in parent.transform)
            {
                float tempY = 0;

                if (t.CompareTag("Wall")) tempY = t.localScale.y;

                if (y < tempY) y = tempY;
            }

            sizes.Add("x", x);
            sizes.Add("y", y += floorY);
            sizes.Add("z", z);

            return sizes;
        }

        public static GameObject GetRandomRoom(List<GameObject> rooms, int totalWeight)
        {
            var randomWeightChance = Random.Range(0, totalWeight + 1);

            foreach (var r in rooms)
            {
                var hWeight = r.GetComponent<Room>().GetWeight();

                randomWeightChance -= hWeight;

                if (randomWeightChance <= 0) return r;
            }

            //Should never reach that point
            return null;
        }
    }
}
