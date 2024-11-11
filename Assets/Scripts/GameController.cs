using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SearchService;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEditor.Progress;

public class GameController : MonoBehaviour
{
    private Dictionary<Color, string> Color_Effects = new Dictionary<Color, string>() { {Color.blue,"lessFriction"}, { Color.yellow, "asteroid" },
                                                                            { Color.green, "decrease" }, { Color.red, "dodge" },{Color.white,"Nothing" },{Color.black,"Nothing" } };    

    private AudioSource audioSource;
    public AudioClip battleMusic;
    public AudioClip menuMusic;

    private Grid currentBoard;
    private GameObject player;
    private GameObject enemy;

    [SerializeField] float interval;
    private float time;

    int difficulty;
    int counter;
    int counter2;

    bool isCurrentWave = false;
    bool CreatingWaves = false;
    bool ShopOpen = false;

    private string[] Effects = new string[3] {"","",""};
    private bool dodgeMode = false;

    private bool Dead = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(battleMusic);
        currentBoard = FindObjectOfType<Grid>();
        GameObject[] temp = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject go in temp)
        {
            if (go.name == "Player")
            {
                player = go;
            }
            if (go.name == "Enemy")
            {
                enemy = go;
            }
        }
        difficulty = 0;
        counter = 0;
        counter2 = 0;
        GenExplosive();
        //currentBoard.GetComponent<BoardControl>().NextStage();
        
    }

    void Update()
    {
        if (!Dead)
        {
            if (player.GetComponent<PlayerController>().GetHealth() == 0)
            {
                Death();
            }
            else
            {
                UpdateEffects();
                UpdateInterval();
                UpdateWave();
            }
        }
    }

    private void UpdateEffects()
    {
        bool update = false;
        if (Effects[0] != Color_Effects[currentBoard.GetComponent<BoardControl>().CurrentColours()[0]] )
        {
            update = true;
        }
        if (Effects[1] != Color_Effects[currentBoard.GetComponent<BoardControl>().CurrentColours()[1]])
        {
            update = true;
        }
        if (update)
        {
            DeRunEffects();
            Effects[0] = Color_Effects[currentBoard.GetComponent<BoardControl>().CurrentColours()[0]];
            Effects[1] = Color_Effects[currentBoard.GetComponent<BoardControl>().CurrentColours()[1]];
            RunEffects();
        }
        

    }
    private void RunEffects()
    {
        if (Effects.Contains("lessFriction"))
        {
            player.GetComponent<PlayerController>().SetFriction(player.GetComponent<PlayerController>().GetFriction()-1);
        }
        if (Effects.Contains("decrease"))
        {
            currentBoard.GetComponent<BoardControl>().DecreaseSize();
        }
        if (Effects.Contains("dodge"))
        {
            dodgeMode = true;
        }
        if (Effects.Contains("asteroid"))
        {
            //GenAsteroid();
        }
        if (Effects.Contains("Shotgun"))
        {
            player.GetComponent<PlayerController>().GetShotgun();
        }
        if (Effects.Contains("Sniper"))
        {
            player.GetComponent<PlayerController>().GetSniper();
        }
        if (Effects.Contains("Piercing"))
        {
            player.GetComponent<PlayerController>().SetPiercing(true);
        }
        if (Effects.Contains("Tophat"))
        {
            player.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("SpritesPNG/Tophat");
        }
        if (Effects.Contains("VetoBlue"))
        {
            currentBoard.GetComponent<BoardControl>().VetoColor(Color.blue);
        }
        if (Effects.Contains("VetoYellow"))
        {
            currentBoard.GetComponent<BoardControl>().VetoColor(Color.yellow);
        }
        if (Effects.Contains("VetoRed"))
        {
            currentBoard.GetComponent<BoardControl>().VetoColor(Color.red);
        }
        if (Effects.Contains("VetoGreen"))
        {
            currentBoard.GetComponent<BoardControl>().VetoColor(Color.green);
        }
        if (Effects.Contains("BoardExpansion"))
        {
            currentBoard.GetComponent<BoardControl>().IncreaseSize(); ;
        }
        if (Effects.Contains("Wall"))
        {
            GenWall();
        }
        if (Effects.Contains("Explosives"))
        {
            GenExplosive(); 
        }

    }

    private void DeRunEffects()
    {
        if (Effects.Contains("lessFriction"))
        {
            player.GetComponent<PlayerController>().SetFriction(player.GetComponent<PlayerController>().GetFriction() + 1);
        }
        if (Effects.Contains("decrease"))
        {
            currentBoard.GetComponent<BoardControl>().IncreaseSize();
        }
        if (Effects.Contains("dodge"))
        {
            dodgeMode = false;
        }
        if (Effects.Contains("asteroid"))
        {
            //GenExploded();
        }
        if (Effects.Contains("VetoBlue"))
        {
            currentBoard.GetComponent<BoardControl>().AddColor(Color.blue);
        }
        if (Effects.Contains("VetoYellow"))
        {
            currentBoard.GetComponent<BoardControl>().AddColor(Color.yellow);
        }
        if (Effects.Contains("VetoRed"))
        {
            currentBoard.GetComponent<BoardControl>().AddColor(Color.red);
        }
        if (Effects.Contains("VetoGreen"))
        {
            currentBoard.GetComponent<BoardControl>().AddColor(Color.green);
        }
        if (Effects.Contains("BoardExpansion"))
        {
            currentBoard.GetComponent<BoardControl>().DecreaseSize(); ;
        }
        if (Effects.Contains("Wall"))
        {
            RemoveGO("Wall");
        }
        if (Effects.Contains("Explosives"))
        {
            RemoveGO("Explosive");
        }
    }
    private void GenAsteroid()
    {
        int[] size = currentBoard.GetComponent<BoardControl>().GetSize();
        CreateSprite("X-Mark", "XSprite", new Vector3(Random.Range(size[0], size[1]), Random.Range(size[2], size[3])));
    }
    private void GenExploded()
    {
        GameObject[] g = GameObject.FindGameObjectsWithTag("Asteroid");
        g[0].AddComponent<Expolsion>();
        g[0].GetComponent<Expolsion>().SetCenter(g[0].transform.position);
        g[0].GetComponent<Expolsion>().Explode();
    }

    private void GenWall(int x = 1000, int y = 1000)
    {
        int[] size = currentBoard.GetComponent<BoardControl>().GetSize();
        if (x == 1000 && y == 1000)
        {
            x = Random.Range(size[0], size[1]);
            y = Random.Range(size[2], size[3]);
        }
        CreateSprite("Wall", "Wall", new Vector3(x,y));
    }
    
    private void GenExplosive()
    {
        int[] size = currentBoard.GetComponent<BoardControl>().GetSize();
        Instantiate(Resources.Load("Explosive") as GameObject);

    }

    private void RemoveGO(string tag)
    {
        GameObject[] g = GameObject.FindGameObjectsWithTag(tag);
        foreach(GameObject g2 in g)
        {
            GameObject.Destroy(g2);
        }
    }

    private void UpdateWave()
    {
        if (ShopOpen == true)
        {
            if (player.GetComponent<PlayerController>().GetItem() != "")
            {
                Effects[2] = player.GetComponent<PlayerController>().GetItem();
                player.GetComponent<PlayerController>().ResetItem();
                currentBoard.GetComponent<BoardControl>().CloseShop();
                currentBoard.GetComponent<BoardControl>().NextStage();
            }
        }
        else
        {
            if (!CreatingWaves)
            {
                if (counter2 == (difficulty + 1) * 2)
                {
                    if (!isCurrentWave)
                    {
                        counter++;
                        if (counter % 5 == 0)
                        {
                            ShopOpen = true;
                            currentBoard.GetComponent<BoardControl>().Shop();
                        }
                        if (counter % 10 == 0)
                        {
                            difficulty++;
                        }
                        else
                        {
                            currentBoard.GetComponent<BoardControl>().NextRound();
                        }
                        StartCoroutine(WaveTime());
                        counter2 = 0;

                    }
                }
                else
                {
                    enemy.GetComponent<ShurikenSpawn>().NextWave(counter % 5, dodgeMode);
                }
                StartCoroutine(WaveTime2());
            }
        }
    }

    private IEnumerator WaveTime()
    {
        isCurrentWave = true;
        yield return new WaitForSeconds(interval);
        isCurrentWave = false;
    }
    private IEnumerator WaveTime2()
    {
        CreatingWaves = true;
        yield return new WaitForSeconds(interval/2);
        CreatingWaves = false;
    }

    private void Death()
    {
        Destroy(player);
        Dead = true;
    }

    private void UpdateInterval()
    {
        if (difficulty == 0)
        {
            interval = 10f;
        }
        else if (difficulty == 1)
        {
            interval = 5f;
        }
        else if (difficulty == 2)
        {
            interval = 3f;
        }
        else
        {
            interval = 1f;
        }
    }

    private void CreateSprite(string name, string item, Vector3 pos)
    {
        Sprite s = Resources.Load<Sprite>("SpritesPNG/" + item);
        var new_sprite = new GameObject();
        new_sprite.name = name;
        new_sprite.AddComponent<SpriteRenderer>();
        if (name == "Wall")
        {
            new_sprite.tag = "Wall";
        }
        if (name == "Explosive")
        {
            new_sprite.tag = "Explosive";
            new_sprite.AddComponent<Expolsion>();
        }
        if (name == "X-Mark")
        {
            new_sprite.tag = "Asteroid";
        }
        var ui_renderer = new_sprite.GetComponent<SpriteRenderer>();
        ui_renderer.sprite = s; // Change to load the sprite file

        new_sprite.transform.localPosition = pos;
        new_sprite.transform.localScale = new Vector3(5, 5, 0);
    }
}
