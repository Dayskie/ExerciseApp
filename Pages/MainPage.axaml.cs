using System;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Newtonsoft.Json.Linq;

namespace ExerciseApp.Pages;

public partial class MainPage : UserControl
{
    public MainPage()
    {
        InitializeComponent();
        StaticSetter._WorkoutJObject = JObject.Parse(File.ReadAllText(StaticSetter.WorkoutJsonLoc));
        
        ShowWorkouts();
    }

    private void ShowWorkouts()
    {
        var workoutToken = StaticSetter._WorkoutJObject["workouts"];
        try
        {
            if (!workoutToken.Any())
            {
                SelectWorkout.Text = "No workouts found!";
            }
            
            for (int i = 0; i < workoutToken.Count(); i++)
            {
                string tempName = (string)workoutToken[i]["name"];
                Button tempBtn = new Button()
                {
                    Name = tempName?.Replace(" ", ""),
                    Content = tempName, 
                    Background = new SolidColorBrush(StaticSetter.Blurple),
                    Foreground = new SolidColorBrush(Colors.White)
                };
                tempBtn.Click += OpenExercises;
                WorkoutPanel.Children.Add(tempBtn);
            }
        }
        catch (Exception e)
        {
            SelectWorkout.Text = "No workouts found!";
        }
    }

    private void OpenExercises(object sender, RoutedEventArgs e)
    {
        StaticSetter.WorkoutName = (sender as Button)?.Content.ToString();
        StaticSetter.MainWindow.Content = new ExerciseInfoPage();
    }

    private void NavigateToNewWorkout(object sender, RoutedEventArgs e)
    {
        StaticSetter.MainWindow.Content = new NewWorkoutPage();
    }
}