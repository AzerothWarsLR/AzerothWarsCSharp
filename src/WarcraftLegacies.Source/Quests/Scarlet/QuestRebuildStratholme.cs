﻿using MacroTools.ControlPointSystem;
using MacroTools.Extensions;
using MacroTools.FactionSystem;
using MacroTools.ObjectiveSystem.Objectives.ControlPointBased;
using MacroTools.ObjectiveSystem.Objectives.UnitBased;
using MacroTools.QuestSystem;
using MacroTools.LegendSystem;
using WCSharp.Shared.Data;

namespace WarcraftLegacies.Source.Quests.Scarlet
{
  /// <summary>
  /// Rebuild Stratholme and get a bunch of exp on Saiden.
  /// </summary>
  public sealed class QuestRebuildStratholme : QuestData
  {
    private readonly LegendaryHero _saiden;
    private const int ExperienceReward = 6000;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="QuestRebuildStratholme"/> class.
    /// </summary>
    public QuestRebuildStratholme(Rectangle questRect, LegendaryHero saiden) : base(
      "Stratholme",
      "Before the Plague wiped out Stratholme, Saiden had established himself there as Lord Commander of the Silver Hand. This once-glorious city must be reclaimed.",
      @"ReplaceableTextures\CommandButtons\BTNStromgardeCastle.blp")
    {
      Required = true;
      AddObjective(new ObjectiveBuildInRect(questRect, "in Stratholme", Constants.UNIT_H0BM_TOWN_HALL_CRUSADE_T1));
      AddObjective(new ObjectiveBuildInRect(questRect, "in Stratholme", Constants.UNIT_H0BP_HOUSEHOLD_CRUSADE_FARM, 6));
      AddObjective(new ObjectiveBuildInRect(questRect, "in Stratholme", Constants.UNIT_H0AG_HALL_OF_SWORDS_CRUSADE_BARRACKS, 2));
      AddObjective(new ObjectiveBuildInRect(questRect, "in Stratholme", Constants.UNIT_H09X_SHIPYARD_CRUSADE_SHIPYARD));
      AddObjective(new ObjectiveBuildInRect(questRect, "in Stratholme", Constants.UNIT_N0D8_TRADE_HOUSE_CRUSADE_SHOP));
      AddObjective(new ObjectiveControlLevel(
        ControlPointManager.Instance.GetFromUnitType(Constants.UNIT_N01M_STRATHOLME), 4));
      _saiden = saiden;
    }

    /// <inheritdoc/>
    protected override void OnComplete(Faction whichFaction)
    {
      _saiden.Unit?.AddExperience(ExperienceReward);
    }

    /// <inheritdoc/>
    protected override string RewardFlavour =>
      "The city of Stratholme once more stands as a bastion of human civilization. Though still a mere shadow of its former glory, it will reclaim its majesty in time.";

    /// <inheritdoc/>
    protected override string RewardDescription =>
      $"Saiden Dathrohan gains {ExperienceReward} experience";
  }
}