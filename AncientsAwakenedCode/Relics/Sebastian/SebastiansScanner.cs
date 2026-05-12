using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Sebastian;


[Pool(typeof(EventRelicPool))]
public class SebastiansScanner : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override bool HasUponPickupEffect => true;
    
    private const string _combatsKey = "Combats";
    private int _sebastiansScannerActIndex = -1;
    
    [SavedProperty]
  private int SebastiansScannerActIndex
  {
    get => _sebastiansScannerActIndex;
    set
    {
      AssertMutable();
      _sebastiansScannerActIndex = value;
    }
  }

  [SavedProperty]
  private int[] SebastiansScannerCoordCols { get; set; } = Array.Empty<int>();

  [SavedProperty]
  private int[] SebastiansScannerCoordRows { get; set; } = Array.Empty<int>();

  [SavedProperty]
  private bool SebastiansScannerCoordsSet { get; set; }

  protected override IEnumerable<DynamicVar> CanonicalVars => [new(_combatsKey, 10M)];

  public override Task AfterObtained()
  {
    SebastiansScannerActIndex = Owner.RunState.CurrentActIndex;
    AddMarkedRooms(Owner.RunState.Map);
    return Task.CompletedTask;
  }

  public override ActMap ModifyGeneratedMapLate(IRunState runState, ActMap map, int actIndex)
  {
    if (actIndex != SebastiansScannerActIndex)
    {
      SebastiansScannerCoordCols = [];
      SebastiansScannerCoordRows = [];
      SebastiansScannerCoordsSet = false;
      return map;
    }
    return AddMarkedRooms(map);
  }

  private ActMap AddMarkedRooms(ActMap map)
  {
    if (Owner.RunState.CurrentActIndex != SebastiansScannerActIndex)
      return map;
    List<MapCoord> markedCoords = GetMarkedCoords();
    bool flag1 = markedCoords == null;
    if (!flag1)
      flag1 = !markedCoords.TrueForAll(c =>
      {
        if (!map.HasPoint(c))
          return false;
        return map.GetPoint(c).PointType == MapPointType.Monster || map.GetPoint(c).PointType == MapPointType.Elite;
      });
    if (flag1)
    {
      Rng rng = new Rng((uint) ((int) Owner.RunState.Rng.Seed + (int) (uint) Owner.NetId + StringHelper.GetDeterministicHashCode(nameof (SebastiansScanner))));
      List<MapPoint> list1 = map.GetAllMapPoints().Where((p =>
      {
        bool flag2;
        switch (p.PointType)
        {
          case MapPointType.Monster:
          case MapPointType.Elite:
            flag2 = true;
            break;
          default:
            flag2 = false;
            break;
        }
        return flag2 && !p.Quests.Any(q => q is SebastiansScanner);
      })).ToList();
      list1.UnstableShuffle(rng);
      int intValue = DynamicVars[_combatsKey].IntValue;
      List<MapPoint> list2 = list1.Take(intValue).ToList();
      SebastiansScannerCoordCols = new int[list2.Count];
      SebastiansScannerCoordRows = new int[list2.Count];
      for (int index = 0; index < list2.Count; ++index)
      {
        SebastiansScannerCoordCols[index] = list2[index].coord.col;
        SebastiansScannerCoordRows[index] = list2[index].coord.row;
      }
      SebastiansScannerCoordsSet = true;
      foreach (MapPoint mapPoint in list2)
        mapPoint.AddQuest(this);
    }
    else
    {
      foreach (MapCoord coord in markedCoords)
        (map.GetPoint(coord) ?? throw new InvalidOperationException($"Loaded a scanner map with coordinate {coord}, but the generated map does not contain that coordinate!")).AddQuest((AbstractModel) this);
    }
    return map;
  }

  public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
  {
    List<MapCoord> markedCoords = GetMarkedCoords();
    if (markedCoords == null || !markedCoords.Contains(Owner.RunState.CurrentMapPoint.coord) || player.Relics.All(r => r.Id != Id))
      return false;
    rewards.Add(new RelicReward(player));
    return true;
  }

  private List<MapCoord>? GetMarkedCoords()
  {
    if (!SebastiansScannerCoordsSet)
      return null;
    List<MapCoord> markedCoords = new List<MapCoord>();
    for (int index = 0; index < SebastiansScannerCoordCols.Length; ++index)
      markedCoords.Add(new MapCoord()
      {
        col = SebastiansScannerCoordCols[index],
        row = SebastiansScannerCoordRows[index]
      });
    return markedCoords;
  }
    
}