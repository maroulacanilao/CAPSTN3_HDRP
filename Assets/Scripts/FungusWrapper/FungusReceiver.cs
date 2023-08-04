using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using BaseCore;
using CustomEvent;
using UnityEngine;
using Fungus;
using UnityEngine.Events;

namespace FungusWrapper
{
    public class FungusReceiver : MonoBehaviour
    {
        [SerializeField] private Flowchart flowchart;
        [SerializedDictionary("Key", "Event")]
        [SerializeField] private SerializedDictionary<string,UnityEvent> eventDictionary = new SerializedDictionary<string, UnityEvent>();

        public static readonly Evt<string> OnReceiveMessage = new Evt<string>();


        public void ReceiveFungusMessage(string message)
        {
            OnReceiveMessage.Invoke(message);
        }
    }
}