using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BuildingData", menuName = "BuildingData")]
public class BuildingData : ScriptableObject
{
    public int minForgeScraps = 40;
    public int minForgeCoal = 30;

    public int minMaterialBaseScraps = 20;
    public int minMaterialBaseTitanium = 20;

    public int minTurretTitanium = 10;
    public int minTurretCoal = 10;

    public int minBodyPartsScraps = 10;
    public int minBodyPartsCoal = 5;

    public enum BuildingType
    {
        forge,
        materialBase,
        turret,
        stronghold
    };

}
