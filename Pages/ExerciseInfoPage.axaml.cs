using System;
using System.IO;
using System.Linq;
using System.Drawing;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Newtonsoft.Json.Linq;
using Bitmap = System.Drawing.Bitmap;

namespace ExerciseApp.Pages;

public partial class ExerciseInfoPage : UserControl
{
    private string[] _data = {"reps", "weight", "distance", "time"};
    private string _currentExercise;
    private JObject _WIobj = JObject.Parse(File.ReadAllText(StaticSetter.WorkoutJsonInfoLoc));
    
    public ExerciseInfoPage()
    {
        InitializeComponent();

        WorkoutName.Text = $"{StaticSetter.WorkoutName} Exercises!";
        ShowExercises(StaticSetter.WorkoutName, StaticSetter._WorkoutJObject);
    }

    private void ShowExercises(string workoutName, JObject _jObject)
    {
        ExercisePanel.Children.Clear();
        ExerciseDataNamePnl.Children.Clear();
        ExerciseDataValuePnl.Children.Clear();

        var temp = _jObject.SelectToken($"$.workouts[?(@.name == '{workoutName}')]");
        foreach (var item in temp["exercises"])
        {
            Button tempBtn = new Button()
            {
                Name = item["type"].ToString().Replace(" ", ""),
                Content = item["type"],
                Background = new SolidColorBrush(StaticSetter.Blurple),
                Foreground = new SolidColorBrush(Colors.White)
            };

            tempBtn.Click += ShowExerciseData;
            ExercisePanel.Children.Add(tempBtn);
        }
    }
    private void ShowExerciseData(object sender, RoutedEventArgs e)
    {
        ExerciseDataNamePnl.Children.Clear();
        ExerciseDataValuePnl.Children.Clear();

        string? exerciseBtnName = (sender as Button)?.Content.ToString();
        JToken? temp =
            StaticSetter._WorkoutJObject.SelectToken($"$.workouts[?(@.name == '{StaticSetter.WorkoutName}')].exercises.[?(@.type == '{exerciseBtnName}')]");
            
        _currentExercise = exerciseBtnName;

        for (int i = 0; i < _data.Length; i++)
        {
            if (temp[_data[i]] != null)
            {
                TextBlock nameBlock = new TextBlock()
                {
                    Name = _data[i],
                    Text = _data[i],
                    Background = new SolidColorBrush(StaticSetter.Blurple),
                    Foreground = new SolidColorBrush(Colors.White),
                    Padding = new Thickness(5,5,5,5),
                    
                };
                   ExerciseDataNamePnl.Children.Add(nameBlock);
                TextBox tempBlock = new TextBox()
                {
                    Name = _data[i],
                    Text = $"{temp[_data[i]]}",
                    MaxWidth = 75,
                    Background = new SolidColorBrush(StaticSetter.Blurple),
                    Foreground = new SolidColorBrush(Colors.White)
                };
                tempBlock.AddHandler(TextInputEvent, TextEdited, RoutingStrategies.Tunnel);
                ExerciseDataValuePnl.Children.Add(tempBlock);
            }
        }
        
        ShowExerciseInfo();
    }

    private void TextEdited(object sender, TextInputEventArgs e)
    {
        TextBox TextBoxTemp = (TextBox)sender;
        JToken? tempExercise =
            StaticSetter._WorkoutJObject.SelectToken(
                $"$.workouts[?(@.name == '{StaticSetter.WorkoutName}')].exercises.[?(@.type == '{_currentExercise}')]");
        tempExercise[TextBoxTemp.Name] = TextBoxTemp.Text + e.Text;
    }

    private void ShowExerciseInfo()
    {
        var workoutInfo = _WIobj.SelectToken($"$.workouts.[?(@.Name == '{_currentExercise}')]");
        WorkoutInfoDescription.Text = workoutInfo["Info"].ToString();
        
    }

    private void SaveData(object sender, RoutedEventArgs e)
    {
        File.WriteAllText(StaticSetter.WorkoutJsonLoc, StaticSetter._WorkoutJObject.ToString());
    }
    
    private void ReturnToMenu(object sender, RoutedEventArgs e)
    {
        StaticSetter.MainWindow.Content = new MainPage();
    }

    private void EditData(object sender, RoutedEventArgs e)
    {
        StaticSetter.MainWindow.Content = new EditWorkoutPage();
    }
}
