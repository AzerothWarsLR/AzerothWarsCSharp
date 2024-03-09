﻿using MacroTools.Extensions;
using MacroTools.PassiveAbilitySystem;
using WCSharp.Effects;


namespace MacroTools.PassiveAbilities
{
  /// <summary>
  /// Causes the unit to summon a unit upon death.
  /// </summary>
  public sealed class SummonUnitOnDeath : PassiveAbility
  {
    /// <summary>
    /// How long the summoned unit should last.
    /// </summary>
    public float Duration { get; init; }
    
    /// <summary>
    /// The unit type to summon on death.
    /// </summary>
    public int SummonUnitTypeId { get; init; }

    /// <summary>
    /// How many units to summon.
    /// </summary>
    public int SummonCount { get; init; } = 1;

    /// <summary>
    /// The special effect that appears when the ability triggers.
    /// </summary>
    public string SpecialEffectPath { get; init; } = "";
    
    /// <summary>
    /// The player must have this research for the ability to take effect.
    /// </summary>
    public int RequiredResearch { get; init; }
    
    /// <inheritdoc />
    public SummonUnitOnDeath(int unitTypeId) : base(unitTypeId)
    {
    }
    
    /// <inheritdoc />
    public override void OnDeath()
    {
      var triggerUnit = GetTriggerUnit();
      if (GetPlayerTechCount(triggerUnit.Owner, RequiredResearch, false) == 0 || IsUnitType(triggerUnit, UNIT_TYPE_SUMMONED))
        return;
      
      var pos = triggerUnit.GetPosition();
      for (var i = 0; i < SummonCount; i++)
      {
        var summonedUnit = CreateUnit(triggerUnit.Owner, SummonUnitTypeId, pos.X, pos.Y, triggerUnit.Facing);
        summonedUnit.AddType(UNIT_TYPE_SUMMONED);
        summonedUnit.ApplyTimedLife(0, Duration);
      }

      EffectSystem.Add(AddSpecialEffect(SpecialEffectPath, pos.X, pos.Y), 1);
      triggerUnit.Dispose();
    }
  }
}