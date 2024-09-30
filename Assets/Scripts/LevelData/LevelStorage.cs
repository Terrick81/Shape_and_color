using System;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class LevelStorage : MonoBehaviour
{
    public static Level LevelDownload(int seed)
    {
        Level.UnitRandom(seed);
        Level lvl;
        switch (seed)
        {
            case 1:
                lvl = new Level(new CD[2, 1]
                {
                    {new CD(Shapes.rectangle, Colors.purple, Shapes.rectangle, Colors.yellow) },
                    {new CD(Shapes.rectangle, Colors.yellow) }
                });
                Education.StartEducation(seed);
                break;
            case 2:
                lvl = new Level(new CD[1, 3]
                {
                    {new CD(Shapes.circle, Colors.orange, Colors.aquamarine),
                     new CD(Shapes.circle, Colors.aquamarine, Colors.aquamarine),
                     new CD(Shapes.circle, Colors.aquamarine )}
                });
                break;
            case 3:
                lvl = new Level(new CD[2, 2]
                {
                    {new CD(Shapes.rectangle, Colors.cyan, Colors.green),
                     new CD(Shapes.rectangle, Colors.green, Colors.cyan)               },
                    {new CD(Shapes.rectangle, Colors.green, Colors.cyan),
                     new CD(Shapes.rectangle, Colors.cyan              )}
                });
                Education.StartEducation(seed);
                break;
            case 4:
                lvl = new Level(new CD[2, 2]
                {
                    {new CD(Shapes.rectangle, Colors.cyan, Colors.orange),
                     new CD(Shapes.rectangle, Colors.red)                },
                    {new CD(Shapes.rectangle, Colors.orange, Colors.red ),
                     new CD(Shapes.rectangle, Colors.green, Colors.cyan )}
                });
                break;
            case >= 5 and <= 6:
                lvl = new Level(seed, countColor: 4, countShape: 1);
                break;
            case 7:
                lvl = new Level(new CD[2, 1]
                {
                    {new CD(Shapes.circle, Colors.cyan, Shapes.rectangle) },
                    {new CD(Shapes.rectangle, Colors.cyan) }
                });
                break;
            case 8:
                lvl = new Level(new CD[3, 1]
                {
                    {new CD(Shapes.circle, Colors.yellow, Shapes.rectangle, Colors.yellow) },
                    {new CD(Shapes.rectangle, Colors.yellow) },
                    {new CD(Shapes.rectangle, Colors.yellow, Shapes.circle, Colors.yellow) },
                });
                Education.StartEducation(seed);
                break;
            case 9:
                lvl = new Level(new CD[2, 2]
                {
                    {new CD(Shapes.rectangle, Colors.cyan, Shapes.heard , Colors.cyan),
                        new CD(Shapes.rectangle, Colors.cyan)   },
                    {new CD(Shapes.circle, Colors.cyan, Shapes.rectangle),
                        new CD(Shapes.heard, Colors.cyan, Shapes.circle) },
                });
                break;
            case 10:
                lvl = new Level(seed, countColor: 1, countShape: 5);
                break;
            case 11:
                lvl = new Level(new CD[3, 1] 
                {
                    {new CD(Shapes.rectangle, Colors.orange, Shapes.circle) },
                    {new CD(Shapes.rectangle, Colors.cyan) },
                    {new CD(Shapes.circle, Colors.orange, Shapes.rectangle) }
                });
                Education.StartEducation(seed);
                break;
            case 12:
                lvl = new Level(seed);
                Education.StartEducation(seed);
                break;
            case 20:
                lvl = new Level(seed, sizeX: 3, sizeY: 3, countColor: 4, countShape: 1);
                lvl.SetHiddenMode(true);
                Education.StartEducation(seed);
                break;
            case 25:
                lvl = new Level(seed, sizeX: 3, sizeY: 3, countColor: 3, countShape: 2);
                lvl.SetHiddenMode(true);
                break;
            case 40:
                lvl = new Level(seed, sizeX: 4, sizeY: 4, countColor: 4, countShape: 2);
                lvl.SetHiddenMode(true);
                break;

            default:
                if (seed % 9 == 0)
                {
                    lvl = new Level(seed + 48);
                    lvl.SetHiddenMode(false);
                }
                else
                {
                    lvl = new Level(seed);
                }
                
                break;
        }
        return lvl;
    }
}
