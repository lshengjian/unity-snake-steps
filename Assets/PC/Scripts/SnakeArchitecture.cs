 namespace QFramework.MyGame
{
 
 public class SnakeArchitecture : Architecture<SnakeArchitecture>
    {
        protected override void Init()
        {
            RegisterModel(new FoodModel());
            RegisterModel(new GameModel());

            RegisterSystem<ISysMove>(new SysMove());
            RegisterSystem<ISysSpawn>(new SysSpawn());
            RegisterSystem<ISysAchievement>(new SysAchievement());

            RegisterUtility<IStorage>(new PlayerPrefsStorage());
         
;
        }
    }
}