using System;
using System.Collections.Generic;
using System.Linq;
using MacroTools.ControlPointSystem;
using MacroTools.Extensions;
using MacroTools.FactionSystem;
using MacroTools.UserInterface;
using static War3Api.Common;

namespace MacroTools.FactionChoices
{
  /// <summary>Allows a player to choose between one of two factions at the start of the game.</summary>
  public sealed class FactionChoiceDialogPresenter : ChoiceDialogPresenter<Faction>
  {
    /// <summary>Initializes a new instance of the <see cref="FactionChoiceDialogPresenter"/> class.</summary>
    public FactionChoiceDialogPresenter(params Faction[] factions) : base(ConvertToFactionChoices(factions),
      "Pick your Faction")
    {
    }

    protected override void OnChoicePicked(player pickingPlayer, Choice<Faction> choice)
    {
      var pickedFaction = choice.Data;
      HasChoiceBeenPicked = true;
      Console.WriteLine($"Faction picked: {pickedFaction.Name}");

      if (pickedFaction.StartingCameraPosition != null)
      {
        Console.WriteLine("Repositioning camera to starting position.");
        pickingPlayer.RepositionCamera(pickedFaction.StartingCameraPosition);
      }

      Console.WriteLine("Setting faction for player.");
      pickingPlayer.SetFaction(pickedFaction);
      FactionManager.Register(pickedFaction);

      var startingUnits = pickedFaction.StartingUnits;
      Console.WriteLine($"Rescuing {startingUnits.Count} starting units.");
      pickingPlayer.RescueGroup(startingUnits);

      foreach (var unpickedFaction in Choices.Where(x => x.Data != choice.Data))
      {
        Console.WriteLine($"Removing unpicked faction: {unpickedFaction.Data.Name}");
        // RemoveFaction(unpickedFaction.Data);
      }
    }

    /// <inheritdoc />
    protected override Choice<Faction> GetDefaultChoice(player whichPlayer) => Choices.First();

     private static void RemoveFaction(Faction faction)
    {
      var startingUnits = faction.StartingUnits;
      Console.WriteLine($"Removing faction: {faction.Name}. Number of starting units: {startingUnits.Count}");

      foreach (var unit in startingUnits)
      {
        if (ControlPointManager.Instance.UnitIsControlPoint(unit))
        {
          Console.WriteLine($"Unit {unit} is a control point. Rescuing to neutral aggressive.");
          unit.Rescue(Player(PLAYER_NEUTRAL_AGGRESSIVE));
        }
        else
        {
          Console.WriteLine($"Removing unit: {unit}");
         // unit.Remove();
        }
      }

      Console.WriteLine($"Calling OnNotPicked for faction: {faction.Name}");
      // faction.OnNotPicked();
    }

    private static Choice<Faction>[] ConvertToFactionChoices(IEnumerable<Faction> factions)
    {
      return factions
        .Select(x => new Choice<Faction>(x, $"{x.Name} {x.LearningDifficulty.ToColoredText()}"))
        .ToArray();
    }
  }
}
