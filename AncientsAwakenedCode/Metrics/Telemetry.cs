using System.Reflection;
using System.Text.Json;
using AncientsAwakened.AncientsAwakenedCode.Ancients;
using AncientsAwakened.AncientsAwakenedCode.UI;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.Metrics;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Metrics;

public static class Telemetry
{
    private const int MinimumFloors = 5;

    private static readonly HashSet<Assembly> AllowedAssemblies =
    [
        typeof(CharacterModel).Assembly,
        typeof(AncientsAwakenedAncient).Assembly
    ];

    public static void OnMetricsUpload(
        SerializableRun run,
        bool isVictory,
        ulong localPlayerId)
    {
        if (!ShouldUpload(run, localPlayerId))
            return;

        try
        {
            RunMetrics metrics;

            // Keeps any localized strings in the collected data consistent.
            LocManager.Instance.StartOverridingLanguageAsEnglish();

            try
            {
                metrics = GetRunMetrics(
                    run,
                    isVictory,
                    localPlayerId);
            }
            finally
            {
                LocManager.Instance.StopOverridingLanguageAsEnglish();
            }

            string dataJson = JsonSerializer.Serialize(
                metrics,
                MetricsSerializerContext.Default.RunMetrics);

            bool hasForeignContent =
                HasForeignContent(run);

            string wrappedJson =
                $$"""
                {
                    "schema_version": 1,
                    "mod_version": {{JsonSerializer.Serialize(GetModVersion())}},
                    "has_foreign_content": {{JsonSerializer.Serialize(hasForeignContent)}},
                    "data": {{dataJson}}
                }
                """;

            Console.WriteLine(
                $"[AncientsAwakened] Uploading run metrics. " +
                $"Foreign content: {hasForeignContent}");

            _ = MetricsClient.Upload(wrappedJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"[AncientsAwakened] Failed to prepare metrics:\n{ex}");
        }
    }

    private static bool ShouldUpload(
        SerializableRun run,
        ulong localPlayerId)
    {
        // Respect the game's own metrics preference.
        if (!MetricUtilities.ShouldUploadMetrics(true))
        {
            Console.WriteLine(
                "[AncientsAwakened] Metrics skipped: " +
                "game metrics are disabled.");

            return false;
        }

        // Engineer-specific opt-in.
        if (!AncientConfigs.UploadMetrics)
        {
            Console.WriteLine(
                "[AncientsAwakened] Metrics skipped: " +
                "AncientsAwakened metrics are disabled.");

            return false;
        }

        // Don't treat deliberately abandoned runs as normal losses.
        if (RunManager.Instance.IsAbandoned)
        {
            Console.WriteLine(
                "[AncientsAwakened] Metrics skipped: " +
                "run was abandoned.");

            return false;
        }

        // Keep the dataset focused on normal runs.
        if (run.GameMode != GameMode.Standard)
        {
            Console.WriteLine(
                "[AncientsAwakened] Metrics skipped: " +
                "not a Standard run.");

            return false;
        }

        int floorCount =
            run.MapPointHistory
                .SelectMany(entries => entries)
                .Count();

        // Mostly filters runs that barely started.
        if (floorCount < MinimumFloors)
        {
            Console.WriteLine(
                $"[AncientsAwakened] Metrics skipped: " +
                $"only {floorCount} floors reached.");

            return false;
        }

        SerializablePlayer? localPlayer =
            run.Players.FirstOrDefault(
                player =>
                    (long)player.NetId ==
                    (long)localPlayerId);

        if (localPlayer?.CharacterId is not { } characterId)
        {
            Console.WriteLine(
                "[AncientsAwakened] Metrics skipped: " +
                "could not find local player.");

            return false;
        }

        CharacterModel? character =
            ModelDb.GetByIdOrNull<CharacterModel>(
                characterId);

        if (character == null)
        {
            Console.WriteLine(
                "[AncientsAwakened] Metrics skipped: " +
                "could not resolve character.");

            return false;
        }

        return true;
    }

