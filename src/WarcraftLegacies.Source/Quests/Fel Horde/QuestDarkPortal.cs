﻿using MacroTools.Extensions;
using MacroTools.FactionSystem;
using MacroTools.ObjectiveSystem.Objectives.FactionBased;
using MacroTools.ObjectiveSystem.Objectives.TimeBased;
using MacroTools.QuestSystem;
using WarcraftLegacies.Source.Setup.FactionSetup;
using static War3Api.Common;

namespace WarcraftLegacies.Source.Quests.Fel_Horde
{
  public sealed class QuestDarkPortal : QuestData
  {
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
    public QuestDarkPortal(unit innerWaygate1, unit innerWaygate2, unit innerWaygate3, unit outerWaygate1,
      unit outerWaygate2, unit outerWaygate3)
      : base("The Dark Portal",
        "Following the Second War, the archmage Khadgar and his fellow magi sealed the Dark Portal so that it would never again be used to threaten Azeroth. Little did they know that their magicks were only temporary, and that the portal would open again in time.",
        "ReplaceableTextures\\CommandButtons\\BTNDarkPortal.blp")
    {
      _innerWaygate1 = innerWaygate1.Show(false);
      _innerWaygate2 = innerWaygate2.Show(false);
      _innerWaygate3 = innerWaygate3.Show(false);
      _outerWaygate1 = outerWaygate1.Show(false);
      _outerWaygate2 = outerWaygate2.Show(false);
      _outerWaygate3 = outerWaygate3.Show(false);
      AddObjective(new ObjectiveTime(420));
      Global = true;
    }

    /// <inheritdoc />
    protected override string RewardFlavour
    {
      get
      {
        var flavour = "The Dark Portal, previously thought to have been sealed forever, has been opened once more.";
        if (StormwindSetup.Stormwind?.ScoreStatus != ScoreStatus.Defeated)
          flavour +=
            " The people of Stormwind are about to relive their worst nightmares, as the demonic Fel Horde spills forth from Outland to resume their slaughterous rampage.";
        else if (IllidariSetup.Illidari?.ScoreStatus != ScoreStatus.Defeated)
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

    private void OpenPortal()
    {
      _innerWaygate1
        .Show(true)
        .SetWaygateDestination(Regions.Dark_Portal_Exit_1.Center);
      _innerWaygate2
        .Show(true)
        .SetWaygateDestination(Regions.Dark_Portal_Exit_2.Center);
      _innerWaygate3
        .Show(true)
        .SetWaygateDestination(Regions.Dark_Portal_Exit_3.Center);
      _outerWaygate1
        .Show(true)
        .SetWaygateDestination(Regions.Dark_Portal_Entrance_1.Center);
      _outerWaygate2
        .Show(true)
        .SetWaygateDestination(Regions.Dark_Portal_Entrance_2.Center);
      _outerWaygate3
        .Show(true)
        .SetWaygateDestination(Regions.Dark_Portal_Entrance_3.Center);
    }
  }
}