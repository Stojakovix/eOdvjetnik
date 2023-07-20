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
    public event EventHandler onCreateDocument;
    NaplataViewModel naplataViewModel;

    public Racun(NaplataViewModel naplataViewModel)
	{
        InitializeComponent();
        this.naplataViewModel = naplataViewModel;
        BindingContext = naplataViewModel;

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