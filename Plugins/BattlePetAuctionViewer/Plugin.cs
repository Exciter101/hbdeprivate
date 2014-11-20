using System;
using System.Collections.Generic;
using System.Linq;
using Styx.CommonBot;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Threading;
using System.Windows.Forms;

namespace BattlePetAuctionViewer
{
    public class Plugin : HBPlugin
    {
        private IPluginLogger _logger;
        private IPetLua _petLua;
        private IPetJournal _petJournal;
        private List<AhScanner> _aHScans = new List<AhScanner>();
        private FrmProgress _frmProgress;

        private const string BattlePetsQuery=  "QueryAuctionItems(nil ,nil,nil,nil,11,nil,{0}) return 1";
        private const string CompanionPetQuery = "QueryAuctionItems(nil ,nil,nil,nil,9,3,{0}) return 1";
       
        private int _lastPulse = int.MinValue;

        public Plugin()
        {
            _logger = new PluginLogger();
            _petLua = new PetLua(_logger);
        }

        private int step = 0;
        private int maxStep = 4;

        public override void Pulse()
        {
            try
            {
                if (_lastPulse + 3000 < Environment.TickCount || !_petJournal.IsLoaded)
                {
                    if (_frmProgress == null)
                    {
                        _frmProgress = new FrmProgress();
                        _frmProgress.Show();
                        _petJournal = new PetJournal(_logger, _petLua, _frmProgress);
                        _aHScans.Add(new AhScanner(_logger, _frmProgress, BattlePetsQuery, "Battle Pets"));
                        _aHScans.Add(new AhScanner(_logger, _frmProgress,CompanionPetQuery,"Companion Pets"));
                    }

                    bool updateFrmProgress = false;

                    if (!_petJournal.IsLoaded)
                    {
                        _petJournal.PopulatePetJournal();
                        updateFrmProgress = true;
                        step = 2;
                    }

                    bool allScansComplete = true;
                    foreach (AhScanner ahscanner in _aHScans)
                    {
                        if (updateFrmProgress)
                        {
                            string message = "Scanning Auction House (" + ahscanner.Descripton + ")";
                            _logger.Write(message);
                            step++;
                            _frmProgress.Step(step, maxStep, message + ":");
                        }

                        if (!ahscanner.Scanned)
                        {
                            ahscanner.Scan();

                            if (ahscanner.Scanned)
                            {
                                if (ahscanner.Pets.Count == 0)
                                {
                                    DialogResult d = MessageBox.Show("To scan the Auction House you need to have talked to an auctioneer and have the auction window open.\r\n\r\n Do you want to try again?", "Auction House Scan Failed!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);
                                    if (d == DialogResult.Cancel)
                                    {
                                        TreeRoot.Stop("Cancel Clicked");
                                        return;
                                    }
                                    if (d == DialogResult.Yes) { ahscanner.Scanned = false; }
                                    if (d == DialogResult.No) 
                                    {
                                        allScansComplete = true;
                                        break; 
                                    }
                                }
                                else
                                {
                                    updateFrmProgress = true;
                                }
     
                            }
                        }
                        if (!ahscanner.Scanned)
                        {
                            allScansComplete = false;
                            break;
                        }
                    }

                    if (allScansComplete)
                    {
                        List<Pet> allAhPets = new List<Pet>();
                        foreach (AhScanner ahscanner in _aHScans)
                        {
                            allAhPets.AddRange(ahscanner.Pets);
                        }

                        //Pet.Dump(@"c:\AHPets.csv", allAhPets, _logger);

                        _frmProgress.Close();
                        FrmPets pets = new FrmPets(_petJournal.PetsOwned, _petJournal.PetsNotOwned, allAhPets);
                        pets.ShowDialog();
                        TreeRoot.Stop("Viewer Closed");
                    }
                   
                    _lastPulse = Environment.TickCount;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError("Pulse() ",ex);
            }
        }

        #region Plugin Properties / Settings Button

        private static LocalPlayer Me { get { return Styx.StyxWoW.Me; } }
        public override string Name { get { return "Battle Pet Auction Viewer"; } }
        public override string Author { get { return "Andy West"; } }
        public override Version Version { get { return new Version(1, 0, 0, 0); } }
        public override string ButtonText { get { return "Configuration"; } }
        public override bool WantButton { get { return true; } }

        public override void OnButtonPress()
        {
            //new PluginSettingsForm(_pluginSettings, _logger).Show();
        }

        #endregion

        #region Bot Events

        public override void Initialize()
        {
            BotEvents.OnBotStarted += BotEvents_OnBotStarted;
            BotEvents.OnBotStopped += BotEvents_OnBotStopped;
            _logger.Write(Name + " loaded (V" + Version.ToString() + ")");
        }

        void BotEvents_OnBotStarted(EventArgs args)
        {
            if (Me.IsDead || Me.IsGhost || !Styx.StyxWoW.IsInGame) return;
        }

        void BotEvents_OnBotStopped(EventArgs args)
        {
            try
            {
                _frmProgress.Close();
                _frmProgress.Dispose();
                _frmProgress = null;
            }
            catch { }
        }

        #endregion
    }
}