    private static RunMetrics GetRunMetrics(
        SerializableRun run,
        bool isVictory,
        ulong localPlayerId)
    {
        var history = run.MapPointHistory.SelectMany(logs => logs).ToList();

        var killedByEncounter = ModelId.none;
        var lastEntry = run.MapPointHistory.LastOrDefault()?.LastOrDefault();
        if (!isVictory && lastEntry is not null && lastEntry.Rooms.Last().RoomType.IsCombatRoom())
        {
            killedByEncounter = lastEntry.Rooms.Last().ModelId!;
        }

        var localPlayer = run.Players.First(p => p.NetId == localPlayerId);

        var encounters = history
            .Where(e => e.Rooms.Last().RoomType.IsCombatRoom())
            .Select(e => new EncounterMetric(
                e.Rooms.Last().ModelId!.Entry,
                Math.Min(e.GetEntry(localPlayerId).DamageTaken, localPlayer.MaxHp),
                e.Rooms.Last().TurnsTaken + 1))
            .ToList();

        var cardChoices = history
            .Where(e => e.GetEntry(localPlayerId).CardChoices.Count > 0)
            .Select(e => new CardChoiceMetric(e.GetEntry(localPlayerId).CardChoices))
            .ToList();

        var ancientChoices = history
            .Where(e => e.MapPointType == MapPointType.Ancient)
            .Where(e => e.GetEntry(localPlayerId).AncientChoices.Count > 0)
            .Select(e => new AncientMetric(e, e.GetEntry(localPlayerId)))
            .ToList();

        var actWins = new List<ActWinMetric>();
        var eventChoices = new List<EventChoiceMetric>();
        for (var actIndex = 0; actIndex < run.MapPointHistory.Count; actIndex++)
        {
            foreach (var entry in run.MapPointHistory[actIndex])
            {
                if (entry.Rooms.First().RoomType == RoomType.Event
                    && entry.GetEntry(localPlayerId).EventChoices.Count != 0
                    && entry.MapPointType != MapPointType.Ancient)
                {
                    eventChoices.Add(new EventChoiceMetric(entry, localPlayerId, run.Acts[actIndex]));
                }
            }

            var actWin = actIndex < run.MapPointHistory.Count - 1 || isVictory;
            actWins.Add(new ActWinMetric(run.Acts[actIndex].Id!.Entry, actWin));
        }

        var progress = SaveManager.Instance.Progress;

        return new RunMetrics
        {
            Ascension = run.Ascension,
            TotalPlaytime = progress.TotalPlaytime,
            TotalWinRate = (float)progress.Wins / progress.NumberOfRuns,
            NumReloads = run.NumReloads,
            BuildId = ReleaseInfoManager.Instance.ReleaseInfo?.Version ?? "NON-RELEASE-VERSION",
            BuildType = PlatformUtil.GetPlatformBranch()
                .ToName(),
            PlayerId = progress.UniqueId,
            Character = localPlayer.CharacterId!,
            NumPlayers = run.Players.Count,
            Team = run.Players.Count > 1
                ? run.Players.Select(p => p.CharacterId!)
                    .ToList()
                :
                [
                ],
            Win = isVictory,
            FloorReached = history.Count,
            KilledByEncounter = killedByEncounter,
            Deck = localPlayer.Deck.Select(c => c.Id!),
            Relics = localPlayer.Relics.Select(r => r.Id!),
            RunPlaytime = run.WinTime > 0
                ? run.WinTime
                : run.RunTime,
            Encounters = encounters,
            CardChoices = cardChoices,
            EventChoices = eventChoices,
            AncientChoices = ancientChoices,
            ActWins = actWins,
            CampfireUpgrades = history
                .Where(e => e.MapPointType == MapPointType.RestSite)
                .SelectMany(e => e.GetEntry(localPlayerId)
                    .UpgradedCards)
                .Select(c => c.Entry)
                .ToList(),
            RelicBuys = history.SelectMany(e => e.GetEntry(localPlayerId)
                    .BoughtRelics)
                .Select(r => r.Entry)
                .ToList(),
            PotionBuys = history.SelectMany(e => e.GetEntry(localPlayerId)
                    .BoughtPotions)
                .Select(p => p.Entry)
                .ToList(),
            ColorlessBuys = history.SelectMany(e => e.GetEntry(localPlayerId)
                    .BoughtColorless)
                .Select(c => c.Entry)
                .ToList(),
            PotionDiscards = history.SelectMany(e => e.GetEntry(localPlayerId)
                    .PotionDiscarded)
                .Select(p => p.Entry)
                .ToList(),
            GameMode = run.GameMode.ToString(),
        };
    }

    private static bool HasForeignContent(
        SerializableRun run)
    {
        // Custom acts
        if (run.Acts.Any(
                act =>
                    !IsAllowed<ActModel>(act.Id)))
        {
            return true;
        }

        // Custom encounters / rooms
        if (run.MapPointHistory
            .SelectMany(entries => entries)
            .SelectMany(entry => entry.Rooms)
            .Any(room =>
                !IsAllowed<AbstractModel>(
                    room.ModelId)))
        {
            return true;
        }

        foreach (SerializablePlayer player in run.Players)
        {
            // Custom characters
            if (!IsAllowed<CharacterModel>(
                    player.CharacterId))
            {
                return true;
            }

            // Custom cards
            if (player.Deck.Any(card =>
                    !IsAllowed<CardModel>(
                        card.Id)))
            {
                return true;
            }

            // Custom relics
            if (player.Relics.Any(relic =>
                    !IsAllowed<RelicModel>(
                        relic.Id)))
            {
                return true;
            }

            // Custom potions
            if (player.Potions.Any(potion =>
                    !IsAllowed<PotionModel>(
                        potion.Id)))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsAllowed<T>(
        ModelId? id)
        where T : AbstractModel
    {
        if (id is null ||
            id == ModelId.none)
        {
            return true;
        }

        T? model =
            ModelDb.GetByIdOrNull<T>(id);

        return model != null &&
               AllowedAssemblies.Contains(
                   model.GetType().Assembly);
    }

    private static string GetModVersion()
    {
        var mod =
            ModManager
                .GetLoadedMods()
                .FirstOrDefault(
                    loaded =>
                        loaded.manifest?.id ==
                        "AncientsAwakened");

        return mod?.manifest?.version
               ?? "unknown";
    }
}