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

public partial class Naplata : ContentPage
{


    public event EventHandler onCreateDocument;

    public string nazivTvrtke { get; set; }
    public string OIBTvrtke { get; set; }
    public string adresaTvrtke { get; set; }
    public string nazivKlijenta { get; set; } = "Placeholder Company";
    public string OIBklijenta { get; set; } = "11111111111";
    public string adresaKlijenta { get; set; } = "Zagreb 10000, Napuštenih snova BB";

    public Naplata()
    {
        InitializeComponent();
        this.BindingContext = new NaplataViewModel();
        nazivTvrtke = Preferences.Get("naziv_tvrtke", "");
        OIBTvrtke = Preferences.Get("OIBTvrtke", "");
        adresaTvrtke = Preferences.Get("adresaTvrtke", "");


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
        style.CharacterFormat.FontSize = 11f;
        style.CharacterFormat.Bold = true;
        style.ParagraphFormat.BeforeSpacing = 0;
        style.ParagraphFormat.AfterSpacing = 0;
        style.ParagraphFormat.LineSpacing = 13.8f;

        style = document.AddParagraphStyle("Bold") as WParagraphStyle;
        style.CharacterFormat.FontName = "Calibri";
        style.CharacterFormat.FontSize = 11f;
        style.ParagraphFormat.BeforeSpacing = 0;
        style.ParagraphFormat.AfterSpacing = 0;
        style.ParagraphFormat.LineSpacing = 13.8f;

        style = document.AddParagraphStyle("Heading 1") as WParagraphStyle;
        style.ApplyBaseStyle("Normal");
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
        picture.WidthScale = 26;
        picture.HeightScale = 24;
        paragraph.ApplyStyle("Heading 1");
        paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
        WTextRange textRange = paragraph.AppendText("\n \n \n_____________________ GLUIĆ NINČEVIĆ ODVJETNIČKO DRUŠTVO _____________________") as WTextRange;
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