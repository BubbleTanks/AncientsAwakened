using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.Sebastian;


[Pool(typeof(CurseCardPool))]
public class WeighedDown : AncientsAwakenedCard
{
    [SavedProperty]
    private int[] TreasureCoordCols { get; set; } = Array.Empty<int>();

    [SavedProperty]
    private int[] TreasureCoordRows { get; set; } = Array.Empty<int>();

    [SavedProperty]
    private bool TreasureCoordsSet { get; set; }

    public WeighedDown()
        : base(-1, CardType.Curse, CardRarity.Curse, TargetType.None)
    {
    }
    
    public override bool CanBeGeneratedByModifiers => false;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("ActNumber", 2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable, CardKeyword.Innate, CardKeyword.Retain];
    public override int MaxUpgradeLevel => 0;
    
    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        List<MapCoord> markedCoords = GetMarkedCoords();
        if (markedCoords == null || !markedCoords.Contains(Owner.RunState.CurrentMapPoint.coord))
            return;
        await CardPileCmd.RemoveFromDeck(this);
    }

    public void FindTreasureCoords()
    {
        if (TreasureCoordsSet) return;
        if (IsMutable && Owner != null)
        {
            List<MapPoint> list = Owner.RunState.Map.GetAllMapPoints().Where(p =>
            {
                bool flag;
                switch (p.PointType)
                {
                    case MapPointType.Treasure:
                        flag = true;
                        break;
                    default:
                        flag = false;
                        break;
                }
                return flag;
            }).ToList();
            
            TreasureCoordCols = new int[list.Count];
            TreasureCoordRows = new int[list.Count];
            for (int index = 0; index < list.Count; ++index)
            {
                TreasureCoordCols[index] = list[index].coord.col;
                TreasureCoordRows[index] = list[index].coord.row;
            }
            TreasureCoordsSet = true;
            DynamicVars["ActNumber"].BaseValue = Owner.RunState.Act.ActNumber();
        }
    }
    
    private List<MapCoord>? GetMarkedCoords()
    {
        if (!TreasureCoordsSet)
            FindTreasureCoords();
        List<MapCoord> markedCoords = new List<MapCoord>();
        for (int index = 0; index < TreasureCoordCols.Length; ++index)
            markedCoords.Add(new MapCoord()
            {
                col = TreasureCoordCols[index],
                row = TreasureCoordRows[index]
            });
        return markedCoords;
    }
}