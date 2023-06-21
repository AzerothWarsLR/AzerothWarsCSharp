﻿using System.Collections.Generic;
using MacroTools.ControlPointSystem;
using MacroTools.Extensions;
using MacroTools.FactionSystem;
using MacroTools.ObjectiveSystem.Objectives.ControlPointBased;
using MacroTools.ObjectiveSystem.Objectives.FactionBased;
using MacroTools.ObjectiveSystem.Objectives.UnitBased;
using MacroTools.QuestSystem;
using static War3Api.Common;


namespace WarcraftLegacies.Source.Quests.Goblin
{
  /// <summary>
  /// The Goblins can acquire Kezan.
  /// </summary>
  public sealed class QuestKezan : QuestData
  {
    private readonly List<unit> _rescueUnits;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuestKezan"/>.
    /// </summary>
    public QuestKezan() : base("Offshore Investment",
      "The island of Kezan should be the first expansion of our trade empire.",
      "ReplaceableTextures\\CommandButtons\\BTNIronHordeMerchant.blp")
    {
      AddObjective(new ObjectiveControlPoint(ControlPointManager.Instance.GetFromUnitType(Constants.UNIT_N092_ZUL_FARRAK_15GOLD_MIN)));
      AddObjective(new ObjectiveControlPoint(ControlPointManager.Instance.GetFromUnitType(Constants.UNIT_N0BK_LOST_CITY_OF_THE_TOL_VIR_15GOLD_MIN)));
      AddObjective(new ObjectiveControlPoint(ControlPointManager.Instance.GetFromUnitType(Constants.UNIT_N025_UN_GORO_CRATER_10GOLD_MIN)));
      AddObjective(new ObjectiveUpgrade(Constants.UNIT_O03N_FORTRESS_GOBLIN_T3, Constants.UNIT_O03L_GREAT_HALL_GOBLIN_T1));
      AddObjective(new ObjectiveSelfExists());
      Required = true;
      _rescueUnits = Regions.KezanUnlock.PrepareUnitsForRescue(RescuePreparationMode.HideNonStructures,
        filterUnit => filterUnit.GetTypeId() != FourCC("ngme"));
    }

    /// <inheritdoc />
    protected override string RewardFlavour => "We have succesfully expanded our trade empire!";

    /// <inheritdoc />
    protected override string RewardDescription => "You can now train Traders";

    /// <inheritdoc />
    protected override void OnFail(Faction completingFaction)
    {
      Player(PLAYER_NEUTRAL_AGGRESSIVE).RescueGroup(_rescueUnits);
    }

    /// <inheritdoc />
    protected override void OnComplete(Faction completingFaction)
    {
      completingFaction.Player.RescueGroup(_rescueUnits);
      if (GetLocalPlayer() == completingFaction.Player) 
        PlayThematicMusic("war3mapImported\\GoblinTheme.mp3");
    }
  }
}