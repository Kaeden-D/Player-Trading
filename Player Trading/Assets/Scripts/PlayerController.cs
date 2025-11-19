using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{

    public float speed;
    private Rigidbody rig;
    private float startTime;
    private float timeTaken;
    private int collectablesPicked;
    public int maxCollectables = 10;
    private bool isPlaying;

    public GameObject[] collectables;
    public GameObject pusher;

    public GameObject playButton;
    public TextMeshProUGUI curTimeText;

    void Awake()
    {

        rig = GetComponent<Rigidbody>();

    }

    void Update()
    {

        if (!isPlaying)
            return;

        if(rig.position.y < 0)
        {
            End(1000000f);
        }

        float x = Input.GetAxis("Horizontal") * speed;
        float z = Input.GetAxis("Vertical") * speed;
        rig.linearVelocity = new Vector3(x, rig.linearVelocity.y, z);
        curTimeText.text = (Time.time - startTime).ToString("F2");

    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Collectable"))
        {
            collectablesPicked++;
            other.gameObject.SetActive(false);
            other.isTrigger = false;
            if (collectablesPicked == maxCollectables)
                End();
        }

    }

    public void Begin()
    {
        rig.position = new Vector3(0f, 0.5f, 1.5f);
        for (int i = 0; i < collectables.Length; i++)
        {
            collectables[i].gameObject.SetActive(true);
            collectables[i].GetComponent<Collider>().isTrigger = true;
        }
        pusher.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
        pusher.gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        startTime = Time.time;
        playButton.SetActive(false);
        isPlaying = true;
    }

    void End(float fail = 0.0f)
    {
        timeTaken = Time.time - startTime + fail;
        collectablesPicked = 0;
        isPlaying = false;
        playButton.SetActive(true);
        Leaderboard.instance.SetLeaderboardEntry(-Mathf.RoundToInt(timeTaken * 1000.0f));
    }

}
