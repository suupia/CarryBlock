using Main;

namespace Boss
{
    public class NetworkBoss1Controller : PoolableObject
    {
        protected override void OnInactive()
        {
            //　ファイナライザ的な処理を書く
        }
    }
}