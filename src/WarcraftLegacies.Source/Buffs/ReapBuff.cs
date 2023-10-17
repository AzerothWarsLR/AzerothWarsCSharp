﻿using MacroTools.Extensions;
using WCSharp.Buffs;
using static War3Api.Common;

namespace WarcraftLegacies.Source.Buffs
{
  /// <summary>Gives the caster additional Strength.</summary>
  public sealed class ReapBuff : BoundBuff
  {
    /// <summary>The buff holder gains this much Strength.</summary>
    public int StrengthGain { get; init; }
    
    /// <inheritdoc />
    public ReapBuff(unit caster, string effectPath) : base(caster, caster)
    {
      EffectString = effectPath;
      EffectAttachmentPoint = "origin";
      EffectScale = 2;
      Bind(Constants.ABILITY_ZB03_REAP_BUFF_APPLICATOR, Constants.BUFF_ZB04_REAP);
    }
    
    public override void OnApply()
    {
      Caster.AddHeroAttributes(StrengthGain, 0, 0);
      base.OnApply();
    }

    public override void OnDispose()
    {
      Caster.AddHeroAttributes(-StrengthGain, 0, 0);
      base.OnDispose();
    }
  }
}