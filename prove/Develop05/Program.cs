using System;

class Program
{
    static void Main(string[] args)
    {
         List<Goal> goals = new List<Goal>();

        int totalPoints = 0;
        bool isAchieving = true;
        
        do
        {
            Console.WriteLine("");
            Console.WriteLine($"You have {totalPoints} points.");
            Console.WriteLine("");

            Console.WriteLine("Menu Options:");
            Console.WriteLine(" 1. Create New Goal");
            Console.WriteLine(" 2. List Goals");
            Console.WriteLine(" 3. Save Goals");
            Console.WriteLine(" 4. Load Goals");
            Console.WriteLine(" 5. Record Event");
            Console.WriteLine(" 6. Quit");
            Console.Write("Select a choice from the menu: ");
            string choice = Console.ReadLine();

            if (choice == "1") 
            {
                Console.WriteLine("The types of goals are:");
                Console.WriteLine(" 1. Simple Goal");
                Console.WriteLine(" 2. Eternal Goal");
                Console.WriteLine(" 3. Checklist Goal");
                Console.Write("Which type of goal would you like to create? ");
                string type = Console.ReadLine();
                Console.Write("What is the name of your goal? ");
                string name = Console.ReadLine();
                Console.Write("What is a short description of the goal? ");
                string description = Console.ReadLine();
                Console.Write("How many points is this goal worth? ");
                int points = int.Parse(Console.ReadLine());

                if (type == "1")
                {
                    SimpleGoal simpleGoal = new SimpleGoal(false, points, name, description);
                    goals.Add(simpleGoal);
                } 
                
                else if (type == "2")
                {
                    EternalGoal eternalGoal = new EternalGoal(points, name, description);
                    goals.Add(eternalGoal);
                } 
                
                else if (type == "3")
                {
                    Console.Write("How many times does this goal need to be complete to earn a bonus? ");
                    int completeTotal = int.Parse(Console.ReadLine());
                    Console.Write("How many bonus points are earned when accomplished that many times? ");
                    int bonusPoints = int.Parse(Console.ReadLine());

                    ChecklistGoal checklistGoal = new ChecklistGoal(completeTotal,0,bonusPoints,points,name,description);

                    goals.Add(checklistGoal);
                }

            } 
            
            else if (choice == "2")
            {
                for (int i = 0; i < goals.Count; i++)
                {
                    Goal goal = goals[i];

                    string completeSymbol = " ";
                    if (goal.CheckOff())
                    {
                        completeSymbol = "X";
                    }

                    string goalDetails = goal.DisplayGoal();

                    Console.WriteLine($" {i+1}. [{completeSymbol}] {goalDetails}");
                }
            } 
            
            else if (choice == "3")
            {
                Console.Write("Please enter a filename: ");
                string filename = Console.ReadLine();

                using (StreamWriter writer = new StreamWriter($"{filename}"))
                {
                    writer.WriteLine($"{totalPoints}");

                    foreach(Goal goal in goals)
                    {
                        writer.WriteLine(goal.GetFileLine());
                    }
                }

                Console.WriteLine("Your file has been saved!");

            } 
            
            else if (choice == "4")
            {
                try
                {
                    Console.Write("Enter the filename: ");
                    string filename = Console.ReadLine();

                    using (StreamReader reader = new StreamReader($"{filename}"))
                    {
                        string content = reader.ReadToEnd();

                        string[] fileEntries = content.Split('\n');

                        foreach(string fileEntry in fileEntries)
                        {
                            if (!string.IsNullOrEmpty(fileEntry) && fileEntry.Contains(":"))
                            {
                                string[] keyValSplit = fileEntry.Split(':');
                                string type = keyValSplit[0];
                                string[] values = keyValSplit[1].Split(',');

                                if (type == "SimpleGoal")
                                {
                                    SimpleGoal simpleGoal = new SimpleGoal(bool.Parse(values[3]),int.Parse(values[2]),values[0],values[1]);
                                    goals.Add(simpleGoal);
                                } 
                                
                                else if (type == "EternalGoal")
                                {
                                    EternalGoal eternalGoal = new EternalGoal(int.Parse(values[2]),values[0],values[1]);
                                    goals.Add(eternalGoal);
                                } 
                                
                                else if (type == "ChecklistGoal")
                                {
                                    ChecklistGoal checklistGoal = new ChecklistGoal(int.Parse(values[4]),int.Parse(values[5]),int.Parse(values[3]),int.Parse(values[2]),values[0],values[1]);
                                    goals.Add(checklistGoal);
                                }
                            }
                            
                        }
                    }

                    Console.WriteLine("\nYour goals have been loaded");
                }
                
                catch (Exception)
                {
                    Console.WriteLine("File does not exist");
                }
            } 
            
            else if (choice == "5")
            {
                if (goals.Count > 0)
                {
                    Console.WriteLine("The goals are:");
                    for (int i = 0; i < goals.Count; i++)
                    {
                        if (!goals[i].CheckOff())
                        {
                            string goalName = goals[i].GetName();
                            Console.WriteLine($" [{i+1}] - {goalName}");
                        }
                    }

                    Console.Write("What goal did you accomplish? ");
                    int completeGoal = int.Parse(Console.ReadLine());

                    int pointsEarned = goals[completeGoal-1].RecordEvent();
                    totalPoints += pointsEarned;


                    Console.WriteLine($"Congrats! You have earned {pointsEarned} points!");
                    Console.WriteLine($"You now have {totalPoints} points.");
                } 
                
                else
                {
                    Console.WriteLine("\nYou have not set any goals yet.");
                }
            } 
            
            else if (choice == "6")
            {
                isAchieving = false;
            }
        
        } while (isAchieving);
    }

}