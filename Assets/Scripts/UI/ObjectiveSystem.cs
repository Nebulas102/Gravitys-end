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
        public static ObjectiveSystem instance { get; private set; }

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

        private void Awake()
        {
            if (instance is null)
                instance = this;
            else
                Destroy(gameObject);

            // Get the Canvas Scaler component
            canvasScaler = GetComponentInParent<CanvasScaler>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            EnemyBase.OnEnemyKilled += HandleEnemyKilled;

            objectivesHolder = GameObject.Find("ObjectivesHolder");
            objectives.Add(new Objective("find_br_key", "Find the bossroom key", ObjectiveType.COLLECT_ITEM, Color.white));
            objectives.Add(new Objective("kill_50", "Kill 50 enemies", ObjectiveType.DEFEAT_ENEMY, Color.white));
            objectives.Add(new Objective("kill_boss", "Defeat the Boss", ObjectiveType.DEFEAT_ENEMY, Color.white));

            UpdateObjectiveUI();
        }


        public void CompleteObjective(string objectiveName)
        {
            Objective objective = objectives.Find(o => o.id == objectiveName);

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


        private void OnDestroy()
        {
            EnemyBase.OnEnemyKilled -= HandleEnemyKilled;
        }

        public void HandleKeycardCollected()
        {
            CompleteObjective("find_br_key");
        }

        public void HandleEnemyKilled(EnemyBase enemy)
        {
            enemiesKilledCount++;
            if (enemiesKilledCount == 50)
            {
                // Mark the "Collect the Key" objective as completed
                CompleteObjective("kill_50");
            }
        }

        public void HandleBossKilled()
        {
            // Mark the "Collect the Key" objective as completed
            CompleteObjective("kill_boss");
        }

        public int GetCompletedObjectives()
        {
            return objectivesCompleted;
        }

        public bool GetKeycardCollected()
        {
            return keycardCollected;
        }

        public bool GetBossKilled()
        {
            return bossKilled;
        }

        public void SetKeycardCollected(bool keyCardCollected)
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
            if (canvasScaler is null || canvasScaler.uiScaleMode != CanvasScaler.ScaleMode.ScaleWithScreenSize)
                return 1f;

            float referenceHeight = canvasScaler.referenceResolution.y;
            float currentHeight = Screen.height;
            return currentHeight / referenceHeight;
        }

        public enum ObjectiveType
        {
            NONE,
            COLLECT_ITEM,
            REACH_LOCATION,
            DEFEAT_ENEMY
        }

        public class Objective
        {
            public string id;
            public string name;
            public ObjectiveType type;
            public bool completed;
            public Color color;

            public Objective(string index, string n, ObjectiveType t, Color c)
            {
                id = index;
                name = n;
                type = t;
                completed = false;
                color = c;
            }
        }
    }
}