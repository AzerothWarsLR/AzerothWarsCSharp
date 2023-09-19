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

namespace WarcraftLegacies.Source.Quests.Sunfury
{
  /// <summary>
  /// Build various structures inside <see cref="Regions.UpperNetherstorm"/>
  /// </summary>
  public sealed class QuestUpperNetherstorm : QuestData
  {
    private readonly List<unit> _rescueUnits;
    private const int GoldReward = 400;
    private const int LumberReward = 200;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuestUpperNetherstorm"/> class.
    /// </summary>
    public QuestUpperNetherstorm(Rectangle rescueRect) : base("Upper Netherstorm",
      "Upper Netherstorm is continously wracked by devastating magical storms. Lands such as these represent opportunity for our people, as starved for mana as they are.",
      @"ReplaceableTextures\CommandButtons\BTNArcaneCastle.blp")
    {
      Required = true;
      AddObjective(new ObjectiveKillAllInArea(new List<Rectangle> { Regions.UpperNetherstorm },
        "in upper Netherstorm"));
      AddObjective(
        new ObjectiveControlPoint(
          ControlPointManager.Instance.GetFromUnitType(Constants.UNIT_N0CW_FARAHLON_10GOLD_MIN)));
      AddObjective(new ObjectiveExpire(1430, Title));
      AddObjective(new ObjectiveSelfExists());
      _rescueUnits = rescueRect.PrepareUnitsForRescue(RescuePreparationMode.HideAll);
    }

    /// <inheritdoc/>
    protected override void OnComplete(Faction whichFaction)
    {
      whichFaction.Player?.AddGold(400);
      whichFaction.Player?.AddLumber(200);
      if (whichFaction.Player != null)
        whichFaction.Player.RescueGroup(_rescueUnits);
      else
        Player(PLAYER_NEUTRAL_AGGRESSIVE).RescueGroup(_rescueUnits);
    }

    /// <inheritdoc/>
    protected override string RewardFlavour => "Our people spread throughout the lands of Upper Netherstorm, erecting their homes amidst its arcane crystals and basking in its magical storms.";

    /// <inheritdoc/>
    protected override string RewardDescription => $"Gain {GoldReward} gold, {LumberReward} lumber, and a base in Upper Netherstorm";
  }
}