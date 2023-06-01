using System.Collections;
using System.Collections.Generic;
using Core.Enemy;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ObjectiveSystem : MonoBehaviour
    {
        public List<Objective> objectives = new List<Objective>();

        [SerializeField] GameObject objectivesHolder;

        [SerializeField] int textVerticalMargin = 50;
        [SerializeField] int objectiveFontSize = 24;
        [SerializeField] int objectiveHorizontalPosition = -75;

        private int enemiesKilledCount;

        private int objectivesCompleted = 0;
        private bool keycardCollected = false;
        private bool bossKilled = false;

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


            GameObject previousChild = null;
            // Loop on the ui element of the screen to add all these objectives onto the screen
            foreach (Objective objective in objectives)
            {
                // Create a new child object
                GameObject childObject = new GameObject(objective.name);

                // Add a TextMeshProUGUI component to the child object
                TextMeshProUGUI textMeshProComponent = childObject.AddComponent<TextMeshProUGUI>();

                // Set the text of the TextMeshPro component
                textMeshProComponent.text = objective.name;

                // Set the font size of the text
                textMeshProComponent.fontSize = objectiveFontSize;

                textMeshProComponent.color = objective.color;

                // Set the parent of the child object to the parent object
                childObject.transform.SetParent(objectivesHolder.transform);

                // Set the position and size of the child object
                if (previousChild == null)
                {
                    childObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(objectiveHorizontalPosition, 0);
                }
                else
                {
                    Vector2 previousPosition = previousChild.GetComponent<RectTransform>().anchoredPosition;
                    Vector2 previousSize = previousChild.GetComponent<TextMeshProUGUI>().preferredHeight * Vector2.up;
                    childObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(previousPosition.x, previousPosition.y - previousSize.y - textVerticalMargin);
                }
                childObject.GetComponent<RectTransform>().sizeDelta = new Vector2(objectivesHolder.GetComponent<RectTransform>().sizeDelta.x, 50f);

                // Set the current child object as the previous child object for the next iteration
                previousChild = childObject;
            }
        }


        public void CompleteObjective(string objectiveName)
        {
            Objective objective = objectives.Find(o => o.name == objectiveName);

            // Check if the object is not null
            if (!ReferenceEquals(objective, null))
            {
                objective.completed = true;
                objective.color = Color.green;

                // Find the UI element of the completed objective
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
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Key"))
            {
                // Mark the "Collect the Key" objective as completed
                FindObjectOfType<ObjectiveSystem>().CompleteObjective("Find the bossroom key");
                Destroy(other.gameObject);
                keycardCollected = true;
            }
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