using FighterMaker.Visual.Controls;
using FighterMaker.Visual.Core;
using FighterMaker.Visual.Core.Attributes;
using FighterMaker.Visual.Core.Extensions.Models;
using FighterMaker.Visual.Models;
using FighterMaker.Visual.Plugins;
using FighterMaker.Visual.Windows;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FighterMaker.Visual.Pages
{
    /// <summary>
    /// Interaction logic for ActorManagerPage.xaml
    /// </summary>
    public partial class ActorManagerPage : Page
    {
        private readonly ActorModelCollection actorCollection = [];
        private readonly Dictionary<ActorModel, PropertiesTreeView> propertiesControlDictionary = [];
        private ActorModel? currentActor;

        public ActorManagerPage()
        {
            InitializeComponent();
            actorCollection.ItemAdded += Actors_ItemAdded;
            ActorComboBox.ItemsSource = actorCollection;           
        }

        private void Actors_ItemAdded(object? sender, ActorModel e)
        {
            ActorComboBox.ItemsSource = null;
            ActorComboBox.ItemsSource = actorCollection;
            
            //ActorComboBox.UpdateLayout();
            ActorComboBox.SelectedIndex = actorCollection.Count - 1;
        }

        private PropertiesTreeView GetOrCreatePropertiesViewControl(ActorModel model)
        {
            Debug.Assert(model != null);

            if (propertiesControlDictionary.TryGetValue(model, out PropertiesTreeView? control))
                return control;

            model.AddPlugin(new AnimationPlugin());

            control = new PropertiesTreeView();
            control.Width = 500;
            control.SetModel(model);
            control.ScanModel();
            //control.PopulateControl();
            propertiesControlDictionary.Add(model, control);

            return control;
        }

        private void SetCurrentPropertiesControl(PropertiesTreeView control)
        {
            Debug.Assert(control != null);

            if (ExpanderDockInStackPanel.Children.Count > 0)
                ExpanderDockInStackPanel.Children.Clear();

            ExpanderDockInStackPanel.Children.Add(control);
        }

        private void AddActorButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new NewTextWindow();
            var dialog = window.ShowDialog();

            if (dialog != null && dialog.Value)
            {
                var model = new ActorModel().WithName(window.SettedValue);
                currentActor = model;
                AnalizeActorPlugins();

                var propertiesViewControl = GetOrCreatePropertiesViewControl(model);

                SetCurrentPropertiesControl(propertiesViewControl);
                actorCollection.Add(model);
            }
        }

        private void AnalizeActorPlugins()
        {
            if (currentActor == null)
                return;

            var plugins = currentActor.Plugins;
            MainTab.Items.Clear();

            foreach (var plugin in plugins) 
            {
                var attribute = plugin.GetType().GetCustomAttribute<PluginAttribute>();
                var properties = plugin.GetType().GetProperties();

                foreach (var property in properties) 
                {                    
                    var value = property.GetValue(plugin, null);
                    if(value is Page page)
                    {                        
                        var frame = new Frame
                        {
                            Content = page
                        };

                        var item = new TabItem
                        {
                            Content = frame,
                            IsSelected = true,
                            Header = attribute?.Name
                        };

                        MainTab.Items.Add(item);
                    }
                }
            }
        }

        private void ActorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ActorComboBox.SelectedItem is not ActorModel model)
            {
                DisableLayout();
                return;
            }

            var propertyView = propertiesControlDictionary[model];

            if (ExpanderDockInStackPanel.Children.Count > 0)
                ExpanderDockInStackPanel.Children.Clear();

            ExpanderDockInStackPanel.Children.Add(propertyView);
            AnalizeActorPlugins();

            EnableLayout();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DisableLayout();            
        }

        private void DisableLayout()
        {            
            MainTab.IsEnabled = false;
            MainExpander.IsEnabled = false;
        }

        private void EnableLayout()
        {
            MainTab.IsEnabled = true;
            MainExpander.IsEnabled = true;
        }

        private void ExpanderAddPluginButton_Click(object sender, RoutedEventArgs e)
        {           
            var analiser = AssemblyAnalizer.FromExecutingAssembly();
            var types = analiser.GetTypesWithAttribute<PluginAttribute>();

            var window = new NewListViewWindow();
            window.ListSource = types;
            var dialog = window.ShowDialog();

            if (dialog != null && dialog.Value)
            {
                var selectedType = window.SelectedValue as Type;

                if (selectedType == null)
                    return;

                var selectedPlugin = Activator.CreateInstance(selectedType);

                if (selectedPlugin == null || selectedPlugin is not Plugin)
                    return;

                currentActor.AddPlugin((Plugin)selectedPlugin);
                AnalizeActorPlugins();
            }
        }
    }
}
