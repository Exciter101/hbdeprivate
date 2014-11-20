using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Styx.WoWInternals;
using Styx;

namespace BattlePetAuctionViewer
{
    public class AhScanner
    {
        IPluginLogger _logger;
        private IProgress _progress;
        private string _query;
        private string _descripton;

        public string Descripton
        {
            get { return _descripton; }
            set { _descripton = value; }
        }

        public AhScanner(IPluginLogger logger,IProgress progress,string query,string description)
        {
            _logger = logger;
            _progress = progress;
            _query = query;
            _descripton = description;
        }

        private readonly Stopwatch _queueTimer = new Stopwatch();
        private int _page;
        bool _scanned = false;
        public bool Scanned
        {
            get { return _scanned; }
            set { _scanned = value; }
        }
        List<string> results = new List<string>();

        List<Pet> _pets = new List<Pet>();
        public List<Pet> Pets
        {
            get { return _pets; }
        }

        public bool Scan()
        {
            ScanAh();
            if (_scanned)
            {

                for (int i = 0; i < results.Count; i += 3)
                {
                    long buyout = 0;
                    long.TryParse(results[i + 1], out buyout);
                    Pet pet = new Pet(results[i], buyout);

                    int id = 0;
                    int.TryParse(results[i + 2], out id);
                    pet.Id = id;
                    pet.Auction = this.Descripton;
                    _pets.Add(pet);
                }
            }

            return _scanned;
        }

        private bool ScanAh()
        {
            if (!_queueTimer.IsRunning)
            {
                QueryBattlePets();
                _queueTimer.Start();
            }
            else if (_queueTimer.ElapsedMilliseconds <= 10000)
            {
                using (AquireFrame())
                {
                    if (CanSendAuctionQuery())
                    {
                        _queueTimer.Reset();
                        int totalAuctions = GetNumAuctionItems();
                        int maxPages = ((int)Math.Ceiling((double)totalAuctions / 50));
                        _progress.SubStep(_page + 1, maxPages < 1 ? 1 : maxPages);
                        List<string> retVals = GetAuctionItems();
                        if (retVals != null) { results.AddRange(retVals); }
                        if (++_page >= (int)Math.Ceiling((double)totalAuctions / 50))
                        {
                            _scanned = true;
                        }
                    }
                }
            }
            else
            {
                _scanned = true;
            }

            // reset to default values in preparations for next scan
            if (_scanned)
            {
                _queueTimer.Stop();
                _queueTimer.Reset();
                _page = 0;
            }
            return _scanned;
        }

        private static List<string> GetAuctionItems()
        {
            List<string> retVals = Lua.GetReturnValues(ScanAhFormatLua);
            return retVals;
        }

        private static int GetNumAuctionItems()
        {
            int totalAuctions = Lua.GetReturnVal<int>("return GetNumAuctionItems('list')", 1);
            return totalAuctions;
        }

        private static bool CanSendAuctionQuery()
        {
            return Lua.GetReturnVal<int>("if CanSendAuctionQuery('list') == 1 then return 1 else return 0 end ", 0)==1;
        }

        private static Styx.MemoryManagement.FrameLock AquireFrame()
        {
            return StyxWoW.Memory.AcquireFrame();
        }

        private void QueryBattlePets()
        {
            string lua = string.Format(_query, _page);
            Lua.GetReturnVal<int>(lua, 0);
        }

        public const string ScanAhFormatLua = @"
            local A,totalA= GetNumAuctionItems('list');
            local RetInfo = {};
            for index=1, A do 
	            local name,_,cnt,_,_,_,_,minBid,_,buyout,_,_,owner,sold,id=GetAuctionItemInfo('list', index);
                local itemLink = GetAuctionItemLink('list', index);
	            table.insert(RetInfo,tostring(name));
	            table.insert(RetInfo,tostring(buyout));
                table.insert(RetInfo,tostring(id));
            end; 
            return unpack(RetInfo);
            ";
    }
}
