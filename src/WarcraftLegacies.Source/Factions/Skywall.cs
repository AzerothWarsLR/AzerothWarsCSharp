using System.Collections.Generic;
using MacroTools.FactionSystem;
using MacroTools.PassiveAbilities;
using MacroTools.PassiveAbilitySystem;
using MacroTools.Powers;
using MacroTools.ResearchSystems;
using MacroTools.Spells;
using MacroTools.SpellSystem;
using WarcraftLegacies.Shared.FactionObjectLimits;
using WarcraftLegacies.Source.Quests.Skywall;
using WarcraftLegacies.Source.Researches;
using WarcraftLegacies.Source.Setup;
using WCSharp.Shared.Data;

namespace WarcraftLegacies.Source.Factions
{
  public sealed class Skywall : Faction
  {
    /// <inheritdoc />
    public Skywall() : base("Skywall", PLAYER_COLOR_LIGHT_GRAY, "|cffffffff",
      @"ReplaceableTextures\CommandButtons\BTNFrostRevenant2.blp")
    {
      ControlPointDefenderUnitTypeId = UNIT_NECP_CONTROL_POINT_DEFENDER_SKYWALL_TOWER;
      TraditionalTeam = TeamSetup.OldGods;
      StartingGold = 200;
      IntroText = @"You are playing as the Elementals of Skywall|r|r.

At the start, clear Uldum and take control of Tanaris. 

Coordinate with your Qiraji ally to push the Horde before the Druids can get there.

You have a very powerful event in the Burning of the World Tree. Use it at the right time to surprise the Druids and maybe attack them from behind.";
      Nicknames = new List<string>
      {
        "sky",
        "skywall",
        "ele",
        "eles",
        "elemental",
        "elementals",
        "rag",
        "ragnaros"
      };
      ProcessObjectInfo(SkywallObjectInfo.GetAllObjectLimits());
    }

    /// <inheritdoc />
    public override void OnRegistered()
    {
      RegisterResearches();
      RegisterSpells();
      RegisterQuests();
      RegisterFactionDependentInitializer<Druids, Ahnqiraj>(RegisterInvasionRelatedQuests);
      SharedFactionConfigSetup.AddSharedFactionConfig(this);
    }

    private void RegisterQuests()
    {
      var newQuest = AddQuest(new QuestVortexPinnacle(Regions.Tempest_Rain));
      StartingQuest = newQuest;
      AddQuest(new QuestEmissary());
      AddQuest(new QuestThroneWind(Regions.ThroneoftheFourWind));
      AddQuest(new QuestSubduing());

    }

    private void RegisterInvasionRelatedQuests(Druids druids, Ahnqiraj ahnqiraj)
    {
      var invasionParameters = new InvasionParameters();
      invasionParameters.InvasionRects = new List<Rectangle>
      {
        Regions.Invasion1,
        Regions.Invasion2,
        Regions.Invasion3,
        Regions.Invasion4,
        Regions.Invasion5,
        Regions.Invasion6,
      };
      invasionParameters.InvasionArmySummonParameters = new List<InvasionArmySummonParameter>
      {
        new(1, UNIT_LS05_SHAPER_ELEMENTAL_WORKER),
        new(2, UNIT_N0CG_CORE_HOUND_RAGNAROS),
        new(2, UNIT_VSW0_FIRE_ELEMENTAL_ELEMENTALS),
      };
      invasionParameters.AttackTargets = new List<Point>
      {
        new Point(-9788, 11040),

      };

      AddQuest(new QuestFirelandInvasion(invasionParameters, druids, ahnqiraj, Regions.SulfuronSpire));
    }

    private void RegisterResearches()
    {
      ResearchManager.Register(new PowerResearch(UPGRADE_RELT_WINDFORGING_SKYWALL, 100,
        new Windforging(UNIT_O01I_ANIMATED_ARMOR_ELEMENTAL, 0.25f, new Point( - 10396.5f, -20963.6f), "the Vortex Pinnacle", Regions.ElementalRealm)
        {
          IconName = "ItemForging",
          Name = "Windforging",
        }));
    }

    private void RegisterSpells()
    {

      var purgeAttack = new SpellOnAttack(UNIT_O01I_ANIMATED_ARMOR_ELEMENTAL,
        ABILITY_AELP_SHOCKING_BLADES_ANIMATED_ARMOR)
      {
        DummyAbilityId = ABILITY_AEPU_PURGE_SHOCKING_BLADE,
        DummyOrderId = OrderId("purge"),
        ProcChance = 0.20f,
        RequiredResearch = UPGRADE_RELP_SHOCKING_BLADES_SKYWALL
      };
      PassiveAbilityManager.Register(purgeAttack);

      var stormSurge = new Stomp(ABILITY_AESS_STORM_SURGE_ARMORED_MISTRAL)
      {
        Radius = 200,
        DamageBase = 50,
        DurationBase = 3,
        StunAbilityId = ABILITY_AEPU_PURGE_SHOCKING_BLADE,
        StunOrderId = OrderId("purge"),
        SpecialEffect = @"war3mapImported\Cyclon Explosion.mdx"
      };
      SpellSystem.Register(stormSurge);

      var massEnsnare = new MassAnySpell(ABILITY_A01N_MASS_ENSNARE_SKYWALL)
      {
        DummyAbilityId = ABILITY_A01V_MASS_ENSNARE_SKYWALL_DUMMY,
        DummyAbilityOrderId = OrderId("ensnare"),
        Radius = 150,
        Chance = 0.5f,
        CastFilter = CastFilters.IsTargetEnemyAndAlive,
        TargetType = SpellTargetType.Point
      };
      SpellSystem.Register(massEnsnare);

    }
  }
}