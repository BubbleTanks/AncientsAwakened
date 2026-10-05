using AncientsAwakened.AncientsAwakenedCode.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Mithrix;

[Pool(typeof(EventRelicPool))]
public sealed class MountainShrine : AncientsAwakenedRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];
    
    public MountainShrine()
    {
        this.BlacklistFromEulogy();
    }
    
        // map code courtesy of The Brute because I was not gonna bother figuring out how to do this myself.
    public override Task AfterObtained()
    {
        var runState = Owner.RunState;
        if (runState.Map == null)
            return Task.CompletedTask;

        var count = runState.Players.Count(p => p.GetRelic<MountainShrine>() != null);
        runState.Map = new MountainMap(count, runState.Map);
        NMapScreen.Instance?.SetMap(runState.Map, runState.Rng.Seed, false);
        Flash();
        return Task.CompletedTask;
    }
    
    public override ActMap ModifyGeneratedMapLate(IRunState runState, ActMap map, int actIndex)
    {
        var relicCount = runState.Players.Count(player => player.GetRelic<MountainShrine>() != null);
        if (relicCount <= 0)
            return map;
        
        Flash();
        return new MountainMap(relicCount, map);
    }
}

internal class MountainMap : ActMap
{
    private readonly MapPoint? _secondBoss;

    public MountainMap(int count, ActMap original)
    {
        if (count <= 0)
            return;
        
        var oldRows = original.GetRowCount();
        var columnCount = original.GetColumnCount();
        Grid = new MapPoint?[columnCount, oldRows + count];

        for (var row = 1; row < oldRows; row++)
        {
            for (var column = 0; column < columnCount; column++)
            {
                var point = original.GetPoint(column, row);

                if (point == null)
                {
                    continue;
                }

                Grid[column, row] = point;
            }
        }

        StartingMapPoint = original.StartingMapPoint;

        BossMapPoint = original.BossMapPoint;

        _secondBoss = original.SecondBossMapPoint;
        if (_secondBoss != null)
        {
        }
        else
        {
            var secondBoss = new MapPoint(BossMapPoint.coord.col, BossMapPoint.coord.row + 1)
            {
                PointType = MapPointType.Boss,
                CanBeModified = false
            };
            BossMapPoint.AddChildPoint(secondBoss);
            _secondBoss = secondBoss;
            var act = RunManager.Instance.State.Act;
            act.SetSecondBossEncounter(act.AllBossEncounters.First());
        }

        /*var restSites = BossMapPoint.parents.ToList();

        foreach (var restSite in restSites)
        {
            Grid[restSite.coord.col, restSite.coord.row] = null;
            restSite.coord.row += count;
            Grid[restSite.coord.col, restSite.coord.row] = restSite;
            
            var parents = restSite.parents.ToList();
            foreach (var parent in parents)
            {
                parent.RemoveChildPoint(restSite);

                var previous = parent;

                for (var i = 0; i < count; i++)
                {
                    var shop = new MapPoint(parent.coord.col, parent.coord.row + i + 1)
                    {
                        PointType = MapPointType.Shop,
                        CanBeModified = false
                    };

                    Grid[shop.coord.col, shop.coord.row] = shop;

                    previous.AddChildPoint(shop);

                    previous = shop;
                }

                previous.AddChildPoint(restSite);
            }
        }*/
    }

    public override MapPoint? SecondBossMapPoint => _secondBoss;
    public override MapPoint BossMapPoint { get; }
    public override MapPoint StartingMapPoint { get; }
    protected override MapPoint?[,] Grid { get; }
}
