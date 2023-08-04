using System.Collections;
using Character;
using Managers;
using ScriptableObjectData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dungeon
{
    public class DungeonBossMap : DungeonMap
    {
        [SerializeField] private ProgressionData progressionData;
        [SerializeField] private GameObject controllerDisabler;
        [SerializeField] private GameObject camera;
        [SerializeField] private EnemyCharacter sidapa;
        [SerializeField] private string defeatSidapaMessage = "DefeatSidapa";
        [SerializeField] private string fungusMessage = "spawnEnding";

        protected override void Awake()
        {
            base.Awake();
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }
        
        private void OnActiveSceneChanged(Scene arg0_, Scene next_)
        {
            if(next_.name != gameObject.scene.name) return;
            if(DungeonManager.Instance.currDungeon != this) return;
            StartCoroutine(Co_SendMessage());
        }
        
        

        protected override void RemoveEnemy(EnemyCharacter enemyCharacter_)
        {
            Debug.Log(enemyCharacter_.gameObject);
            Debug.Log(sidapa.gameObject);
            
            if(enemyCharacter_ != sidapa) return;

            Debug.Log(sidapa.gameObject);
            Destroy(enemyCharacter_.gameObject);
            progressionData.hasDefeatedSidapa = true;
            controllerDisabler.SetActive(false);
            camera.SetActive(false);
            
            var _gameDatabase = GameManager.Instance.GameDataBase;
            var _farmSceneName = _gameDatabase.FarmSceneName;

            void SendToFungus()
            {
                Fungus.Flowchart.BroadcastFungusMessage(fungusMessage);
                FarmSceneManager.Instance.EnableEndingCutscene();
                Debug.Log("Sent to fungus");
            }
            
            _gameDatabase.eventQueueData.AddEvent(_farmSceneName,SendToFungus);
        }

        private IEnumerator Co_SendMessage()
        {
            yield return new WaitForSecondsRealtime(1.5f);
            Fungus.Flowchart.BroadcastFungusMessage(defeatSidapaMessage);
        }
    }
}
