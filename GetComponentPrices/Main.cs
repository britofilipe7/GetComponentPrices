using GetComponentPrices.Digikey;
using GetComponentPrices.Farnell;
using GetComponentPrices.Mouser;
using GetComponentPrices.TME;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using TextBox = System.Windows.Forms.TextBox;



namespace GetComponentPrices
{
    public partial class Main : Form
    {
        List<string> datasheetList = new List<string>();
        List<string> imageListURL = new List<string>();

        private string partNumber;
        private int quantity;

        public Main()
        {
            InitializeComponent();
        }

        //Clear the form 
        private void clear()
        {
            //Clear the textboxes (price, moq, lead time fields) except txtPartNumber and txtQuantity
            foreach (Control control in this.Controls)
            {
                if (control is TextBox && !control.Name.Equals("txtPartNumber") && !control.Name.Equals("txtQuantity"))
                {
                    ((TextBox)control).Text = String.Empty;
                    ((TextBox)control).BackColor = SystemColors.Window;
                }
            }

            //Clear images
            pictureBox.ImageLocation = null;
            picMouserWarning.Visible = false;
            picDigikeyWarning.Visible = false;
            picFarnellWarning.Visible = false;

            //clear linklabels links collection
            foreach (Control control in this.Controls)
            {
                if (control is LinkLabel)
                {
                    ((LinkLabel)control).LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
                    ((LinkLabel)control).Links.Clear();
                }
            }

            //Clear datasheets list
            if (datasheetList.Any()) datasheetList.Clear();

            //Clear the imageURL list
            if (imageListURL.Any()) imageListURL.Clear();

        }

        private void btnPrices_Click(object sender, EventArgs e)
        {
            
            clear();

            //check for valid part numbers
            partNumber = txtPartNumber.Text.Trim();
            if (partNumber.Equals(""))
            {
                MessageBox.Show("Insert a valid Part Number.", "Alert");
                return;
            }

            //check for valid quantities, has to be positive integer
            if (int.TryParse(txtQuantity.Text, out quantity) && !txtQuantity.Text.Equals("0"))
            {
                quantity = Convert.ToInt32(quantity);
            }
            else
            {
                txtQuantity.Clear();
                MessageBox.Show("Insert a positive integer Quantity", "Alert");
                return;
            }
            
            GetMouserPrices(partNumber, quantity);
            GetDigikeyPrices(partNumber, quantity);
            GetTMEPrices(partNumber, quantity);
            GetFarnellPrices(partNumber, quantity);

            //Displays the first image of the imageListURL
            if (imageListURL.Any())
            {
                pictureBox.ImageLocation = imageListURL[0];
                pictureBox.Refresh();                 
            }
        }

        private void GetMouserPrices(string partNumber, int quantity)
        {
            //Instanciates a mouser class
            Mouser.Mouser mouser = new Mouser.Mouser(partNumber, quantity);
            
            if (mouser.ExactResult == null)
            {
                return;
            }

            //Set the texbox values
            if (mouser.FinalPrice != -1) txtPricesMouser.Text = mouser.FinalPrice.ToString();
            txtStockMouser.Text = mouser.Stock.ToString();
            if (mouser.LeadTime != 0 && int.Parse(mouser.Stock) < quantity) txtLeadTimeMouser.Text = mouser.LeadTime.ToString();

            //Set stock textbox color
            txtStockMouser.BackColor = CheckTextBoxColorStock(txtStockMouser);

            //check if there's MOQs
            if (mouser.Moq != -1) txtMoqMouser.Text = mouser.Moq.ToString();

            //gets the product direct url
            if (mouser.ProductURL != null)
            {
                linklblMouser.Links.Add(0, linklblMouser.Text.Length, mouser.ProductURL);
                linklblMouser.LinkBehavior = System.Windows.Forms.LinkBehavior.SystemDefault;
            }
            
            //adds imageURL to imageURLList
            if (mouser.ImageURL != null)
            {
                imageListURL.Add(mouser.ImageURL);
            }

            //adds datasheet to datasheetList
            if (mouser.DatasheetURL != null)
            {
                datasheetList.Add(mouser.DatasheetURL);
            }

            if (mouser.HaveAlternativePacking)
            {
                picMouserWarning.Visible = true;
            }
        }

        private void GetDigikeyPrices(string partNumber, int quantity)
        {
            //Instanciates a mouser class
            Digikey.Digikey digikey = new Digikey.Digikey(partNumber, quantity);

            if (digikey.ExactResult == null)
            {
                return;
            }

            //Set the texbox values
            if (digikey.FinalPrice != -1) txtPricesDigikey.Text = digikey.FinalPrice.ToString();
            txtStockDigikey.Text = digikey.Stock.ToString();
            if (digikey.LeadTime != 0 && digikey.Stock < quantity) txtLeadTimeDigikey.Text = digikey.LeadTime.ToString();

            //Set stock textbox color
            txtStockDigikey.BackColor = CheckTextBoxColorStock(txtStockDigikey);

            //check if there's MOQs
            if (digikey.Moq != -1) txtMoqDigikey.Text = digikey.Moq.ToString();

            //gets the product direct url
            if (digikey.ProductURL != null)
            {
                linklblDigikey.Links.Add(0, linklblDigikey.Text.Length, digikey.ProductURL);
                linklblDigikey.LinkBehavior = System.Windows.Forms.LinkBehavior.SystemDefault;
            }

            //adds imageURL to imageURLList
            if (digikey.ImageURL != null)
            {
                imageListURL.Add(digikey.ImageURL);
            }

            //adds datasheet to datasheetList
            if (digikey.DatasheetURL != null)
            {
                datasheetList.Add(digikey.DatasheetURL);
            }

            if (digikey.HaveAlternativePacking)
            {
                picDigikeyWarning.Visible = true;
            }
        }

