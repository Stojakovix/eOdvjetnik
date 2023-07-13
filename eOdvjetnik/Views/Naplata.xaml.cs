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
    private NaplataViewModel viewModel;

    public event EventHandler onCreateDocument;

    public string nazivTvrtke { get; set; }
    public string OIBTvrtke { get; set; }
    public string adresaTvrtke { get; set; }
    public string nazivKlijenta { get; set; } = "Placeholder Company";
    public string OIBklijenta { get; set; } = "11111111111";
    public string adresaKlijenta { get; set; } = "Zagreb 10000, Radnička 1";
    string current_date { get; set; }
    string payment_date { get; set; }

    public string imeUsluge { get; set; }
    public string tbr { get; set; }
    public string points { get; set; }

    public Naplata()
    {
        InitializeComponent();
        viewModel = new NaplataViewModel();
        this.BindingContext = viewModel;
        nazivTvrtke = Preferences.Get("naziv_tvrtke", "");
        OIBTvrtke = Preferences.Get("OIBTvrtke", "");
        adresaTvrtke = Preferences.Get("adresaTvrtke", "");
        current_date = DateTime.Now.ToString("dd.MM.yyyy.");
        DateTime dateTime = DateTime.Now;
        dateTime = dateTime.AddDays(7);
        payment_date = dateTime.ToString("dd.MM.yyyy");
        

    }
    public void getReceiptData()
    {
        try
        {
            ObservableCollection<ReceiptItem> receiptItems = viewModel.ReceiptItems;

            Debug.WriteLine("ulazak u try" + receiptItems);
            foreach (ReceiptItem item in receiptItems)
            {
                if (item != null)
                {
                    imeUsluge = item.Name;
                    tbr = item.Tbr;
                    points = item.Points;
                    Debug.WriteLine("name - " + item.Name, "tbr" + item.Tbr, "cijena" + item.Points);
                }
                else
                {
                    if(item == null)
                    {
                        Debug.WriteLine("item is null");
                    }
                }

            }
        }
        catch (Exception ex)
        {

            Debug.WriteLine(ex.Message);
        }
    }
    private void CreateDocument(object sender, EventArgs e)
    {
        getReceiptData();
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
        textRange = paragraph.AppendText("\n" + nazivTvrtke + "\n" + adresaTvrtke + "\nOIB: " + OIBTvrtke) as WTextRange;
         
        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Bold");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Right;
        textRange = paragraph.AppendText(nazivKlijenta + "\n" + adresaKlijenta + "\nOIB: " + OIBklijenta) as WTextRange;

        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
        textRange = paragraph.AppendText("Broj računa: 123/5") as WTextRange;


        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal");
        paragraph.BreakCharacterFormat.FontSize = 12f;
        textRange = paragraph.AppendText("Datum računa\t\tMjesto izdavanja\tDospijeće računa") as WTextRange;

        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal");
        paragraph.BreakCharacterFormat.FontSize = 12f;
        textRange = paragraph.AppendText(current_date + "\t\tZadar\t\t\t" + payment_date + "\n") as WTextRange;

        //Tablica za stavke računa // binding za item.Count u tarrifitem

        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
        textRange = paragraph.AppendText("_____________________________________________________________________________________") as WTextRange;

        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal");
        paragraph.BreakCharacterFormat.FontSize = 12f;
        textRange = paragraph.AppendText(imeUsluge + "\t\tTarifa\t\tCijena\t\tPDV iznos\t\tIznos sa PDV") as WTextRange;

        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
        textRange = paragraph.AppendText("_____________________________________________________________________________________") as WTextRange;

        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal");
        paragraph.BreakCharacterFormat.FontSize = 12f;
        textRange = paragraph.AppendText(tbr + "\t\tOpis tarife\t\t500,00\t\t125,00\t\t\t625,00 ") as WTextRange;

        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
        textRange = paragraph.AppendText("_____________________________________________________________________________________") as WTextRange;

        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal");
        paragraph.BreakCharacterFormat.FontSize = 12f;
        textRange = paragraph.AppendText("\t\t\t\t\t\t" + points + "\t\t125,00\t\t\t625,00 EUR") as WTextRange;

        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal");
        paragraph.BreakCharacterFormat.FontSize = 12f;
        textRange = paragraph.AppendText("\t\t\t\t\t\t\t\t\t\t\t4.709,06 kn") as WTextRange;

        //Tablica za PDV

        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal2");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
        textRange = paragraph.AppendText("\n\tPDV %\t\tIznos\t\tPDV iznos\tIznos s PDV ") as WTextRange;

        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
        textRange = paragraph.AppendText("_____________________________________________________________") as WTextRange;

        //Appends the paragraph. //povući ukupnu sumu iz tarrifitem
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal2");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
        textRange = paragraph.AppendText("\t25,00\t\t500,00\t\t125,00\t\t625,00 ") as WTextRange;

        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
        textRange = paragraph.AppendText("_____________________________________________________________") as WTextRange;

        //Appends the paragraph. //povući ukupnu sumu iz tarrifitem
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
        textRange = paragraph.AppendText("\t\t\t500,00\t\t125,00\t\t625,00 EUR") as WTextRange;

        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
        textRange = paragraph.AppendText("\t\t\t\t\t\t\t4.709,06 kn") as WTextRange;

        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal2");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
        textRange = paragraph.AppendText("Konverzija u euro izvršena po fiksnom tečaju konverzije: 7,53450 HRK = 1 EUR.") as WTextRange;

        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal2");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
        textRange = paragraph.AppendText("Obračun prema naplaćenoj naknadi") as WTextRange;

        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Normal2");
        paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
        textRange = paragraph.AppendText("Upozorenje:\tU slučaju neispunjenja dospjele obveze po ovom računu vjerovnik je ovlašten zatražiti \t\todređivanje ovrhe na temelju vjerodostojne isprave") as WTextRange;

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

        await SaveToPreferences(selectedTariffItem.oznaka, selectedTariffItem.bodovi, selectedTariffItem.concatenated_name);

        ((ListView)sender).SelectedItem = null;
    }



    private Task SaveToPreferences(string oznaka, string bodovi, string concatenatedName)
    {
 
        Preferences.Set("SelectedOznaka", oznaka);
        Preferences.Set("SelectedBodovi", bodovi);
        Preferences.Set("SelectedConcatenatedName", concatenatedName);

        return Task.CompletedTask;
    }

}