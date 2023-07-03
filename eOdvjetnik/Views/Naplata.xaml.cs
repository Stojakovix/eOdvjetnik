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


    public Naplata()
    {
        InitializeComponent();
        this.BindingContext = new NaplataViewModel();

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
        style.ParagraphFormat.BeforeSpacing = 0;
        style.ParagraphFormat.AfterSpacing = 8;
        style.ParagraphFormat.LineSpacing = 13.8f;

        style = document.AddParagraphStyle("Heading 1") as WParagraphStyle;
        style.ApplyBaseStyle("Normal");
        style.CharacterFormat.FontName = "Calibri Light";
        style.CharacterFormat.FontSize = 16f;
        style.CharacterFormat.TextColor = Syncfusion.Drawing.Color.FromArgb(46, 116, 181);
        style.ParagraphFormat.BeforeSpacing = 12;
        style.ParagraphFormat.AfterSpacing = 0;
        style.ParagraphFormat.Keep = true;
        style.ParagraphFormat.KeepFollow = true;
        style.ParagraphFormat.OutlineLevel = OutlineLevel.Level1;
        IWParagraph paragraph = section.HeadersFooters.Header.AddParagraph();

        Assembly assembly = typeof(MainPage).GetTypeInfo().Assembly;
        string resourcePath = "eOdvjetnik.Resources.DocIO.AdventureCycle.jpg";
        //Gets the image stream.
        Stream imageStream = assembly.GetManifestResourceStream(resourcePath);
        if (imageStream != null)
        {
            Debug.WriteLine("Radiiiiiiiiiiiiiiiiiiiiiiii Nije prazno");
        }
        else
        {
            Debug.WriteLine("prazno -.-");
        }
        IWPicture picture = paragraph.AppendPicture(imageStream);
        picture.TextWrappingStyle = TextWrappingStyle.InFrontOfText;
        picture.VerticalOrigin = VerticalOrigin.Margin;
        picture.VerticalPosition = -45;
        picture.HorizontalOrigin = HorizontalOrigin.Column;
        picture.HorizontalPosition = 263.5f;
        picture.WidthScale = 20;
        picture.HeightScale = 15;
        paragraph.ApplyStyle("Normal");
        paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Left;
        WTextRange textRange = paragraph.AppendText("GLUIĆ NINČEVIĆ odvjetničko društvo d.o.o.") as WTextRange;
        paragraph = section.AddParagraph();
        textRange = paragraph.AppendText("OIB:75663620109") as WTextRange;
        paragraph = section.AddParagraph();
        textRange = paragraph.AppendText("Adresa: Zadar, Špire Brusine 16") as WTextRange;
        paragraph = section.AddParagraph();
        textRange.CharacterFormat.FontSize = 12f;
        textRange.CharacterFormat.FontName = "Calibri";
        //textRange.CharacterFormat.TextColor = Syncfusion.Drawing.Color.Red;

        //Appends the paragraph.
        paragraph = section.AddParagraph();
        
        paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
        paragraph = section.AddParagraph();
        paragraph = section.AddParagraph();
        paragraph = section.AddParagraph();
        paragraph = section.AddParagraph();
        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Heading 1");
        textRange = paragraph.AppendText("UGOVOR") as WTextRange;
        textRange.CharacterFormat.FontSize = 28f;
        textRange.CharacterFormat.FontName = "Calibri";

        //Append the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ParagraphFormat.FirstLineIndent = 36;
        paragraph.BreakCharacterFormat.FontSize = 12f;
        textRange = paragraph.AppendText("Adventure Works Cycles, the fictitious company on which the AdventureWorks sample databases are based, is a large, multinational manufacturing company. The company manufactures and sells metal and composite bicycles to North American, European and Asian commercial markets. While its base operation is in Bothell, Washington with 290 employees, several regional sales teams are located throughout their market base.") as WTextRange;
        textRange.CharacterFormat.FontSize = 12f;

        //Appends the paragraph.
        paragraph = section.AddParagraph();
        paragraph.ParagraphFormat.FirstLineIndent = 36;
        paragraph.BreakCharacterFormat.FontSize = 12f;
        textRange = paragraph.AppendText("In 2000, AdventureWorks Cycles bought a small manufacturing plant, Importadores Neptuno, located in Mexico. Importadores Neptuno manufactures several critical subcomponents for the AdventureWorks Cycles product line. These subcomponents are shipped to the Bothell location for final product assembly. In 2001, Importadores Neptuno, became the sole manufacturer and distributor of the touring bicycle product group.") as WTextRange;
        textRange.CharacterFormat.FontSize = 12f;

        paragraph = section.AddParagraph();
        paragraph.ApplyStyle("Heading 1");
        paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Left;
        textRange = paragraph.AppendText("Product Overview") as WTextRange;
        textRange.CharacterFormat.FontSize = 16f;
        textRange.CharacterFormat.FontName = "Calibri";

        //Appends the table.
        IWTable table = section.AddTable();
        table.ResetCells(3, 2);
        table.TableFormat.Borders.BorderType = BorderStyle.None;
        table.TableFormat.IsAutoResized = true;

        //Appends the paragraph.
        paragraph = table[0, 0].AddParagraph();
        paragraph.ParagraphFormat.AfterSpacing = 0;
        paragraph.BreakCharacterFormat.FontSize = 12f;
        //Appends the picture to the paragraph.
        resourcePath = "eOdvjetnik.Resources.DocIO.Mountain-200.jpg";
        imageStream = assembly.GetManifestResourceStream(resourcePath);
        picture = paragraph.AppendPicture(imageStream);
        picture.TextWrappingStyle = TextWrappingStyle.TopAndBottom;
        picture.VerticalOrigin = VerticalOrigin.Paragraph;
        picture.VerticalPosition = 4.5f;
        picture.HorizontalOrigin = HorizontalOrigin.Column;
        picture.HorizontalPosition = -2.15f;
        picture.WidthScale = 79;
        picture.HeightScale = 79;

        //Appends the paragraph.
        paragraph = table[0, 1].AddParagraph();
        paragraph.ApplyStyle("Heading 1");
        paragraph.ParagraphFormat.AfterSpacing = 0;
        paragraph.ParagraphFormat.LineSpacing = 12f;
        paragraph.AppendText("Mountain-200");
        //Appends the paragraph.
        paragraph = table[0, 1].AddParagraph();
        paragraph.ParagraphFormat.AfterSpacing = 0;
        paragraph.ParagraphFormat.LineSpacing = 12f;
        paragraph.BreakCharacterFormat.FontSize = 12f;
        paragraph.BreakCharacterFormat.FontName = "Times New Roman";

        textRange = paragraph.AppendText("Product No: BK-M68B-38\r") as WTextRange;
        textRange.CharacterFormat.FontSize = 12f;
        textRange.CharacterFormat.FontName = "Times New Roman";
        textRange = paragraph.AppendText("Size: 38\r") as WTextRange;
        textRange.CharacterFormat.FontSize = 12f;
        textRange.CharacterFormat.FontName = "Times New Roman";
        textRange = paragraph.AppendText("Weight: 25\r") as WTextRange;
        textRange.CharacterFormat.FontSize = 12f;
        textRange.CharacterFormat.FontName = "Times New Roman";
        textRange = paragraph.AppendText("Price: $2,294.99\r") as WTextRange;
        textRange.CharacterFormat.FontSize = 12f;
        textRange.CharacterFormat.FontName = "Times New Roman";
        //Appends the paragraph.
        paragraph = table[0, 1].AddParagraph();
        paragraph.ParagraphFormat.AfterSpacing = 0;
        paragraph.ParagraphFormat.LineSpacing = 12f;
        paragraph.BreakCharacterFormat.FontSize = 12f;

        //Appends the paragraph.
        paragraph = table[1, 0].AddParagraph();
        paragraph.ApplyStyle("Heading 1");
        paragraph.ParagraphFormat.AfterSpacing = 0;
        paragraph.ParagraphFormat.LineSpacing = 12f;
        paragraph.AppendText("Mountain-300 ");
        //Appends the paragraph.
        paragraph = table[1, 0].AddParagraph();
        paragraph.ParagraphFormat.AfterSpacing = 0;
        paragraph.ParagraphFormat.LineSpacing = 12f;
        paragraph.BreakCharacterFormat.FontSize = 12f;
        paragraph.BreakCharacterFormat.FontName = "Times New Roman";
        textRange = paragraph.AppendText("Product No: BK-M47B-38\r") as WTextRange;
        textRange.CharacterFormat.FontSize = 12f;
        textRange.CharacterFormat.FontName = "Times New Roman";
        textRange = paragraph.AppendText("Size: 35\r") as WTextRange;
        textRange.CharacterFormat.FontSize = 12f;
        textRange.CharacterFormat.FontName = "Times New Roman";
        textRange = paragraph.AppendText("Weight: 22\r") as WTextRange;
        textRange.CharacterFormat.FontSize = 12f;
        textRange.CharacterFormat.FontName = "Times New Roman";
        textRange = paragraph.AppendText("Price: $1,079.99\r") as WTextRange;
        textRange.CharacterFormat.FontSize = 12f;
        textRange.CharacterFormat.FontName = "Times New Roman";
        //Appends the paragraph.
        paragraph = table[1, 0].AddParagraph();
        paragraph.ParagraphFormat.AfterSpacing = 0;
        paragraph.ParagraphFormat.LineSpacing = 12f;
        paragraph.BreakCharacterFormat.FontSize = 12f;

        //Appends the paragraph.
        paragraph = table[1, 1].AddParagraph();
        paragraph.ApplyStyle("Heading 1");
        paragraph.ParagraphFormat.LineSpacing = 12f;
        //Appends the picture to the paragraph.
        resourcePath = "eOdvjetnik.Resources.DocIO.Mountain-300.jpg";
        imageStream = assembly.GetManifestResourceStream(resourcePath);

        picture = paragraph.AppendPicture(imageStream);
        picture.TextWrappingStyle = TextWrappingStyle.TopAndBottom;
        picture.VerticalOrigin = VerticalOrigin.Paragraph;
        picture.VerticalPosition = 8.2f;
        picture.HorizontalOrigin = HorizontalOrigin.Column;
        picture.HorizontalPosition = -14.95f;
        picture.WidthScale = 75;
        picture.HeightScale = 75;

        //Append the paragraph.
        paragraph = table[2, 0].AddParagraph();
        paragraph.ApplyStyle("Heading 1");
        paragraph.ParagraphFormat.LineSpacing = 12f;
        //Appends the picture to the paragraph.
        resourcePath = "eOdvjetnik.Resources.DocIO.Road-550-W.jpg";
        imageStream = assembly.GetManifestResourceStream(resourcePath);
        picture = paragraph.AppendPicture(imageStream);
        picture.TextWrappingStyle = TextWrappingStyle.TopAndBottom;
        picture.VerticalOrigin = VerticalOrigin.Paragraph;
        picture.VerticalPosition = 3.75f;
        picture.HorizontalOrigin = HorizontalOrigin.Column;
        picture.HorizontalPosition = -5f;
        picture.WidthScale = 92;
        picture.HeightScale = 92;

        //Appends the paragraph.
        paragraph = table[2, 1].AddParagraph();
        paragraph.ApplyStyle("Heading 1");
        paragraph.ParagraphFormat.AfterSpacing = 0;
        paragraph.ParagraphFormat.LineSpacing = 12f;
        paragraph.AppendText("Road-150 ");
        //Appends the paragraph.
        paragraph = table[2, 1].AddParagraph();
        paragraph.ParagraphFormat.AfterSpacing = 0;
        paragraph.ParagraphFormat.LineSpacing = 12f;
        paragraph.BreakCharacterFormat.FontSize = 12f;
        paragraph.BreakCharacterFormat.FontName = "Times New Roman";
        textRange = paragraph.AppendText("Product No: BK-R93R-44\r") as WTextRange;
        textRange.CharacterFormat.FontSize = 12f;
        textRange.CharacterFormat.FontName = "Times New Roman";
        textRange = paragraph.AppendText("Size: 44\r") as WTextRange;
        textRange.CharacterFormat.FontSize = 12f;
        textRange.CharacterFormat.FontName = "Times New Roman";
        textRange = paragraph.AppendText("Weight: 14\r") as WTextRange;
        textRange.CharacterFormat.FontSize = 12f;
        textRange.CharacterFormat.FontName = "Times New Roman";
        textRange = paragraph.AppendText("Price: $3,578.27\r") as WTextRange;
        textRange.CharacterFormat.FontSize = 12f;
        textRange.CharacterFormat.FontName = "Times New Roman";
        //Appends the paragraph.
        section.AddParagraph();

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