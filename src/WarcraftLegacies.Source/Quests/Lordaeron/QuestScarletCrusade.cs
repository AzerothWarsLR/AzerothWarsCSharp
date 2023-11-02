﻿using System;
using System.Collections.Generic;
using MacroTools.Extensions;
using MacroTools.FactionSystem;
using MacroTools.QuestSystem;
using WCSharp.Shared.Data;
using static War3Api.Common;
using MacroTools.LegendSystem;
using MacroTools.ObjectiveSystem.Objectives.UnitBased;
using System.Linq;
using MacroTools.Libraries;
using static MacroTools.Libraries.GeneralHelpers;
using MacroTools.ObjectiveSystem.Objectives.QuestBased;

namespace WarcraftLegacies.Source.Quests.Lordaeron
{
  /// <summary>
  /// Lordaeron gives up and is destroyed, the scarlet crusade spawns.
  /// </summary>
  public sealed class QuestScarletCrusade : QuestData
  {
    private readonly unit _tyrsHand;
    private readonly LegendaryHero _saiden;
    private readonly List<unit> _rescueUnits;
    private readonly Faction _scarletCrusade;
    private const int StartingGold = 300;
    private const int StartingLumber = 500;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuestScarletCrusade"/> class.
    /// </summary>
    public QuestScarletCrusade(Rectangle rescueRect, Capital tyrsHand, LegendaryHero saiden, Faction scarletCrusade, QuestData fortifiedCity) :
      base("The Scarlet Crusade",
        "Lordaeron is destined to fall to the Scourge. Should such an event come to pass, the Silver Hand will be transformed beyond recognition, abandoning ideals of justice in favour of those of vengeance.",
        @"ReplaceableTextures\CommandButtons\BTNDivine_Reckoning_Icon.blp")
    {
      if (tyrsHand.Unit == null)
        throw new ArgumentException($"Expected {nameof(tyrsHand)} to have a non-null {tyrsHand.Unit} property.", nameof(tyrsHand));
      
      AddObjective(new ObjectiveResearch(Constants.UPGRADE_R0XZ_THE_SCARLET_CRUSADE_LORDAERON_SCARLET,
        Constants.UNIT_H030_TYR_S_HAND_CITADEL_LORDAERON_OTHER));
      AddObjective(new ObjectiveQuestComplete(fortifiedCity));
      _rescueUnits = rescueRect.PrepareUnitsForRescue(RescuePreparationMode.HideAll);
      _tyrsHand = tyrsHand.Unit;
      _saiden = saiden;
      _scarletCrusade = scarletCrusade;
      Global = true;
    }

    /// <inheritdoc/>
    protected override string RewardFlavour =>
      "Lordaeron has been destroyed by the vile Scourge, leaving those left alive with naught but vengeance in their hearts. From the ashes rises the Scarlet Crusade, the untempered bright that will bring to the undying dead the wrath of the living.";

    /// <inheritdoc/>
    protected override string RewardDescription =>
      "Your forces turn hostile, then you restart the game as the Scarlet Crusade in Tyr's Hand";

    /// <inheritdoc/>
    protected override void OnComplete(Faction completingFaction)
    {
      var whichPlayer = completingFaction.Player;
      if (whichPlayer == null) 
        return;
      
      _scarletCrusade.CopyObjectLevelsFrom(completingFaction);
      AbandonCurrentUnits(whichPlayer);
      PlayerDistributor.DistributePlayer(whichPlayer);
      EvacuateTyrsHand(whichPlayer);
      whichPlayer.SetFaction(_scarletCrusade);
      GrantTyrsHand(whichPlayer);
      whichPlayer.RescueGroup(_rescueUnits);
      _tyrsHand.Rescue(whichPlayer);
      _saiden.ForceCreate(whichPlayer, Regions.Scarlet_Spawn.Center, 270);
      _saiden.Unit?.SetLevel(7, false);
    }

    private static void AbandonCurrentUnits(player whichPlayer)
    {
      foreach (var unit in CreateGroup()
                 .EnumUnitsOfPlayer(whichPlayer)
                 .EmptyToList())
      {
        if (!IsUnitType(unit, UNIT_TYPE_HERO))
          unit.Rescue(Player(PLAYER_NEUTRAL_AGGRESSIVE));
      }
    }

