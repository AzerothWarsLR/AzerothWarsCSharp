﻿using System.Linq;
using MacroTools.ChannelSystem;
using MacroTools.Extensions;
using MacroTools.SpellSystem;
using MacroTools.Utils;
using WCSharp.Buffs;
using WCSharp.Effects;
using WCSharp.Shared.Data;


namespace WarcraftLegacies.Source.Spells.ExactJustice
{
  /// <summary>
  /// Provides the channeling effect for <see cref="ExactJusticeSpell"/>.
  /// </summary>
  public sealed class ExactJusticeChannel : Channel
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ExactJusticeChannel"/> class.
    /// </summary>
    /// <param name="caster"><inheritdoc /></param>
    /// <param name="spellId"><inheritdoc /></param>
    public ExactJusticeChannel(unit caster, int spellId) : base(caster, spellId)
    {
    }

    /// <summary>
    /// Settings for the visual effects created by the channel effect.
    /// </summary>
    public ExactJusticeEffectSettings EffectSettings { get; init; } = new();
    
    /// <summary>
    /// The maximum amount of damage that will be dealt at the end of the channel.
    /// If the channel ends early, less damage is dealt.
    /// </summary>
    public float MaximumDamage { get; init; }
    
    /// <summary>
    /// The radius in which units are affected.
    /// </summary>
    public float Radius { get; init; }

    private float _damage;
    private float _maximumDuration;
    private float _ringAlpha;
    private effect? _ringEffect;
    private effect? _sparkleEffect;
    private effect? _progressEffect;
    private ExactJusticeAura? _aura;

    /// <inheritdoc />
    public override void OnCreate()
    {
      var x = GetUnitX(Caster);
      var y = GetUnitY(Caster);
      
      _maximumDuration = Duration;
      _ringEffect = AddSpecialEffect(EffectSettings.RingPath, x, y);
      _ringEffect.SetAlpha(0);
      _ringEffect.SetTimeScale(0);
      _ringEffect.SetColor(235, 235, 50);
      _ringEffect.SetTimeScale(EffectSettings.RingScale);

      _sparkleEffect = AddSpecialEffect(EffectSettings.SparklePath, x, y);
      _sparkleEffect.SetTimeScale(EffectSettings.SparkleScale);
      _sparkleEffect.SetColor(255, 255, 0);

      _progressEffect = AddSpecialEffect(EffectSettings.ProgressBarPath, x, y);
      _progressEffect.SetTimeScale(1 / Duration);
      _progressEffect.SetColor(Player(4));
      _progressEffect.SetTimeScale(EffectSettings.ProgressBarScale);
      _progressEffect.SetHeight(EffectSettings.ProgressBarHeight);

      _aura = new ExactJusticeAura(Caster)
      {
        Radius = Radius,
        Active = true,
        Duration = 1.1f,
        SearchInterval = 1
      };
      AuraSystem.Add(_aura);
    }

    /// <inheritdoc />
    protected override void OnPeriodic()
    {
      if (_damage < MaximumDamage) 
        _damage += MaximumDamage / (_maximumDuration / Interval);
      if (!(_ringAlpha < EffectSettings.AlphaRing))
        return;
      _ringAlpha += EffectSettings.AlphaRing / (_maximumDuration / Interval);
      _ringEffect?.SetAlpha(R2I(_ringAlpha));
    }

    /// <inheritdoc />
    protected override void OnDispose()
    {
      var explodeEffect = AddSpecialEffect(EffectSettings.ExplodePath, GetUnitX(Caster), GetUnitY(Caster));
      explodeEffect.SetTimeScale(EffectSettings.ExplodeScale);
      EffectSystem.Add(explodeEffect);
      
      foreach (var unit in GroupUtils.GetUnitsInRange(Caster.GetPosition(), Radius)
                 .Where(target => CastFilters.IsTargetEnemyAndAlive(Caster, target)))
      {
        unit.TakeDamage(Caster, _damage, false, false, damageType: DAMAGE_TYPE_MAGIC);
      }
      
      //The below effects have no death animations so they have//to be moved off the map as they are destroyed.
      var dummyRemovalPoint = new Point(-100000, -100000);
      effect tempQualifier2 = _sparkleEffect;
      if (tempQualifier2 != null)
      {
        tempQualifier2.SetPosition(dummyRemovalPoint.X, dummyRemovalPoint.Y, 0);
      }

      effect tempQualifier = _sparkleEffect;
      if (tempQualifier != null)
      {
        tempQualifier.Dispose();
      }

      effect tempQualifier3 = _progressEffect;
      if (tempQualifier3 != null)
      {
        tempQualifier3.SetPosition(dummyRemovalPoint.X, dummyRemovalPoint.Y, 0);
      }

      effect tempQualifier1 = _progressEffect;
      if (tempQualifier1 != null)
      {
        tempQualifier1.Dispose();
      }

      _ringEffect?.SetTimeScale(1);
      _ringEffect?.Dispose();
      if (_aura != null) 
        _aura.Active = false;
    }
  }
}