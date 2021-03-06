using System.Collections;
using System.Collections.Generic;

public class RandomSystem
{
    public System.Random dstPointRandom;
    public System.Random bombRandom;
    public System.Random candyRandom;


    private static readonly RandomSystem instance = new RandomSystem();
    private RandomSystem()
    {
        dstPointRandom = new System.Random();
        bombRandom = new System.Random();
        candyRandom = new System.Random();
    }

    public static RandomSystem GetInstance()
    {
        return instance;
    }
}
