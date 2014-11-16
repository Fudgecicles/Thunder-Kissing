using UnityEngine;
using System.Collections;

public class GUIHandler : Photon.MonoBehaviour
{

    TextMesh[] texts;
    GameObject state;
    GameObject score;
    GameObject arrow;
    GameObject timer;
    GameObject end;
    Camera cam;
    GameObject camContainer;
    public float timeLeft = 600;


    // Use this for initialization
    void Start()
    {
        
        camContainer = transform.Find("Main Camera").gameObject;
        cam = camContainer.camera;
        end = camContainer.transform.Find("end").gameObject;
        state = camContainer.transform.FindChild("State").gameObject;
        score = camContainer.transform.FindChild("Score").gameObject;
        arrow = camContainer.transform.FindChild("Arrows").gameObject;
        timer = camContainer.transform.Find("Timer").gameObject;
        texts = GetComponentsInChildren<TextMesh>();
        Debug.Log(cam.pixelHeight);
        state.transform.position = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight - 20, 3));
        score.transform.position = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight - 40, 3));
        arrow.transform.position = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight - 60, 3));
        timer.transform.position = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth / 2, cam.pixelHeight - 20, 3));

    }

    public void updateArrows(int arrows)
    {
        texts[2].text = "Arrows: " + arrows;
    }

    public void updateState(bool cupid)
    {
        if (cupid)
        {
            texts[0].text = "Cupid";
        }
        else
        {
            texts[0].text = "Lover";
        }
    }

    public void updateScore(int score)
    {
        texts[1].text = "Score: " + score;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                texts[3].text = "Timeleft: " + (int)timeLeft;
            }
            else
            {
                end.SetActive(true);
            }
        }
        
    }
}
