using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace ExerciseApp.Pages;

public partial class NewWorkoutPage : UserControl
{
    /// <summary>
    /// Creates and displays all possible exercises
    /// that can be added to a workout by getting their
    /// names from 'workoutsInfo.json'
    /// </summary>
 
    public NewWorkoutPage()
    {
        InitializeComponent();
        StaticSetter._WorkoutJObject = JObject.Parse(File.ReadAllText(StaticSetter.WorkoutJsonInfoLoc));
        var ExerciseLists = StaticSetter._WorkoutJObject["workouts"];

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

    private void SaveWorkout(object sender, RoutedEventArgs e)
    {
        if (SelectedExercises.Children.Count == 0)
        {
            Info.IsVisible = true;
            Info.Text = "No exercises added.";
            return;
        }
        if(CheckForDuplicates()) { return; }

        NewWorkoutExercise workoutExercise = new NewWorkoutExercise();
        workoutExercise.name = WorkoutName.Text;
        
        foreach (var item in SelectedExercises.Children)
        {
            string tempInfo = "";
            
            for (int i = 0; i < StaticSetter._WorkoutJObject["workouts"].Count(); i++) {
                if (StaticSetter._WorkoutJObject["workouts"][i]["Name"].ToString() == (item as Button).Content.ToString())
                {
                    tempInfo = StaticSetter._WorkoutJObject["workouts"][i]["Type"].ToString();
                }
            }

            switch (tempInfo)
            {
                case "Lifting":
                    Lifting lift = new Lifting();
            
                    lift.type = (item as Button).Content.ToString();
                    lift.reps = "0";
                    lift.weight = "0";
                    workoutExercise.exercises.Add(lift);
                    break;
                case "Cardio":
                    Cardio cardio = new Cardio();
            
                    cardio.type = (item as Button).Content.ToString();
                    cardio.distance = "0";
                    cardio.time = "0";
                    workoutExercise.exercises.Add(cardio);
                    break;
                
                case "BodyWeight":
                    BodyWeight bodyweight = new BodyWeight();
            
                    bodyweight.type = (item as Button).Content.ToString();
                    bodyweight.reps = "0";
                    workoutExercise.exercises.Add(bodyweight);
                    break;
            }
        }
        var workouts = JsonConvert.DeserializeObject<NewWorkout>(File.ReadAllText(StaticSetter.WorkoutJsonLoc));
        workouts.workouts.Add(workoutExercise);
        File.WriteAllText(StaticSetter.WorkoutJsonLoc, JsonConvert.SerializeObject(workouts, Formatting.Indented));
        
        ReturnToMenu(sender, e);
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

    private bool CheckForDuplicates()
    {
        var temp = JObject.Parse(File.ReadAllText(StaticSetter.WorkoutJsonLoc));
        if(temp.SelectToken($"$.workouts[?(@.name == '{WorkoutName.Text}')]")?["name"].ToString() != null)
        {
            Info.IsVisible = true;
            Info.Text = "This workout name already exists!";
            return true;
        }

        return false;
    }
}