    private static void EvacuateTyrsHand(player newControlPointOwner)
    {
      foreach (var unit in CreateGroup().EnumUnitsInRect(Regions.TyrUnlock).EmptyToList()
                 .Where(x => x.OwningPlayer() == Player(PLAYER_NEUTRAL_AGGRESSIVE)))
      {
        if (!IsUnitType(unit, UNIT_TYPE_ANCIENT))
          unit.Kill();

        if (IsUnitType(unit, UNIT_TYPE_ANCIENT))
          unit.Rescue(newControlPointOwner);
      }
    }

    private static void GrantTyrsHand(player whichPlayer)
    {
      CreateStructureForced(whichPlayer, Constants.UNIT_H0BJ_IMPROVED_BOMBARD_TOWER_CRUSADE_TOWER, 19082, 8573,
        4.712389f * MathEx.DegToRad, 256);
      CreateStructureForced(whichPlayer, Constants.UNIT_H0BJ_IMPROVED_BOMBARD_TOWER_CRUSADE_TOWER, 18427, 7929,
        4.712389f * MathEx.DegToRad, 256);

      CreateStructureForced(whichPlayer, Constants.UNIT_H0BJ_IMPROVED_BOMBARD_TOWER_CRUSADE_TOWER, 20040, 8111,
        4.712389f * MathEx.DegToRad, 256);
      CreateStructureForced(whichPlayer, Constants.UNIT_H0BJ_IMPROVED_BOMBARD_TOWER_CRUSADE_TOWER, 20037, 7406,
        4.712389f * MathEx.DegToRad, 256);

      CreateStructureForced(whichPlayer, Constants.UNIT_H0BJ_IMPROVED_BOMBARD_TOWER_CRUSADE_TOWER, 21500, 7378,
        4.712389f * MathEx.DegToRad, 256);
      CreateStructureForced(whichPlayer, Constants.UNIT_H0BJ_IMPROVED_BOMBARD_TOWER_CRUSADE_TOWER, 20669, 8047,
        4.712389f * MathEx.DegToRad, 256);
      CreateStructureForced(whichPlayer, Constants.UNIT_H0BJ_IMPROVED_BOMBARD_TOWER_CRUSADE_TOWER, 19310, 7800,
        4.712389f * MathEx.DegToRad, 256);

      CreateStructureForced(whichPlayer, Constants.UNIT_H0AG_HALL_OF_SWORDS_CRUSADE_BARRACKS, 19484, 7205,
        4.712389f * MathEx.DegToRad, 256);
      CreateStructureForced(whichPlayer, Constants.UNIT_H0BE_STUDIUM_CRUSADE_MAGIC, 19435, 8580,
        4.712389f * MathEx.DegToRad, 256);
      CreateStructureForced(whichPlayer, Constants.UNIT_H0BL_ROOKERY_CRUSADE_BEAST, 21216, 8400,
        4.712389f * MathEx.DegToRad, 256);

      CreateStructureForced(whichPlayer, Constants.UNIT_N0D8_VENDOR_HALL_CRUSADE_SHOP, 20352, 8650,
        4.712389f * MathEx.DegToRad, 256);
      CreateStructureForced(whichPlayer, Constants.UNIT_H0A3_BLACKSMITH_CRUSADE_RESEARCH, 21218, 9000,
        4.712389f * MathEx.DegToRad, 256);
      CreateStructureForced(whichPlayer, Constants.UNIT_H0BQ_ALTAR_OF_CRUSADERS_CRUSADE_ALTAR, 21724, 9180,
        4.712389f * MathEx.DegToRad, 256);

      CreateStructureForced(whichPlayer, Constants.UNIT_H0BP_FARMSTEAD_CRUSADE_FARM, 19705, 8324,
        4.712389f * MathEx.DegToRad, 256);
      CreateStructureForced(whichPlayer, Constants.UNIT_H0BP_FARMSTEAD_CRUSADE_FARM, 20871, 7300,
        4.712389f * MathEx.DegToRad, 256);
      CreateStructureForced(whichPlayer, Constants.UNIT_H0BP_FARMSTEAD_CRUSADE_FARM, 20917, 8193,
        4.712389f * MathEx.DegToRad, 256);

      whichPlayer.AddGold(StartingGold);
      whichPlayer.AddLumber(StartingLumber);

      if (GetLocalPlayer() == whichPlayer)
        SetCameraPosition(20629, 10112);
    }
  }
}