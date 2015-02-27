using Styx;
using Styx.Common;
using Styx.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using DefaultValue = Styx.Helpers.DefaultValueAttribute;
using Styx.WoWInternals.WoWObjects;

namespace RestoDruid
{
    class RSettings : Settings
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        public static readonly RSettings myPrefs = new RSettings();
        
        public RSettings() 
            :base(Path.Combine(Utilities.AssemblyDirectory, string.Format(@"Routines/Settings/Druid/{0}/{1}RSettings.xml", StyxWoW.Me.RealmName, StyxWoW.Me.Name)))
        {
        }

        [Setting, DefaultValue(true)]
        [Category("Movement")]
        [DisplayName("Auto Movement")]
        public bool MovementAuto { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Movement")]
        [DisplayName("Auto Targeting")]
        public bool MovementTargeting { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Movement")]
        [DisplayName("Auto Facing")]
        public bool MovementFacing { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Movement in Group")]
        [DisplayName("Disable Movement")]
        public bool MovementAutoDisable { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Movement in Group")]
        [DisplayName("Disable Targeting")]
        public bool MovementTargetingDisable { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Movement in Group")]
        [DisplayName("Disable Facing")]
        public bool MovementFacingDisable { get; set; }











        public enum MotW
        {
            Manual,
            Me_Only,
            All_Group_Members
        }

        [Setting, DefaultValue(MotW.Manual)]
        [Category("Misc")]
        [DisplayName("Mark of the Wild")]
        public MotW buffMarkOfTheWild { get; set; }

        [Setting, DefaultValue(45)]
        [Category("Misc")]
        [DisplayName("Healthstone %")]
        public int HealthstonePercent { get; set; }

        public enum TrinketUse
        {
            Manual,
            OnCoolDown,
            LowMana,
            LowHP,
            CrowdControlled
        }
        [Setting, DefaultValue(TrinketUse.Manual)]
        [Category("Misc")]
        [DisplayName("Trinket 1")]
        public TrinketUse Trinket1 { get; set; }

        [Setting, DefaultValue(65)]
        [Category("Misc")]
        [DisplayName("Trinket 1 low %")]
        public int Trinket1LowResources { get; set; }

        [Setting, DefaultValue(TrinketUse.Manual)]
        [Category("Misc")]
        [DisplayName("Trinket 2")]
        public TrinketUse Trinket2 { get; set; }

        [Setting, DefaultValue(0)]
        [Category("Misc")]
        [DisplayName("Trinket 2 low %")]
        public int Trinket2LowResources { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Misc")]
        [DisplayName("Auto Dispel")]
        public bool AutoDispel { get; set; }











        [Setting, DefaultValue(false)]
        [Category("Misc")]
        [DisplayName("Nature's Vigil")]
        public bool NaturesVigil { get; set; }

        [Setting, DefaultValue(65)]
        [Category("Misc")]
        [DisplayName("Nature's Vigil %")]
        public int NaturesVigilPercent { get; set; }

        [Setting, DefaultValue(3)]
        [Category("Misc")]
        [DisplayName("Nature's Vigil Players")]
        public int NaturesVigilPlayersCount { get; set; }











        public enum UseIncarnation
        {
            Manual,
            OnCoolDown,
            OnCoolDownBosses,
            OnLowHealthPlayers
        }

        [Setting, DefaultValue(UseIncarnation.Manual)]
        [Category("Healing Dungeons")]
        [DisplayName("Incarnation")]
        public UseIncarnation IncarnationD { get; set; }

        [Setting, DefaultValue(UseIncarnation.Manual)]
        [Category("Healing Raids")]
        [DisplayName("Incarnation")]
        public UseIncarnation IncarnationR { get; set; }

        [Setting, DefaultValue(0)]
        [Category("Healing Dungeons")]
        [DisplayName("Incarnation Players")]
        public int IncarnationPlayerCountD { get; set; }

        [Setting, DefaultValue(0)]
        [Category("Healing Raids")]
        [DisplayName("Incarnation Players")]
        public int IncarnationPlayerCountR { get; set; }

        [Setting, DefaultValue(0)]
        [Category("Healing Dungeons")]
        [DisplayName("Incarnation %")]
        public int IncarnationPercentD { get; set; }

        [Setting, DefaultValue(0)]
        [Category("Healing Raids")]
        [DisplayName("Incarnation %")]
        public int IncarnationPercentR { get; set; }












        [Setting, DefaultValue(85)]
        [Category("Healing Dungeons")]
        [DisplayName("Healing Touch %")]
        public int HealingTouchD { get; set; }

        [Setting, DefaultValue(80)]
        [Category("Healing Raids")]
        [DisplayName("Healing Touch %")]
        public int HealingTouchR { get; set; }











        [Setting, DefaultValue(100)]
        [Category("Healing Dungeons")]
        [DisplayName("Rejuvenation %")]
        public int RejuvenationD { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Healing Dungeons")]
        [DisplayName("Rejuvenation Tank")]
        [Description("Keep Rejuvenation Up On Tanks")]
        public bool RejuvenationDOnTank { get; set; }

        [Category("Healing Raids")]
        [DisplayName("Rejuvenation %")]
        [Setting, DefaultValue(90)]
        public int RejuvenationR { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Healing Raids")]
        [DisplayName("Rejuvenation Tank")]
        [Description("Keep Rejuvenation Up On Tanks")]
        public bool RejuvenationROnTank { get; set; }











        [Setting, DefaultValue(true)]
        [Category("Healing Dungeons")]
        [DisplayName("Germination Enable")]
        public bool GerminationEnableD { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Healing Raids")]
        [DisplayName("Germination Enable")]
        public bool GerminationEnableR { get; set; }

        [Setting, DefaultValue(90)]
        [Category("Healing Dungeons")]
        [DisplayName("Germination %")]
        public int GerminationD { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Healing Dungeons")]
        [DisplayName("Germination Tank")]
        [Description("Keep Germination Up On Tanks")]
        public bool GerminationDOnTank { get; set; }

        [Setting, DefaultValue(85)]
        [Category("Healing Raids")]
        [DisplayName("Germination %")]
        public int GerminationR { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Healing Raids")]
        [DisplayName("Germination Tank")]
        [Description("Keep Germination Up On Tanks")]
        public bool GerminationROnTank { get; set; }











        [Setting, DefaultValue(65)]
        [Category("Healing Dungeons")]
        [DisplayName("Regrowth %")]
        public int RegrowthD { get; set; }

        [Setting, DefaultValue(60)]
        [Category("Healing Raids")]
        [DisplayName("Regrowth %")]
        public int RegrowthR { get; set; }











        [Setting, DefaultValue(90)]
        [Category("Healing Dungeons")]
        [DisplayName("Force of Nature %")]
        public int FoND { get; set; }

        [Setting, DefaultValue(80)]
        [Category("Healing Raids")]
        [DisplayName("Force of Nature %")]
        public int FoNR { get; set; }











        [Setting, DefaultValue(4)]
        [Category("Healing Dungeons")]
        [DisplayName("Genesis Count")]
        [Description("Number of Players to use Genesis")]
        public int GenesisPlayerCountD { get; set; }

        [Setting, DefaultValue(4)]
        [Category("Healing Dungeons")]
        [DisplayName("Genesis %")]
        public int GenesisHealthPercentD { get; set; }

        [Setting, DefaultValue(false)]
        [Category("Healing Dungeons")]
        [DisplayName("Genesis Enable")]
        [Description("Use Genesis or Not (very mana expensive !)")]
        public bool GenesisUseD { get; set; }

        [Setting, DefaultValue(4)]
        [Category("Healing Raids")]
        [DisplayName("Genesis Count")]
        [Description("Number of Players to use Genesis")]
        public int GenesisPlayerCountR { get; set; }

        [Setting, DefaultValue(55)]
        [Category("Healing Raids")]
        [DisplayName("Genesis %")]
        public int GenesisHealthPercentR { get; set; }

        [Setting, DefaultValue(false)]
        [Category("Healing Raids")]
        [DisplayName("Genesis Auto")]
        [Description("Use Genesis or Not (very mana expensive !)")]
        public bool GenesisUseR { get; set; }











        [Setting, DefaultValue(4)]
        [Category("Healing Dungeons")]
        [DisplayName("Tranquility Count")]
        [Description("Number of Players to use Tranquility")]
        public int TranquilityPlayerCountD { get; set; }

        [Setting, DefaultValue(45)]
        [Category("Healing Dungeons")]
        [DisplayName("Tranquility %")]
        public int TranquilityHealthPercentD { get; set; }

        [Setting, DefaultValue(false)]
        [Category("Healing Dungeons")]
        [DisplayName("Tranquility Auto")]
        [Description("Auto Use or Not")]
        public bool TranquilityUseD { get; set; }

        [Setting, DefaultValue(6)]
        [Category("Healing Raids")]
        [DisplayName("Tranquility Count")]
        [Description("Number of Players to use Tranquility")]
        public int TranquilityPlayerCountR { get; set; }

        [Setting, DefaultValue(45)]
        [Category("Healing Raids")]
        [DisplayName("Tranquility %")]
        public int TranquilityHealthPercentR { get; set; }

        [Setting, DefaultValue(false)]
        [Category("Healing Raids")]
        [DisplayName("Tranquility Auto")]
        [Description("Auto Use or Not")]
        public bool TranquilityUseR { get; set; }











        [Setting, DefaultValue(2)]
        [Category("Healing Dungeons")]
        [DisplayName("Wild Growth Count")]
        [Description("Number of Players to use Wild Growth")]
        public int WildGrowthPlayerCountD { get; set; }

        [Setting, DefaultValue(85)]
        [Category("Healing Dungeons")]
        [DisplayName("Wild Growth %")]
        public int WildGrowthHealthPercentD { get; set; }

        [Setting, DefaultValue(5)]
        [Category("Healing Raids")]
        [DisplayName("Wild Growth Count")]
        [Description("Number of Players to use Wild Growth")]
        public int WildGrowthPlayerCountR { get; set; }

        [Setting, DefaultValue(85)]
        [Category("Healing Raids")]
        [DisplayName("Wild Growth %")]
        public int WildGrowthHealthPercentR { get; set; }










        [Setting, DefaultValue(2)]
        [Category("Healing Dungeons")]
        [DisplayName("Wild Mushroom Count")]
        [Description("Number of Players to use Wild Growth")]
        public int WildMushroomPlayerCountD { get; set; }

        [Setting, DefaultValue(85)]
        [Category("Healing Dungeons")]
        [DisplayName("Wild Mushroom %")]
        public int WildMushroomHealthPercentD { get; set; }

        [Setting, DefaultValue(5)]
        [Category("Healing Raids")]
        [DisplayName("Wild Mushroom Count")]
        [Description("Number of Players to use Wild Growth")]
        public int WildMushroomPlayerCountR { get; set; }

        [Setting, DefaultValue(85)]
        [Category("Healing Raids")]
        [DisplayName("Wild Mushroom %")]
        public int WildMushroomHealthPercentR { get; set; }










        [Setting, DefaultValue(45)]
        [Category("Healing Dungeons")]
        [DisplayName("Swiftmend %")]
        public int SwiftmendHealthPercentD { get; set; }

        [Setting, DefaultValue(45)]
        [Category("Healing Raids")]
        [DisplayName("Swiftmend %")]
        public int SwiftmendHealthPercentR { get; set; }

        [Setting, DefaultValue(45)]
        [Category("Healing Dungeons")]
        [DisplayName("Iron Bark Tank %")]
        public int IronBarkHealthPercentD { get; set; }

        [Setting, DefaultValue(45)]
        [Category("Healing Raids")]
        [DisplayName("Ironbark Tank %")]
        public int IronBarkHealthPercentR { get; set; }

    }
}
