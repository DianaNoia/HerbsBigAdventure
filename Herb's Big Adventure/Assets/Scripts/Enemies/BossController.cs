using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class with all boss properties
public class BossController : MonoBehaviour
{   
    [SerializeField]
    private AudioManager am;

    private BossController bc;

    [SerializeField]
    private BossActivator ba;

    private GameManager gm;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private GameObject victoryZone;

    private float waitToShowExit = 5f;

    // Boss phases
    public enum BossPhase { intro, phase1, phase2, phase21, phase31, phase32, phase33, end };
    public BossPhase currentPhase = BossPhase.intro;
    
    [SerializeField]
    private int bossMusic, bossDeath, bossDeathShout, bossHit;

    public bool canTakeDamage;

    private void Awake()
    {
        bc = this;
    }

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Starts boss music
    void OnEnable()
    {
        am.PlayMusic(bossMusic);
    }

    // Update is called once per frame
    void Update()
    {
        // If the player dies and respawns
        if (gm.isRespawning)
        {
            currentPhase = BossPhase.intro;

            anim.SetBool("Phase1", false);
            anim.SetBool("Phase2", false);
            anim.SetBool("Phase21", false);
            anim.SetBool("Phase31", false);
            anim.SetBool("Phase32", false);
            anim.SetBool("Phase33", false);

            am.PlayMusic(am.levelMusicToPlay);

            // Resets boss to inactive
            gameObject.SetActive(false);

            // Resets the collider and the boss entrance to active
            ba.gameObject.SetActive(true);
            ba.entrance.SetActive(true);

            gm.isRespawning = false;
        }
    }

    public void DamageBoss()
    {
        am.PlaySFX(bossHit);

        currentPhase++;

        if (currentPhase != BossPhase.end)
        {
            anim.SetTrigger("Hurt");
        }

        switch (currentPhase)
        {
            case BossPhase.intro:
                anim.SetBool("Intro", true);
                break;

            case BossPhase.phase1:
                anim.SetBool("Phase1", true);
                anim.SetBool("Intro", false);
                canTakeDamage = true;
                break;

            case BossPhase.phase2:
                anim.SetBool("Phase2", true);
                anim.SetBool("Phase1", false);
                break;

            case BossPhase.phase21:
                anim.SetBool("Phase21", true);
                anim.SetBool("Phase2", false);
                break;

            case BossPhase.phase31:
                anim.SetBool("Phase31", true);
                anim.SetBool("Phase21", false);
                break;

            case BossPhase.phase32:
                anim.SetBool("Phase32", true);
                anim.SetBool("Phase31", false);
                break;

            case BossPhase.end:
                anim.SetTrigger("End");
                StartCoroutine(EndBoss());
                break;
        }
    }

    IEnumerator EndBoss()
    {
        am.PlaySFX(bossDeath);
        am.PlaySFX(bossDeathShout);
        am.PlayMusic(am.levelMusicToPlay);
        yield return new WaitForSeconds(waitToShowExit);
        victoryZone.SetActive(true);
    }
}
