using CommonBehaviors.Actions;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.Pathing;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using Action = Styx.TreeSharp.Action;

#region methods
using S = Druid.DSpells.SpellCasts;
#endregion

namespace Druid.DRotations
{
    class Balance
    {
        public static Composite BalanceRot()
        {
            return new S.FrameLockSelector(
                S.castMoonfire(),
                S.castWrath()
            );
        }
    }
}
