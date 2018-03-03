using UnityEngine;

[System.Serializable]
public class MatchSettings {

    public float respawnTime = 3f;
    public int barrelNum = 10;
    public Vector3 floorScale = new Vector3(10, 1, 10);
    public float thrusterForce = 1200f;
    public float thrusterFuelBurnSpeed = 1f;
    public float thrusterFuelRegenSpeed = 0.3f;
    public float jointMaxForce = 20f;

    public MatchSettings(int _opt)
    {
        ChooseLevel(_opt);
    }
    public MatchSettings()
    {
        ChooseLevel(1);
    }

    public void ChooseLevel(int _option)
    {
        switch (_option)
        {
            case 1:
                Lvl1();
                break;
            case 2:
                Lvl2();
                break;
            case 3:
                Lvl3();
                break;
            default:
                Lvl1();
                break;
        }
    }

    void Lvl1()
    {
        barrelNum = 10;
        floorScale = new Vector3(10, 1, 10);
    }

    void Lvl2()
    {
        barrelNum = 20;
        floorScale = new Vector3(15, 1, 15);
    }

    void Lvl3()
    {
        // Aerial Combat
        thrusterForce = 1600f;
        thrusterFuelBurnSpeed = 0.4f;
        thrusterFuelRegenSpeed = 0.8f;
        jointMaxForce = 30f;
        barrelNum = 30;
        floorScale = new Vector3(20, 1, 20);
    }
}
