using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Minecraft.NBT;
using NBTSharp;
using System.Diagnostics;
using PS3FileSystem;

namespace Minecraft_PS3_Save_Tool
{
    public partial class Form1 : Form
    {



        string saveTo;
        static int num = 0;
        private TagByte scale;
        private TagInt dimension;
        private TagShort width;
        private TagShort height;
        private TagInt xCenter;
        private TagInt zCenter;
        public static int mapPixelSize = 2;
        private TagByteArray colours;
        public static Color[] colourTable;
        private TagCompound root;
        private TagCompound proot;
        TagCompound lroot;
        
        private string path = "";
        private NBTFileReader reader;
        private NBTFileReader preader;
        NBTFileReader lreader;
        
        TagShort Health;
        TagShort DeathTime;
        TagShort AttackTime;
        TagShort HurtTime;
        TagInt PDimension;
        TagByte instabuild;
        TagByte flying;
        TagByte mayfly;
        TagByte invulnerable;
        TagByte Sleeping;

        TagByte hasStrongholdEndPortal;
        TagByte raining;
        TagByte hasBeenInCreative;
        TagByte thundering;
        TagByte spawnBonusChest;
        TagByte MapFeatures;
        TagByte hasStronghold;
        TagByte hardcore;
        TagShort SleepTimer;
        TagInt SpawnX;
        TagInt SpawnY;
        TagInt SpawnZ;
        TagInt LSpawnX;
        TagInt LSpawnY;
        TagInt LSpawnZ;
        TagInt StrongholdX;
        TagInt StrongholdY;
        TagInt StrongholdZ;
        TagInt StrongholdEndPortalX;
        TagInt StrongholdEndPortalZ;
        TagLong Time;
        TagInt rainTime;
        TagInt thunderTime;
        TagInt GameType;
        TagLong LastPlayed;
        TagLong RandomSeed;

        TagInt foodLevel;
        TagInt foodTickTimer;
        TagFloat foodExhaustionLevel;
        TagFloat foodSaturationLevel;
        TagFloat XpP;
        TagInt XpLevel;
        TagInt XpTotal;
        static int numplayer = 0;
        static int nummaps = 0;
        int c = 0;
        string[] level = new string[1];
        
        string[] players = new string[numplayer];
        string[] maps = new string[nummaps];
        byte[] HEADER;
        byte[] TABLE;
        byte[][] GAMEDATA_Files = new byte[num][];
        byte[][] GAMEDATA_Files_len = new byte[num][];
        byte[][] GAMEDATA_Add = new byte[num][];
        byte[][] GAMEDATA_rest = new byte[num][];
        string[] nametext = new string[num];
        byte[][] files = new byte[num][];
        string pathtofile;
        EnchantForm enchantForm = null;
        EditForm editForm = null;
        ItemDataForm itemDataForm = null;
        List<CheckBox> groups = new List<CheckBox>();
        MemoryStream GAMEDATA = new MemoryStream();
        MemoryStream TABLE_read = new MemoryStream();
        


        public Form1()
        {
            
	
            InitializeComponent();
           // List<CheckBox> groups = new List<CheckBox>();
			Data.Init("items.txt");
			
			//labelVersion.Text = Data.mcVersion;
           
			boxItems.LargeImageList = Data.list;
			boxItems.ItemDrag += ItemDrag;
			
			foreach (Data.Group group in Data.groups.Values) {
				CheckBox box = new CheckBox();
				box.Size = new Size(26, 26);
                box.Location = new Point(groupBox2.Width - 205 + (groups.Count / 12) * 27, 42 + (groups.Count % 12) * 27);
				box.ImageList = Data.list;
				box.ImageIndex = group.imageIndex;
				box.Appearance = Appearance.Button;
				box.Checked = true;
				box.Tag = group;
				box.MouseDown += ItemMouseDown;
                groupBox2.Controls.Add(box);
				groups.Add(box);
			}
			Width += ((groups.Count-1) / 12) * 27;
			
			UpdateItems();
			
			
		}

        private void Form1_Load(object sender, EventArgs e)
        {

            this.loadColorTable();
            this.clearTags();
            this.panel2.Paint += new PaintEventHandler(this.panel2_Paint);
            base.Focus();
            this.defaultTagValues(true);
            if (this.path != "")
            {
                if (this.loadFile(this.path))
                {
                    //  this.enableControls(this.path);
                    this.Refresh();
                }
                else
                {
                    //MessageBox.Show("Failed to load " + this.path, "MapItemEdit");
                }
            }
            /* else if (this.loadPicturePath != "")
             {
                 this.newFile(true);
                 this.importPicture(this.loadPicturePath);
             }*/
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.toolStripLabel1.Visible = false;
          //  byte[] test = {0x00, 0xA6, 0xBE, 0x9A};
          //  label1.Text = ConversionUtils.getHexString(test);
            this.folderBrowserDialog1.Reset();
            DialogResult openfolder = this.folderBrowserDialog1.ShowDialog();
            if (openfolder == DialogResult.OK)
                this.toolStripLabel1.Text = this.folderBrowserDialog1.SelectedPath;
            pathtofile = this.folderBrowserDialog1.SelectedPath;
            if (!File.Exists(pathtofile + "/~files_decrypted_by_pfdtool.txt"))
            {
              //File.Create(pathtofile + "/~files_decrypted_by_pfdtool.txt");
              ps3_Decrypt(pathtofile);
            }
/*
            this.openFileDialog1.Reset();
            DialogResult openFile = this.openFileDialog1.ShowDialog();
            if (openFile == DialogResult.OK)
                this.toolStripLabel1.Text = this.openFileDialog1.FileName;*/
            GAMEDATA.Flush();
            FileStream GAMEDATA1 = File.Open(toolStripLabel1.Text + "/GAMEDATA", FileMode.Open);
            GAMEDATA1.CopyTo(GAMEDATA);
            GAMEDATA1.Close();
            GAMEDATA.Seek(0, SeekOrigin.Begin);
            byte[] GAMEDATA04 = new byte[0x04];

            GAMEDATA.Read(GAMEDATA04, 0, GAMEDATA04.Length);
            
            if (BitConverter.IsLittleEndian)
            { 
                Array.Reverse(GAMEDATA04);
            }
            uint test2 =
            BitConverter.ToUInt32(GAMEDATA04, 0);
     
            byte[] numtemp = new byte[0x04];
            GAMEDATA.Read(numtemp, 0, 4);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(numtemp);
            }
            num = BitConverter.ToInt32(numtemp, 0);


            GAMEDATA.Seek(0, SeekOrigin.Begin);
            HEADER = new byte[0x0C];

            GAMEDATA.Read(HEADER, 0, HEADER.Length);

            GAMEDATA.Seek(test2, SeekOrigin.Begin);
            TABLE = new byte[0x90 * num];

            GAMEDATA.Read(TABLE, 0, TABLE.Length);


           // num = (int)((GAMEDATA.Length - test2 )/ 144);
            GAMEDATA_Files = new byte[num][];
            GAMEDATA_Files_len = new byte[num][];
            GAMEDATA_Add = new byte[num][];
            GAMEDATA_rest = new byte[num][];
            nametext = new string[num];
            files = new byte[num][];
            GAMEDATA.Seek(test2, SeekOrigin.Begin);
            int i = 0;
            uint test3 = test2;
            
            while (i < num)
            {
                GAMEDATA_Files[i] = new byte[0x80];
                GAMEDATA_Files_len[i] = new byte[0x04];
                GAMEDATA_Add[i] = new byte[0x04];
                GAMEDATA_rest[i] = new byte[0x08];
                GAMEDATA.Read(GAMEDATA_Files[i], 0, GAMEDATA_Files[i].Length);
                GAMEDATA.Read(GAMEDATA_Files_len[i], 0, GAMEDATA_Files_len[i].Length);
                GAMEDATA.Read(GAMEDATA_Add[i], 0, GAMEDATA_Add[i].Length);
                GAMEDATA.Read(GAMEDATA_rest[i], 0, GAMEDATA_rest[i].Length);

                string btext = BitConverter.ToString(GAMEDATA_Files[i]).Replace("-", "").Replace("00", "");
                byte[] bname = StringToByteArray(btext);
                char[] text = System.Text.Encoding.UTF8.GetString(bname).ToCharArray();
                UTF8Encoding temp = new UTF8Encoding(true);
                nametext[i] = temp.GetString(bname);
                test3 = +144;
                i++;
            }

