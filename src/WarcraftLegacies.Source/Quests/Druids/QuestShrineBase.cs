﻿using System.Collections.Generic;
using MacroTools.ControlPointSystem;
using MacroTools.Extensions;
using MacroTools.FactionSystem;
using MacroTools.ObjectiveSystem.Objectives.ControlPointBased;
using MacroTools.ObjectiveSystem.Objectives.FactionBased;
using MacroTools.ObjectiveSystem.Objectives.TimeBased;
using MacroTools.ObjectiveSystem.Objectives.UnitBased;
using MacroTools.QuestSystem;
using WCSharp.Shared.Data;
using static War3Api.Common;

namespace WarcraftLegacies.Source.Quests.Druids
{
  public sealed class QuestShrineBase : QuestData
  {
    private readonly List<unit> _rescueUnits = new();

    public QuestShrineBase(Rectangle rescueRect) : base("Hyjal's Rest",
      "Mount Hyjal has been invaded by the corruption already affecting Felwood. Clear them out to awaken the Ancients",
      "ReplaceableTextures\\CommandButtons\\BTNAncientOfTheMoon.blp")
    {
      AddObjective(new ObjectiveControlPoint(ControlPointManager.Instance.GetFromUnitType(Constants.UNIT_N0BI_SHRINE_TO_MALORNE_20GOLD_MIN)));
      AddObjective(new ObjectiveKillAllInArea(new List<Rectangle> { Regions.ShrineBaseUnlock }, "in Hyjal"));
      AddObjective(new ObjectiveExpire(1283, Title));
      AddObjective(new ObjectiveSelfExists());
      Required = true;
      _rescueUnits = rescueRect.PrepareUnitsForRescue(RescuePreparationMode.HideNonStructures);

      Required = true;
    }

    /// <inheritdoc />
    protected override string RewardFlavour =>
      "The World Tree has been rid of corruption and the Ancients can awaken";

    /// <inheritdoc />
    protected override string RewardDescription =>
      "Control of all units in the Shrine of Malorne base";

    /// <inheritdoc />
    protected override void OnFail(Faction completingFaction)
    {
      foreach (var unit in _rescueUnits) unit.Rescue(Player(PLAYER_NEUTRAL_AGGRESSIVE));
    }

    /// <inheritdoc />
    protected override void OnComplete(Faction completingFaction)
    {
      foreach (var unit in _rescueUnits) unit.Rescue(completingFaction.Player);
    }
  }
}