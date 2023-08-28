using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;

namespace Carry.CarrySystem.Map.Scripts
{
    public class AllPresenterBuilder : IPresenterBuilder
    {
        List< IPresenterBuilder>presenterBuilders = new List<IPresenterBuilder>();

        public  AllPresenterBuilder(         
            BlockPresenterBuilder blockPresenterBuilder,
            WallPresenterBuilder wallPresenterBuilder,
            GroundPresenterBuilder groundPresenterBuilder
            )
        {
            presenterBuilders.Add(blockPresenterBuilder);
            presenterBuilders.Add(wallPresenterBuilder);
            presenterBuilders.Add(groundPresenterBuilder);
        }
        
        public void Build(EntityGridMap map)
        {
            foreach (var presenterBuilder in presenterBuilders)
            {
                presenterBuilder.Build(map);
            }
        }
    }
}