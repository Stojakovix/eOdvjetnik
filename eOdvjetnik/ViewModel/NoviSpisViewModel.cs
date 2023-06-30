using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using eOdvjetnik.Model;
using eOdvjetnik.Services;
using Syncfusion.Maui.Scheduler;

namespace eOdvjetnik.ViewModel
{
    public class NoviSpisViewModel : INotifyPropertyChanged
    {
        public ICommand AddFilesToRemoteServer { get; set; }

        ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();

        FileItem fileitem;
        //private FileItem fileitem;
        public FileItem FileItem
        {
            get { return fileitem; }
            set
            {
                if (fileitem != value)
                {
                    fileitem = value;
                    RaiseOnPropertyChanged(nameof(FileItem));
                }
            }
        }

        //Napravi još bindinge sa svakim entry fieldom za svako polje da dodaje, vjerojatno neće bit dobar i trebat će se nešto konvertat, date time ili slično
        public NoviSpisViewModel(FileItem fileitem)
        {
            try
            {
                this.fileitem = fileitem;
                AddFilesToRemoteServer = new Command(() => AddSpisToRemoteServer(fileitem));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private void AddSpisToRemoteServer(FileItem fileItem)
        {
            try
            {
                ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
                string query = $"INSERT INTO Files (id, broj_spisa, spisicol, client_id, opponent_id, inicijali_voditelj_id, inicijali_dodao, filescol, inicijali_dodjeljeno, created, aktivno_pasivno, referenca, datum_promjene_statusa, uzrok, datum_kreiranja_spisa, datum_izmjene_spisa, kreirao, zadnje_uredio, jezik,broj_predmeta ) " +
                    $"VALUES ('{fileItem.Id}', '{fileItem.BrojSpisa}', '{fileItem.Spisicol}', '{fileItem.ClientId}', '{fileItem.OpponentId}', '{fileItem.InicijaliVoditeljId}' , '{fileItem.InicijaliDodao}' , '{fileItem.Filescol}' , '{fileItem.InicijaliDodjeljeno}' , '{fileItem.Created}' , '{fileItem.AktivnoPasivno}' , '{fileItem.Referenca}' , '{fileItem.DatumPromjeneStatusa}' , '{fileItem.Uzrok}' , '{fileItem.DatumKreiranjaSpisa}' , '{fileItem.DatumIzmjeneSpisa}' , '{fileItem.Kreirao}' , '{fileItem.ZadnjeUredio}' , '{fileItem.Jezik}' , '{fileItem.BrojPredmeta}' )";
                Debug.WriteLine(query + " in novi spis viewModel");
                //externalSQLConnect.sqlQuery(query);  
                Debug.WriteLine("Appointment added to remote server in novi spis viewModel");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " in noviSpisViewModel AddAppointmentToServer");
            }
        }
       
        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaiseOnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


    }


}