                i = 0;
            while(i < num)
            {
                if (nametext[i].EndsWith(".dat") && !nametext[i].Contains("map") && !nametext[i].Contains("map_") && !nametext[i].Contains("level") && !nametext[i].Contains("villages"))
                {
                    numplayer++;
                }
                else if (nametext[i].EndsWith(".dat") && nametext[i].Contains("map_"))
                {
                    nummaps++;
                }
                i++;
            }
            //domainUpDown1.Items.AddRange(nametext);

            string[] players = new string[numplayer];
            string[] maps = new string[nummaps];
            int p = 0;
            int m = 0;
            i = 0;
            while (i < num)
            {
               /* if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(GAMEDATA_Add[i]);
                    Array.Reverse(GAMEDATA_Files_len[i]);
                }
                uint add =
                BitConverter.ToUInt32(GAMEDATA_Add[i], 0);
                int len =
                BitConverter.ToInt32(GAMEDATA_Files_len[i], 0);
                GAMEDATA.Seek(add, SeekOrigin.Begin);
                files[i] = new byte[len];
                GAMEDATA.Read(files[i], 0, len);*/


                if (nametext[i].EndsWith(".dat") && !nametext[i].Contains("map") && !nametext[i].Contains("map_") && !nametext[i].Contains("level") && !nametext[i].Contains("villages"))
                    {
                        players[p] = nametext[i];
                        p++;
                    }
                else if (nametext[i].EndsWith(".dat") && nametext[i].Contains("map_"))
                    {
                        maps[m] = nametext[i];
                        m++;
                    }
                else if (nametext[i].EndsWith(".dat") && nametext[i].Contains("level"))
                {
                    level[0] = nametext[i];
                    
                }
                i++;
            }

            domainUpDown1.Items.AddRange(players);
            domainUpDown2.Items.AddRange(maps);
            toolStripButton1.Enabled = false;

            save_files();

        }

        private void save_files()
        {
            byte[][] files = new byte[num][];

            
            //this.folderBrowserDialog1.Reset();
           // DialogResult openfolder = this.folderBrowserDialog1.ShowDialog();
            //if (openfolder == DialogResult.OK)
           // saveTo = AppDomain.CurrentDomain.BaseDirectory + "/Extracted";//this.folderBrowserDialog1.SelectedPath + "/Working";
            saveTo = pathtofile + "/Extracted";//this.folderBrowserDialog1.SelectedPath + "/Working";
            
           // if (Directory.Exists(saveTo)) { Directory.Delete(saveTo, true); };
            Directory.CreateDirectory(saveTo + "/DIM1");
            //Directory.CreateDirectory(saveTo + "/DIM-1");
            Directory.CreateDirectory(saveTo + "/data");

            
            FileStream T = File.Open(saveTo + "/TABLE", FileMode.Create);
            T.Write(TABLE, 0, TABLE.Length);
            T.Close();
            FileStream H = File.Open(saveTo + "/HEADER", FileMode.Create);
            H.Write(HEADER, 0, HEADER.Length);
            H.Close();
            
            int i = 0;

            while (i < num)
            {
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(GAMEDATA_Add[i]);
                    Array.Reverse(GAMEDATA_Files_len[i]);
                }
                uint add =
                BitConverter.ToUInt32(GAMEDATA_Add[i], 0);
                int len =
                BitConverter.ToInt32(GAMEDATA_Files_len[i], 0);
                GAMEDATA.Seek(add, SeekOrigin.Begin);
                files[i] = new byte[len];
                GAMEDATA.Read(files[i], 0, len);

                FileStream o = File.Open(saveTo + "/" + nametext[i], FileMode.Create);




                o.Write(files[i], 0, len);
                o.Close();
                i++;
            }
            if (domainUpDown1.Items.Count == 0) 
            {
                domainUpDown1.Items.AddRange(level);
            }
            domainUpDown1.SelectedIndex = 0;

            domainUpDown2.SelectedIndex = 0;
            loadlevelFile();
            //toolStripButton1.Enabled = false;
            toolStripButton2.Enabled = true;
            /* string directoryPath = saveTo;
            

             DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);

             foreach (FileInfo fileToCompress in directorySelected.GetFiles("*.mcr", SearchOption.AllDirectories))
             {
                 Compress(fileToCompress);
             }*/
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
           /* if (c == 1){
            DialogResult dialogResult = MessageBox.Show("Save Player", "Save Player", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                //saveplayer();
            }
            }*/

