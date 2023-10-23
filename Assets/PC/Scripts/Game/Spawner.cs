
using UnityEngine;

namespace QFramework.MyGame
{
    public class Spawner : MonoBehaviour, IController
    {
        [SerializeField] GameObject foodPrefab;
        [SerializeField] GameObject snakePrefab;
        [SerializeField] GameObject playerInfoPrefab;
        [SerializeField] Transform canvas;

        private void Awake()
        {

            var mGameModel = this.GetModel<GameModel>();
            int i=0;
            var offset=new Vector2(180,-130);
            foreach (var name in mGameModel.PlayerNames)
            {
                GameObject snake = Instantiate(snakePrefab, this.transform);
                snake.name = name;
                snake.transform.localPosition = Vector3.zero;
                GameObject ui = Instantiate(playerInfoPrefab,canvas);
                var info = ui.GetComponent<PlayerInfo>();
                info.playerName = name;
                var rt = ui.GetComponent<RectTransform>();
                rt.anchoredPosition=new Vector2(i*240.0f,0f)+offset;
                i++;
            }
            MakeFoods();

        }

    
        void MakeFoods()
        {
            var sys = this.GetSystem<ISysSpawn>();
            var foods = sys.FoodBindPositions;

            int i = 1;
            foreach (var bp in foods)
            {
                GameObject food = Instantiate(foodPrefab, this.transform);
                food.name = "Food_" + i.ToString();
             //   Debug.Log(food.name);
                bp.RegisterWithInitValue((pos) =>
                {
                    food.transform.localPosition = pos;
                }).UnRegisterWhenGameObjectDestroyed(food);
                i++;
            }
        }

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return SnakeArchitecture.Interface;
        }
    }
}
