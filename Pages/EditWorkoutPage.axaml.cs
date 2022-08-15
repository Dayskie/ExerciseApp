using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExerciseApp.Pages;

public partial class EditWorkoutPage : UserControl
{
    /// <summary>
    /// Creates and displays all possible exercises
    /// that can be added to a workout by getting their
    /// names from 'workoutsInfo.json'
    /// </summary>
    private JObject _WIobj = JObject.Parse(File.ReadAllText(StaticSetter.WorkoutJsonInfoLoc));

    private JToken tempWorkoutData;
    public EditWorkoutPage()
    {
        InitializeComponent();
        
        tempWorkoutData =
            StaticSetter._WorkoutJObject.SelectToken($"$.workouts[?(@.name == '{StaticSetter.WorkoutName}')]");
        var ExerciseLists = _WIobj["workouts"];

        for (int i = 0; i < ExerciseLists.Count(); i++)
        {
            string exerciseName = (string) ExerciseLists[i]["Name"];
            Button tempBtn = new Button()
            {
                Name = exerciseName?.Replace(" ", ""),
                Content = exerciseName, 
                Background = new SolidColorBrush(StaticSetter.Blurple),
                Foreground = new SolidColorBrush(Colors.White)
            };
            tempBtn.Click += AddExerciseToList;
            AvaliableExercies.Children.Add(tempBtn);
        }

        LoadExercises(tempWorkoutData);
    }

    /// <summary>
    /// When a button is clicked with the
    /// exercise you would like to add
    /// it creates a copy of that as a new button
    /// and displays it
    /// </summary>
    private void AddExerciseToList(object sender, RoutedEventArgs e)
    {
        var btnName = (sender as Button).Content.ToString();
        
        // stops a duplicate button from being added to the list
        for (int i = 0; i < SelectedExercises.Children.Count; i++)
        {
            if ((SelectedExercises.Children[i] as Button).Content.ToString() == btnName) { return; }
        }
        Button tempBtn = new Button()
        {
            Name = btnName?.Replace(" ", ""),
            Content = btnName, 
            Background = new SolidColorBrush(StaticSetter.Blurple),
            Foreground = new SolidColorBrush(Colors.White)
        };
        tempBtn.Click += RemoveExercise;
        
        SelectedExercises.Children.Add(tempBtn);
    }

    private void DeleteWorkout(object sender, RoutedEventArgs e)
    {
        var workouts = new NewWorkout();
        JsonConvert.PopulateObject(File.ReadAllText(StaticSetter.WorkoutJsonLoc), workouts);
        foreach (var item in workouts.workouts)
        {
            if (item.name == WorkoutName.Text)
            {
                workouts.workouts.Remove(item);
                break;
            }
        }
        File.WriteAllText(StaticSetter.WorkoutJsonLoc, JsonConvert.SerializeObject(workouts, Formatting.Indented));
        ReturnToMenu(sender, e);
    }

    private void SaveWorkout(object sender, RoutedEventArgs e)
    {
        if (SelectedExercises.Children.Count == 0)
        {
            Info.IsVisible = true;
            Info.Text = "Must have at least one exercise!";
        }
        
        var workouts = JsonConvert.DeserializeObject<NewWorkout>(File.ReadAllText(StaticSetter.WorkoutJsonLoc));
        AddExercise tempAdd = new AddExercise();
        StaticSetter._WorkoutJObject = JObject.Parse(File.ReadAllText(StaticSetter.WorkoutJsonInfoLoc));

        for (int x = 0; x < workouts.workouts.Count; x++)
        {
            if (workouts.workouts[x].name != StaticSetter.WorkoutName) { continue; }
            
            workouts.workouts[x].name = WorkoutName.Text;
            workouts.workouts[x].exercises.Clear();
            
            foreach (var item in SelectedExercises.Children)
            {
                string tempInfo = "";

                for (int i = 0; i < StaticSetter._WorkoutJObject["workouts"].Count(); i++)
                {
                    if (StaticSetter._WorkoutJObject["workouts"][i]["Name"].ToString() ==
                        (item as Button).Content.ToString())
                    {
                        tempInfo = StaticSetter._WorkoutJObject["workouts"][i]["Type"].ToString();
                    }
                }
                
                tempAdd.AddExerciseToWorkout(workouts.workouts[x], tempInfo, item);
            }
        }
        File.WriteAllText(StaticSetter.WorkoutJsonLoc, JsonConvert.SerializeObject(workouts, Formatting.Indented));
    }
    private void LoadExercises(JToken tempWorkoutData)
    {
        WorkoutName.Text = tempWorkoutData["name"].ToString();
        
        for (int i = 0; i < tempWorkoutData["exercises"].Count(); i++)
        {
            string tempName = tempWorkoutData["exercises"][i]["type"].ToString();
            Button tempBtn = new Button()
            {
                Name = tempName?.Replace(" ", ""),
                Content = tempName, 
                Background = new SolidColorBrush(StaticSetter.Blurple),
                Foreground = new SolidColorBrush(Colors.White)
            };
            tempBtn.Click += RemoveExercise;
        
            SelectedExercises.Children.Add(tempBtn);
        }
    }
    
    private void RemoveExercise(object sender, RoutedEventArgs e)
    {
        //Sender is the specific exercise you clicked on
        for (int i = 0; i < SelectedExercises.Children.Count; i++) {
            if ((SelectedExercises.Children[i] as Button).Content.ToString() == (sender as Button).Content.ToString())
            {
                SelectedExercises.Children.RemoveAt(i);
            }
        }
    }
    private void ReturnToMenu(object sender, RoutedEventArgs e)
    {
        StaticSetter.MainWindow.Content = new MainPage();
    }
}