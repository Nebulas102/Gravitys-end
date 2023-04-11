using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Inventory
{
    public class TokenMenu : MonoBehaviour
    {
        public enum TokenType
        {
            ATTACK_TOKEN,
            HEALTH_TOKEN,
            TIME_TOKEN
        }

        // Singleton for TokenMenu
        public static TokenMenu Instance;

        [SerializeField]
        private Sprite emptyTokenSlotImage;

        [SerializeField]
        private Sprite tokenSlotEquippedImage;

        [SerializeField]
        private GameObject attackTokenGameObject;

        [SerializeField]
        private GameObject healthTokenGameObject;

        [SerializeField]
        private GameObject timeTokenGameObject;

        private string _tokensCounterText;
        private TextMeshProUGUI _tokensCounterTextField;

        private void Awake()
        {
            if (Instance != null)
                return;

            Instance = this;
        }

        public void AddToken()
        {
            Init();

            // Get the Tokens Parent object
            var tokensParent = transform.Find("Tokens").gameObject;

            // Convert the string to an Integer
            var tokensCounter = Convert.ToInt32(_tokensCounterText);

            // Loop through all the child tokens in the tokens parent object
            foreach (Transform tokenChild in tokensParent.transform)
                // If this token slot is not equipped
                if (tokenChild.GetComponent<TokenSlot>().isEquipped == false)
                {
                    // If the token counter is higher than 0 then you can add the token
                    if (tokensCounter <= 0) continue;
                    HandleAddToken(tokenChild, tokensCounter);
                    return;
                }
        }

        public void RemoveToken()
        {
            Init();

            // Get the Tokens Parent object
            var tokensParent = transform.Find("Tokens").gameObject;

            // Convert the string to an Integer
            var tokensCounter = Convert.ToInt32(_tokensCounterText);

            // Loop through all the child tokens in the tokens parent object
            for (var i = tokensParent.transform.childCount - 1; i >= 0; i--)
                // If this token slot is equipped
                if (tokensParent.transform.GetChild(i).GetComponent<TokenSlot>().isEquipped)
                {
                    HandleRemoveToken(tokensParent, tokensCounter, i);
                    return;
                }
        }

        private void Init()
        {
            // Get the tokensCounterText component
            _tokensCounterTextField = gameObject.transform.parent.transform.Find("TokensCounter")
                .GetComponent<TextMeshProUGUI>();

            // Put the text into a string variable
            _tokensCounterText = _tokensCounterTextField.text;
        }

        private void HandleAddToken(Transform tokenChild, int tokensCounter)
        {
            tokenChild.GetComponent<TokenSlot>().isEquipped = true;
            tokensCounter--;
            _tokensCounterText = tokensCounter.ToString();
            _tokensCounterTextField.text = _tokensCounterText;
            tokenChild.GetComponent<Image>().sprite = tokenSlotEquippedImage;
        }

        private void HandleRemoveToken(GameObject tokensParent, int tokensCounter, int i)
        {
            tokensParent.transform.GetChild(i).GetComponent<TokenSlot>().isEquipped = false;
            tokensCounter++;
            _tokensCounterText = tokensCounter.ToString();
            _tokensCounterTextField.text = _tokensCounterText;
            tokensParent.transform.GetChild(i).GetComponent<Image>().sprite = emptyTokenSlotImage;
        }

        // This method is going to be used in the calculations for the tokens to check how many tokens you have in order to make the calculation
        // Use of it for example will be: TokenMenu.instance.GetTokens(TokenType.AttackToken); and it will output how many tokens are assigned to the attack skill
        public int GetTokens(TokenType tokenType)
        {
            var tokensParent = tokenType switch
            {
                TokenType.ATTACK_TOKEN => attackTokenGameObject.transform.Find("Tokens").gameObject,
                TokenType.HEALTH_TOKEN => healthTokenGameObject.transform.Find("Tokens").gameObject,
                TokenType.TIME_TOKEN => timeTokenGameObject.transform.Find("Tokens").gameObject,
                _ => throw new ArgumentOutOfRangeException(nameof(tokenType), tokenType, null)
            };

            // Loop through all the child tokens in the tokens parent object and return the amount of tokens that are equipped
            return tokensParent.transform.Cast<Transform>()
                .Count(tokenChild => tokenChild.GetComponent<TokenSlot>().isEquipped);
        }
    }
}