        private void GetTMEPrices(string partNumber, int quantity)
        {
            //Instanciates a mouser class
            TME.TME tme = new TME.TME(partNumber, quantity);
            
            if (tme.ExactResult == null)
            {
                return;
            }

            //Set the texbox values
            txtPricesTme.Text = tme.FinalPrice.ToString();
            txtStockTme.Text = tme.Stock.ToString();
            txtLeadTimeTme.Text = tme.LeadTime.ToString();
            if (!tme.LeadTime.Equals("") && tme.Stock < quantity) txtLeadTimeTme.Text = tme.LeadTime.ToString();

            //Set stock textbox color
            txtStockTme.BackColor = CheckTextBoxColorStock(txtStockTme);

            //check if there's MOQs
            if (tme.Moq != -1) txtMoqMouser.Text = tme.Moq.ToString();

            //gets the product direct url
            if (tme.ProductURL != null)
            {
                linklblTme.Links.Add(0, linklblTme.Text.Length, tme.ProductURL);
                linklblTme.LinkBehavior = System.Windows.Forms.LinkBehavior.SystemDefault;
            }

            /* Not used in TME
            //adds imageURL to imageURLList
            if (tme.ImageURL != null)
            {
                imageListURL.Add(tme.ImageURL);
            }*/

            /* Not used in TME
            //adds datasheet to datasheetList
            if (tme.DatasheetURL != null)
            {
                datasheetList.Add(tme.DatasheetURL);
            }

            if (tme.HaveAlternativePacking)
            {
                picMouserWarning.Visible = true;
            }*/
            
        }

        private void GetFarnellPrices(string partNumber, int quantity)
        {
            //Instanciates a farnell class
            Farnell.Farnell farnell = new Farnell.Farnell(partNumber, quantity);

            if (farnell.ExactResult == null)
            {
                return;
            }

            //Set the texbox values
            if (farnell.FinalPrice != -1) txtPricesFarnell.Text = farnell.FinalPrice.ToString();
            txtStockFarnell.Text = farnell.Stock.ToString();
            if (farnell.LeadTime != 0 && farnell.Stock < quantity) txtLeadTimeFarnell.Text = farnell.LeadTime.ToString();

            //Set stock textbox color
            txtStockFarnell.BackColor = CheckTextBoxColorStock(txtStockFarnell);

            //check if there's MOQs
            if (farnell.Moq != -1) txtMoqFarnell.Text = farnell.Moq.ToString();

            //gets the product direct url
            if (farnell.ProductURL != null)
            {
                linklblFarnell.Links.Add(0, linklblFarnell.Text.Length, farnell.ProductURL);
                linklblFarnell.LinkBehavior = System.Windows.Forms.LinkBehavior.SystemDefault;
            }

            /*
            //adds imageURL to imageURLList
            if (mouser.ImageURL != null)
            {
                imageListURL.Add(mouser.ImageURL);
            }*/

            //adds datasheet to datasheetList
            if (farnell.DatasheetURL != null)
            {
                datasheetList.Add(farnell.DatasheetURL);
            }

            if (farnell.HaveAlternativePacking)
            {
                picFarnellWarning.Visible = true;
            }
        }

        private Color CheckTextBoxColorStock(TextBox textBox)
        {
            if (textBox.Text.Equals(""))
            {
                return Color.White;
            }
            else if (int.Parse(textBox.Text) < quantity)
            {
                return Color.Red;
            }
            else
            {
                return Color.Green;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        //Events for clicking the linklabels
        private void linklblMouser_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (linklblMouser.Links.LinksAdded == true)
            {
                Process myProcess = new Process();
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = linklblMouser.Links[0].LinkData.ToString();
                myProcess.Start();
            }
            
        }

        private void linklblDigikey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (linklblDigikey.Links.LinksAdded == true)
            {
                Process myProcess = new Process();
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = linklblDigikey.Links[0].LinkData.ToString();
                myProcess.Start();
            }

        }

        private void linklblTme_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (linklblTme.Links.LinksAdded == true)
            {
                Process myProcess = new Process();
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = linklblTme.Links[0].LinkData.ToString();
                myProcess.Start();
            }

        }

        private void linklblFarnell_MouseClick(object sender, MouseEventArgs e)
        {
            if (linklblFarnell.Links.LinksAdded == true)
            {
                Process myProcess = new Process();
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = linklblTme.Links[0].LinkData.ToString();
                myProcess.Start();
            }
        }

        //Event for clicking datasheet button
        private void btnDatasheet_Click(object sender, EventArgs e)
        {
            if (datasheetList.Any())
            {
                Process myProcess = new Process();
                myProcess.StartInfo.UseShellExecute = true;
                //Open the first datasheet of the list
                myProcess.StartInfo.FileName = datasheetList[0];
                myProcess.Start();
            }
            else MessageBox.Show("No datasheets available");
        }
        
        private void picMouserWarning_MouseHover(object sender, EventArgs e)
        {
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(this.picMouserWarning, "This part number have an alternative packing");
        }

        private void picDigikeyWarning_MouseHover(object sender, EventArgs e)
        {
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(this.picDigikeyWarning, "This part number have an alternative packing");

        }

        private void picFarnellWarning_MouseHover(object sender, EventArgs e)
        {
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(this.picDigikeyWarning, "This part number have an alternative packing");

        }

        private void txtQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnPrices.PerformClick();
            }
        }
    }   
}
