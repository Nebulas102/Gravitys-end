using System.Collections;
using System.Collections.Generic;
using Core.Enemy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ObjectiveSystem : MonoBehaviour
    {
        // Singleton instance
        private static ObjectiveSystem instance;

        public static ObjectiveSystem Instance
        {
            get { return instance; }
        }

        public List<Objective> objectives = new List<Objective>();

        [SerializeField] GameObject objectivesHolder;

        [SerializeField] int textVerticalMargin = 50;
        [SerializeField] int objectiveFontSize = 24;
        [SerializeField] int objectiveHorizontalPosition = -75;

        [SerializeField] TMP_FontAsset font;

        private int enemiesKilledCount;

        private int objectivesCompleted = 0;
        private bool keycardCollected = false;
        private bool bossKilled = false;

        private CanvasScaler canvasScaler;
        private float screenRatio;

        void Awake() {
            // Check if an instance already exists
            if (instance != null && instance != this)
            {
                // Destroy this instance if another one already exists
                Destroy(gameObject);
                return;
            }

            // Set the instance to this instance
            instance = this;

                        // Get the Canvas Scaler component
            canvasScaler = GetComponentInParent<CanvasScaler>();
        }

        // Start is called before the first frame update
        void Start()
        {
            EnemyBase.OnEnemyKilled += HandleEnemyKilled;

            objectivesHolder = GameObject.Find("ObjectivesHolder");

            Objective objective1 = new Objective();
            objective1.name = "Find the bossroom key";
            objective1.type = ObjectiveType.CollectItem;
            objective1.completed = false;
            objective1.color = Color.white;
            objectives.Add(objective1);

            Objective objective2 = new Objective();
            objective2.name = "Kill 50 enemies";
            objective2.type = ObjectiveType.DefeatEnemy;
            objective2.completed = false;
            objective2.color = Color.white;
            objectives.Add(objective2);

            Objective objective3 = new Objective();
            objective3.name = "Defeat the Boss";
            objective3.type = ObjectiveType.DefeatEnemy;
            objective3.completed = false;
            objective3.color = Color.white;
            objectives.Add(objective3);

            UpdateObjectiveUI();
        }


        public void CompleteObjective(string objectiveName)
        {
            Objective objective = objectives.Find(o => o.name == objectiveName);

            // Check if the object is not null
            if (!ReferenceEquals(objective, null))
            {
                objective.completed = true;
                objective.color = Color.green;

                objectivesCompleted++;
                SoundEffectsManager.instance.PlaySoundEffect(SoundEffectsManager.SoundEffect.ObjectiveCompleted);

                                // // Find the UI element of the completed objective
                Transform objectiveUI = objectivesHolder.transform.Find(objective.name);
                if (objectiveUI != null)
                {
                    // Get the TextMeshProUGUI component of the UI element and set its color
                    TextMeshProUGUI objectiveText = objectiveUI.GetComponent<TextMeshProUGUI>();
                    objectiveText.color = objective.color;

                    objectivesCompleted++;

                    SoundEffectsManager.instance.PlaySoundEffect(SoundEffectsManager.SoundEffect.ObjectiveCompleted);
                }
            }
            else
            {
                Debug.LogWarning("Objective not found: " + objectiveName);
            }

            // UpdateObjectiveUI();
        }


        void OnDestroy()
        {
            EnemyBase.OnEnemyKilled -= HandleEnemyKilled;
        }

        void HandleEnemyKilled(EnemyBase enemy)
        {
            enemiesKilledCount++;
            if (enemiesKilledCount == 50)
            {
                // Mark the "Collect the Key" objective as completed
                FindObjectOfType<ObjectiveSystem>().CompleteObjective("Kill 50 enemies");
            }
        }

        public static void HandleBossKilled()
        {
            // Mark the "Collect the Key" objective as completed
            FindObjectOfType<ObjectiveSystem>().CompleteObjective("Defeat the Boss");
        }

        public int getCompletedObjectives()
        {
            return objectivesCompleted;
        }

        public bool getKeycardCollected()
        {
            return keycardCollected;
        }

        public bool getBossKilled()
        {
            return bossKilled;
        }

        public void setKeycardCollected(bool keyCardCollected)
        {
            keycardCollected = keyCardCollected;
        }

        private void UpdateObjectiveUI()
        {
            // Clear existing objectives
            foreach (Transform child in objectivesHolder.transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < objectives.Count; i++)
            {
                Objective objective = objectives[i];

                // Create a new child object
                GameObject childObject = new GameObject(objective.name);
                childObject.transform.SetParent(objectivesHolder.transform);

                RectTransform childRectTransform = childObject.AddComponent<RectTransform>();
                childRectTransform.pivot = new Vector2(0, 1);
                childRectTransform.anchorMin = new Vector2(0, 1);
                childRectTransform.anchorMax = new Vector2(0, 1);

                TextMeshProUGUI textMeshProComponent = childObject.AddComponent<TextMeshProUGUI>();
                textMeshProComponent.font = font;
                textMeshProComponent.color = objective.color;
                textMeshProComponent.text = objective.name;

                // Calculate the scaled size of the text based on the screen size
                float scaledFontSize = objectiveFontSize * GetScreenRatio();
                textMeshProComponent.fontSize = scaledFontSize;

                // Set the position of the child object
                childRectTransform.anchoredPosition = new Vector2(objectiveHorizontalPosition, -i * textVerticalMargin * GetScreenRatio());

                // Set the size of the child object
                Vector2 textSize = textMeshProComponent.GetPreferredValues();
                childRectTransform.sizeDelta = new Vector2(textSize.x, textSize.y);
            }
        }

        private float GetScreenRatio()
        {
            if (canvasScaler != null)
            {
                if (canvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
                {
                    float referenceHeight = canvasScaler.referenceResolution.y;
                    float currentHeight = Screen.height;
                    return currentHeight / referenceHeight;
                }
            }

            return 1f;
        }

        public enum ObjectiveType
        {
            None,
            CollectItem,
            ReachLocation,
            DefeatEnemy
        }

        public struct Objective
        {
            public string name;
            public ObjectiveType type;
            public bool completed;
            public Color color;
        }
    }
}