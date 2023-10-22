using System.Collections.Generic;

using UnityEngine;

namespace QFramework.MyGame
{
    public class FoodModel : AbstractModel
    {
        public  int NumFood {get;}= 3;

        List<BindableProperty<Vector2>> mPositions = new List<BindableProperty<Vector2>>();
        protected override void OnInit()
        {
            for (int i = 0; i < NumFood; i++)
            {
                mPositions.Add(new BindableProperty<Vector2>());
            }
        }
         public void  SetPositionAt(int idx,Vector2 pos)
        {
            mPositions[idx].Value=pos;
        }
        public BindableProperty<Vector2> GetBindPropAt(int idx)
        {
            return mPositions[idx];

        }

    }
}