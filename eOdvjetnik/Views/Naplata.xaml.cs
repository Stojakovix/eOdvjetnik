namespace eOdvjetnik.Views;
using Syncfusion.Maui.DataForm;
using Syncfusion.Maui.Core;
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

public partial class Naplata : ContentPage
{
    public ObservableCollection<ReceiptItem> ReceiptItems  = new ObservableCollection<ReceiptItem>();
    private NaplataViewModel ViewModel;

    public event EventHandler onCreateDocument;

    public string CompanyName { get; set; }
    public string CompanyOIB { get; set; }
    public string CompanyAddress { get; set; }
    public string ClientName { get; set; } = "Klijent nije odabran";
    public string ClientOIB { get; set; } = "Nema OIB-a";
    public string ClientAddress { get; set; } = "Nema adrese";
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



    public Naplata()
    {
        InitializeComponent();
        ViewModel = new NaplataViewModel();
        this.BindingContext = ViewModel;
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

        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
        textRange = paragraph.AppendText("_____________________________________________________________________________________") as WTextRange;

        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Bold");
        paragraph.BreakCharacterFormat.FontSize = 12f;
        textRange = paragraph.AppendText("Odvjetnička usluga\tTarifa\tCijena\tPDV iznos\tIznos sa PDV") as WTextRange;

        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
        textRange = paragraph.AppendText("_____________________________________________________________________________________") as WTextRange;


        ObservableCollection<ReceiptItem> receiptItems = ViewModel.ReceiptItems;

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
                string AmountBeforePDV = amountBeforePDV.ToString("0.00");

                float PDVamount = (ReceiptItemAmount * 1.25f) - ReceiptItemAmount;
                string PDVAmount = PDVamount.ToString("0.00");

                float amountAfterPDV = (ReceiptItemAmount*1.25f);
                string AmountAfterPDV = amountAfterPDV.ToString("0.00");


                paragraph = section.AddParagraph();
                paragraph.ApplyStyle("Normal");
                paragraph.BreakCharacterFormat.FontSize = 12f;

                int indexNumber = i + 1; // Get the index number (starting from 1)
                textRange = paragraph.AppendText(indexNumber + ". " + ReceiptItemParentName + "\t" + ReceiptItemTBR + " (" + ReceiptItemPoints + " bodova)\t" + AmountBeforePDV + "\t" + PDVAmount + "\t" + AmountAfterPDV) as WTextRange;
            }
            else
            {
                if (item == null)
                {
                    Debug.WriteLine("Item is null");
                }
            }
        }

        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
        textRange = paragraph.AppendText("_____________________________________________________________________________________") as WTextRange;

        float totalAmountBeforePDV = (float)Math.Round(ReceiptItemTotalAmount, 2);
        string TotalAmountBeforePDV = totalAmountBeforePDV.ToString("0.00");

        float totalPDVamount = (ReceiptItemTotalAmount * 1.25f) - ReceiptItemTotalAmount;
        string TotalPDVAmount = totalPDVamount.ToString("0.00");

        float totalAmountAfterPDV = (ReceiptItemTotalAmount * 1.25f);
        string TotalAmountAfterPDV = totalAmountAfterPDV.ToString("N2");

        float totalAmountAfterPDVhrk = ((ReceiptItemTotalAmount * 1.25f)* 7.5345f);
        string TotalAmountAfterPDVhrk = totalAmountAfterPDVhrk.ToString("N2");

        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal");
        paragraph.BreakCharacterFormat.FontSize = 12f;
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Right;
        textRange = paragraph.AppendText( "\t\t\t\t" + TotalAmountBeforePDV + "\t\t"+ TotalPDVAmount+ "\t\t" + TotalAmountAfterPDV + " EUR\n" + TotalAmountAfterPDVhrk + " kn") as WTextRange;

       
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
   
 

    protected override void OnAppearing()
    {
        base.OnAppearing();
        this.Window.MinimumHeight = 620;
        this.Window.MinimumWidth = 860;
    }
    public double MinWidth { get; set; }

    private async void ListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
            return;

        var selectedTariffItem = (TariffItem)e.SelectedItem;

        await SaveToPreferences(selectedTariffItem.oznaka, selectedTariffItem.bodovi, selectedTariffItem.concatenated_name, selectedTariffItem.parent_name);

        ((ListView)sender).SelectedItem = null;
    }



    private Task SaveToPreferences(string oznaka, string bodovi, string concatenatedName, string parent_name)
    {
 
        Preferences.Set("SelectedOznaka", oznaka);
        Preferences.Set("SelectedBodovi", bodovi);
        Preferences.Set("SelectedConcatenatedName", concatenatedName);
        Preferences.Set("SelectedParentName", parent_name);
        Debug.WriteLine("parent name je: " + parent_name);


        return Task.CompletedTask;
    }

}