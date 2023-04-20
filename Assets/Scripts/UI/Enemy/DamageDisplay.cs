using TMPro;
using UnityEngine;

namespace UI.Enemy
{
    public class DamageDisplay : MonoBehaviour
    {
        public void Show(string damage, GameObject display, Canvas canvas)
        {
            var damageDisplay = Instantiate(display, canvas.transform.TransformPoint(0, 0, 0), Quaternion.identity,
                canvas.transform);

            damageDisplay.GetComponentInChildren<TextMeshProUGUI>().text = damage;

            Destroy(damageDisplay, 1f);
        }
    }
}
