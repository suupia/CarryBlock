using System.Collections.Generic;
using System.Linq;
using Fusion;

namespace Decoration
{
    public class Boss1DecorationDetector
    {
        private int _preTackleCount;
        private readonly List<IBoss1Decoration> _decorations;
        private int _preHp;

        public struct Data: INetworkStruct
        {
            public int TackleCount;
        }

        public Boss1DecorationDetector(params IBoss1Decoration[] decorations)
        {
            _decorations = decorations.ToList();
        }
        public void OnStartTackle(ref Data data)
        {
            data.TackleCount++;
        }

        public void OnEndTackle(ref Data data)
        {
            data.TackleCount--;
        }

        public void OnRendered(ref Data data, int hp)
        {
            _decorations.ForEach(d => d.OnMoved());
            
            if (data.TackleCount > _preTackleCount)
            {
                _decorations.ForEach(d => d.OnTackle(true));
                _preTackleCount = data.TackleCount;
            }
            if (data.TackleCount < _preTackleCount)
            {
                _decorations.ForEach(d => d.OnTackle(false));
                _preTackleCount = data.TackleCount;
            }
            
            if (hp != _preHp) OnChangedHp(hp);

        }

        private void OnChangedHp(int hp)
        {
            if (hp <= 0)
            {
                _decorations.ForEach(d => d.OnDead());
            }
            else if (hp < _preHp)
            {
                _decorations.ForEach(d => d.OnDamaged());
            }

            _preHp = hp;
        }
    }
}