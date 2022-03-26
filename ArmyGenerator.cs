using System;
using System.Linq;
using System.Collections.Generic;


public static class Program
{
    #region public methods
    public static void Main(string[] args)
    {   

        int count = 0;
        while(count <= 0)
        {
            Console.WriteLine("Enter army count : ");
            string countLine = Console.ReadLine();

            int.TryParse(countLine, out count);

            if(count <= 0)

            Console.WriteLine("Incorrect value!!!");

        }

        Run(count);

        Console.ReadKey();
    } 

    #endregion
    
    #region  private methods

    private static void Run(int count)
    {
        var armies = GenerateArmy(count);
        var duplicates = DuplicatesCount(armies);

        Output(armies,duplicates);
    }

    private static List<Army> GenerateArmy(int count)
    {
           
        var generator = new ArmyGenerator();
        var armies = new List<Army>();

        for(int i = 0; i <100; i++)
        {
            //It works better with big numbers (143 has no duplicates after 115 iterations)
            var army = generator.GenerateArmy(count);
            armies.Add(army);
        }

        return armies;
    }

    private static int DuplicatesCount(List<Army> armies)
    {
        int duplicates = 0;

        for(int i = 0; i < armies.Count; i ++)
        {   
            for(int j = i + 1; j < armies.Count; j ++)
            {
                if(armies[j].IsEqual(armies[i]))
                {
                   armies[j].IsDuplicate = true;
                   armies[i].IsOrigin = true;
                   duplicates++;
                }
            }
        }

        return duplicates;
    }

    private static void Output(List<Army> armies, int duplicates)
    {
        //Output and duplicates coloring
        foreach(var army in armies)
        {
            if(army.IsDuplicate)
            {
                Console.BackgroundColor = ConsoleColor.Red;
            }
            else if(army.IsOrigin)
            {
                Console.BackgroundColor = ConsoleColor.Magenta;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Black;
            }

            Console.WriteLine(army.ToString());
        }

        Console.WriteLine("Duplicates " + duplicates);
        Console.ReadKey();
    }

    #endregion

}

//LCG realization
public static class RandomGenerator
{
    #region constants

    private const long A = 25214903917;
    private const long C = 121;
    
    #endregion

    #region atributes
   
    private static ulong seed = 1;

    #endregion

    #region  public methods

    public static double Next()
    {
        seed = (A * seed + C) & ((1L<<48)-1);
        return (double)(seed)/((double)(1L<<48));
    }

    #endregion
}


public class ArmyGenerator
{
    #region constants

    private const int LOW_BOUND = 1;

    #endregion

    #region public methods

    public Army GenerateArmy(int count)
    {
        //We need to generate normal seed first
        RandomGenerator.Next(); 

        int archer = GetManCount(count);
        count -= archer;
        int spearman = GetManCount(count);
        count -= spearman;
        int swordman = count;
        
        return new Army(archer, spearman,swordman);
    }

    #endregion

    #region private methods

    private int GetManCount(int count)
    {
        return (int)((count - LOW_BOUND) * RandomGenerator.Next() + LOW_BOUND);
    }

    #endregion
} 

public class Army
{
    #region properties

    public bool IsDuplicate {get; set;}
    public bool IsOrigin {get; set;}

    public int Archer 
    {
        get
        {
            return _archer;
        } 
    }

    public int Spearman 
    {
        get
        {
            return _spearman;
        } 
    }

    public int Swordman
    {
        get
        {
            return _swordman;
        } 
    }

    #endregion

    #region atributes

    private int _archer;
    private int _spearman;
    private int _swordman;

    #endregion

    #region constructor

    public Army(int archer, int spearman, int swordman)
    {
        _archer = archer;
        _spearman = spearman;
        _swordman = swordman;
    }

    #endregion

    #region public methods

    public bool IsEqual(Army army)
    {
        return army != null 
             && _archer == army._archer 
             && _spearman == army._spearman 
             && _swordman == army._swordman;
    }


    public override string ToString()
    {
        return "archer =  " + _archer 
        + " spearman = " + _spearman 
        + " swordman = " + _swordman;
    }

    #endregion
}

