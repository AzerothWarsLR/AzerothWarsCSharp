using MacroTools.Data;
using MacroTools.PassiveAbilities;
using MacroTools.PassiveAbilitySystem;
using MacroTools.Spells;
using MacroTools.SpellSystem;
using WarcraftLegacies.Source.Spells;
using WarcraftLegacies.Source.Spells.Slipstream;
using WCSharp.Shared.Data;

namespace WarcraftLegacies.Source.Setup.Spells
{
  public static class LegionSpellSetup
  {
    public static void Setup()
    {
      var darkPact = new DarkPact(ABILITY_A0A0_DARK_PACT_LEGION_ARCHIMONDE);
      SpellSystem.Register(darkPact);
      
      var inspireMadness = new InspireMadness(ABILITY_A10M_INSPIRE_MADNESS_TICHONDRIUS)
      {
        Radius = 300,
        CountBase = 2,
        CountLevel = 4,
        Duration = 30,
        EffectTarget = @"Abilities\Spells\Other\Charm\CharmTarget.mdl",
        EffectScaleTarget = 0.5f
      };
      SpellSystem.Register(inspireMadness);
      
      var summonBurningLegion = new SummonLegionSpell(ABILITY_A00J_SUMMON_THE_BURNING_LEGION_ALL_FACTIONS,
        ABILITY_A0KZ_SPELL_IMMUNITY_LEGION_SUMMON);
      SpellSystem.Register(summonBurningLegion);
      
      var massSummonUnit = new DelayedMultiTargetRecall( ABILITY_A02T_DARK_SUMMONING_DREADLORD)
      {
        Radius = 400,
        AmountToTarget = 12,
        MinDuration = 2,
        MaxDuration = 20,
        CrossDimensionalDuration = 15,
        DistanceDivider = 1000,
        DeathPenalty = 0.5f,
        TargetType = SpellTargetType.Point
      };
      SpellSystem.Register(massSummonUnit);

      RegisterSlipstreams();

      var summonFelHounds = new SummonUnits(ABILITY_A12B_HOUND_COMPANION_LEGION_FELGUARD)
      {
        SummonUnitTypeId = UNIT_NFEL_FEL_STALKER_SUMMONER_WARLOCK_EYE_OF_SARGERAS,
        SummonCount = 1,
        Duration = 60,
        Radius = 50,
      };
      SpellSystem.Register(summonFelHounds);

      var dreadlordHeroes = new[]
      {
        UNIT_UMAL_THE_CUNNING_LEGION,
        UNIT_UTIC_THE_DARKENER_LEGION,
        UNIT_U00L_ENVOY_OF_ARCHIMONDE_LEGION
      };

      PassiveAbilityManager.Register(new RestoreHealthFromEachTargetDamaged(dreadlordHeroes, ABILITY_A01N_VAMPIRIC_SIPHON_LEGION_HEROES)
      {
        HealthPerTarget = new LeveledAbilityField<int>
        {
          Base = 25,
          PerLevel = 25
        },
        Effect = @"Abilities\Spells\Undead\ReplenishMana\SpiritTouchTarget.mdl"
      });
    }

    private static void RegisterSlipstreams()
    {
      var slipstreamOrigin = new Point(22951.6f, -29964.4f);
      var slipstreamNorthrend = new SlipstreamSpellLegionTeleporter(ABILITY_A0UB_PORTAL_TO_NORTHREND_LEGION)
      {
        PortalUnitTypeId = UNIT_N0D9_SLIPSTREAM_PORTAL_STORMWIND_KHADGAR,
        OpeningDelay = 10,
        ClosingDelay = 5,
        OriginLocation = slipstreamOrigin,
        TargetLocation = new Point(3587, 20680),
        Color = new Color(55, 50, 250, 255),
        CloseAbilityId = ABILITY_ZBCP_CLOSE_PORTALS_LEGION_TELEPORTERS
      };
      SpellSystem.Register(slipstreamNorthrend);

      var slipstreamAlterac = new SlipstreamSpellLegionTeleporter(ABILITY_A0UC_PORTAL_TO_ALTERAC_LEGION)
      {
        PortalUnitTypeId = UNIT_N0D9_SLIPSTREAM_PORTAL_STORMWIND_KHADGAR,
        OpeningDelay = 10,
        ClosingDelay = 5,
        OriginLocation = slipstreamOrigin,
        TargetLocation = new Point(11000, 6424),
        Color = new Color(155, 250, 50, 255),
        CloseAbilityId = ABILITY_ZBCP_CLOSE_PORTALS_LEGION_TELEPORTERS
      };
      SpellSystem.Register(slipstreamAlterac);

      slipstreamNorthrend.RelatedSlipstreams.Add(slipstreamAlterac);
      slipstreamNorthrend.RelatedSlipstreams.Add(slipstreamNorthrend);
      
      slipstreamAlterac.RelatedSlipstreams.Add(slipstreamNorthrend);
      slipstreamAlterac.RelatedSlipstreams.Add(slipstreamAlterac);
    }
  }
}