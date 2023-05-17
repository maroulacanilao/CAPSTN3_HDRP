using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace ScriptableObjectData.WeightedDictionary
{
    public abstract class WeightedDictionary_ScriptableObject<T> : ScriptableObject
    {
        [SerializedDictionary("Item", "Weight")]
        [SerializeField] protected SerializedDictionary<T, float> itemDictionary = new SerializedDictionary<T, float>();

        protected Dictionary<T, float> fixedList;

        protected bool hasInitialized = false;
        protected float totalWeight;
        protected T itemWithLargestWeight;

        protected System.Random rand;

        private void OnEnable()
        {
            hasInitialized = false;
            Initialize();
        }

        private void Initialize()
        {
            if (hasInitialized) return;

            rand = new System.Random();
            fixedList = new Dictionary<T, float>();
            totalWeight = 0;
            itemWithLargestWeight = default;
            RecalculateChances();

            hasInitialized = true;
        }

        /// <summary>
        /// Recalculate the chances of the list. Should not be used outside of this class, only used for debugging. 
        /// </summary>
        public void RecalculateChances()
        {
            fixedList = new Dictionary<T, float>();
            if (itemDictionary.Count == 0) return;

            float largestWeight = 0;
            totalWeight = 0;

            foreach (var w in itemDictionary)
            {
                if (w.Value > largestWeight)
                {
                    largestWeight = w.Value;
                    itemWithLargestWeight = w.Key;
                }

                totalWeight += w.Value;
                fixedList.Add(w.Key, totalWeight);
            }
        }

        /// <summary>
        /// Get a random item according to the weighted chance of the collection
        /// </summary>
        /// <returns></returns>
        public T GetWeightedRandom()
        {
            // init class if it hasn't already
            Initialize();

            // check if the dictionary size the item list count is the same. recalculate chances if it is not 
            if (fixedList.Count != itemDictionary.Count) RecalculateChances();

            // early returns
            if (fixedList.Count == 0) return default;

            // random number
            var rngVal = (float) rand.NextDouble() * totalWeight;

            foreach (var itemWeightPair in fixedList)
            {
                if (itemWeightPair.Value <= rngVal) continue;

                return itemWeightPair.Key;
            }

            // fallback option, returns item with largest weight. this should not happen tho
            return itemWithLargestWeight;
        }

        /// <summary>
        /// Get a random item without any weight
        /// </summary>
        /// <returns></returns>
        public T GetFixedRandom()
        {
            var index = rand.Next(0, fixedList.Count);
            return fixedList.ElementAt(index).Key;
        }

        public void AddItem(T itemToAdd, float weight)
        {
            itemDictionary.Add(itemToAdd, weight);

            totalWeight += weight;

            fixedList.Add(itemToAdd, totalWeight);
        }

        public void RemoveItem(T itemToRemove)
        {
            if (!itemDictionary.Remove(itemToRemove)) return;
            if (!fixedList.Remove(itemToRemove)) return;

            RecalculateChances();
        }

        public float GetWeightOfItem(T itemToEvaluate)
        {
            return itemDictionary[itemToEvaluate];
        }

        public void ChangeWeightOfItem(T itemToChange, float newWeight)
        {
            itemDictionary[itemToChange] = newWeight;
            RecalculateChances();
        }
    }
}