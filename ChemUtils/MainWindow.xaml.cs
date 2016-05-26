using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using ChemUtils.Annotations;
using ChemUtils.Models;
using MahApps.Metro.Controls;
using Newtonsoft.Json;
using PropertyChanged;

namespace ChemUtils
{
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        public List<Atom> Atoms { get; } = JsonConvert.DeserializeObject<List<Atom>>(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Atoms.json")));

        private Atom _previewAtom;
        public Atom PreviewAtom
        {
            get { return this._previewAtom; }
            set
            {
                this._previewAtom = value;
                this.OnPropertyChanged();
            }
        }

        private Atom _selectedAtom;
        public Atom SelectedAtom
        {
            get { return this._selectedAtom; }
            set
            {
                this._selectedAtom = value;
                this.OnPropertyChanged();
            }
        }

        private string _filterQuery = null;
        public string FilterQuery
        {
            get { return this._filterQuery; }
            set
            {
                this._filterQuery = value;
                this.OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void OpenInformationCard(object sender, MouseButtonEventArgs e)
        {
            this.PreviewAtom = (Atom) sender;
            // TODO: open atom information card popup
        }

        private void FilterQueryChanged(object sender, TextChangedEventArgs e)
        {
            string query = ((TextBox) sender).Text.ToLowerInvariant();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
