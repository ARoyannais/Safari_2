using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    const int NMB_SEGMENT = 10;
    public static GameObject forme;
    public GameObject popUp;
    public static int stage;
    public GameObject cursor;
    Vector3 temp = new Vector3();
    PointData ihmTarget;
    List<PointData> segment;
    List<List<PointData>> ihmList;

	// Use this for initialization
	void Start () {
        stage = 0;
        ihmTarget = new PointData(0,0);
        segment = new List<PointData>();
        ihmList = new List<List<PointData>>();
	}

    // Update is called once per frame
    void Update()
    {
        if (Spawn.chooseForm == 1)
        {
            switch (stage)
            {
                case 1:
                    transform.position = PlacementCheckpoint('-', '-');
                    //TODO : list de points pour la trajectoire
                    SetIhmList();
                    break;

                case 2:
                    transform.position = PlacementCheckpoint('-', '+');
                    SetIhmList();
                    break;

                case 3:
                    transform.position = PlacementCheckpoint('+', '+');
                    SetIhmList();
                    break;

                case 4:
                    transform.position = PlacementCheckpoint('+', '-');
                    SetIhmList();
                    break;

                case 5:
                    Global_manager.score += (stage-1) * 10;
                    stage = 0;
                    forme.transform.position = new Vector3(-100, 0, 1);
                    Spawn.timeLeft = 0.0f;
                    break;

                default:
                    transform.position = PlacementCheckpoint('-', '-');
                    SetIhmList();
                    break;

            }

        }
        else if (Spawn.chooseForm == 2)
        {
            //placement checkpoint étoile
            if (stage <= 10)
            {
                Debug.Log(stage);
                transform.position = forme.transform.Find(stage.ToString()).position;
                SetIhmList();
            }else{
                transform.position = new Vector3(-10, -10, 0);
                forme.transform.position = new Vector3(-10, -10, 0);
                Global_manager.score += (stage-1) * 10;
                stage = 0;
                Spawn.timeLeft = 0.0f;
            }
        }
        //placement checkpoint Octogone
        else if (Spawn.chooseForm == 3)
        {
            if (stage <= 8)
            {
                Debug.Log(stage);
                transform.position = forme.transform.Find(stage.ToString()).position;
                SetIhmList();
            }else{
                transform.position = new Vector3(-10, -10, 0);
                forme.transform.position = new Vector3(-10, -10, 0);
                Global_manager.score += (stage-1) * 10;
                stage = 0;
                Spawn.timeLeft = 0.0f;
            }
        }
    }

    //Fonction de placement du checkpoint (seulement pour le carré)
    private Vector3 PlacementCheckpoint(char signe1, char signe2){
        Vector3 temp = new Vector3();
        
        if ((int)signe1 == '+'){
            temp.x = (popUp.transform.position.x + forme.GetComponent<BoxCollider2D>().bounds.size.x / 2);
        }
        else if ((int)signe1 == '-'){
            temp.x = (popUp.transform.position.x - forme.GetComponent<BoxCollider2D>().bounds.size.x / 2);
        }

        if ((int)signe2 == '+'){
            temp.y = (popUp.transform.position.y + forme.GetComponent<BoxCollider2D>().bounds.size.y / 2);
        }
        else if ((int)signe2 == '-'){
            temp.y = (popUp.transform.position.y - forme.GetComponent<BoxCollider2D>().bounds.size.y / 2);
        }
        temp.z = -9;
        return temp;
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "cursor"){
            stage += 1;
        }
    }


    //calcule la distance entre 2 points
    private float Pythagore(Vector3 a, Vector3 b)
    {
        //vérifie que c'est pas une droite verticale ou horizontale.
        if (a.x == b.x){
            return (Mathf.Abs(a.x) + Mathf.Abs(b.x));
        }else if(a.y == b.y){
            return (Mathf.Abs(a.y) + Mathf.Abs(b.y));
        }

        float ac;
        float bc;
        float ab;
        //crée le 3 point du triangle réctangle
        Vector2 c = new Vector2(a.x, b.y);

        //Calcule la longueur des cotés opposés
        if (a.x != c.x){
            ac = Mathf.Abs(a.x) + Mathf.Abs(c.x);
            bc = Mathf.Abs(b.y) + Mathf.Abs(c.y);
        }else{
            ac = Mathf.Abs(a.y) + Mathf.Abs(c.y);
            bc = Mathf.Abs(b.x) + Mathf.Abs(c.x);
        }

        //carré de l'hypothènuse
        ab = ac*ac + bc*bc;

        //renvoie l'hypothènuse (distance entre les 2 points)
        return Mathf.Sqrt(ab);
    }

    void SetIhmList()
        //calcule l'emplacement des points du segement et les met dans la list
    {
        for (int i = 0; i < NMB_SEGMENT; i++)
        {
            //calcule position du PointData
            temp = Vector3.Lerp(cursor.transform.position, transform.position, Pythagore(cursor.transform.position, transform.position) / (NMB_SEGMENT - i));
            //Debug.Log("temp.x = " + temp.x + " temp.y = " + temp.y);
            ihmTarget.Xd = temp.x;
            ihmTarget.Yd = temp.y;

            segment.Add(ihmTarget);

        }
        ihmList.Add(segment);
        Global_manager.ihm.SetTrajectory(ihmList);
    }
}
