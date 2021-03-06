﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using ChemUtils.Annotations;
using ChemUtils.Models;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using Newtonsoft.Json;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace ChemUtils
{
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        private int _selectedTab = 0;
        public int SelectedTab
        {
            get { return this._selectedTab; }
            set
            {
                this._selectedTab = value;
                this.OnPropertyChanged();
            }
        }

        public string FilterQuery => this.FilterQueryTextBox.Text;

        public List<Atom> Atoms { get; } = JsonConvert.DeserializeObject<List<Atom>>(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Atoms.json")));

        private HashSet<string> selectedSafetysheets = new HashSet<string>();

        public MainWindow()
        {
            this.InitializeComponent();

            this.Loaded += delegate
            {
                // TODO: move to XAML, :cat_lazy:
                var files = Directory.EnumerateFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Substancesheets"));
                foreach (string file in files)
                {
                    CheckBox checkBox = new CheckBox()
                    {
                        Content = Path.GetFileName(file).Replace(".pdf", ""),
                        Style = (Style)this.FindResource("MaterialDesignAccentCheckBox")
                    };
                    checkBox.Checked += delegate
                    {
                        this.selectedSafetysheets.Add(file);
                    };
                    checkBox.Unchecked += delegate
                    {
                        if (this.selectedSafetysheets.Contains(file))
                        {
                            this.selectedSafetysheets.Remove(file);
                        }
                    };
                    this.SafetysheetsListView.Items.Add(checkBox);
                }
            };
        }

        private void SaveSafetySheets(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                OverwritePrompt = true,
                Title = "Save file",
                AddExtension = true,
                FileName = "Werkplekinstructiekaarten.pdf"
            };
            dialog.ShowDialog();

            using (Stream stream = dialog.OpenFile())
            {
                using (PdfDocument outpdf = new PdfDocument())
                {
                    foreach (string selectedSafetysheet in this.selectedSafetysheets)
                    {
                        using (PdfDocument safetysheet = PdfReader.Open(selectedSafetysheet, PdfDocumentOpenMode.Import))
                        {
                            for (int i = 0; i < safetysheet.PageCount; i++)
                            {
                                outpdf.AddPage(safetysheet.Pages[i]);
                            }
                        }
                    }
                    outpdf.Save(stream);
                }
            }
        }

        private void FilterQueryChangedCallback(object sender, TextChangedEventArgs e) => this.SafetysheetsListView.Items.Filter = o => this.FilterQuery.Length <= 2 || ((string)((CheckBox)o).Content).Contains(this.FilterQuery);

        private void SwitchTab(object sender, RoutedEventArgs e) => this.SelectedTab = int.Parse((string)((Button)sender).Tag);

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
