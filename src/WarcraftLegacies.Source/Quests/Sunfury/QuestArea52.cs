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
  /// Build various structures inside <see cref="Regions.Area52Unlock"/>
  /// </summary>
  public sealed class QuestArea52 : QuestData
  {
    private readonly List<unit> _rescueUnits;
    private const int GoldReward = 400;
    private const int LumberReward = 200;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="QuestArea52"/> class.
    /// </summary>
    public QuestArea52(Rectangle rescueRect) : base("Area 52",
      "The goblins of Area 52 have lived in Netherstorm long before our arrival. In other circumstances, they may have been potential allies - but desperate times call for desperate conquests.",
      @"ReplaceableTextures\CommandButtons\BTNLordaeronPalace.blp")
    {
      Required = true;
      AddObjective(new ObjectiveKillAllInArea(new List<Rectangle> { Regions.Area52Unlock }, "in Area 52"));
      AddObjective(
        new ObjectiveControlPoint(
          ControlPointManager.Instance.GetFromUnitType(Constants.UNIT_N07Q_AREA_52_10GOLD_MIN)));
      AddObjective(new ObjectiveExpire(1430, Title));
      AddObjective(new ObjectiveSelfExists());
      _rescueUnits = rescueRect.PrepareUnitsForRescue(RescuePreparationMode.HideAll);
    }

    /// <inheritdoc/>
    protected override void OnComplete(Faction whichFaction)
    {
      whichFaction.Player?.AddGold(GoldReward);
      whichFaction.Player?.AddLumber(LumberReward);
      if (whichFaction.Player != null)
        whichFaction.Player.RescueGroup(_rescueUnits);
      else
        Player(PLAYER_NEUTRAL_AGGRESSIVE).RescueGroup(_rescueUnits);
    }

    /// <inheritdoc/>
    protected override string RewardFlavour =>
      "The goblins of Area 52 once aspired to travel amongst the stars. Now they aspire to nothing, buried one layer of dirt beneath our new settlement.";

    /// <inheritdoc/>
    protected override string RewardDescription =>
      $"Gain {GoldReward} gold, {LumberReward} lumber, and a base in Area 52";
  }
}