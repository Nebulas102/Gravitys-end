using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace UI.Damage
{
    public class DamageDisplay : MonoBehaviour
    {
        public void Show(string damage, GameObject display, Canvas canvas)
        {
            GameObject damageDisplay = Instantiate(display, canvas.transform.TransformPoint(0, 0, 0), Quaternion.identity, canvas.transform);

            damageDisplay.GetComponentInChildren<TextMeshProUGUI>().text = damage;

            Destroy(damageDisplay, 1f);
        }
    }
}
