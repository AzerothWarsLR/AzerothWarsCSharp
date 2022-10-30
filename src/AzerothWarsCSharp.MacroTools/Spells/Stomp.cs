﻿using AzerothWarsCSharp.MacroTools.SpellSystem;
using AzerothWarsCSharp.MacroTools.Wrappers;
using WCSharp.Shared.Data;
using static War3Api.Common;

namespace AzerothWarsCSharp.MacroTools.Spells
{
  /// <summary>
  /// Deals damage and casts a dummy spell at each unit in the area.
  /// </summary>
  public sealed class Stomp : Spell
  {
    /// <summary>
    /// How much base damage to deal.
    /// </summary>
    public float DamageBase { get; init; } = 20;
    
    /// <summary>
    /// How much additional damage to deal per level.
    /// </summary>
    public float DamageLevel { get; init; } = 30;
    
    /// <summary>
    /// How long the stun should last.
    /// </summary>
    public int DurationBase { get; init; }
    
    /// <summary>
    /// How much extra duration the stun should get per level.
    /// </summary>
    public int DurationLevel { get; init; }
    
    /// <summary>
    /// The radius in which units are affected.
    /// </summary>
    public float Radius { get; init; } = 300;
    
    /// <summary>
    /// The ID of an ability that stuns units. Should be based on Storm Bolt.
    /// </summary>
    public int StunAbilityId { get; init; }

    /// <summary>
    /// The order string for <see cref="StunAbilityId"/>.
    /// </summary>
    public string StunOrderString { get; init; } = "";
    
    private void DamageUnit(unit caster, widget target)
    {
      UnitDamageTarget(caster, target, DamageBase + DamageLevel * GetAbilityLevel(caster), false, false, ATTACK_TYPE_NORMAL,
        DAMAGE_TYPE_MAGIC, WEAPON_TYPE_WHOKNOWS);
    }

    private void StunUnit(unit caster, unit target)
    {
      var duration = DurationBase + DurationLevel * GetAbilityLevel(caster);
      if (StunAbilityId == 0 || StunOrderString == "" || duration <= 0)
      {
        return;
      }
      DummyCast.CastOnUnit(caster, StunAbilityId, StunOrderString, duration, target);
    }

    /// <inheritdoc />
    public override void OnCast(unit caster, unit target, Point targetPoint)
    {
      var tempGroup = new GroupWrapper();
      tempGroup.EnumUnitsInRange(new Point(GetUnitX(caster), GetUnitY(caster)), Radius);
      foreach (var enumUnit in tempGroup.EmptyToList())
      {
        if (CastFilters.IsTargetEnemyAndAlive(caster, enumUnit))
        {
          DamageUnit(caster, enumUnit);
          StunUnit(caster, enumUnit);
        }
      }
      DestroyEffect(AddSpecialEffect(@"Abilities\\Spells\\Orc\\Warstomp\\WarStompCaster.mdl", GetUnitX(caster),
        GetUnitY(caster)));
    }

    /// <inheritdoc />
    public Stomp(int id) : base(id)
    {
      
    }
  }
}