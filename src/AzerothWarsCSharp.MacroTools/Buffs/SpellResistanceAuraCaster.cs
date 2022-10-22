﻿using WCSharp.Buffs;

namespace AzerothWarsCSharp.MacroTools.Buffs
{
  public sealed class SpellResistanceAuraCaster : Aura<SpellResistanceAuraTarget>
  {
    public SpellResistanceAuraCaster(unit caster) : base(caster)
    {
      StackBehaviour = StackBehaviour.Stack;
      EffectString = @"Abilities\Spells\Human\DevotionAura\DevotionAura.mdl";
      Radius = 800;
    }

    protected override SpellResistanceAuraTarget CreateAuraBuff(unit unit)
    {
      return new SpellResistanceAuraTarget(Caster, unit);
    }

    protected override bool UnitFilter(unit unit)
    {
      return IsUnitAlly(unit, CastingPlayer);
    }
  }
}