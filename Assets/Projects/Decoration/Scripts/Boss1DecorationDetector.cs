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
        private int _preJumpCount;
        private int _preSpitOutCount;

        public struct Data : INetworkStruct
        {
            public int TackleCount;
            public int JumpCount;
            public int SpitOutCount;
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


        public void OnStartJump(ref Data data)
        {
            data.JumpCount++;
        }

        public void OnEndJump(ref Data data)
        {
            data.JumpCount--;
        }

        public void OnSpitOut(ref Data data)
        {
            data.SpitOutCount++;
        }

        public void OnRendered(Data data, int hp)
        {
            _decorations.ForEach(d => d.OnMoved());

            CheckTackle(data);

            CheckJump(data);
            if (data.SpitOutCount > _preSpitOutCount)
            {
                _decorations.ForEach(d => d.OnSpitOut());
                _preSpitOutCount = data.SpitOutCount;
            }

            if (hp != _preHp) OnChangedHp(hp);
        }

        private void CheckTackle(Data data)
        {
            if (data.TackleCount > _preTackleCount)
            {
                _decorations.ForEach(d => d.OnTackle(true));
                _preTackleCount = data.TackleCount;
            }
            else if (data.TackleCount < _preTackleCount)
            {
                _decorations.ForEach(d => d.OnTackle(false));
                _preTackleCount = data.TackleCount;
            }
        }

        private void CheckJump(Data data)
        {
            if (data.JumpCount > _preJumpCount)
            {
                _decorations.ForEach(d => d.OnJump(true));
                _preJumpCount = data.JumpCount;
            }
            else if (data.JumpCount < _preJumpCount)
            {
                _decorations.ForEach(d => d.OnJump(false));
                _preJumpCount = data.JumpCount;
            }
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