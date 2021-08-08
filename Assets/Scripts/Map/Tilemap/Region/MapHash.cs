using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hash is used to get a pseudo-random value for a world position.
/// You get this value by calling GetHashedValue for the position
/// </summary>
public class MapHash
{
    private int NumValues; // The amount of different values the hash can return
    private int NumRepeats; // How many times each value occurs in the array, used for more randomness (values get bigger and are then modulo'd)
    private int[] HashValues; // The pseudo-random array of values, where one gets returned
    private int HashMask; // Used to hash the position

    public MapHash(int numValues, int numRepeats = 256)
    {
        // Initialize
        NumValues = numValues;
        NumRepeats = numRepeats;
        HashValues = new int[NumValues * NumRepeats * 2];
        HashMask = (NumRepeats * NumValues) - 1;

        // Create a list with all values the hash array will contain
        int counter = 0;
        List<int> hashValues = new List<int>();
        for (int i = 0; i < NumValues * NumRepeats; i++)
        {
            hashValues.Add(i);
        }
        // Fill the values inside the hash array in random order
        while (hashValues.Count > 0)
        {
            int value = hashValues[Random.Range(0, hashValues.Count)];
            HashValues[counter] = value;
            HashValues[(NumValues * NumRepeats) + counter] = value;
            hashValues.Remove(value);
            counter++;
        }
    }

    /// <summary>
    /// Returns a pseudo-random value unique to exactly that position
    /// </summary>
    public int GetHashedValue(int x, int y)
    {
        x &= HashMask;
        y &= HashMask;
        return HashValues[(HashValues[x] + y) & HashMask] % NumValues;
    }
}