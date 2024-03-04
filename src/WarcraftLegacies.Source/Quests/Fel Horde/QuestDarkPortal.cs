﻿using MacroTools.Extensions;
using MacroTools.FactionSystem;
using MacroTools.ObjectiveSystem.Objectives.MetaBased;
using MacroTools.ObjectiveSystem.Objectives.TimeBased;
using MacroTools.ObjectiveSystem.Objectives.UnitBased;
using MacroTools.QuestSystem;
using WCSharp.Shared.Data;


namespace WarcraftLegacies.Source.Quests.Fel_Horde
{
  public sealed class QuestDarkPortal : QuestData
  {
    private readonly Faction _stormwind;
    private readonly Faction _illidari;
    private readonly unit _innerWaygate1;
    private readonly unit _innerWaygate2;
    private readonly unit _innerWaygate3;
    private readonly unit _outerWaygate1;
    private readonly unit _outerWaygate2;
    private readonly unit _outerWaygate3;

    /// <summary>
    /// Initializes a new instance of the class <see cref="QuestDarkPortal"/>.
    /// </summary>
    /// <param name="innerWaygate1">A Waygate inside outland, next to the Dark Portal.</param>
    /// <param name="innerWaygate2">A Waygate inside outland, next to the Dark Portal.</param>
    /// <param name="innerWaygate3">A Waygate inside outland, next to the Dark Portal.</param>
    /// <param name="outerWaygate1">A Waygate outside outland, next to the Dark Portal.</param>
    /// <param name="outerWaygate2">A Waygate outside outland, next to the Dark Portal.</param>
    /// <param name="outerWaygate3">A Waygate outside outland, next to the Dark Portal.</param>
    /// <param name="stormwind">Affects the reward flavour.</param>
    /// <param name="illidari">Affects the reward flavour.</param>
    public QuestDarkPortal(unit innerWaygate1, unit innerWaygate2, unit innerWaygate3, unit outerWaygate1,
      unit outerWaygate2, unit outerWaygate3, Faction stormwind, Faction illidari)
      : base("The Dark Portal",
        "Following the Second War, the archmage Khadgar and his fellow magi sealed the Dark Portal so that it would never again be used to threaten Azeroth. Little did they know that their magicks were only temporary, and that the portal would open again in time.",
        @"ReplaceableTextures\CommandButtons\BTNDarkPortal.blp")
    {
      _stormwind = stormwind;
      _illidari = illidari;
      _innerWaygate1 = innerWaygate1.IsVisible = false;
      _innerWaygate2 = innerWaygate2.IsVisible = false;
      _innerWaygate3 = innerWaygate3.IsVisible = false;
      _outerWaygate1 = outerWaygate1.IsVisible = false;
      _outerWaygate2 = outerWaygate2.IsVisible = false;
      _outerWaygate3 = outerWaygate3.IsVisible = false;
      AddObjective(new ObjectiveEitherOf(
        new ObjectiveResearch(Constants.UPGRADE_R02C_THE_DARK_PORTAL_FEL_HORDE, Constants.UNIT_O008_HELLFIRE_CITADEL_FEL_HORDE, true),
        new ObjectiveTime(600)));
      AddObjective(new ObjectiveTime(480));
      Global = true;
    }

    /// <inheritdoc />
    public override string RewardFlavour
    {
      get
      {
        var flavour = "The Dark Portal, previously thought to have been sealed forever, has been opened once more.";
        if (_stormwind.ScoreStatus != ScoreStatus.Defeated)
          flavour +=
            " The people of Stormwind are about to relive their worst nightmares, as the demonic Fel Horde spills forth from Outland to resume their slaughterous rampage.";
        else if (_illidari.ScoreStatus != ScoreStatus.Defeated)
          flavour +=
            " Illidan's forces brace themselves, ready to visit destruction upon Azeroth in the name of their new master.";
        else
          flavour +=
            " Intrepid explorers may now move freely between the otherwise distant worlds of Azeroth and Outland.";
        return flavour;
      }
    }

    /// <inheritdoc />
    protected override string RewardDescription =>
      "The Dark Portal can be used to teleport units between the Eastern Kingdoms and Outland";

    /// <inheritdoc />
    protected override void OnComplete(Faction completingFaction) => OpenPortal();

    /// <inheritdoc />
    protected override void OnAdd(Faction whichFaction) =>
      whichFaction.ModObjectLimit(Constants.UPGRADE_R02C_THE_DARK_PORTAL_FEL_HORDE, Faction.UNLIMITED);

    private void OpenPortal()
    {
      _innerWaygate1.IsVisible = true;
      _innerWaygate1.WaygateActive = true;
      _innerWaygate1.WaygateDestinationX = Regions.Dark_Portal_Exit_1.Center.X;
      _innerWaygate1.WaygateDestinationY = Regions.Dark_Portal_Exit_1.Center.Y;
      _innerWaygate2.IsVisible = true;
      _innerWaygate2.WaygateActive = true;
      _innerWaygate2.WaygateDestinationX = Regions.Dark_Portal_Exit_2.Center.X;
      _innerWaygate2.WaygateDestinationY = Regions.Dark_Portal_Exit_2.Center.Y;
      _innerWaygate3.IsVisible = true;
      _innerWaygate3.WaygateActive = true;
      _innerWaygate3.WaygateDestinationX = Regions.Dark_Portal_Exit_3.Center.X;
      _innerWaygate3.WaygateDestinationY = Regions.Dark_Portal_Exit_3.Center.Y;
      _outerWaygate1.IsVisible = true;
      _outerWaygate1.WaygateActive = true;
      _outerWaygate1.WaygateDestinationX = Regions.Dark_Portal_Entrance_1.Center.X;
      _outerWaygate1.WaygateDestinationY = Regions.Dark_Portal_Entrance_1.Center.Y;
      _outerWaygate2.IsVisible = true;
      _outerWaygate2.WaygateActive = true;
      _outerWaygate2.WaygateDestinationX = Regions.Dark_Portal_Entrance_2.Center.X;
      _outerWaygate2.WaygateDestinationY = Regions.Dark_Portal_Entrance_2.Center.Y;
      _outerWaygate3.IsVisible = true;
      _outerWaygate3.WaygateActive = true;
      _outerWaygate3.WaygateDestinationX = Regions.Dark_Portal_Entrance_3.Center.X;
      _outerWaygate3.WaygateDestinationY = Regions.Dark_Portal_Entrance_3.Center.Y;
    }
  }
}