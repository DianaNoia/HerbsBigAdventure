using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public static BossController instance;

    private GameManager gm;
    private AudioManager am;

    public Animator anim;

    public GameObject victoryZone;
    public float waitToShowExit;

    public enum BossPhase { intro, phase1, phase2, phase21, phase31, phase32, phase33, end };
    public BossPhase currentPhase = BossPhase.intro;

    public int bossMusic, bossDeath, bossDeathShout, bossHit;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        am.PlayMusic(bossMusic);
    }

    // Update is called once per frame
    void Update()
    {
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

            gameObject.SetActive(false);

            BossActivator.instance.gameObject.SetActive(true);
            BossActivator.instance.entrance.SetActive(true);

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
