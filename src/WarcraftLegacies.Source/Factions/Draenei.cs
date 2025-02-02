using System;
using System.Collections.Generic;
using System.Linq;
using MacroTools;
using MacroTools.Extensions;
using MacroTools.FactionChoices;
using MacroTools.FactionSystem;
using WarcraftLegacies.Shared.FactionObjectLimits;
using WarcraftLegacies.Source.Quests;
using WarcraftLegacies.Source.Quests.Draenei;
using WarcraftLegacies.Source.Setup;
using WCSharp.Shared.Data;

namespace WarcraftLegacies.Source.Factions
{
  public sealed class Draenei : Faction
  {
    private const int replacementUnitTypeId = UNIT_O05A_GEMCRAFTER_DRAENEI_WORKER;
    private readonly SharedGoldMineManager _sharedGoldMineManager;
    private readonly PreplacedUnitSystem _preplacedUnitSystem;
    private readonly AllLegendSetup _allLegendSetup;
    private readonly ArtifactSetup _artifactSetup;
    private bool _onNotPickedCalled = false;

    public Draenei(PreplacedUnitSystem preplacedUnitSystem, AllLegendSetup allLegendSetup, ArtifactSetup artifactSetup, SharedGoldMineManager sharedGoldMineManager) : base("The Exodar",
      PLAYER_COLOR_NAVY, "|cff000080", @"ReplaceableTextures\CommandButtons\BTNBOSSVelen.blp")
    {
      _sharedGoldMineManager = sharedGoldMineManager;
      TraditionalTeam = TeamSetup.NightElves;
      _preplacedUnitSystem = preplacedUnitSystem;
      _allLegendSetup = allLegendSetup;
      this._artifactSetup = artifactSetup;
      StartingGold = 200;
      ControlPointDefenderUnitTypeId = UNIT_U008_CONTROL_POINT_DEFENDER_DRAENEI;
      StartingCameraPosition = Regions.DraeneiStartPos.Center;
     // StartingUnits = Regions.SentStartPos.PrepareUnitsForRescue(RescuePreparationMode.Invulnerable);
      LearningDifficulty = FactionLearningDifficulty.Advanced;
      IntroText = @"You are playing as the exiled |cff000080Draenei|r.

    You begin on Azuremyst Island, amid the wreckage of your flight from the Burning Legion.

    Further inland your Night-elf allies will need your help against the Orcish Horde, quickly build your base and gain entry to the Exodar.

    The Exodar is a mighty fortress-base with the ability to move around the map, but it will take a long time to repair.";

      GoldMines = new List<unit>
            {
                preplacedUnitSystem.GetUnit(FourCC("ngol"), new Point(-21300, 8400))
            };

      foreach (var goldMine in GoldMines)
      {
        _sharedGoldMineManager.RegisterSharedGoldMine(goldMine, this);
      }
      Nicknames = new List<string>
            {
                "draenei",
                "dranei",
                "exo",
                "exodar",
                "theexodar",
                "goats"
            };
    }

    public override void OnRegistered()
    {
      ReplaceWorkersInRectangle(Regions.SentStartPos, replacementUnitTypeId);
      RegisterObjectLimits();
      RegisterQuests();
      SharedFactionConfigSetup.AddSharedFactionConfig(this);
    }

    
    public void ReplaceWorkersInRectangle(Rectangle rectangle, int replacementUnitTypeId)
    {
      Console.WriteLine($"Replacing workers in rectangle: {rectangle}");
      Func<unit, bool> condition = unit => IsUnitType(unit, UNIT_TYPE_PEON);
      var replacedUnits = rectangle.ReplaceWorkers(replacementUnitTypeId, condition);

      Console.WriteLine($"Number of units replaced: {replacedUnits.Count}");
      foreach (var unit in replacedUnits)
      {
        SetUnitOwner(unit, Player(18), true);
        Console.WriteLine("Replaced unit with ID " + GetUnitTypeId(unit));
      }
    }
    

    private void RegisterObjectLimits()
    {
      foreach (var (objectTypeId, objectLimit) in DraeneiObjectLimitData.GetAllObjectLimits())
        ModObjectLimit(FourCC(objectTypeId), objectLimit.Limit);
    }

    private void RegisterQuests()
    {
      var questRepairHull = new QuestRepairExodarHull(Regions.ExodarBaseUnlock, _allLegendSetup.Draenei.LegendExodar);
      StartingQuest = questRepairHull;
      AddQuest(questRepairHull);
      AddQuest(new QuestRebuildCivilisation(Regions.Darkshore));
      AddQuest(new QuestShipArgus(
          _preplacedUnitSystem.GetUnit(UNIT_H03V_ENTRANCE_PORTAL, Regions.OutlandToArgus.Center),
          _preplacedUnitSystem.GetUnit(UNIT_H03V_ENTRANCE_PORTAL, Regions.TempestKeepSpawn.Center),
          _allLegendSetup.Draenei.Velen
      ));
      var crystalProtectors = CreateGroup()
          .EnumUnitsInRect(Regions.ExodarBaseUnlock.Rect)
          .EmptyToList()
          .Where(x => GetUnitTypeId(x) == UNIT_U00U_CRYSTAL_PROTECTOR_DRAENEI_TOWER);
      var questRepairGenerator = new QuestRepairGenerator(_allLegendSetup.Draenei.LegendExodarGenerator, questRepairHull, crystalProtectors);
      AddQuest(questRepairGenerator);
      AddQuest(new QuestTriumvirate(_allLegendSetup.Draenei.Velen));
      var questDimensionalShip = new QuestDimensionalShip(Regions.ExodarBaseUnlock, questRepairGenerator, _allLegendSetup.Draenei.LegendExodarGenerator);
      AddQuest(questDimensionalShip);
      AddQuest(new QuestExtractSunwellVial(_allLegendSetup.Quelthalas.Sunwell, _artifactSetup.SunwellVial));
    }
  }
}