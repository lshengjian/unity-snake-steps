using UnityEngine;

namespace QFramework.MyGame
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class SnakeBody : MonoBehaviour, IController
    {
        Detector mDetector;
        float mCooldown = 5f;
        float mTimer = 0f;
        GameModel mGameModel;
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return SnakeArchitecture.Interface;
        }



        void Start()
        {
            mGameModel = this.GetModel<GameModel>();
            mDetector = GetComponent<Detector>();
            mDetector.OnTouched += OnHitBody;


        }
        private void Update()
        {
            mTimer -= Time.deltaTime;
        }
        void OnDestroy()
        {
            mDetector.OnTouched -= OnHitBody;
        }
        void OnHitBody(Transform self, Transform other)
        {
            if (mTimer < 0f&&self.name==this.name)
            {
                mTimer = mCooldown;
              //  Debug.Log(self.parent.name + "-->" + other.parent.name);
                var pd = mGameModel.GetPlayerData(this.name);

                pd.Score.Value = 0;//碰撞双方均会调用！
            }

        }



    }
}