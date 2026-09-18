using AncientsAwakened.AncientsAwakenedCode.Relics;
using AncientsAwakened.AncientsAwakenedCode.Relics.Gaster.CircletRelics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;

[Pool(typeof(EventRelicPool))]
public class SpikedCirclet() : AncientsAwakenedRelic
{
  public const string _starterRelicKey = "StarterRelic";
  public const string _upgradedRelicKey = "UpgradedRelic";
  public ModelId? _starterRelic;
  public ModelId? _upgradedRelic;
  public List<IHoverTip> _extraHoverTips = new List<IHoverTip>();

  public override RelicRarity Rarity => RelicRarity.Ancient;

  public static Dictionary<ModelId, RelicModel> RefinementUpgrades
  {
    get
    {
      return new Dictionary<ModelId, RelicModel>()
      {
        {
          ModelDb.Relic<BurningBlood>().Id,
          (RelicModel) ModelDb.Relic<BlackBlood>()
        },
        {
          ModelDb.Relic<RingOfTheSnake>().Id,
          (RelicModel) ModelDb.Relic<RingOfTheDrake>()
        },
        {
          ModelDb.Relic<DivineRight>().Id,
          (RelicModel) ModelDb.Relic<DivineDestiny>()
        },
        {
          ModelDb.Relic<BoundPhylactery>().Id,
          (RelicModel) ModelDb.Relic<PhylacteryUnbound>()
        },
        {
          ModelDb.Relic<CrackedCore>().Id,
          (RelicModel) ModelDb.Relic<FrigidCore>()
        }
      };
    }
  }

  [SavedProperty]
  public ModelId? StarterRelic
  {
    get => this._starterRelic;
    set
    {
      this.AssertMutable();
      this._starterRelic = !(this._starterRelic != (ModelId) null) ? value : throw new InvalidOperationException("Recursive Core setup called twice!");
      if (!(this._starterRelic != (ModelId) null))
        return;
      RelicModel relicModel = SaveUtil.RelicOrDeprecated(this._starterRelic);
      this._extraHoverTips.AddRange(relicModel.HoverTips);
      ((StringVar) this.DynamicVars[nameof (StarterRelic)]).StringValue = relicModel.Title.GetFormattedText();
    }
  }

  [SavedProperty]
  public ModelId? UpgradedRelic
  {
    get => this._upgradedRelic;
    set
    {
      this.AssertMutable();
      this._upgradedRelic = !(this._upgradedRelic != (ModelId) null) ? value : throw new InvalidOperationException("Recursive Core setup called twice!");
      if (!(this._upgradedRelic != (ModelId) null))
        return;
      RelicModel relicModel = SaveUtil.RelicOrDeprecated(this._upgradedRelic);
      this._extraHoverTips.AddRange(relicModel.HoverTips);
      ((StringVar) this.DynamicVars[nameof (UpgradedRelic)]).StringValue = relicModel.Title.GetFormattedText();
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => (IEnumerable<IHoverTip>) this._extraHoverTips;
  }

  protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar("StarterRelic"), new StringVar("UpgradedRelic")];

  protected override void AfterCloned()
  {
    base.AfterCloned();
    this._extraHoverTips = new List<IHoverTip>();
  }

  public RelicModel? GetStarterRelic(Player p)
  {
    return p.Relics.FirstOrDefault<RelicModel>((Func<RelicModel, bool>) (r => r.Rarity == RelicRarity.Starter));
  }

  public RelicModel GetUpgradedStarterRelic(RelicModel starterRelic)
  {
    RelicModel relicModel;
    return SpikedCirclet.RefinementUpgrades.TryGetValue(starterRelic.Id, out relicModel) ? relicModel : ModelDb.Relic<Circlet>().ToMutable();
  }

  /// <summary>
  /// Sets up the upgraded starter relic based off of the player.
  /// </summary>
  /// <param name="player">The player who we are upgrading the starter relic for</param>
  /// <returns>Returns false if player doesn't have the original starter relic.</returns>
  public bool SetupForPlayer(Player player)
  {
    this.AssertMutable();
    RelicModel starterRelic = this.GetStarterRelic(player);
    if (starterRelic == null)
      return false;
    this.StarterRelic = starterRelic.Id;
    this.UpgradedRelic = this.GetUpgradedStarterRelic(starterRelic).Id;
    return true;
  }

  public void SetupForTests(ModelId starterRelic, ModelId upgradedRelic)
  {
    this.AssertMutable();
    this.StarterRelic = starterRelic;
    this.UpgradedRelic = upgradedRelic;
  }

  public override async Task AfterObtained()
  {
    SpikedCirclet spikedCirclet = this;
    ModelId modelId = spikedCirclet.StarterRelic;
    if ((object) modelId == null)
      modelId = spikedCirclet.Owner.Relics.First<RelicModel>((Func<RelicModel, bool>) (r => r.Rarity == RelicRarity.Starter)).Id;
    ModelId id1 = modelId;
    RelicModel relicById = spikedCirclet.Owner.GetRelicById(id1);
    ModelId id2 = spikedCirclet.UpgradedRelic;
    if ((object) id2 == null)
      id2 = spikedCirclet.GetUpgradedStarterRelic(relicById).Id;
    RelicModel mutable = ModelDb.GetById<RelicModel>(id2).ToMutable();
    RelicModel relicModel = await RelicCmd.Replace(relicById, mutable);
  }
}