using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Tokens
{
    public class TokenSection : MonoBehaviour
    {
        [Header("Graphical Settings")]
        [SerializeField]
        private Sprite emptyTokenImage;

        [SerializeField]
        private Sprite filledTokenImage;

        [Header("Core Settings")]
        [SerializeField]
        private List<Image> tokens;

        private int invested;

        public void Invest()
        {
            if (!TokenManager.instance.Invest(invested < TokenManager.instance.maxTokens)) return;

            tokens[invested].enabled = false;
            tokens[invested].sprite = filledTokenImage;
            tokens[invested++].enabled = true;
        }

        public void Refund()
        {
            if (!TokenManager.instance.Refund(invested > 0)) return;

            Debug.Log(invested);

            tokens[--invested].enabled = false;
            tokens[invested].sprite = emptyTokenImage;
            tokens[invested].enabled = true;
        }
    }
}