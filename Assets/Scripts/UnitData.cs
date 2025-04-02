using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UnitData", menuName = "UnitData")]
public class UnitData : ScriptableObject
{

    public int hp;
    public int power;
    public int speed;

    public int minFighterScraps = 5;
    public int minFighterBodyParts = 1;

    public int minFlyingScraps = 10;
    public int minFlyingCoal = 4;
    public int minFlyingBodyParts = 2;

    public int minScavangerScraps = 8;
    public int minScavangerTitanium = 5;
    public int minScavangerBodyParts = 2;

    public int minMedicTitanium = 10;
    public int minMedicBodyParts = 1;

 
    public enum UnitStates{
         Idle,
         Moving,
         Attacking,
         Interacting //Can be with either a doodad or a resource point
    }

    public enum UnitType
    {
        fighter,
        medic,
        scavanger,
        flying
    };
}
