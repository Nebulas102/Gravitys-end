using TMPro;
using UnityEngine;

namespace UI.Tokens
{
    public class TokenManager : MonoBehaviour
    {
        [Header("Core Settings")]
        [SerializeField]
        private int tokens = 5;

        [SerializeField]
        public int maxTokens = 5;

        [SerializeField]
        private TextMeshProUGUI spendableText;

        public int spendableTokens
        {
            get { return _spendableTokens; }
            private set
            {
                _spendableTokens = value;
                spendableText.text = _spendableTokens.ToString();
            }
        }
        private int _spendableTokens;

        public static TokenManager instance { get; private set; }

        private void Awake()
        {
            // Initialize the amount of spendable tokens
            spendableTokens = tokens;

            // Singleton
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        public bool Invest(bool canInvest)
        {
            canInvest = canInvest && spendableTokens > 0;
            if (canInvest)
                spendableTokens--;

            return canInvest;
        }

        public bool Refund(bool canRefund)
        {
            canRefund = canRefund && spendableTokens < tokens;
            if (canRefund)
                spendableTokens++;

            return canRefund;
        }
    }
}