            BtnCloseTabClick();
            string name = saveTo + "/" + domainUpDown1.Text;
            Open(name);
            loadplayerFile();
            c = 1;
        }

        private bool loadplayerFile()
        {
            this.path = saveTo + "/" + domainUpDown1.Text; ;
            try
            {
                 preader = new NBTFileReader(path);
                 proot = preader.compound;

                Health = (TagShort)proot.getNode(@"\Health");
                DeathTime = (TagShort)proot.getNode(@"\DeathTime");
                AttackTime = (TagShort)proot.getNode(@"\AttackTime");
                HurtTime = (TagShort)proot.getNode(@"\HurtTime");
                PDimension = (TagInt)proot.getNode(@"\Dimension");
                Sleeping = (TagByte)proot.getNode(@"\Sleeping");
                invulnerable = (TagByte)proot.getNode(@"\abilities\invulnerable");
                instabuild = (TagByte)proot.getNode(@"\abilities\instabuild");
                flying = (TagByte)proot.getNode(@"\abilities\flying");
                mayfly = (TagByte)proot.getNode(@"\abilities\mayfly");
               
                SleepTimer = (TagShort)proot.getNode(@"\SleepTimer");
                SpawnX = (TagInt)proot.getNode(@"\SpawnX");
                SpawnY = (TagInt)proot.getNode(@"\SpawnY");
                SpawnZ = (TagInt)proot.getNode(@"\SpawnZ");
                foodLevel = (TagInt)proot.getNode(@"\foodLevel");
                foodTickTimer = (TagInt)proot.getNode(@"\foodTickTimer");
                foodExhaustionLevel = (TagFloat)proot.getNode(@"\foodExhaustionLevel");
                foodSaturationLevel = (TagFloat)proot.getNode(@"\foodSaturationLevel");
                XpP = (TagFloat)proot.getNode(@"\XpP");
                XpLevel = (TagInt)proot.getNode(@"\XpLevel");
                XpTotal = (TagInt)proot.getNode(@"\XpTotal");
                /*  this.reader = new NBTFileReader(path);
                  this.root = this.reader.compound;
                  if (!((((this.loadColorsArray()))))) //&& this.loadScale()) && (this.loadDimension() && this.loadWidth())) && (this.loadHeight() && this.loadXCenter())) && this.loadZCenter()))
                  {
                      return false;
                  }*/
            }
            catch (FailedReadException exception)
            {
                if (exception.InnerException != null)
                {
                    MessageBox.Show(exception.InnerException.Message);
                }
                return false;
            }
            this.txtHealth.Text = this.Health.ToString();
            // this.txtDeathTime.Text = this.DeathTime.ToString();
            // this.txtAttackTime.Text = this.AttackTime.ToString();
            // this.txtHurtTime.Text = this.HurtTime.ToString();
            this.txtPDimension.Text = this.PDimension.ToString();
            if (this.Sleeping.ToString() == "1")
            { this.chkSleeping.Checked = true; }
            else { this.chkSleeping.Checked = false; }

            if (this.instabuild.ToString() == "1")
            { this.chkInstabuild.Checked = true; }
            else { this.chkInstabuild.Checked = false; }

            if (this.invulnerable.ToString() == "1")
            { this.chkInvulnerable.Checked = true; }
            else { this.chkInvulnerable.Checked = false; }

            if (this.mayfly.ToString() == "1")
            { this.chkMayfly.Checked = true; }
            else { this.chkMayfly.Checked = false; }

            if (this.flying.ToString() == "1")
            { this.chkFlying.Checked = true; }
            else { this.chkFlying.Checked = false; }
            
            // this.txtSleepTimer.Text = this.SleepTimer.ToString();
            this.txtSpawnX.Text = this.SpawnX.ToString();
            this.txtSpawnY.Text = this.SpawnY.ToString();
            this.txtSpawnZ.Text = this.SpawnZ.ToString();
            this.txtFoodLevel.Text = this.foodLevel.ToString();
            this.txtFoodTickTimer.Text = this.foodTickTimer.ToString();
            this.txtFoodExhaustionLevel.Text = this.foodExhaustionLevel.ToString();
            this.txtFoodSaturationLevel.Text = this.foodSaturationLevel.ToString();
            this.txtXpP.Text = this.XpP.ToString();
            this.txtXpLevel.Text = this.XpLevel.ToString();
            this.txtXpTotal.Text = this.XpTotal.ToString();
            return true;
        }

        private bool loadlevelFile()
        {
            this.path = saveTo + "/level.dat"; ;
            try
            {
                 lreader = new NBTFileReader(path);
                 lroot = lreader.compound;

                hasStrongholdEndPortal = (TagByte)lroot.getNode(@"\Data\hasStrongholdEndPortal");
                hasStronghold = (TagByte)lroot.getNode(@"\Data\hasStronghold");
                hardcore = (TagByte)lroot.getNode(@"\Data\hardcore");
                thundering = (TagByte)lroot.getNode(@"\Data\thundering");
                raining = (TagByte)lroot.getNode(@"\Data\raining");
                hasBeenInCreative = (TagByte)lroot.getNode(@"\Data\hasBeenInCreative");
                spawnBonusChest = (TagByte)lroot.getNode(@"\Data\spawnBonusChest");
                MapFeatures = (TagByte)lroot.getNode(@"\Data\MapFeatures");
                hasBeenInCreative = (TagByte)lroot.getNode(@"\Data\hasBeenInCreative");


                LSpawnX = (TagInt)lroot.getNode(@"\Data\SpawnX");
                LSpawnY = (TagInt)lroot.getNode(@"\Data\SpawnY");
                LSpawnZ = (TagInt)lroot.getNode(@"\Data\SpawnZ");

                StrongholdX = (TagInt)lroot.getNode(@"\Data\StrongholdX");
                StrongholdY = (TagInt)lroot.getNode(@"\Data\StrongholdY");
                StrongholdZ = (TagInt)lroot.getNode(@"\Data\StrongholdZ");

                StrongholdEndPortalX = (TagInt)lroot.getNode(@"\Data\StrongholdEndPortalX");
                StrongholdEndPortalZ = (TagInt)lroot.getNode(@"\Data\StrongholdEndPortalZ");

                RandomSeed = (TagLong)lroot.getNode(@"\Data\RandomSeed");
                LastPlayed = (TagLong)lroot.getNode(@"\Data\LastPlayed");
                Time = (TagLong)lroot.getNode(@"\Data\Time");

                GameType = (TagInt)lroot.getNode(@"\Data\GameType");
                rainTime = (TagInt)lroot.getNode(@"\Data\rainTime");
                thunderTime = (TagInt)lroot.getNode(@"\Data\thunderTime");


                
            }
            catch (FailedReadException exception)
            {
                if (exception.InnerException != null)
                {
                    MessageBox.Show(exception.InnerException.Message);
                }
                return false;
            }

            if (this.hasStrongholdEndPortal.ToString() == "1")
            { this.chkhasStrongholdEndPortal.Checked = true; }
            else { this.chkhasStrongholdEndPortal.Checked = false; }

            if (this.hasStronghold.ToString() == "1")
            { this.chkhasStronghold.Checked = true; }
            else { this.chkhasStronghold.Checked = false; }

            if (this.hardcore.ToString() == "1")
            { this.chkhardcore.Checked = true; }
            else { this.chkhardcore.Checked = false; }

            if (this.thundering.ToString() == "1")
            { this.chkthundering.Checked = true; }
            else { this.chkthundering.Checked = false; }

            if (this.raining.ToString() == "1")
            { this.chkraining.Checked = true; }
            else { this.chkraining.Checked = false; }

            if (this.hasBeenInCreative.ToString() == "1")
            { this.chkhasBeenInCreative.Checked = true; }
            else { this.chkhasBeenInCreative.Checked = false; }

            if (this.spawnBonusChest.ToString() == "1")
            { this.chkspawnBonusChest.Checked = true; }
            else { this.chkspawnBonusChest.Checked = false; }

            if (this.MapFeatures.ToString() == "1")
            { this.chkMapFeatures.Checked = true; }
            else { this.chkMapFeatures.Checked = false; }

            if (this.hasBeenInCreative.ToString() == "1")
            { this.chkhasBeenInCreative.Checked = true; }
            else { this.chkhasBeenInCreative.Checked = false; }
				

            this.txtLSpawnX.Text = this.LSpawnX.ToString();
            this.txtLSpawnY.Text = this.LSpawnY.ToString();
            this.txtLSpawnZ.Text = this.LSpawnZ.ToString();

            this.txtStrongholdX.Text = this.StrongholdX.ToString();
            this.txtStrongholdY.Text = this.StrongholdY.ToString();
            this.txtStrongholdZ.Text = this.StrongholdZ.ToString();

            this.txtStrongholdEndPortalX.Text = this.StrongholdEndPortalX.ToString();
            this.txtStrongholdEndPortalZ.Text = this.StrongholdEndPortalZ.ToString();

            this.txtRandomSeed.Text = this.RandomSeed.ToString();
            this.txtLastPlayed.Text = this.LastPlayed.ToString();
            this.txtTime.Text = this.Time.ToString();

            this.txtGameType.Text = this.GameType.ToString();
            this.txtrainTime.Text = this.rainTime.ToString();
            this.txtthunderTime.Text = this.thunderTime.ToString();
            return true;
        }

        #region INVedit


        void CreateBackup(string file)
        {
            string backup = Path.ChangeExtension(file, Path.GetExtension(file) + ".backup");
            if (File.Exists(backup))
                try { File.Delete(backup); }
                catch { return; }
            File.Copy(file, backup);
        }

        void Open(string file)
        {
            Page page = new Page();
            page.Changed += Change;
            Change(null);
            Open(page, file);
            tabControl.TabPages.Add(page);
            tabControl.SelectedTab = page;
        }

        void Open(Page page, string file)
        {
            try
            {
                FileInfo info = new FileInfo(file);
                page.file = info.FullName;
                if (info.Name == "level.dat") { page.Text = info.Directory.Name; }
                else { page.Text = info.Name; }
                //Text = "INVedit - " + page.Text;
                page.changed = false;
                btnSave.Enabled = true;
                btnEdit.Enabled = true;
                btnEnchant.Enabled = true;
                // btnCloseTab.Enabled = true;
                btnReload.Enabled = true;
                NbtTag tag = NbtTag.Load(file);
                if (tag.Type == NbtTagType.Compound && tag.Contains("Data")) { tag = tag["Data"]; }
                if (tag.Type == NbtTagType.Compound && tag.Contains("Player")) { tag = tag["Player"]; }
                if (tag.Type == NbtTagType.Compound && tag.Contains("Inventory")) { tag = tag["Inventory"]; }
                if (tag.Name != "Inventory") { throw new Exception("Can't find Inventory tag."); }
                Inventory.Load(tag, page.slots);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        void save(string file) 
        {
            Page page = (Page)tabControl.SelectedTab;
            //saveFileDialog.FileName = page.file;
           // if (saveFileDialog.ShowDialog() == DialogResult.OK)
             //   Save(page, file);
		
        }
        void Save(string file)
        {
            try
            {
                Page page = (Page)tabControl.SelectedTab;
                FileInfo info = new FileInfo(file);
                if (info.Exists && page.file != info.FullName)
                {
                    string str;
                    if (info.Name == "level.dat")
                        str = "Are you sure you want to overwrite " + info.Directory.Name + "?";
                    else str = "Are you sure you want to overwrite '" + info.Name + "'?";
                    DialogResult result = MessageBox.Show(str, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result != DialogResult.Yes) return;
                }
                page.file = info.FullName;
                NbtTag root, tag;
                if (info.Exists)
                {
                    root = NbtTag.Load(page.file);
                    if (info.Extension.ToLower() == ".dat")
                        CreateBackup(file);
                    tag = root;
                }
                else
                {
                    if (info.Extension.ToLower() == ".dat")
                    {
                        MessageBox.Show("You can't create a new Minecraft level/player file.\n" +
                                        "Select an existing one instead.", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    } root = NbtTag.CreateCompound("Inventory", NbtTag.CreateList(NbtTagType.Compound));
                    tag = root;
                } if (tag.Type == NbtTagType.Compound && tag.Contains("Data")) { tag = tag["Data"]; }
                if (tag.Type == NbtTagType.Compound && tag.Contains("Player")) { tag = tag["Player"]; }
                if (tag.Type != NbtTagType.Compound || !tag.Contains("Inventory")) { throw new Exception("Can't find Inventory tag."); }
                Inventory.Save(tag, page.slots);
                root.Save(page.file);
                if (info.Name == "level.dat") { page.Text = info.Directory.Name; }
                else { page.Text = info.Name; }
                Text = "INVedit - " + page.Text;
                page.changed = false;
                btnReload.Enabled = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = ((string[])e.Data.GetData(DataFormats.FileDrop));
                foreach (string file in files)
                {
                    FileInfo info = new FileInfo(file);
                    if (info.Extension.ToLower() == ".inv" || info.Extension.ToLower() == ".dat")
                        e.Effect = DragDropEffects.Copy;
                }
            }
        }
        protected override void OnDragDrop(DragEventArgs e)
        {
            OnDragEnter(e);
            BringToFront();
            if (e.Effect == DragDropEffects.None) return;
            string[] files = ((string[])e.Data.GetData(DataFormats.FileDrop));
            foreach (string file in files)
                if (File.Exists(file)) Open(file);
        }

        void UpdateItems()
        {
            boxItems.BeginUpdate();
            boxItems.Clear();
            if (boxSearch.Text == "" || boxSearch.Font.Italic)
            {
                foreach (CheckBox box in groups) if (box.Checked)
                        foreach (Data.Item item in ((Data.Group)box.Tag).items)
                            boxItems.Items.Add(new ListViewItem(item.name, item.imageIndex) { Tag = new Item(item.id, 0, 0, item.damage) });
            }
            else
            {
                short id;
                if (short.TryParse(boxSearch.Text, out id))
                {
                    if (Data.items.ContainsKey(id))
                        foreach (Data.Item item in Data.items[id].Values)
                            boxItems.Items.Add(new ListViewItem(item.name, item.imageIndex) { Tag = new Item(item.id, 0, 0, item.damage) });
                    else boxItems.Items.Add(new ListViewItem("Unknown item " + id, 0) { Tag = new Item(id) });
                }
                else foreach (CheckBox box in groups) if (box.Checked)
                            foreach (Data.Item item in ((Data.Group)box.Tag).items)
                                if (item.name.IndexOf(boxSearch.Text, StringComparison.InvariantCultureIgnoreCase) >= 0)
                                    boxItems.Items.Add(new ListViewItem(item.name, item.imageIndex) { Tag = new Item(item.id, 0, 0, item.damage) });
            }
            boxItems.EndUpdate();
        }

        void ItemMouseDown(object sender, MouseEventArgs e)
        {
            CheckBox self = (CheckBox)sender;
            bool changed = false;
            if (e.Button == MouseButtons.Left)
            {
                bool other = true;
                foreach (CheckBox box in groups)
                    if (box.Checked == (self != box))
                        other = false;
                foreach (CheckBox box in groups)
                    if (box.Checked == (self != box) || other)
                    {
                        changed = true;
                        box.Checked = (self == box) || other;
                    }
            }
            else if (e.Button == MouseButtons.Right)
            {
                self.Checked = !self.Checked;
                changed = true;
            }
            else return;
            self.Select();
            if (changed) UpdateItems();
            if (e.Button == MouseButtons.Left)
                self.Checked = !self.Checked;
        }

        void BoxSearchEnter(object sender, EventArgs e)
        {
            if (!boxSearch.Font.Italic) return;
            boxSearch.Font = new Font(boxSearch.Font, FontStyle.Regular);
            boxSearch.ForeColor = SystemColors.ControlText;
            boxSearch.Text = "";
        }

        void BoxSearchLeave(object sender, EventArgs e)
        {
            if (boxSearch.Text != "") return;
            boxSearch.Font = new Font(boxSearch.Font, FontStyle.Italic);
            boxSearch.ForeColor = Color.Gray;
            boxSearch.Text = "Search...";
        }

        void BoxSearchTextChanged(object sender, EventArgs e)
        {
            UpdateItems();
        }

        void ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            Item item = (Item)((ListViewItem)e.Item).Tag;
            item = new Item(item.ID, item.Preferred, 0, item.Damage);
            DoDragDrop(item, DragDropEffects.Copy | DragDropEffects.Move);
        }

        void BtnNewClick(object sender, EventArgs e)
        {
            Page page = new Page();
            page.Changed += Change;
            Change(null);
            page.Text = "unnamed.inv";
            Text = "INVedit - unnamed.inv";
            tabControl.TabPages.Add(page);
            tabControl.SelectedTab = page;
            //btnSave.Enabled = true;
            //btnCloseTab.Enabled = true;
            //btnReload.Enabled = false;
        }

      /*  void BtnOpenClick(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                Open(openFileDialog.FileName);
        }*/

        void BtnSaveClick(object sender, EventArgs e)
        {/*
            Page page = (Page)tabControl.SelectedTab;
            saveFileDialog1.FileName = page.file;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                Save(page, saveFileDialog1.FileName);
       */ }

        void BtnReloadClick(object sender, EventArgs e)
        {
            if (((Page)tabControl.SelectedTab).changed)
            {
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to reload this tab?\n" +
                    "All unsaved changes will be lost.", "Question",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes) return;
            }
            try
            {
                Page page = (Page)tabControl.SelectedTab;
                Open(page, page.file);
                page.ItemChanged(false);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        void BtnCloseTabClick()
        {
            if (tabControl.TabPages.Count != 0 && ((Page)tabControl.SelectedTab).changed)
            {
                /*DialogResult result = MessageBox.Show(
                    "Are you sure you want to close this tab?\n" +
                    "All unsaved changes will be lost.", "Question",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)*/ return;
            }
            if (tabControl.TabPages.Count != 0)
            {
            tabControl.TabPages.Remove(tabControl.SelectedTab);
            if (tabControl.TabPages.Count == 0)
            {
                //btnSave.Enabled = false;
                //btnCloseTab.Enabled = false;
            }
            }
        }

        void BtnEnchantClick(object sender, EventArgs e)
        {
            btnEnchant.Enabled = false;
            enchantForm = new EnchantForm();
            enchantForm.Closed += delegate
            {
                btnEnchant.Enabled = true;
                enchantForm = null;
            };
            if (tabControl.SelectedTab != null)
                enchantForm.Update(((Page)tabControl.SelectedTab).selected);
            enchantForm.Show(this);
        }

        void BtnEditClick(object sender, EventArgs e)
        {
            btnEdit.Enabled = false;
            editForm = new EditForm();
            editForm.Closed += delegate
            {
                btnEdit.Enabled = true;
                editForm = null;
            };
            if (tabControl.SelectedTab != null)
                editForm.Update(((Page)tabControl.SelectedTab).selected);
            editForm.Show(this);
        }

      /*  void BtnItemDataClick(object sender, EventArgs e)
        {
            btnItemData.Enabled = false;
            itemDataForm = new ItemDataForm();
            itemDataForm.Closed += delegate
            {
                btnItemData.Enabled = true;
                itemDataForm = null;
            };
            if (tabControl.SelectedTab != null)
                itemDataForm.Update(((Page)tabControl.SelectedTab).selected);
            itemDataForm.Show(this);
        }*/

        void Change(ItemSlot slot)
        {
            if (enchantForm != null)
                enchantForm.Update(slot);
            if (editForm != null)
                editForm.Update(slot);
            if (itemDataForm != null)
                itemDataForm.Update(slot);
        }

        void BtnAboutClick(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        void MainFormFormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (TabPage page in tabControl.TabPages)
                if (((Page)page).changed)
                {
                    DialogResult result = MessageBox.Show(
                        "Are you sure you want to close INVedit?\n" +
                        "There still are unsaved inventory tabs.", "Question",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result != DialogResult.Yes) e.Cancel = true;
                    break;
                }
        }

        void TabControlDragOver(object sender, DragEventArgs e)
        {
            Point point = tabControl.PointToClient(new Point(e.X, e.Y));
            TabPage hover = null;
            for (int i = 0; i < tabControl.TabPages.Count; ++i)
                if (tabControl.GetTabRect(i).Contains(point))
                {
                    hover = tabControl.TabPages[i];
                    break;
                }
            if (hover == null || hover == tabControl.SelectedTab) return;
            if (!e.Data.GetDataPresent(typeof(Item))) return;
            tabControl.SelectedTab = hover;
            Change(((Page)tabControl.SelectedTab).selected);
        }

        void TabControlSelected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage != null)
            {
                Text = "INVedit - " + e.TabPage.Text;
                btnReload.Enabled = (((Page)e.TabPage).file != null);
                Change(((Page)tabControl.SelectedTab).selected);
            }
            else
            {
                Text = "INVedit - Minecraft Inventory Editor";
                btnReload.Enabled = false;
                Change(null);
            }

        }

      /*  void BtnOpenDropDownOpening(object sender, EventArgs e)
        {
            btnOpen.DropDownItems.Clear();
            ResourceManager resources = new ResourceManager("INVedit.Resources", GetType().Assembly);
            Image world = (Image)resources.GetObject("world");
            DirectoryInfo dirs = new DirectoryInfo(appdata + "/saves");
            if (dirs.Exists) foreach (DirectoryInfo dir in dirs.GetDirectories())
                {
                    if (dir.GetFiles("level.dat").Length > 0)
                    {
                        ToolStripItem item = btnOpen.DropDownItems.Add("Open from „" + dir.Name + "“", world);
                        item.Tag = dir.FullName + "/level.dat";
                    }
                }
        }*/

        void BtnOpenDropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Open((string)e.ClickedItem.Tag);
        }

      /*  void BtnSaveDropDownOpening(object sender, EventArgs e)
        {
            btnSave.DropDownItems.Clear();
            ResourceManager resources = new ResourceManager("INVedit.Resources", GetType().Assembly);
            Image world = (Image)resources.GetObject("world");
            DirectoryInfo dirs = new DirectoryInfo(appdata + "/saves");
            if (dirs.Exists) foreach (DirectoryInfo dir in dirs.GetDirectories())
                {
                    if (dir.GetFiles("level.dat").Length > 0)
                    {
                        ToolStripItem item = btnSave.DropDownItems.Add("Save to „" + dir.Name + "“", world);
                        item.Tag = dir.FullName + "/level.dat";
                    }
                }
        }*/

       /* void BtnSaveDropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Page page = (Page)tabControl.SelectedTab;
            Save(page, (string)e.ClickedItem.Tag);
        }*/
		
       #endregion

        #region map

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            int num = 0x80;
            int num2 = 0x80;
            if (this.colours != null)
            {
                for (int i = 0; i < num2; i++)
                {
                    for (int j = 0; j < num; j++)
                    {
                        Brush brush;
                        int index = this.colours[(i * 0x80) + j];
                        if (index == 0)
                        {
                            brush = new SolidBrush(SystemColors.Control);
                        }
                        else
                        {
                            brush = new SolidBrush(colourTable[index]);
                        }
                        graphics.FillRectangle(brush, new Rectangle(j * mapPixelSize, i * mapPixelSize, mapPixelSize, mapPixelSize));
                    }
                }
            }
        }

        private bool loadFile(string path)
        {
            this.path = path;
            try
            {
                this.reader = new NBTFileReader(path);
                this.root = this.reader.compound;
                if (!((((this.loadColorsArray() && this.loadScale()) && (this.loadDimension() && this.loadWidth())) && (this.loadHeight() && this.loadXCenter())) && this.loadZCenter()))
                {
                    return false;
                }
            }
            catch (FailedReadException exception)
            {
                if (exception.InnerException != null)
                {
                    MessageBox.Show(exception.InnerException.Message);
                }
                return false;
            }
             this.txtScale.Text = this.scale.ToString();
             this.txtDimension.Text = this.dimension.ToString();
             this.txtWidth.Text = this.width.ToString();
             this.txtHeight.Text = this.height.ToString();
             this.txtXCenter.Text = this.xCenter.ToString();
             this.txtZCenter.Text = this.zCenter.ToString();
            //this.lblImage.Visible = false;
            return true;
        }

        public void clearTags()
        {
            this.root = null;
            this.colours = null;
            /*  this.scale = null;
              this.dimension = null;
              this.width = null;
              this.height = null;
              this.xCenter = null;
              this.zCenter = null;
              this.txtScale.Text = "";
              this.txtDimension.Text = "";
              this.txtWidth.Text = "";
              this.txtHeight.Text = "";
              this.txtXCenter.Text = "";
              this.txtZCenter.Text = "";*/
            // this.Text = "Map Item Editor";
        }

        public bool loadColorsArray()
        {
            try
            {
                Tag tag = this.root.getNode(@"\data\colors");
                if (tag.tagType != 7)
                {
                    MessageBox.Show("Colors tag found, but it is a " + NBTSharp.Util.getTypeName(tag.tagType) + " where Tag_Byte_Array was expected");
                    return false;
                }
                this.colours = (TagByteArray)tag;
            }
            catch (InvalidPathException exception)
            {
                MessageBox.Show("Couldn't find the colors tag\n" + exception.Message);
                return false;
            }
            return true;
        }

        private void loadColorTable()
        {
            /* if (this.radNewColours.Checked)
             {
                 loadNewColours();
             }
             else
             {*/
            loadOldColours();
            // }
            /*  this.cmbColour.Items.Clear();
              for (int i = 0; i < colourTable.Length; i++)
              {
                  this.cmbColour.Items.Add(i);
              }
              if (this.cmbColour.Items.Count > defaultColor)
              {
                  this.cmbColour.SelectedIndex = defaultColor;
              }*/
        }

        public static void loadNewColours()
        {
            colourTable = new Color[] { 
                Color.FromArgb(0xff, 0xff, 0xff), Color.FromArgb(0xff, 0xff, 0xff), Color.FromArgb(0xff, 0xff, 0xff), Color.FromArgb(0xff, 0xff, 0xff), Color.FromArgb(0x59, 0x7d, 0x27), Color.FromArgb(0x6d, 0x99, 0x30), Color.FromArgb(0x7f, 0xb2, 0x38), Color.FromArgb(0x43, 0x5e, 0x1d), Color.FromArgb(0xae, 0xa4, 0x73), Color.FromArgb(0xd5, 0xc9, 140), Color.FromArgb(0xf7, 0xe9, 0xa3), Color.FromArgb(130, 0x7b, 0x56), Color.FromArgb(0x75, 0x75, 0x75), Color.FromArgb(0x90, 0x90, 0x90), Color.FromArgb(0xa7, 0xa7, 0xa7), Color.FromArgb(0x58, 0x58, 0x58), 
                Color.FromArgb(180, 0, 0), Color.FromArgb(220, 0, 0), Color.FromArgb(0xff, 0, 0), Color.FromArgb(0x87, 0, 0), Color.FromArgb(0x70, 0x70, 180), Color.FromArgb(0x8a, 0x8a, 220), Color.FromArgb(160, 160, 0xff), Color.FromArgb(0x54, 0x54, 0x87), Color.FromArgb(0x75, 0x75, 0x75), Color.FromArgb(0x90, 0x90, 0x90), Color.FromArgb(0xa7, 0xa7, 0xa7), Color.FromArgb(0x58, 0x58, 0x58), Color.FromArgb(0, 0x57, 0), Color.FromArgb(0, 0x6a, 0), Color.FromArgb(0, 0x7c, 0), Color.FromArgb(0, 0x41, 0), 
                Color.FromArgb(180, 180, 180), Color.FromArgb(220, 220, 220), Color.FromArgb(0xff, 0xff, 0xff), Color.FromArgb(0x87, 0x87, 0x87), Color.FromArgb(0x73, 0x76, 0x81), Color.FromArgb(0x8d, 0x90, 0x9e), Color.FromArgb(0xa4, 0xa8, 0xb8), Color.FromArgb(0x56, 0x58, 0x61), Color.FromArgb(0x81, 0x4a, 0x21), Color.FromArgb(0x9d, 0x5b, 40), Color.FromArgb(0xb7, 0x6a, 0x2f), Color.FromArgb(0x60, 0x38, 0x18), Color.FromArgb(0x4f, 0x4f, 0x4f), Color.FromArgb(0x60, 0x60, 0x60), Color.FromArgb(0x70, 0x70, 0x70), Color.FromArgb(0x3b, 0x3b, 0x3b), 
                Color.FromArgb(0x2d, 0x2d, 180), Color.FromArgb(0x37, 0x37, 220), Color.FromArgb(0x40, 0x40, 0xff), Color.FromArgb(0x21, 0x21, 0x87), Color.FromArgb(0x49, 0x3a, 0x23), Color.FromArgb(0x59, 0x47, 0x2b), Color.FromArgb(0x68, 0x53, 50), Color.FromArgb(0x37, 0x2b, 0x1a), Color.FromArgb(180, 0xb1, 0xac), Color.FromArgb(220, 0xd9, 0xd3), Color.FromArgb(0xff, 0xfc, 0xf5), Color.FromArgb(0x87, 0x85, 0x81), Color.FromArgb(0x98, 0x59, 0x24), Color.FromArgb(0xba, 0x6d, 0x2c), Color.FromArgb(0xd8, 0x7f, 0x33), Color.FromArgb(0x72, 0x43, 0x1b), 
                Color.FromArgb(0x7d, 0x35, 0x98), Color.FromArgb(0x99, 0x41, 0xba), Color.FromArgb(0xb2, 0x4c, 0xd8), Color.FromArgb(0x5e, 40, 0x72), Color.FromArgb(0x48, 0x6c, 0x98), Color.FromArgb(0x58, 0x84, 0xba), Color.FromArgb(0x66, 0x99, 0xd8), Color.FromArgb(0x36, 0x51, 0x72), Color.FromArgb(0xa1, 0xa1, 0x24), Color.FromArgb(0xc5, 0xc5, 0x2c), Color.FromArgb(0xe5, 0xe5, 0x33), Color.FromArgb(0x79, 0x79, 0x1b), Color.FromArgb(0x59, 0x90, 0x11), Color.FromArgb(0x6d, 0xb0, 0x15), Color.FromArgb(0x7f, 0xcc, 0x19), Color.FromArgb(0x43, 0x6c, 13), 
                Color.FromArgb(170, 0x59, 0x74), Color.FromArgb(0xd0, 0x6d, 0x8e), Color.FromArgb(0xf2, 0x7f, 0xa5), Color.FromArgb(0x80, 0x43, 0x57), Color.FromArgb(0x35, 0x35, 0x35), Color.FromArgb(0x41, 0x41, 0x41), Color.FromArgb(0x4c, 0x4c, 0x4c), Color.FromArgb(40, 40, 40), Color.FromArgb(0x6c, 0x6c, 0x6c), Color.FromArgb(0x84, 0x84, 0x84), Color.FromArgb(0x99, 0x99, 0x99), Color.FromArgb(0x51, 0x51, 0x51), Color.FromArgb(0x35, 0x59, 0x6c), Color.FromArgb(0x41, 0x6d, 0x84), Color.FromArgb(0x4c, 0x7f, 0x99), Color.FromArgb(40, 0x43, 0x51), 
                Color.FromArgb(0x59, 0x2c, 0x7d), Color.FromArgb(0x6d, 0x36, 0x99), Color.FromArgb(0x7f, 0x3f, 0xb2), Color.FromArgb(0x43, 0x21, 0x5e), Color.FromArgb(0x24, 0x35, 0x7d), Color.FromArgb(0x2c, 0x41, 0x99), Color.FromArgb(0x33, 0x4c, 0xb2), Color.FromArgb(0x1b, 40, 0x5e), Color.FromArgb(0x48, 0x35, 0x24), Color.FromArgb(0x58, 0x41, 0x2c), Color.FromArgb(0x66, 0x4c, 0x33), Color.FromArgb(0x36, 40, 0x1b), Color.FromArgb(0x48, 0x59, 0x24), Color.FromArgb(0x58, 0x6d, 0x2c), Color.FromArgb(0x66, 0x7f, 0x33), Color.FromArgb(0x36, 0x43, 0x1b), 
                Color.FromArgb(0x6c, 0x24, 0x24), Color.FromArgb(0x84, 0x2c, 0x2c), Color.FromArgb(0x99, 0x33, 0x33), Color.FromArgb(0x51, 0x1b, 0x1b), Color.FromArgb(0x11, 0x11, 0x11), Color.FromArgb(0x15, 0x15, 0x15), Color.FromArgb(0x19, 0x19, 0x19), Color.FromArgb(13, 13, 13), Color.FromArgb(0xb0, 0xa8, 0x36), Color.FromArgb(0xd7, 0xcd, 0x42), Color.FromArgb(250, 0xee, 0x4d), Color.FromArgb(0x84, 0x7e, 40), Color.FromArgb(0x40, 0x9a, 150), Color.FromArgb(0x4f, 0xbc, 0xb7), Color.FromArgb(0x5c, 0xdb, 0xd5), Color.FromArgb(0x30, 0x73, 0x70), 
                Color.FromArgb(0x34, 90, 180), Color.FromArgb(0x3f, 110, 220), Color.FromArgb(0x4a, 0x80, 0xff), Color.FromArgb(0x27, 0x43, 0x87), Color.FromArgb(0, 0x99, 40), Color.FromArgb(0, 0xbb, 50), Color.FromArgb(0, 0xd9, 0x3a), Color.FromArgb(0, 0x72, 30), Color.FromArgb(14, 14, 0x15), Color.FromArgb(0x12, 0x11, 0x1a), Color.FromArgb(0x15, 20, 0x1f), Color.FromArgb(11, 10, 0x10), Color.FromArgb(0x4f, 1, 0), Color.FromArgb(0x60, 1, 0), Color.FromArgb(0x70, 2, 0), Color.FromArgb(0x3b, 1, 0)
             };
        }

        public static void loadOldColours()
        {
            colourTable = new Color[0x38];
            colourTable[0] = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
            colourTable[1] = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
            colourTable[2] = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
            colourTable[3] = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
            colourTable[4] = Color.FromArgb(0xff, 0x59, 0x7d, 0x27);
            colourTable[5] = Color.FromArgb(0xff, 0x6d, 0x99, 0x30);
            colourTable[6] = Color.FromArgb(0xff, 0x7f, 0xb2, 0x38);
            colourTable[7] = Color.FromArgb(0xff, 0x6d, 0x99, 0x30);
            colourTable[8] = Color.FromArgb(0xff, 0xae, 0xa4, 0x73);
            colourTable[9] = Color.FromArgb(0xff, 0xd5, 0xc9, 140);
            colourTable[10] = Color.FromArgb(0xff, 0xf7, 0xe9, 0xa3);
            colourTable[11] = Color.FromArgb(0xff, 0xd5, 0xc9, 140);
            colourTable[12] = Color.FromArgb(0xff, 0xae, 0xa4, 0x73);
            colourTable[13] = Color.FromArgb(0xff, 0xd5, 0xc9, 140);
            colourTable[14] = Color.FromArgb(0xff, 0xf7, 0xe9, 0xa3);
            colourTable[15] = Color.FromArgb(0xff, 0xd5, 0xc9, 140);
            colourTable[12] = Color.FromArgb(0xff, 0x75, 0x75, 0x75);
            colourTable[13] = Color.FromArgb(0xff, 0x90, 0x90, 0x90);
            colourTable[14] = Color.FromArgb(0xff, 0xa7, 0xa7, 0xa7);
            colourTable[15] = Color.FromArgb(0xff, 0x90, 0x90, 0x90);
            colourTable[0x10] = Color.FromArgb(0xff, 180, 0, 0);
            colourTable[0x11] = Color.FromArgb(0xff, 220, 0, 0);
            colourTable[0x12] = Color.FromArgb(0xff, 0xff, 0, 0);
            colourTable[0x13] = Color.FromArgb(0xff, 220, 0, 0);
            colourTable[20] = Color.FromArgb(0xff, 0x70, 0x70, 180);
            colourTable[0x15] = Color.FromArgb(0xff, 0x8a, 0x8a, 220);
            colourTable[0x16] = Color.FromArgb(0xff, 160, 160, 0xff);
            colourTable[0x17] = Color.FromArgb(0xff, 0x8a, 0x8a, 220);
            colourTable[0x18] = Color.FromArgb(0xff, 0x75, 0x75, 0x75);
            colourTable[0x19] = Color.FromArgb(0xff, 0x90, 0x90, 0x90);
            colourTable[0x1a] = Color.FromArgb(0xff, 0xa7, 0xa7, 0xa7);
            colourTable[0x1b] = Color.FromArgb(0xff, 0x90, 0x90, 0x90);
            colourTable[0x1c] = Color.FromArgb(0xff, 0, 0x57, 0);
            colourTable[0x1d] = Color.FromArgb(0xff, 0, 0x6a, 0);
            colourTable[30] = Color.FromArgb(0xff, 0, 0x7c, 0);
            colourTable[0x1f] = Color.FromArgb(0xff, 0, 0x6a, 0);
            colourTable[0x20] = Color.FromArgb(0xff, 180, 180, 180);
            colourTable[0x21] = Color.FromArgb(0xff, 220, 220, 220);
            colourTable[0x22] = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
            colourTable[0x23] = Color.FromArgb(0xff, 220, 220, 220);
            colourTable[0x24] = Color.FromArgb(0xff, 0x73, 0x76, 0x81);
            colourTable[0x25] = Color.FromArgb(0xff, 0x8d, 0x90, 0x9e);
            colourTable[0x26] = Color.FromArgb(0xff, 0xa4, 0xa8, 0xb8);
            colourTable[0x27] = Color.FromArgb(0xff, 0x8d, 0x90, 0x9e);
            colourTable[40] = Color.FromArgb(0xff, 0x81, 0x4a, 0x21);
            colourTable[0x29] = Color.FromArgb(0xff, 0x9d, 0x5b, 40);
            colourTable[0x2a] = Color.FromArgb(0xff, 0xb7, 0x6a, 0x2f);
            colourTable[0x2b] = Color.FromArgb(0xff, 0x9d, 0x5b, 40);
            colourTable[0x2c] = Color.FromArgb(0xff, 0x4f, 0x4f, 0x4f);
            colourTable[0x2d] = Color.FromArgb(0xff, 0x60, 0x60, 0x60);
            colourTable[0x2e] = Color.FromArgb(0xff, 0x70, 0x70, 0x70);
            colourTable[0x2f] = Color.FromArgb(0xff, 0x60, 0x60, 0x60);
            colourTable[0x30] = Color.FromArgb(0xff, 0x2d, 0x2d, 180);
            colourTable[0x31] = Color.FromArgb(0xff, 0x37, 0x37, 220);
            colourTable[50] = Color.FromArgb(0xff, 0x40, 0x40, 0xff);
            colourTable[0x33] = Color.FromArgb(0xff, 0x37, 0x37, 220);
            colourTable[0x34] = Color.FromArgb(0xff, 0x49, 0x3a, 0x23);
            colourTable[0x35] = Color.FromArgb(0xff, 0x59, 0x47, 0x2b);
            colourTable[0x36] = Color.FromArgb(0xff, 0x68, 0x53, 50);
            colourTable[0x37] = Color.FromArgb(0xff, 0x59, 0x47, 0x2b);
        }

        public void defaultTagValues(bool resetColors)
        {
            this.root = new TagCompound();
            TagCompound tag = new TagCompound("data");
            this.root.addTag(tag);
              this.scale = new TagByte("scale", 0);
              this.dimension = new TagInt("dimension", 0);
              this.width = new TagShort("width", 0x80);
              this.height = new TagShort("height", 0x80);
              this.xCenter = new TagInt("xCenter", 0x2710);
              this.zCenter = new TagInt("zCenter", 0x2710);
            if (resetColors)
            {
                byte[] bytes = new byte[0x4000];
                for (int i = 0; i < 0x4000; i++)
                {
                    bytes[i] = 0;
                }
                this.colours = new TagByteArray("colors", 0x4000, bytes);
            }
             tag.addTag(this.scale);
             tag.addTag(this.dimension);
             tag.addTag(this.width);
             tag.addTag(this.height);
             tag.addTag(this.xCenter);
             tag.addTag(this.zCenter);
            tag.addTag(this.colours);
        }

        public bool loadDimension()
        {
            try
            {
                Tag tag = this.root.getNode(@"\data\dimension");
                if (tag.tagType == 1)
                {
                    TagByte num = (TagByte)tag;
                    this.dimension = new TagInt("dimension", num.number);
                    TagCompound compound = (TagCompound)this.root.getNode(@"\data");
                    compound.removeTag(tag);
                    TagInt num2 = new TagInt("dimension", 0);
                    compound.addTag(num2);
                    return true;
                }
                if (tag.tagType == 3)
                {
                    this.dimension = (TagInt)tag;
                    return true;
                }
                if ((tag.tagType != 3) && (tag.tagType != 1))
                {
                    MessageBox.Show("Dimension tag found, but it is a " + NBTSharp.Util.getTypeName(tag.tagType) + " where Tag_Int was expected");
                    return false;
                }
            }
            catch (InvalidPathException exception)
            {
                MessageBox.Show("Couldn't find the dimension tag\n" + exception.Message);
                return false;
            }
            return true;
        }

        public bool loadHeight()
        {
            try
            {
                Tag tag = this.root.getNode(@"\data\height");
                if (tag.tagType != 2)
                {
                    MessageBox.Show("Height tag found, but it is a " + NBTSharp.Util.getTypeName(tag.tagType) + " where Tag_Short was expected");
                    return false;
                }
                this.height = (TagShort)tag;
            }
            catch (InvalidPathException exception)
            {
                MessageBox.Show("Couldn't find the height tag\n" + exception.Message);
                return false;
            }
            return true;
        }

        public bool loadScale()
        {
            try
            {
                Tag tag = this.root.getNode(@"\data\scale");
                if (tag.tagType != 1)
                {
                    MessageBox.Show("Scale tag found, but it is a " + NBTSharp.Util.getTypeName(tag.tagType) + " where Tag_Byte was expected");
                    return false;
                }
                this.scale = (TagByte)tag;
            }
            catch (InvalidPathException exception)
            {
                MessageBox.Show("Couldn't find the scale tag\n" + exception.Message);
                return false;
            }
            return true;
        }

        public bool loadWidth()
        {
            try
            {
                Tag tag = this.root.getNode(@"\data\width");
                if (tag.tagType != 2)
                {
                    MessageBox.Show("Width tag found, but it is a " + NBTSharp.Util.getTypeName(tag.tagType) + " where Tag_Short was expected");
                    return false;
                }
                this.width = (TagShort)tag;
            }
            catch (InvalidPathException exception)
            {
                MessageBox.Show("Couldn't find the width tag\n" + exception.Message);
                return false;
            }
            return true;
        }

        public bool loadXCenter()
        {
            try
            {
                Tag tag = this.root.getNode(@"\data\xCenter");
                if (tag.tagType != 3)
                {
                    MessageBox.Show("xCenter tag found, but it is a " + NBTSharp.Util.getTypeName(tag.tagType) + " where Tag_Int was expected");
                    return false;
                }
                this.xCenter = (TagInt)tag;
            }
            catch (InvalidPathException exception)
            {
                MessageBox.Show("Couldn't find the xCenter tag\n" + exception.Message);
                return false;
            }
            return true;
        }

        public bool loadZCenter()
        {
            try
            {
                Tag tag = this.root.getNode(@"\data\zCenter");
                if (tag.tagType != 3)
                {
                    MessageBox.Show("zCenter tag found, but it is a " + NBTSharp.Util.getTypeName(tag.tagType) + " where Tag_Int was expected");
                    return false;
                }
                this.zCenter = (TagInt)tag;
            }
            catch (InvalidPathException exception)
            {
                MessageBox.Show("Couldn't find the zCenter tag\n" + exception.Message);
                return false;
            }
            return true;
        }

        private void domainUpDown2_SelectedItemChanged(object sender, EventArgs e)
        {
            string name = saveTo + "/" + domainUpDown2.Text;
            if (this.loadFile(name))
            {
                //this.enableControls(dialog.FileName);
            }
            else
            {
                this.clearTags();
            }
            this.Refresh();
        }

        private void panel2_DoubleClick(object sender, EventArgs e)
        {
            if (File.Exists("MapItemEdit.exe")) 
            {
                //string exe = "MapItemEdit.exe";
                string name = saveTo + "/" + domainUpDown2.Text;
                // Use ProcessStartInfo class
                ProcessStartInfo startInfo = new ProcessStartInfo();
                //startInfo.CreateNoWindow = false;
                startInfo.UseShellExecute = false;
                startInfo.FileName = "MapItemEdit.exe";
               // startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.Arguments = name;
                Process.Start(startInfo);
            }
        }

        #endregion


        /*  private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "/Working"))
            {
                Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "/Working");
            }
        }*/

        /*  private void Form1_FormClosing(object sender, FormClosingEventArgs e)
          {
              if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory+"/Working")) 
              {
                  Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "/Working");
              }
          }*/


        #region save

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            saveplayer();
            savelevel();
            Import(pathtofile);
            fix_crc(pathtofile);
            ps3_Rebuild(pathtofile);
            if (chkDelext.Checked == true) { Directory.Delete(pathtofile + "/Extracted", true); };
            System.Windows.Forms.Application.Exit();
        }

        private void saveplayer()
        {
            bool flag = true;
            try
            {
                Save(pathtofile + "/Extracted/" +  domainUpDown1.Text);
                NBTFileWriter.write(this.path, this.proot);
            }
            catch (FailedWriteException ex)
            {
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.InnerException.Message);
                }
                flag = false;
            }
            if (flag)
            {
                //MessageBox.Show("Player saved successfully");
            }
        }

        private void savelevel()
        {
            bool flag = true;
            try
            {
                NBTFileWriter.write(this.path, this.lroot);
            }
            catch (FailedWriteException ex)
            {
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.InnerException.Message);
                }
                flag = false;
            }
            if (flag)
            {
                //MessageBox.Show("Level saved successfully");
            }
        }

        public void Import(string pathtofile)
        {
            if (!File.Exists(pathtofile + "/Extracted/TABLE"))
            {
               // Console.WriteLine("TABLE not found");
                //Console.WriteLine("Press any key to exit");
                //Console.ReadKey();
            }
            if (File.Exists(pathtofile + "/Extracted/TABLE"))
            {
                FileStream Table = File.Open(pathtofile + "/Extracted/TABLE", FileMode.Open);
                Table.CopyTo(TABLE_read);
                Table.Seek(0, SeekOrigin.Begin);
                TABLE = new byte[Table.Length];
                Table.Read(TABLE, 0, TABLE.Length);
                Table.Close();
                num = TABLE.Length / 0x90;
                GAMEDATA_Files = new byte[num][];
                GAMEDATA_Files_len = new byte[num][];
                GAMEDATA_Add = new byte[num][];
                GAMEDATA_rest = new byte[num][];
                nametext = new string[num];

                TABLE_read.Seek(0, SeekOrigin.Begin);
                int i = 0;
                while (i < num)
                {
                    GAMEDATA_Files[i] = new byte[0x80];
                    GAMEDATA_Files_len[i] = new byte[0x04];
                    GAMEDATA_Add[i] = new byte[0x04];
                    GAMEDATA_rest[i] = new byte[0x08];
                    TABLE_read.Read(GAMEDATA_Files[i], 0, GAMEDATA_Files[i].Length);
                    TABLE_read.Read(GAMEDATA_Files_len[i], 0, GAMEDATA_Files_len[i].Length);
                    TABLE_read.Read(GAMEDATA_Add[i], 0, GAMEDATA_Add[i].Length);
                    TABLE_read.Read(GAMEDATA_rest[i], 0, GAMEDATA_rest[i].Length);

                    string btext = BitConverter.ToString(GAMEDATA_Files[i]).Replace("-", "").Replace("00", "");
                    byte[] bname = StringToByteArray(btext);
                    char[] text = System.Text.Encoding.UTF8.GetString(bname).ToCharArray();
                    UTF8Encoding temp = new UTF8Encoding(true);
                    nametext[i] = temp.GetString(bname);

                    i++;
                }
            }
            if (!File.Exists(pathtofile + "/Extracted/HEADER"))
            {
                Console.WriteLine("HEADER not found");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
            if (File.Exists(pathtofile + "/Extracted/HEADER"))
            {
                FileStream Header = File.Open(pathtofile + "/Extracted/HEADER", FileMode.Open);
                Header.Seek(0, SeekOrigin.Begin);
                HEADER = new byte[0x0C];
                Header.Read(HEADER, 0, HEADER.Length);
                Header.Close();

            }
            Import_files(pathtofile);
        }

        public void Import_files(string pathtofile)
        {
            FileStream GAMEDATA1 = File.Open(pathtofile + "/GAMEDATA", FileMode.Open);
            GAMEDATA1.Write(HEADER, 0, HEADER.Length);
            int i = 0;
            int l = HEADER.Length;
            while (i < num)
            {
                FileStream o = File.Open(pathtofile + "/Extracted/" + nametext[i], FileMode.Open);
                byte[] text = new byte[(int)o.Length];
                o.Read(text, 0, text.Length);
                GAMEDATA1.Write(text, 0, (int)text.Length);
                GAMEDATA_Files_len[i] = BitConverter.GetBytes(text.Length);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(GAMEDATA_Files_len[i]);
                }
                GAMEDATA_Add[i] = BitConverter.GetBytes(l);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(GAMEDATA_Add[i]);
                }
                l = l + text.Length;

               
                o.Close();
                i++;
            }
            i = 0;
            GAMEDATA1.Seek(l, SeekOrigin.Begin);
            while (i < num)
            {

                GAMEDATA1.Write(GAMEDATA_Files[i], 0, GAMEDATA_Files[i].Length);
                GAMEDATA1.Write(GAMEDATA_Files_len[i], 0, GAMEDATA_Files_len[i].Length);

                GAMEDATA1.Write(GAMEDATA_Add[i], 0, GAMEDATA_Add[i].Length);
                GAMEDATA1.Write(GAMEDATA_rest[i], 0, GAMEDATA_rest[i].Length);

                i++;

            }
            GAMEDATA1.Seek(0x00, SeekOrigin.Begin);
            byte[] start = new byte[0x04];
            start = BitConverter.GetBytes(l);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(start);
            }
            GAMEDATA1.Write(start, 0, 0x04);
            byte[] numfiles = new byte[0x04];
            numfiles = BitConverter.GetBytes(num);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(numfiles);
            }
            GAMEDATA1.Write(numfiles, 0, 0x04);
            GAMEDATA1.Close();

        }

        public void fix_crc(string pathtofile)
        {
            byte[] gamedatalen;
            Crc32 crc32 = new Crc32();
            String hash = String.Empty;
            using (FileStream fs = File.Open(pathtofile + "/GAMEDATA", FileMode.Open))
                foreach (byte b in crc32.ComputeHash(fs)) hash += b.ToString("x2").ToLower();
            
            FileStream METADATA = File.Open(pathtofile + "/METADATA", FileMode.Open);
            byte[] hash2 = StringToByteArray(hash);
            string t = FormatCRC32Result(hash2);
            byte[] hash3 = StringToByteArray(t);
            METADATA.Seek(0x0C, SeekOrigin.Begin);
            METADATA.Write(hash2, 0, 0x04);
            using (FileStream fs = File.Open(pathtofile + "/GAMEDATA", FileMode.Open))
                gamedatalen = BitConverter.GetBytes(fs.Length);
           Array.Reverse(gamedatalen);
           METADATA.Seek(0x04, SeekOrigin.Begin);
           METADATA.Write(gamedatalen, 4, 0x04);
           METADATA.Close();
        }

        public static string FormatCRC32Result(byte[] result)
        {
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(result);
            }
            return BitConverter.ToUInt32(result, 0).ToString("X8").ToLower();
        }

        #endregion

        #region PS3


        static byte[] key = new byte[] { 0xEE, 0xA9, 0x37, 0xCC, 0x5B, 0xD4, 0xD9, 0x0D, 0x55, 0xED, 0x25, 0x31, 0xFA, 0x33, 0xBD, 0xC4 };
           
        public static void ps3_Decrypt(string dirpath)
        {
            FileStream filelist = File.Open(dirpath + "/~files_decrypted_by_pfdtool.txt", FileMode.OpenOrCreate);
            
           //declare ps3 manager using the directory path, and the secure file id
            Ps3SaveManager manager = new Ps3SaveManager(dirpath, key);
            //get file entry using name
            Ps3File file = manager.Files.FirstOrDefault(t => t.PFDEntry.file_name == "METADATA");
            //define byte array that the decrypted data should be allocated
            byte[] filedata = null;
            //check if file is not null
            if (file != null)
            {
                byte[] filel = { 0x20, 0x4D, 0x45, 0x54, 0x41, 0x44, 0x41, 0x54, 0x41, 0x0D, 0x0A };//StringToByteArray(" METADATA");
                file.Decypt();
                //filedata = file.DecryptToBytes();
                filelist.Write(filel, 0, filel.Length);
            }
            if (filedata != null)
            {
                //do stuff with the decrypted data
            }
        }

        public static void ps3_Encrypt(string dirpath)
        {
            if (File.Exists(dirpath + "/~files_decrypted_by_pfdtool.txt"))
            {
                File.Delete(dirpath + "/~files_decrypted_by_pfdtool.txt");
            }
            //declare ps3 manager using the directory path, and the secure file id
            Ps3SaveManager manager = new Ps3SaveManager(dirpath, key);
            //get file entry using name
            Ps3File file = manager.Files.FirstOrDefault(t => t.PFDEntry.file_name == "METADATA");
            //define byte array that should be encrypted
            byte[] filedata = File.ReadAllBytes(dirpath + "/METADATA");
            //check if file is not null
            if (file != null)
            {
                
                //file.Encrypt(filedata); //Encrypt from byte[]
                //filedata = file.EncryptToBytes(); //Encrypt to memory (byte[])
                //FileStream METADATA = File.Open(dirpath + "/METADATA", FileMode.Open);
            
                //METADATA.Seek(0, SeekOrigin.Begin);
                //METADATA.Write(filedata, 0, filedata.Length);
                //METADATA.Close();
            }
        }

        public static void ps3_Rebuild(string dirpath)
        {
             //declare ps3 manager using the directory path, and the secure file id
            Ps3SaveManager manager = new Ps3SaveManager(dirpath, key);
            //manager.ReBuildChanges();//Rebuild param.pfd, and keeps any decrypted file decrypted.
            manager.ReBuildChanges(true);//Rebuild param.pfd, and encrypt any decrypted file.
            if (File.Exists(dirpath + "/~files_decrypted_by_pfdtool.txt"))
            {
                File.Delete(dirpath + "/~files_decrypted_by_pfdtool.txt");
            }
        }

        #endregion

        


    }
}
