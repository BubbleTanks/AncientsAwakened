using AncientsAwakened.AncientsAwakenedCode.Cards.Gaster;
using AncientsAwakened.AncientsAwakenedCode.Extensions;
using AncientsAwakened.AncientsAwakenedCode.Relics.Gaster.CircletRelics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;

[Pool(typeof(EventRelicPool))]
public sealed class SpikedCirclet : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    private const string StarterRelicKey = "StarterCard";
    private const string AncientRelicKey = "AncientCard";

    private List<IHoverTip> _extraHoverTips = [];

    private ModelId? _starterRelic;
    private ModelId? _ancientRelic;

    private static Dictionary<ModelId, ModelId>? _thornRelics;
    
    private static Dictionary<ModelId, ModelId> VanillaThornRelics => new()
    {
        {
            ModelDb.Relic<BurningBlood>().Id,
            ModelDb.Relic<Anchor>().Id
        },
        {
            ModelDb.Relic<RingOfTheSnake>().Id,
            ModelDb.Relic<Anchor>().Id
        },
        {
            ModelDb.Relic<DivineRight>().Id,
            ModelDb.Relic<Anchor>().Id
        },
        {
            ModelDb.Relic<BoundPhylactery>().Id,
            ModelDb.Relic<Anchor>().Id
        },
        {
            ModelDb.Relic<CrackedCore>().Id,
            ModelDb.Relic<FrigidCore>().Id
        }
    };
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => _extraHoverTips;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar(StarterRelicKey), new StringVar(AncientRelicKey)];
    
    protected override bool RelicAllowedToSpawn(Player owner)
    {
        return SetupForPlayer(owner);
    }

    private static Dictionary<ModelId, ModelId> ThornRelics
    {
        get
        {
            if (_thornRelics == null)
            {
                _thornRelics = new Dictionary<ModelId, ModelId>();
                foreach (var kv in VanillaThornRelics)
                {
                    _thornRelics.Add(kv.Key, kv.Value);
                }
                foreach (var kv in CustomThornRingCardExtension.CustomThornRingRelics)
                {
                    _thornRelics.Add(kv.Key, kv.Value);
                }
            }
            return _thornRelics;
        }
    }

    public override bool HasUponPickupEffect => true;

    [SavedProperty]
    private ModelId? StarterRelic
    {
        get => _starterRelic;
        set
        {
            AssertMutable();
            _starterRelic = value;
            UpdateHoverTips();
        }
    }

    [SavedProperty]
    private ModelId? AncientRelic
    {
        get => _ancientRelic;
        set
        {
            AssertMutable();
            _ancientRelic = value;
            UpdateHoverTips();
        }
    }

    public bool SetupForPlayer(Player player)
    {
        if (player == null)
            return false;
        
        AssertMutable();
        
        var starter = GetStarterRelic(player);
        if (starter == null)
            return false;
            
        StarterRelic = starter.Id;
        AncientRelic = GetThornRelic(starter);
        
        UpdateHoverTips();
        return true;
    }

    public override async Task AfterObtained()
    {
        var starterCard = GetStarterRelic(Owner);
        if(starterCard == null)
            return;
        await RelicCmd.Replace(starterCard, ModelDb.GetById<RelicModel>(GetThornRelic(starterCard)).ToMutable());
    }

    private static RelicModel? GetStarterRelic(Player player) => player.Relics.FirstOrDefault(c => ThornRelics.ContainsKey(c.Id));
    
    private static ModelId GetThornRelic(RelicModel starterRelic)
    {
        ModelId? replacement = null;
        if (ThornRelics.TryGetValue(starterRelic.Id, out var thornRelic))
        {
            replacement = thornRelic;
        }

        return replacement == null ? ModelDb.Relic<Circlet>().Id : replacement;
    }
    
    protected override void AfterCloned()
    {
        base.AfterCloned();
        _extraHoverTips = [];
    }

    private void UpdateHoverTips()
    {
        _extraHoverTips.Clear();
        if (StarterRelic != null)
        {
            var relic = ModelDb.GetById<RelicModel>(StarterRelic);
            _extraHoverTips.AddRange(relic.HoverTips);
            _extraHoverTips.AddRange(HoverTipFactory.FromRelic(relic));
            ((StringVar)DynamicVars[AncientRelicKey]).StringValue = relic.Title.GetFormattedText();
        }
        if (AncientRelic != null)
        {
            var relic = ModelDb.GetById<RelicModel>(AncientRelic);
            _extraHoverTips.AddRange(relic.HoverTips);
            _extraHoverTips.AddRange(HoverTipFactory.FromRelic(relic));
            ((StringVar)DynamicVars[AncientRelicKey]).StringValue = relic.Title.GetFormattedText();
        }
    }
}