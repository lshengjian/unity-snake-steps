using UnityEngine;
namespace QFramework.MyGame
{

    public class MockGame : Architecture<MockGame>
    {
        protected override void Init()
        {
            RegisterModel<FoodModel>(new FoodModel());
            RegisterModel<GameModel>(new GameModel());

            RegisterSystem<ISysMove>(new SysMove());

            RegisterUtility<IStorage>(new PlayerPrefsStorage());

           

        }
    }
}