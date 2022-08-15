using Avalonia.Controls;

namespace ExerciseApp;

public class AddExercise
{
    public void AddExerciseToWorkout(NewWorkoutExercise workoutExercise, string exerciseType, IControl item)
    {
        switch (exerciseType)
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
}