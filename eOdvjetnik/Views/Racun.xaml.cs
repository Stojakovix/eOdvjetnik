namespace eOdvjetnik.Views;
using System;
using Syncfusion.DocIO;
using eOdvjetnik.Services;
using Syncfusion.DocIO.DLS;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using eOdvjetnik.ViewModel;
using Microsoft.Maui.Controls;
using eOdvjetnik.Models;
using System.Collections.ObjectModel;


public partial class Racun : ContentPage
{
    public ObservableCollection<ReceiptItem> ReceiptItems = new ObservableCollection<ReceiptItem>();


    public event EventHandler onCreateDocument;

    public string CompanyName { get; set; }
    public string CompanyOIB { get; set; }
    public string CompanyAddress { get; set; }
    public string ClientName { get; set; } = "Klijent nije odabran";
    public string ClientOIB { get; set; } = "OIB";
    public string ClientAddress { get; set; } = "Adresa";
    string CurrentDate { get; set; }
    string PaymentDate { get; set; }
    public string ReceiptItemName { get; set; }
    public string ReceiptItemPoints { get; set; }

    public string ReceiptItemParentName { get; set; }
    public string ReceiptItemTBR { get; set; }
    public string ReceiptItemAmountEUR { get; set; }
    public int ReceiptItemCount { get; set; }
    public float ReceiptItemAmount { get; set; }
    public float ReceiptItemTotalAmount { get; set; }
    public string IndexNumber { get; set; }
    public string AmountBeforePDV { get; set; }
    public string PDVAmount { get; set; }
    public string AmountAfterPDV { get; set; }

    public Racun()
	{
        InitializeComponent();
        this.BindingContext = App.SharedNaplataViewModel;
        CompanyName = Preferences.Get("naziv_tvrtke", "");
        CompanyOIB = Preferences.Get("OIBTvrtke", "");
        CompanyAddress = Preferences.Get("adresaTvrtke", "");
        CurrentDate = DateTime.Now.ToString("dd.MM.yyyy.");
        DateTime dateTime = DateTime.Now;
        dateTime = dateTime.AddDays(7);
        PaymentDate = dateTime.ToString("dd.MM.yyyy");
        ClientName = Preferences.Get("SelectedName", "");
        ClientOIB = Preferences.Get("SelectedOIB", "");
        ClientAddress = Preferences.Get("SelectedAddress", "");

    }

