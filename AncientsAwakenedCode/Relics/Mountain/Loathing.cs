using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Mountain;

[Pool(typeof(EventRelicPool))]
public sealed class Loathing : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override bool HasUponPickupEffect => true;
    
    private const string _combatsKey = "Combats";
    private int _actIndex = -1;
    
    [SavedProperty]
    private int ActIndex
    {
        get => _actIndex;
        set
        {
          AssertMutable();
          _actIndex = value;
        }
    }

  [SavedProperty]
  private int[] CoordCols { get; set; } = Array.Empty<int>();

  [SavedProperty]
  private int[] CoordRows { get; set; } = Array.Empty<int>();

  [SavedProperty]
  private bool CoordsSet { get; set; }

  protected override IEnumerable<DynamicVar> CanonicalVars => [new(_combatsKey, 12M), new MaxHpVar(5M)];

  public override Task AfterObtained()
  {
    ActIndex = Owner.RunState.CurrentActIndex;
    AddMarkedRooms(Owner.RunState.Map);
    return Task.CompletedTask;
  }

  public override ActMap ModifyGeneratedMapLate(IRunState runState, ActMap map, int actIndex)
  {
    if (actIndex != ActIndex)
    {
      CoordCols = [];
      CoordRows = [];
      CoordsSet = false;
      return map;
    }
    return AddMarkedRooms(map);
  }

  private ActMap AddMarkedRooms(ActMap map)
  {
    if (Owner.RunState.CurrentActIndex != ActIndex)
      return map;
    var markedCoords = GetMarkedCoords();
    var flag1 = markedCoords == null;
    if (!flag1)
      flag1 = !markedCoords.TrueForAll(c =>
      {
        if (!map.HasPoint(c))
          return false;
        return map.GetPoint(c).PointType == MapPointType.Monster || map.GetPoint(c).PointType == MapPointType.Elite;
      });
    if (flag1)
    {
      var rng = new Rng(Owner, Id);
      var list1 = map.GetAllMapPoints().Where(p =>
      {
        var flag2 = p.PointType switch
        {
          MapPointType.Monster or MapPointType.Elite => true,
          _ => false
        };
        return flag2 && !p.Quests.Any(q => q is Loathing);
      }).ToList();
      list1.UnstableShuffle(rng);
      var intValue = DynamicVars[_combatsKey].IntValue;
      var list2 = list1.Take(intValue).ToList();
      CoordCols = new int[list2.Count];
      CoordRows = new int[list2.Count];
      for (var index = 0; index < list2.Count; ++index)
      {
        CoordCols[index] = list2[index].coord.col;
        CoordRows[index] = list2[index].coord.row;
      }
      CoordsSet = true;
      foreach (var mapPoint in list2)
        mapPoint.AddQuest(this);
    }
    else
    {
      foreach (var coord in markedCoords)
        (map.GetPoint(coord) ?? throw new InvalidOperationException($"Loaded a loathing map with coordinate {coord}, but the generated map does not contain that coordinate!")).AddQuest(this);
    }
    return map;
  }
  
  public override async Task AfterCombatEnd(CombatRoom room)
  {
    var markedCoords = GetMarkedCoords();
    if (markedCoords == null || !markedCoords.Contains(Owner.RunState.CurrentMapPoint.coord))
      return;
    Flash();
    await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue);
  }
  private List<MapCoord>? GetMarkedCoords()
  {
    if (!CoordsSet)
      return null;
    var markedCoords = new List<MapCoord>();
    for (var index = 0; index < CoordCols.Length; ++index)
      markedCoords.Add(new MapCoord
      {
        col = CoordCols[index],
        row = CoordRows[index]
      });
    return markedCoords;
  }
    
}