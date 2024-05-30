

using System;
using System.Collections.Generic;
using System.Linq;

// Delegate for notifying when total calories exceed a limit
public delegate void CaloriesExceededDelegate(string message);

// Class to represent an ingredient in a recipe
public class Ingredient
{
    // Name of the ingredient
    public string Name { get; set; }

    // Quantity of the ingredient
    public double Quantity { get; set; }

    // Unit of measurement for the ingredient
    public string UnitOfMeasurement { get; set; }

    // Original quantity of the ingredient
    public double OriginalQuantity { get; set; }

    // Number of calories
    public double Calories { get; set; }

    // Food group that the ingredient belongs to
    public string FoodGroup { get; set; }

    // Method to scale the quantity of the ingredient
    // The scaling factor is passed as an argument
    public void Scale(double factor)
    {
        Quantity *= factor;
    }

    // Method to reset the quantity of the ingredient to its original value
    public void Reset()
    {
        Quantity = OriginalQuantity;
    }
    // Method to display the current and original quantities
    public void DisplayQuantities()
    {
        Console.WriteLine($"Current Quantity of {Name}: {Quantity} {UnitOfMeasurement}");
        Console.WriteLine($"Original Quantity of {Name}: {OriginalQuantity} {UnitOfMeasurement}");
    }
}

// Class to represent a step in a recipe
public class Step
{
    // Description of the step
    public string Description { get; set; }
}

// Class to represent a recipe
public class Recipe
{
    // Name of the recipe
    public string Name { get; set; }

    // List of ingredients in the recipe
    public List<Ingredient> Ingredients { get; set; }

    // List of steps in the recipe
    public List<Step> Steps { get; set; }

    // Delegate instance for notifying when total calories exceed a limit
    public CaloriesExceededDelegate CaloriesExceeded { get; set; }

    // Method to display the recipe
    public void DisplayRecipe()
    {
        Console.WriteLine("\nRecipe Details:");
        Console.WriteLine($"Recipe Name: {Name}");
        Console.WriteLine("Ingredients:");
        foreach (var ingredient in Ingredients)
        {
            Console.WriteLine($"- {ingredient.Name}: {ingredient.Quantity} {ingredient.UnitOfMeasurement}, {ingredient.Calories} calories, Food Group: {ingredient.FoodGroup}");
        }

        Console.WriteLine("\nSteps:");
        for (int i = 0; i < Steps.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {Steps[i].Description}");
        }

        double totalCalories = Ingredients.Sum(i => i.Calories);
        Console.WriteLine($"Total Calories: {totalCalories}");
        if (totalCalories > 300)
        {
            CaloriesExceeded?.Invoke("Warning: The total calories of this recipe exceed 300.");
        }
    }

    // Method to scale the recipe
    // The scaling factor is passed as an argument
    public void ScaleRecipe(double factor)
    {
        foreach (var ingredient in Ingredients)
        {
            ingredient.Scale(factor);
        }
    }

    // Method to reset the recipe to its original state
    public void ResetRecipe()
    {
        foreach (var ingredient in Ingredients)
        {
            ingredient.Reset();
            ingredient.DisplayQuantities(); // Display the reset quantities
        }
    }

    // Method to clear the recipe
    public void ClearRecipe()
    {
        Ingredients.Clear();
        Steps.Clear();
    }
}

class Program
{
    static void Main(string[] args)
    {
        var recipes = new List<Recipe>();

        while (true)
        {
            Console.WriteLine("What recipe do you want to make?");
            string recipeName = Console.ReadLine();

            Console.WriteLine($"Let's start creating the recipe for {recipeName}!");

            Console.WriteLine("Enter the number of ingredients:");
            int ingredientCount = Convert.ToInt32(Console.ReadLine());

            var recipe = new Recipe
            {
                Name = recipeName,
                Ingredients = new List<Ingredient>(),
                Steps = new List<Step>(),
                CaloriesExceeded = message => Console.WriteLine(message) // Set the delegate
            };

            for (int i = 0; i < ingredientCount; i++)
            {
                Console.WriteLine($"Enter details for ingredient {i + 1}:");
                Console.WriteLine("Name:");
                string name = Console.ReadLine();
                Console.WriteLine("Quantity:");
                double quantity = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Unit of Measurement:");
                string unitOfMeasurement = Console.ReadLine();
                Console.WriteLine("Number of Calories:");
                double calories = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Food Group:");
                string foodGroup = Console.ReadLine();

                recipe.Ingredients.Add(new Ingredient
                {
                    Name = name,
                    Quantity = quantity,
                    OriginalQuantity = quantity, // Set the OriginalQuantity here
                    UnitOfMeasurement = unitOfMeasurement,
                    Calories = calories,
                    FoodGroup = foodGroup
                });
            }

            Console.WriteLine("Enter the number of steps:");
            int stepCount = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < stepCount; i++)
            {
                Console.WriteLine($"Enter description for step {i + 1}:");
                string description = Console.ReadLine();

                recipe.Steps.Add(new Step
                {
                    Description = description
                });
            }

            recipes.Add(recipe);
            Console.WriteLine("Recipe created successfully!");

            Console.WriteLine("Enter the scaling factor (0.5 for half, 2 for double, 3 for triple):");
            double scalingFactor = Convert.ToDouble(Console.ReadLine());

            recipe.ScaleRecipe(scalingFactor);

            // Display the full recipe
            recipe.DisplayRecipe();

            Console.WriteLine("\nDo you want to reset the quantities to the original values? (yes/no)");
            string resetResponse = Console.ReadLine();

            // If the user wants to reset the quantities
            if (resetResponse.ToLower() == "yes")
            {
                recipe.ResetRecipe();
                Console.WriteLine("Quantities have been reset to their original values.");
            }

            Console.WriteLine("\nDo you want to clear all the data to enter a new recipe? (yes/no)");
            string clearResponse = Console.ReadLine();

            // If the user wants to clear all the data
            if (clearResponse.ToLower() == "yes")
            {
                recipe.ClearRecipe();
                Console.WriteLine("All data has been cleared. You can now enter a new recipe.");
            }
            else
            {
                Console.WriteLine("\nDo you want to enter another recipe? (yes/no)");
                string anotherRecipeResponse = Console.ReadLine();

                // If the user does not want to enter another recipe
                if (anotherRecipeResponse.ToLower() != "yes")
                {
                    break;
                }
            }
        }

        // Display all recipes in alphabetical order
        Console.WriteLine("\nAll Recipes:");
        foreach (var recipe in recipes.OrderBy(r => r.Name))
        {
            Console.WriteLine(recipe.Name);
        }

        Console.WriteLine("\nEnter the name of the recipe you want to display:");
        string displayRecipeName = Console.ReadLine();
        var displayRecipe = recipes.FirstOrDefault(r => r.Name == displayRecipeName);

        if (displayRecipe != null)
        {
            displayRecipe.DisplayRecipe();
        }
        else
        {
            Console.WriteLine("Recipe not found.");
        }
    }
}



