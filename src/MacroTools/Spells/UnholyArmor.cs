﻿using WarcraftLegacies.MacroTools.Extensions;
using WarcraftLegacies.MacroTools.SpellSystem;
using WCSharp.Shared.Data;
using static War3Api.Common;

namespace WarcraftLegacies.MacroTools.Spells
{
  public sealed class UnholyArmor : Spell
  {
    public float PercentageDamage { get; init; }
    
    public UnholyArmor(int id) : base(id)
    {
    }

    public override void OnCast(unit caster, unit target, Point targetPoint)
    {
      target.Damage(caster, GetUnitState(target, UNIT_STATE_LIFE) * PercentageDamage);
    }
  }
}