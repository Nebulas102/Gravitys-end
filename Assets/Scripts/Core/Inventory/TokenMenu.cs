using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


namespace Core.Inventory
{

    public class TokenMenu : MonoBehaviour
    {
        [SerializeField] Sprite emptyTokenSlotImage;
        [SerializeField] Sprite tokenSlotEquippedImage;

        [SerializeField] GameObject attackTokenGameObject;
        [SerializeField] GameObject healthTokenGameObject;
        [SerializeField] GameObject timeTokenGameObject;

        String tokensCounterText;
        TextMeshProUGUI tokensCounterTextField;

        // Singleton for TokenMenu
        public static TokenMenu instance;

        private void Awake()
        {
            if (instance != null)
                return;

            instance = this;
        }


        public void AddToken()
        {
            Init();

            // Get the Tokens Parent object
            GameObject tokensParent = transform.Find("Tokens").gameObject;

            // Convert the string to an Integer
            int tokensCounter = Convert.ToInt32(tokensCounterText);

            // Loop through all the child tokens in the tokens parent object
            foreach (Transform tokenChild in tokensParent.transform)
            {
                // If this tokenslot is not equipped
                if (tokenChild.GetComponent<TokenSlot>().isEquipped == false)
                {
                    // If the tokencounter is higher than 0 then you can add the token
                    if (tokensCounter > 0)
                    {
                        HandleAddToken(tokenChild, tokensCounter);
                        return;
                    }
                }
            }
        }

        public void RemoveToken()
        {
            Init();

            // Get the Tokens Parent object
            GameObject tokensParent = transform.Find("Tokens").gameObject;

            // Convert the string to an Integer
            int tokensCounter = Convert.ToInt32(tokensCounterText);

            // Loop through all the child tokens in the tokens parent object
            for (int i = tokensParent.transform.childCount - 1; i >= 0; i--)
            {
                // If this tokenslot is equipped
                if (tokensParent.transform.GetChild(i).GetComponent<TokenSlot>().isEquipped == true)
                {
                    HandleRemoveToken(tokensParent, tokensCounter, i);
                    return;
                }
            }
        }

        void Init()
        {
            // Get the tokensCounterText component
            tokensCounterTextField = gameObject.transform.parent.transform.Find("TokensCounter").GetComponent<TextMeshProUGUI>();

            // Put the text into a string variable
            tokensCounterText = tokensCounterTextField.text;
        }

        void HandleAddToken(Transform tokenChild, int tokensCounter)
        {
            tokenChild.GetComponent<TokenSlot>().isEquipped = true;
            tokensCounter--;
            tokensCounterText = tokensCounter.ToString();
            tokensCounterTextField.text = tokensCounterText;
            tokenChild.GetComponent<Image>().sprite = tokenSlotEquippedImage;
        }

        void HandleRemoveToken(GameObject tokensParent, int tokensCounter, int i)
        {
            tokensParent.transform.GetChild(i).GetComponent<TokenSlot>().isEquipped = false;
            tokensCounter++;
            tokensCounterText = tokensCounter.ToString();
            tokensCounterTextField.text = tokensCounterText;
            tokensParent.transform.GetChild(i).GetComponent<Image>().sprite = emptyTokenSlotImage;
        }

        // This method is going to be used in the calculations for the tokens to check how many tokens you have in order to make the calculation
        // Use of it for example will be: TokenMenu.instance.GetTokens(TokenType.AttackToken); and it will output how many tokens are assigned to the attack skill
        public int GetTokens(TokenType tokenType)
        {

            GameObject tokensParent = transform.Find("Tokens").gameObject;

            switch (tokenType)
            {
                case TokenType.AttackToken:
                    tokensParent = attackTokenGameObject.transform.Find("Tokens").gameObject;
                    break;
                case TokenType.HealthToken:
                    tokensParent = healthTokenGameObject.transform.Find("Tokens").gameObject;
                    break;
                case TokenType.TimeToken:
                    tokensParent = timeTokenGameObject.transform.Find("Tokens").gameObject;
                    break;
            }

            int tokenCounter = 0;

            // Loop through all the child tokens in the tokens parent object
            foreach (Transform tokenChild in tokensParent.transform)
            {
                // If this tokenslot is equipped, then add it to the counter
                if (tokenChild.GetComponent<TokenSlot>().isEquipped == true)
                {
                    tokenCounter++;
                }
            }

            return tokenCounter;
        }

        public enum TokenType
        {
            AttackToken,
            HealthToken,
            TimeToken
        }
    }


}

