using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattlePetAuctionViewer
{
    public partial class FrmPets : Form
    {
        bool busy = false;
        DataTable _table = new DataTable();
        List<Pet> _displayPetsMasterList = new List<Pet>();

        List<Pet> ahPets;
        List<Pet> allPets;

        public FrmPets(List<Pet> ownedPets, List<Pet> notOwnedPets, List<Pet> auctionPets)
        {
            busy = true;
            InitializeComponent();
            sourceCheckedList.Items.Clear();

            this.ahPets = auctionPets;

            Dictionary<string, Pet> distinctPetsDict = new Dictionary<string, Pet>();
            List<Pet> distinctOwnedPets = new List<Pet>();

            foreach (Pet pet in ownedPets)
            {
                if (!distinctPetsDict.Keys.Contains(pet.Name))
                {
                    distinctPetsDict.Add(pet.Name, pet);
                    distinctOwnedPets.Add(pet);
                }
                distinctPetsDict[pet.Name].OwnedCount++;
            }


            allPets = new List<Pet>();
            allPets.AddRange(distinctOwnedPets);
            allPets.AddRange(notOwnedPets);

            Dictionary<int, int> companionLookup = Lookups.CompanionLookup();

            foreach (Pet ahPet in ahPets)
            {
                if (companionLookup.Keys.Contains(ahPet.Id))
                {
                    ahPet.Id = companionLookup[ahPet.Id];
                }

                foreach (Pet allpet in allPets)
                {
                    if (ahPet.Name == allpet.Name)
                    {
                        ahPet.Id = allpet.Id;
                        ahPet.Info = allpet.Info;
                        ahPet.Source = allpet.Source;
                        ahPet.OwnedCount = allpet.OwnedCount;
                        break;
                    }
                    if (ahPet.Id == allpet.Id)
                    {
                        ahPet.Info = ahPet.Name + " - " + allpet.Info;
                        ahPet.Name = allpet.Name;
                        ahPet.Source = allpet.Source;
                        ahPet.OwnedCount = allpet.OwnedCount;
                        break;
                    }
                }
            }

            SetPopularity(allPets);
            SetPopularity(ahPets);

            SetupDisplayGrid();

            if (ahPets.Count > 0)
            {
                SetPetsToShow(ahPets);
            }
            else
            {
                radAllPets.Checked = true;
                SetPetsToShow(allPets);
            }

            busy = false;
        }

        private void SetPopularity(List<Pet> pets)
        {
            Dictionary<int, double> pop = Lookups.PopularityLookup();
            foreach (Pet pet in pets)
            {
                if (pop.Keys.Contains(pet.Id))
                {
                    pet.Popularity = pop[pet.Id];
                }
            }
        }

        private void SetPetsToShow(List<Pet> pets)
        {
            _displayPetsMasterList = pets;
            GeneratePetSources();
            FilterDisplayPets();
        }

        private void SetupDisplayGrid()
        {
            _table = new DataTable();
            _table.Columns.Add("Name");
            _table.Columns.Add("Id", typeof(int));
            _table.Columns.Add("Source");
            _table.Columns.Add("Info");
            _table.Columns.Add("BuyoutGold", typeof(int));
            _table.Columns.Add("Auction");
            _table.Columns.Add("OwnedCount", typeof(int));
            _table.Columns.Add("Popularity", typeof(double));

            this.dataGridView1.DataSource = _table;
        }

        private void GeneratePetSources()
        {
            foreach (Pet pet in _displayPetsMasterList)
            {
                if (!string.IsNullOrEmpty(pet.Source))
                {
                    if (!sourceCheckedList.Items.Contains(pet.Source))
                    {
                        sourceCheckedList.Items.Add(pet.Source, true);
                    }
                }
            }
        }

        private void DisplayPet(Pet pet)
        {
            DataRow row = _table.NewRow();
            _table.Rows.Add(row);
            row["Name"] = pet.Name;
            row["Id"] = pet.Id;
            row["Source"] = pet.Source;
            row["Info"] = pet.Info;
            row["BuyoutGold"] = pet.BuyoutGold;
            row["Auction"] = pet.Auction;
            row["OwnedCount"] = pet.OwnedCount;
            row["Popularity"] = pet.Popularity;
        }





        private void sourceList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (busy) { return; }
            busy = true;
            StartFilter(100);
            busy = false;
        }

        private void nameOrId_TextChanged(object sender, EventArgs e)
        {
            StartFilter(500);
        }

        Timer timer;
        public void StartFilter(int msDelay)
        {
            if (timer != null)
            {
                timer.Stop();
            }
            timer = new Timer();
            timer.Interval = msDelay;
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            FilterDisplayPets();
        }

        private FilterArgs GetFilter()
        {
            FilterArgs args = new FilterArgs();
            args.Sources = new List<string>();
            foreach (string s in sourceCheckedList.CheckedItems)
            {
                args.Sources.Add(s);
            }
            args.NameOrId = this.nameOrId.Text;

            args.OwnedPets = cbOwned.Checked;
            args.NotOwned = cbNotOwned.Checked;

            return args;
        }


        private void FilterDisplayPets()
        {
            Auction.Visible = !radAllPets.Checked;
            buyoutDataGridViewTextBoxColumn.Visible = !radAllPets.Checked;

            _table.Rows.Clear();
            FilterArgs args = GetFilter();
            List<Pet> petsToDisplay = new PetFilter().Filter(this._displayPetsMasterList, args);

            foreach (Pet pet in petsToDisplay)
            {
                DisplayPet(pet);
            }
        }

        private void radAuctionHousePets_CheckedChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked) { return; }
            SetPetsToShow(ahPets);
        }

        private void radAllPets_CheckedChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked) { return; }
            SetPetsToShow(allPets);
        }

        private void cbOwned_CheckedChanged(object sender, EventArgs e)
        {
            FilterDisplayPets();
        }

        private void cbNotOwned_CheckedChanged(object sender, EventArgs e)
        {
            FilterDisplayPets();
        }

        private void cbAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < sourceCheckedList.Items.Count; i++)
            {
                this.sourceCheckedList.SetItemChecked(i, cbAll.Checked);
            }
            FilterDisplayPets();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string filename = "Dump_" + DateTime.Now.ToString("HHmmss") + ".txt";

            FilterArgs args = GetFilter();
            List<Pet> petsToDisplay = new PetFilter().Filter(this._displayPetsMasterList, args);
            try
            {
                string pluginFilename = @"..\..\Plugins\BattlePetAuctionViewer\Dump_" + DateTime.Now.ToString("HHmmss") + ".txt";
                string path = System.Reflection.Assembly.GetExecutingAssembly().Location + @"\" + pluginFilename;

                Pet.Dump(path, petsToDisplay, null);
                MessageBox.Show("Saved to " + path + ".", "Save");
            }
            catch(DirectoryNotFoundException)
            {
                Pet.Dump(filename, petsToDisplay, null);
                MessageBox.Show("Saved to " + filename + ".", "Save");
            }
        }
    }
}
