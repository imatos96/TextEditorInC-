using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pr4_oop_2_dio
{
    public partial class Form1 : Form
    {
        Image img; //objekt tipa slika, potreban kasnije za učitavanje slike u dokument
        bool font = false, size = false; //provjera vrijednosti font i size za usklađivanje rada 2 comboxa font i size
        string filename = ""; //u njega ćemo spremati imena učitanih datoteka
        int filenameChar = 0;//brojač znakova u richtextBoxu
        public Form1()
        {
            InitializeComponent();
           

        }
        public void SelectAllFunc()
        {
            //funkcija za označivanje svih znakova u richtextBoxu
            richTextBox1.SelectAll();
        }
        public void fontSizeFunc() //potrebna je da se usklade promjene u fontu i size texta preko comboboxa
        {
            if (font)// ako je font true, znači da se u comboboxu promjenio font i taj font se postavlja na označeni text i za daljnu uporabu do sljedeće promjene
            {
                richTextBox1.SelectionFont = new Font(tSCBoxFont.Text, richTextBox1.Font.Size);
            }
            if (size)// ako je size true, znači da se u comboxu promjenio veličina slove i ta se veličine postavlja na označeni text i za daljnu uporabu do sljedeće promjene
            {
                richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, Convert.ToInt32(tSCBoxSize.SelectedItem), richTextBox1.Font.Style);
            }
            if (font && size)// ako je došlo do promjene i size i font, onda obje promjene primjenjujemo na text
            {
                richTextBox1.SelectionFont = new Font(tSCBoxFont.Text, Convert.ToInt32(tSCBoxSize.SelectedItem));
               
            }
        }
        public void newFunc()
        {
            //ako je duljina znakova u tekst boxu različita od duljine znakova prilikom otvaranja novog dokumenta ili ne postoji naslov dokumenta(riječ je o novoj datoteci)
            //onda se postavlja prikladna poruka i poziva upitnik za spremanje 
            if (richTextBox1.Text.Length!=filenameChar|| filename.Length==0)
            {
                string message = "Želite li spremiti datoteku prije zatvaranja?";
                saveUpitnik(message);
            }//inače, ako je ili spremljeno ili ništa nije izmjenjeno u postojećoj datoteci izbriše se sve (stvara se čisti prozor)
            else richTextBox1.Clear();
            filenameChar = 0; //reset vrijednosti znakova na 0 za novi dokument
        }
        public void fontFunc()
        {//otvaranje filedialoga za mijenjanje fonta označenog teksta
            if(fontDialog1.ShowDialog()==DialogResult.OK)
            {
                richTextBox1.SelectionFont = fontDialog1.Font;

            }
        }
        public void colorFunc()
        {//mijenja boju teksta unutar color dialoga prema zelji korisnika
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SelectionColor = colorDialog1.Color;
            }
        }
        public void markerFunc()
        {//mijenja boju markera(pozadine oznacenog teksta) unutar color dialoga prema zelji korisnika
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SelectionBackColor = colorDialog1.Color;
            }
        }
        public void saveFunc()
        {
            //filter za tip datoteke za spremanje - može se spremiti samo u obliku rtf
            
                saveFileDialog1.Filter = "RTF *.rtf|*.rtf";
                //spremanje datoteke unutar save file dialog-a
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    richTextBox1.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.RichText);
                }
            filenameChar = richTextBox1.Text.Length; //postavljanje vrijednosti znakova na trenutnu vrijednost-potrebno u slučaju otvaranja novog ili postojećeg dokumenta nakon spremanja kako se ne bi pojavio save upitinik ako nije došlo do promjene u periodu između save i open/new click-a

        }
        public void saveFuncNoDialog()
        {
            //slična funkcija za save kao i gore samo se provjerava postoji li ime dokumenta kako bi se omogućilo spremanje izmjena u postojeće dokumente
            if (filename.Length > 0)
            {
                richTextBox1.SaveFile(filename, RichTextBoxStreamType.RichText);

            }
            else saveFunc();
            filenameChar = richTextBox1.Text.Length; //postavljanje brojača  znakova na trenutnu vrijednost znakova


        }
        public void LeftFunc()
        {
            //postavlja poravnanje texta lijevo
            richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
        }
        public void RightFunc()
        {
            //postavlja poravnanje texta desno
            richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
        }
        public void CenterFunc()
        {
            //postavlja poravnanje texta centrirano
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
        }
        public void undoFunc()
        {
            //funkcija koja vraća zadnje stanje prije zadnje izmjene u richtextBox
            richTextBox1.Undo();
        }
        public void redoFunc()
        {
            //vraća posljednju izmjenu 
            richTextBox1.Redo();
        }
        public void cpyFunc()
        {//kopira označeni text
            richTextBox1.Copy();
        }
        public void cutFunc()
        {//izrezuje označeni text
            richTextBox1.Cut();
        }
        public void pasteFunc()
        {//lijepi kopirani ili izrezani text
            richTextBox1.Paste();
        }
        public void pageColorFunc()
        {//dialog za promjene boje pozadine pisanja
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.BackColor = colorDialog1.Color;
            }

        }
        public void saveUpitnik(string fileMessage)
        {//upitnik za spremanje datoteke
            //ako korisnik želi spremiti datoteku poziva se save funkcije, u suprotnom se poziva clear funkcija richtextboxa
            //clear funckija briše sve unutar prozora
            var rez = MessageBox.Show(fileMessage, "Editor Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (rez == DialogResult.Yes)
            {
                saveFunc();
            }
            else if (rez == DialogResult.No)
            {
                richTextBox1.Clear();
            }
        }
        public void imageLoadFunc()
        {//omogućava se učitavanje jpg, gif, bmp i png tipova slika u richtextBox
            openFileDialog1.Filter = "Image Files|*.jpg; *.jpeg; *.gif; *.bmp;*.png";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {//otvara se file dialog i bira se određena slika
                //budući da se slika rasprostani preko cijelog richtextboxa, mijenja se veličina iste - određeno na 100*100
                //može se povlačenjem slike ista povečati i smanjiti unutar samog programa
                //slikom se unutar richtextboxa manipulira kao s tekstom - putem tipkovnice
                img = Image.FromFile(openFileDialog1.FileName);
                Bitmap resizeImg = (new Bitmap(img, new Size(100, 100)));
                //slika se najprije sprema u među spremnik pa tek onda lijepi u richtextBox jer nema izravne funkcije
                Clipboard.SetImage(resizeImg);
                pasteFunc();
                richTextBox1.Focus();

            }
        }
        public void openFunc()
        {
            if (richTextBox1.Text.Length > 0 && richTextBox1.Text.Length!=filenameChar) saveUpitnik("Želite li spremiti datoteku prije otvaranja nove?");
            try
            {//otvaranje datoteke - filteri - mogu se otvoriti rtf, txt, cs, vb datoteke 
                openFileDialog1.Filter = "RTF|*.rtf|Text Files|*.txt|VB Files|*.vb|C# Files|*.cs|Sve datoteke|*.*";


                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {//u slučaju čisto txt, cs i vd datoteka čitat će se streamreaderom
                    string extension = System.IO.Path.GetExtension(openFileDialog1.FileName);
                    if (extension == ".txt" || extension == ".cs" || extension == ".vb")
                    {
                        this.Text = openFileDialog1.FileName;
                        System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
                        richTextBox1.Text = sr.ReadToEnd();
                        sr.Close();
                    }//u slučaju rtf datoteke otvara se prkeo Loadfile funkcije kako bi se učitao puni text(boje, fontovi) i slike
                    else if (extension == ".rtf") richTextBox1.LoadFile(openFileDialog1.FileName);
                }
                filename = openFileDialog1.FileName; //spremanje naziva otvorene datoteke u string varijablu
                filenameChar= richTextBox1.Text.Length;//spremanje veličine otvorene datoteke u int varijablu(brojač)
                lblCharNum.Text = filenameChar.ToString();//ispis trenutnog stanja znakova u labeli desno iznad richtextboxa)
            }
            catch (Exception)
            {//hvata se iznimka u slučaju označavanja datoteke koja se ne može otvoriti gore predviđenim kodom
                MessageBox.Show("Datoteka se ne može učitati.");
                var res=MessageBox.Show("Želite li ponovno pokušati otvoriti drugu datoteku?", "Upitnik", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes) openFunc();
            }

        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //svaki put kada dođe do promjene u richtextboxu ispisuje se trenutno stanje znakova
            lblCharNum.Text = richTextBox1.Text.Length.ToString();
        }


        //u ispod navedenim eventima se pozivaju gore navedene funckije kako je prikladno za svaki button ili stripmenu
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFunc();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            saveFunc();
        }
        
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message= "Želite li spremiti datoteku prije zatvaranja?";
            saveUpitnik(message);
            
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {

            richTextBox1.Cut();

        }
      
        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            openFunc();
        }
        
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            saveFuncNoDialog();
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            newFunc();
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            cpyFunc();
        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            pasteFunc();
        }

       
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            imageLoadFunc();
        }
      
        private void tStripBtnColor_Click(object sender, EventArgs e)
        {
            colorFunc();
        }

        private void tstripBtnfont_Click(object sender, EventArgs e)
        {
            fontFunc();
        }

        private void tStripBtnColor_Click_1(object sender, EventArgs e)
        {
            colorFunc();
        }

        private void tStripBtnMarker_Click(object sender, EventArgs e)
        {
            markerFunc();
        }

        private void tSBtnUndo_Click(object sender, EventArgs e)
        {
            undoFunc();
        }

        private void tSBtnRedo_Click(object sender, EventArgs e)
        {
            redoFunc();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            undoFunc();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            redoFunc();
        }

        private void changeFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontFunc();
        }

        private void fontColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorFunc();
        }

        private void penColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            markerFunc();
        }

        private void leftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LeftFunc();
        }

        private void centeredToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CenterFunc();
        }

        private void rightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RightFunc();
        }

        private void insertAPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageLoadFunc();
        }

        private void tStripBtnLeftALl_Click(object sender, EventArgs e)
        {
            LeftFunc();

        }

        private void tStripBtnCenterALi_Click(object sender, EventArgs e)
        {
            CenterFunc();
        }

        private void tStripBtnLeftAli_Click(object sender, EventArgs e)
        {
            RightFunc();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newFunc();
        }

        private void pageColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pageColorFunc();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            pageColorFunc();
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            filenameChar = 0;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectAllFunc();
        }

        private void tSBtnSelectAll_Click(object sender, EventArgs e)
        {
            SelectAllFunc();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //foreach petlja kojima se dohvaćaju svi fontovi iz familije fontova i ispisuju u combo box
            
            foreach (FontFamily font in FontFamily.Families)
            {
                tSCBoxFont.Items.Add(font.Name);
            }
            //brojevi od 4 do 81 se ispisuju u combobox i služit će za manipulaciju fontom iz comboboxa
            for (int i = 4; i < 81; i+=2)
            {
                tSCBoxSize.Items.Add(i);
            }
            tSCBoxFont.Text = this.richTextBox1.Font.Name.ToString();//ispis trenutnog fonta u vidljivi dio comboboxa(bez spuštene trake)
            tSCBoxSize.Text = this.richTextBox1.Font.Size.ToString();//ispis trenutne veličine u vidljivi dio comboboxa(bez spuštene trake)

            richTextBox1.Focus(); //postavlja ulazni fokus na kontrolu - postavlja kursor na richtextbox
        }

        private void tSCBoxSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            size = true;//postavlja se promjena varijabla koja pamti promjenu size u comboboxu u true
            fontSizeFunc();
        }

        private void savenoAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFuncNoDialog();
        }

        private void tSCBoxFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            font = true; //postavlja se promjena varijabla koja pamti promjenu fonta u comboboxu u true
            fontSizeFunc();
        }

    }
   }

