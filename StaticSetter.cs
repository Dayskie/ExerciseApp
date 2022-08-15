using System;
using Avalonia.Media;
using Newtonsoft.Json.Linq;

namespace ExerciseApp;

/// <summary>
/// This file contains all of the variables that need to be
/// accsessed from multiple sources. This way each variable
/// doesn't need to be separately created or instantiated 
/// </summary>
public class StaticSetter
{
    public static MainWindow MainWindow { get; set; }
    public static Color Blurple = Color.FromRgb(114, 137, 218);

    public static string WorkoutJsonLoc = @"/Users/106266/Desktop/ExerciseApp/workouts.json";
    public static string WorkoutJsonInfoLoc = AppDomain.CurrentDomain.BaseDirectory + @"/workoutsInfo.json";
    public static string WorkoutName { get; set; }
    public static JObject _WorkoutJObject { get; set; }
}