﻿using System.Linq;
using CommonBehaviors.Actions;
using Singular.Dynamics;
using Singular.Helpers;
using Singular.Managers;
using Singular.Settings;
using Styx;

using Styx.Pathing;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.CommonBot;
using System;

namespace Singular.ClassSpecific.DeathKnight
{
    public class Unholy
    {
        private const int SuddenDoom = 81340;
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static DeathKnightSettings DeathKnightSettings { get { return SingularSettings.Instance.DeathKnight(); } }

        #region Normal Rotation

        [Behavior(BehaviorType.Combat, WoWClass.DeathKnight, WoWSpec.DeathKnightUnholy, WoWContext.Normal)]
        public static Composite CreateDeathKnightUnholyNormalCombat()
        {
            return new PrioritySelector(

                Helpers.Common.EnsureReadyToAttackFromMelee(),
                Spell.WaitForCastOrChannel(),


                new Decorator(
                    req => !Spell.IsGlobalCooldown(),
                    new PrioritySelector(
                        Helpers.Common.CreateAutoAttack(true),

                        Helpers.Common.CreateInterruptBehavior(),

                        Common.CreateDeathKnightPullMore(),

                        Common.CreateGetOverHereBehavior(),

                        Common.CreateDarkSuccorBehavior(),

                        Common.CreateSoulReaperHasteBuffBehavior(),

                        Common.CreateDarkSimulacrumBehavior(),

                        // *** Cool downs ***
                        Spell.BuffSelf("Unholy Frenzy",
                            req => Me.CurrentTarget.IsWithinMeleeRange 
                                && !PartyBuff.WeHaveBloodlust
                                && Helpers.Common.UseLongCoolDownAbility),

                        Spell.Cast("Summon Gargoyle", req => DeathKnightSettings.UseSummonGargoyle && Helpers.Common.UseLongCoolDownAbility),

                        // aoe
                        new Decorator(
                            req => Spell.UseAOE && Unit.UnfriendlyUnitsNearTarget(12f).Count() >= DeathKnightSettings.DeathAndDecayCount,
                            new PrioritySelector(
                                // Spell.Cast("Gorefiend's Grasp"),
                                Spell.Cast("Remorseless Winter"),
                                CreateUnholyAoeBehavior(),
                                Movement.CreateMoveToMeleeBehavior(true)
                                )
                            ),

                        // Single target rotation.

                        // Target < 35%, Soul Reaper
                        Spell.Cast("Soul Reaper", req => StyxWoW.Me.CurrentTarget.HealthPercent < 35),

                        // Diseases
                        Common.CreateApplyDiseases(),

                        // Scourge Strike/Death and Decay*(Unholy/Death Runes are Capped)
                        new Decorator(
                            req => Common.UnholyRuneSlotsActive >= 2,
                            new PrioritySelector(
                                Spell.CastOnGround("Death and Decay",
                                    on => StyxWoW.Me.CurrentTarget,
                                    req => Spell.UseAOE,
                                    false),
                                Spell.Cast("Scourge Strike")
                                )
                            ),

                        // Dark Transformation
                        Spell.Cast("Dark Transformation",
                            req => StyxWoW.Me.GotAlivePet
                                && !StyxWoW.Me.Pet.ActiveAuras.ContainsKey("Dark Transformation")
                                && StyxWoW.Me.HasAura("Shadow Infusion", 5)),
                        
                        // Death Coil (Sudden Doom, high RP)
                        Spell.Cast("Death Coil", req => Me.HasAura(SuddenDoom) || Me.CurrentRunicPower >= 80),                        

                        // Festering Strike (BB and FF are up)
                        Spell.Cast("Festering Strike", req => StyxWoW.Me.BloodRuneCount == 2 && StyxWoW.Me.FrostRuneCount == 2),
                        
                        // Scourge Strike
                        Spell.Cast("Scourge Strike"),
                        
                        // Festering Strike
                        Spell.Cast("Festering Strike"),

                        // post Single target
                        // attack at range if possible
                        Spell.Cast("Death Coil", req => Me.GotTarget && !Me.CurrentTarget.IsWithinMeleeRange ),

                        // attack with other abilities if we don't know scourge strike yet
                        new Decorator(
                            req => !SpellManager.HasSpell("Scourge Strike"),
                            new PrioritySelector(
                                Spell.Buff("Icy Touch", true, on => Me.CurrentTarget, req => true, "Frost Fever"),
                                Spell.Buff("Plague Strike", true, on => Me.CurrentTarget, req => true, "Blood Plague"),
                                Spell.Cast("Death Strike", req => Me.HealthPercent < 90),
                                Spell.Cast("Death Coil"),
                                Spell.Cast("Icy Touch"),
                                Spell.Cast("Plague Strike")
                                )
                            )
                        )
                    ),

                Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        private static Composite CreateUnholyAoeBehavior()
        {
            return new PrioritySelector(
                // Spell.Cast("Gorefiend's Grasp", on => Me, req => Unit.NearbyUnfriendlyUnits.Count( u => u.Distance.Between(10,20) && u.IsTargetingMeOrPet ),
                Spell.Cast("Remorseless Winter"),

            // Diseases
                Common.CreateApplyDiseases(),

                Spell.Cast("Dark Transformation",
                    req => StyxWoW.Me.GotAlivePet
                        && !StyxWoW.Me.Pet.ActiveAuras.ContainsKey("Dark Transformation")
                        && StyxWoW.Me.HasAura("Shadow Infusion", 5)),

            // spread the disease around.
                new Throttle( TimeSpan.FromSeconds(1.5f),
                    Spell.Cast("Blood Boil",
                        req => StyxWoW.Me.CurrentTarget.DistanceSqr <= 10 * 10
                            && !StyxWoW.Me.HasAura("Unholy Blight") && Common.ShouldSpreadDiseases
                        )
                    ),

                Spell.CastOnGround(
                    "Death and Decay",
                    on => StyxWoW.Me.CurrentTarget,
                    req => Common.UnholyRuneSlotsActive >= 2,
                    false
                    ),

                Spell.Cast("Blood Boil",
                    req => StyxWoW.Me.CurrentTarget.DistanceSqr <= 10 * 10 && StyxWoW.Me.DeathRuneCount > 0 
                        || (StyxWoW.Me.BloodRuneCount == 2 && StyxWoW.Me.FrostRuneCount == 2)
                        ),

                Spell.Cast("Soul Reaper", req => StyxWoW.Me.CurrentTarget.HealthPercent < 35),

                Spell.Cast("Scourge Strike", req => StyxWoW.Me.UnholyRuneCount == 2),

                Spell.Cast("Death Coil",
                    req => StyxWoW.Me.HasAura(SuddenDoom) 
                        || StyxWoW.Me.RunicPowerPercent >= 80 
                        || !StyxWoW.Me.GotAlivePet 
                        || !StyxWoW.Me.Pet.ActiveAuras.ContainsKey("Dark Transformation"))
                );
        }

        #endregion

        #region Battleground Rotation

        [Behavior(BehaviorType.Combat, WoWClass.DeathKnight, WoWSpec.DeathKnightUnholy, WoWContext.Battlegrounds)]
        public static Composite CreateDeathKnightUnholyPvPCombat()
        {
            return new PrioritySelector(

                Helpers.Common.EnsureReadyToAttackFromMelee(),
                Movement.CreateMoveToMeleeTightBehavior(true),
                Spell.WaitForCastOrChannel(),

                new Decorator(
                    ret => !Spell.IsGlobalCooldown(),
                    new PrioritySelector(
                        Helpers.Common.CreateAutoAttack(true),

                        Helpers.Common.CreateInterruptBehavior(),

                        Common.CreateGetOverHereBehavior(),

                        Common.CreateDarkSuccorBehavior(),

                        Common.CreateSoulReaperHasteBuffBehavior(),

                        Common.CreateDarkSimulacrumBehavior(),

                        // *** Cool downs ***
                        Spell.BuffSelf("Unholy Frenzy",
                            ret => StyxWoW.Me.CurrentTarget.IsWithinMeleeRange 
                                && !PartyBuff.WeHaveBloodlust 
                                && Helpers.Common.UseLongCoolDownAbility
                                ),

                        Spell.Cast("Summon Gargoyle", ret => DeathKnightSettings.UseSummonGargoyle && Helpers.Common.UseLongCoolDownAbility),


                        // *** Single target rotation. ***
                        // Execute
                        Spell.Cast("Soul Reaper", ret => StyxWoW.Me.CurrentTarget.HealthPercent < 35),

                        // Diseases
                        Spell.Cast("Outbreak",
                            ret => !StyxWoW.Me.CurrentTarget.HasMyAura("Frost Fever") 
                                || !StyxWoW.Me.CurrentTarget.HasAura("Blood Plague")
                                ),

                        Spell.Buff("Icy Touch", true, ret => !StyxWoW.Me.CurrentTarget.IsImmune(WoWSpellSchool.Frost), "Frost Fever"),

                        Spell.Buff("Plague Strike", true, on => Me.CurrentTarget, req => true, "Blood Plague"),

                        Spell.Cast("Dark Transformation",
                            ret => StyxWoW.Me.GotAlivePet 
                                && !StyxWoW.Me.Pet.ActiveAuras.ContainsKey("Dark Transformation") 
                                && StyxWoW.Me.HasAura("Shadow Infusion", 5)
                                ),
                        Spell.CastOnGround(
                            "Death and Decay",
                            on => StyxWoW.Me.CurrentTarget,
                            req => Common.UnholyRuneSlotsActive >= 2, 
                            false
                            ),
                        Spell.Cast("Scourge Strike", ret => StyxWoW.Me.UnholyRuneCount == 2 || StyxWoW.Me.DeathRuneCount > 0),
                        Spell.Cast("Festering Strike", ret => StyxWoW.Me.BloodRuneCount == 2 && StyxWoW.Me.FrostRuneCount == 2),
                        Spell.Cast("Death Coil", ret => StyxWoW.Me.HasAura(SuddenDoom) || StyxWoW.Me.CurrentRunicPower >= 80),
                        Spell.Cast("Scourge Strike"),
                        Spell.Cast("Festering Strike"),
                        Spell.Cast("Death Coil", ret => !StyxWoW.Me.GotAlivePet || !StyxWoW.Me.Pet.ActiveAuras.ContainsKey("Dark Transformation"))
                        )
                    )
                );
        }

        #endregion

        #region Instance Rotations

        [Behavior(BehaviorType.Combat, WoWClass.DeathKnight, WoWSpec.DeathKnightUnholy, WoWContext.Instances)]
        public static Composite CreateDeathKnightUnholyInstanceCombat()
        {
            return new PrioritySelector(

                Helpers.Common.EnsureReadyToAttackFromMelee(),
                Spell.WaitForCastOrChannel(),

                new Decorator(
                    ret => !Spell.IsGlobalCooldown(),
                    new PrioritySelector(
                        Helpers.Common.CreateAutoAttack(true),
                        Helpers.Common.CreateInterruptBehavior(),

                        // *** Cool downs ***
                        Spell.Cast(
                            "Summon Gargoyle",
                            ret => DeathKnightSettings.UseSummonGargoyle 
                                && Helpers.Common.UseLongCoolDownAbility),

                        // Start AoE section
                        new Decorator(
                            ret => Spell.UseAOE 
                                && Unit.UnfriendlyUnitsNearTarget(12f).Count() >= DeathKnightSettings.DeathAndDecayCount,
                            new PrioritySelector(
                                // Diseases
                                Common.CreateApplyDiseases(),

                                // spread the disease around.
                                new Throttle( 2, 
                                    Spell.Cast("Blood Boil",
                                        ret => !StyxWoW.Me.HasAura("Unholy Blight") 
                                            && StyxWoW.Me.CurrentTarget.DistanceSqr <= 10*10 
                                            && Common.ShouldSpreadDiseases)
                                    ),

                                Spell.Cast("Dark Transformation",
                                    ret => StyxWoW.Me.GotAlivePet 
                                        && !StyxWoW.Me.Pet.ActiveAuras.ContainsKey("Dark Transformation") 
                                        && Me.HasAura("Shadow Infusion", 5) ),

                                Spell.CastOnGround("Death and Decay",
                                    loc => StyxWoW.Me.CurrentTarget,
                                    req => Spell.UseAOE && Common.UnholyRuneSlotsActive >= 2, 
                                    false),

                                Spell.Cast("Blood Boil",
                                    ret => StyxWoW.Me.CurrentTarget.DistanceSqr <= 10*10 
                                        && (StyxWoW.Me.DeathRuneCount > 0  || (StyxWoW.Me.BloodRuneCount == 2 && StyxWoW.Me.FrostRuneCount == 2))),

                                // Execute
                                Spell.Cast("Soul Reaper", ret => StyxWoW.Me.CurrentTarget.HealthPercent < 35),
                                Spell.Cast("Scourge Strike", ret => StyxWoW.Me.UnholyRuneCount == 2),
                                Spell.Cast("Death Coil",
                                    req => StyxWoW.Me.HasAura(SuddenDoom) 
                                        || StyxWoW.Me.RunicPowerPercent >= 80 
                                        || !StyxWoW.Me.GotAlivePet 
                                        || !StyxWoW.Me.Pet.ActiveAuras.ContainsKey("Dark Transformation")),

                                Spell.Cast("Remorseless Winter", ret => Common.HasTalent( DeathKnightTalents.RemorselessWinter)),

                                Movement.CreateMoveToMeleeBehavior(true)
                                )
                            ),
                
                        // *** Single target rotation. ***

                        // Single target rotation.

                        // Target < 35%, Soul Reaper
                        Spell.Cast("Soul Reaper", ret => StyxWoW.Me.CurrentTarget.HealthPercent < 35),

                        // Diseases
                        Common.CreateApplyDiseases(),

                        // Dark Transformation
                        Spell.Cast("Dark Transformation",
                            ret => StyxWoW.Me.GotAlivePet
                                && !StyxWoW.Me.Pet.ActiveAuras.ContainsKey("Dark Transformation")
                                && StyxWoW.Me.HasAura("Shadow Infusion", 5)),

                        // Death Coil if Dark Trans not active
                        Spell.Cast("Death Coil",
                            req => !StyxWoW.Me.GotAlivePet
                                || !StyxWoW.Me.Pet.ActiveAuras.ContainsKey("Dark Transformation")
                            ),

                        // Scourge Strike on Death or Unholy Runes active
                        Spell.Cast("Scourge Strike",
                            req => Common.DeathRuneSlotsActive > 0 
                                || Common.UnholyRuneSlotsActive > 0),

                        // Festering Strike on both Blood and Frost runes active
                        Spell.Cast("Festering Strike",
                            req => Common.BloodRuneSlotsActive > 0
                                && Common.FrostRuneSlotsActive > 0),

                        // Death Coil (Sudden Doom, high RP)
                        Spell.Cast("Death Coil",
                            ret => Me.HasAura(SuddenDoom)
                                || Me.CurrentRunicPower >= 80
                                || (Me.BloodRuneCount + Me.FrostRuneCount + Me.UnholyRuneCount + Me.DeathRuneCount == 0)),                       

                        // post Single target
                        // attack at range if possible
                        Spell.Cast("Death Coil", req => Me.GotTarget && !Me.CurrentTarget.IsWithinMeleeRange )
                        )
                    ),

                Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        #endregion
    }
}