    private void CreateDocument(object sender, EventArgs e)
    {
        //Creates a new document.
        using WordDocument document = new();
        //Adds a new section to the document.
        WSection section = document.AddSection() as WSection;
        //Sets Margin of the section.
        section.PageSetup.Margins.All = 72;
        //Sets the page size of the section.
        section.PageSetup.PageSize = new Syncfusion.Drawing.SizeF(612, 792);

        //Creates Paragraph styles.
        WParagraphStyle style = document.AddParagraphStyle("Normal") as WParagraphStyle;
        style.CharacterFormat.FontName = "Calibri";
        style.CharacterFormat.FontSize = 10f;
        style.ParagraphFormat.BeforeSpacing = 0;
        style.ParagraphFormat.AfterSpacing = 0;
        style.ParagraphFormat.LineSpacing = 18f;

        style = document.AddParagraphStyle("Normal2") as WParagraphStyle;
        style.CharacterFormat.FontName = "Calibri";
        style.CharacterFormat.FontSize = 10f;
        style.ParagraphFormat.BeforeSpacing = 0;
        style.ParagraphFormat.AfterSpacing = 0;
        style.ParagraphFormat.LineSpacing = 12f; //manji spacing

        style = document.AddParagraphStyle("Bold") as WParagraphStyle;
        style.CharacterFormat.FontName = "Calibri";
        style.CharacterFormat.Bold = true;
        style.CharacterFormat.FontSize = 10f;
        style.ParagraphFormat.BeforeSpacing = 0;
        style.ParagraphFormat.AfterSpacing = 0;
        style.ParagraphFormat.LineSpacing = 12f;

        style = document.AddParagraphStyle("Heading 1") as WParagraphStyle;
        style.CharacterFormat.FontName = "Calibri Light";
        style.CharacterFormat.FontSize = 16f;
        style.CharacterFormat.TextColor = Syncfusion.Drawing.Color.FromArgb(46, 116, 181);
        style.ParagraphFormat.BeforeSpacing = 0;
        style.ParagraphFormat.AfterSpacing = 0;
        style.ParagraphFormat.Keep = true;
        style.ParagraphFormat.KeepFollow = true;
        style.ParagraphFormat.OutlineLevel = OutlineLevel.Level1;
        style.CharacterFormat.Bold = false;
        IWParagraph paragraph = section.HeadersFooters.Header.AddParagraph();

        Assembly assembly = typeof(MainPage).GetTypeInfo().Assembly;
        string resourcePath = "eOdvjetnik.Resources.DocIO.logoNincevic.png";
        //Gets the image stream.
        Stream imageStream = assembly.GetManifestResourceStream(resourcePath);
        IWPicture picture = paragraph.AppendPicture(imageStream);
        picture.TextWrappingStyle = TextWrappingStyle.InFrontOfText;
        picture.HorizontalAlignment = ShapeHorizontalAlignment.Center;
        picture.VerticalOrigin = VerticalOrigin.Margin;
        picture.VerticalPosition = -100;
        // 1 cm = 28.35f
        picture.Width = 53.8f;
        picture.Height = 80.8f;
        paragraph.ApplyStyle("Heading 1");
        paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
        WTextRange textRange = paragraph.AppendText("\n\n_____________________ GLUIĆ NINČEVIĆ ODVJETNIČKO DRUŠTVO _____________________") as WTextRange;
        textRange.CharacterFormat.FontSize = 12f;
        textRange.CharacterFormat.FontName = "Calibri";
        textRange.CharacterFormat.TextColor = Syncfusion.Drawing.Color.Black;

        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Bold");
        style.ParagraphFormat.BeforeSpacing = 12;
        textRange = paragraph.AppendText("\n" + CompanyName + "\n" + CompanyAddress + "\nOIB: " + CompanyOIB) as WTextRange;

        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Bold");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Right;
        textRange = paragraph.AppendText(ClientName + "\n" + ClientAddress + "\nOIB: " + ClientOIB) as WTextRange;

        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
        textRange = paragraph.AppendText("Broj računa: 123/5") as WTextRange;


        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal");
        paragraph.BreakCharacterFormat.FontSize = 12f;
        textRange = paragraph.AppendText("Datum računa\t\tMjesto izdavanja\t\tDospijeće računa") as WTextRange;

        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal");
        paragraph.BreakCharacterFormat.FontSize = 12f;
        textRange = paragraph.AppendText(CurrentDate + "\t\tZadar\t\t\t" + PaymentDate + "\n") as WTextRange;


        
        //////////////////// Tablica

        //Appends the table.
        IWTable table = section.AddTable();
        table.ResetCells(2, 5);
        table.TableFormat.Borders.BorderType = BorderStyle.None;
        table.TableFormat.IsAutoResized = false;

        
        float[] columnWidths = new float[] { 170, 80, 60, 70, 80 };

    
        for (int i = 0; i < table.Rows[0].Cells.Count; i++)
        {
            table[0, i].Width = columnWidths[i];
            table[0, i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
        }

        for (int i = 0; i < table.Rows[1].Cells.Count; i++)
        {
            table[1, i].Width = columnWidths[i];
            table[1, i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
        }

        //Zaglavlje računa
        //Appends the paragraph.
        paragraph = table[0, 0].AddParagraph();
        paragraph.ApplyStyle("Bold");
        textRange = paragraph.AppendText("Odvjetnička usluga") as WTextRange;
      

        //Appends the paragraph.
        paragraph = table[0, 1].AddParagraph();
        paragraph.ApplyStyle("Bold");
        textRange = paragraph.AppendText("Tarifa") as WTextRange;
      

        //Appends the paragraph.
        paragraph = table[0, 2].AddParagraph();
        paragraph.ApplyStyle("Bold");
        textRange = paragraph.AppendText("Cijena") as WTextRange;
      

        //Appends the paragraph.
        paragraph = table[0, 3].AddParagraph();
        paragraph.ApplyStyle("Bold");
        textRange = paragraph.AppendText("PDV iznos") as WTextRange;
      

        //Appends the paragraph.
        paragraph = table[0, 4].AddParagraph();
        paragraph.ApplyStyle("Bold");
        textRange = paragraph.AppendText("Iznos s PDV-om") as WTextRange;



        ///////// Stavke računa
        ///
        ObservableCollection<ReceiptItem> receiptItems = App.SharedNaplataViewModel.ReceiptItems;

        Debug.WriteLine("Entering try block: " + receiptItems);

        for (int i = 0; i < receiptItems.Count; i++)
        {
            ReceiptItem item = receiptItems[i];

            if (item != null)
            {
                ReceiptItemCount = receiptItems.Count;
                ReceiptItemName = item.Name;
                ReceiptItemTBR = item.Tbr;
                ReceiptItemAmountEUR = item.Currency;
                ReceiptItemParentName = item.ParentName;
                ReceiptItemAmount = item.Amount;
                ReceiptItemTotalAmount = item.TotalAmount;
                ReceiptItemPoints = item.Points;
                Debug.WriteLine(item.ParentName, item.Tbr);

                float amountBeforePDV = (float)Math.Round(ReceiptItemAmount, 2);
                AmountBeforePDV = amountBeforePDV.ToString("0.00");

                float PDVamount = (ReceiptItemAmount * 1.25f) - ReceiptItemAmount;
                PDVAmount = PDVamount.ToString("0.00");

                float amountAfterPDV = (ReceiptItemAmount * 1.25f);
                AmountAfterPDV = amountAfterPDV.ToString("0.00");


                paragraph = section.AddParagraph();
                paragraph.ApplyStyle("Normal");
                paragraph.BreakCharacterFormat.FontSize = 12f;

                int indexNumber = i + 1; // Get the index number (starting from 1)
                IndexNumber = indexNumber.ToString();

              

                //Appends the paragraph.
                paragraph = table[1, 0].AddParagraph();
                paragraph.ApplyStyle("Normal");
                textRange = paragraph.AppendText(IndexNumber + ". " + ReceiptItemParentName) as WTextRange;

                paragraph = table[1, 0].AddParagraph();
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.ParagraphFormat.LineSpacing = 12f;
                paragraph.BreakCharacterFormat.FontSize = 12f;

                //Appends the paragraph.
                paragraph = table[1, 1].AddParagraph();
                paragraph.ApplyStyle("Normal");
                textRange = paragraph.AppendText(ReceiptItemTBR + " (" + ReceiptItemPoints + " bodova)") as WTextRange;


                paragraph = table[1, 1].AddParagraph();
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.ParagraphFormat.LineSpacing = 12f;
                paragraph.BreakCharacterFormat.FontSize = 12f;

                //Appends the paragraph.
                paragraph = table[1, 2].AddParagraph();
                paragraph.ApplyStyle("Normal");
                textRange = paragraph.AppendText(AmountBeforePDV) as WTextRange;

                paragraph = table[1, 3].AddParagraph();
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.ParagraphFormat.LineSpacing = 12f;
                paragraph.BreakCharacterFormat.FontSize = 12f;

                //Appends the paragraph.
                paragraph = table[1, 3].AddParagraph();
                paragraph.ApplyStyle("Normal");
                textRange = paragraph.AppendText(PDVAmount) as WTextRange;

                paragraph = table[1, 4].AddParagraph();
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.ParagraphFormat.LineSpacing = 12f;
                paragraph.BreakCharacterFormat.FontSize = 12f;

                //Appends the paragraph.
                paragraph = table[1, 4].AddParagraph();
                paragraph.ApplyStyle("Normal");
                textRange = paragraph.AppendText(AmountAfterPDV) as WTextRange;


                paragraph = table[1, 2].AddParagraph();
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.ParagraphFormat.LineSpacing = 12f;
                paragraph.BreakCharacterFormat.FontSize = 12f;
            }
            else
            {
                if (item == null)
                {
                    Debug.WriteLine("Item is null");
                }
            }
        }

        
        ////////////////////

      

        float totalAmountBeforePDV = (float)Math.Round(ReceiptItemTotalAmount, 2);
        string TotalAmountBeforePDV = totalAmountBeforePDV.ToString("0.00");

        float totalPDVamount = (ReceiptItemTotalAmount * 1.25f) - ReceiptItemTotalAmount;
        string TotalPDVAmount = totalPDVamount.ToString("0.00");

        float totalAmountAfterPDV = (ReceiptItemTotalAmount * 1.25f);
        string TotalAmountAfterPDV = totalAmountAfterPDV.ToString("N2");

        float totalAmountAfterPDVhrk = ((ReceiptItemTotalAmount * 1.25f) * 7.5345f);
        string TotalAmountAfterPDVhrk = totalAmountAfterPDVhrk.ToString("N2");

        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal");
        paragraph.BreakCharacterFormat.FontSize = 12f;
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Right;
        textRange = paragraph.AppendText("\t\t\t\t" + TotalAmountBeforePDV + "\t\t" + TotalPDVAmount + "\t\t" + TotalAmountAfterPDV + " EUR\n" + TotalAmountAfterPDVhrk + " kn") as WTextRange;


        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal2");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
        textRange = paragraph.AppendText("Konverzija u euro izvršena po fiksnom tečaju konverzije: 7,53450 HRK = 1 EUR.") as WTextRange;

        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
        textRange = paragraph.AppendText("Obračun prema naplaćenoj naknadi") as WTextRange;

        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal2");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
        textRange = paragraph.AppendText("Upozorenje:\nU slučaju neispunjenja dospjele obveze po ovom računu vjerovnik je ovlašten zatražiti određivanje ovrhe na temelju vjerodostojne isprave") as WTextRange;

        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal2");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
        textRange = paragraph.AppendText("\nNačin plaćanja: \tTRANSAKCIJSKI RAČUN\nIBAN: \t\tHR76 234 000 9111 077 0402\nSWIFT CODE: \tPBZGHR2X\nModel: \t\t00\nPoziv na broj:\t303-2-2") as WTextRange;

        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal2");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Right;
        textRange = paragraph.AppendText("Ante Gluić, odvjetnik") as WTextRange;

        //Footer
        paragraph = section.HeadersFooters.Footer.AddParagraph();
        paragraph.ApplyStyle("Normal2");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Justify;
        textRange = paragraph.AppendText("GLUIĆ NINČEVIĆ ODVJETNIČKO DRUŠTVO D.O.O. Špire Brusine 16, HR-2300 Zadar, tel: +385 23 700 750 fax: +385 23700751, ured@gn.hr, www.gn.hr, OIB: 75663620109, PBZ D.D. IBAN: HR7623400091110770402 MBS: 110057936, Trgovački sud u Zadru, Temeljni kapital u iznosu od 350 000,00 HKR uplaćen u cijelosti") as WTextRange;
        textRange.CharacterFormat.FontSize = 10f;
        textRange.CharacterFormat.FontName = "Calibri";
        textRange.CharacterFormat.TextColor = Syncfusion.Drawing.Color.Black;

        using MemoryStream ms = new();
        //Saves the Word document to the memory stream.
        document.Save(ms, FormatType.Docx);
        ms.Position = 0;
        //Saves the memory stream as file.
        SaveService saveService = new();
        saveService.SaveAndView("Sample.docx", "application/msword", ms);
    }

}