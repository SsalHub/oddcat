using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] AudioClip dashSound;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip gameOverSound;
    [SerializeField] GameObject guide;
    [SerializeField] float speed;

    public int HP;

    private UIManager uiManager;
    private float h, v;
    private bool isDash;
    private bool isImmuned;
    private AudioSource audioSource;
    RectTransform guideTransform;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
        HP = 3;
        isDash = false;
        isImmuned = false;
        audioSource = GetComponent<AudioSource>();
        guideTransform = guide.GetComponent<RectTransform>();
        StartCoroutine(countScore());
        StartCoroutine(showGuide());
    }

    // Update is called once per frame
    void Update()
    {
        if (isDash)
            return;

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        rotate(h, v);
        move(h, v);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(dash(h, v));
        }
    }

    private IEnumerator showGuide()
    {
        float time = 0;
        guide.SetActive(true);
        while (true)
        {
            guideTransform.position = transform.position + new Vector3(0f, 0.9f, 0);
            time += Time.deltaTime;
            if (5f < time)
                break;
            yield return new WaitForEndOfFrame();
        }
        guide.SetActive(false);
    }

    private IEnumerator countScore()
    {
        yield return new WaitForSeconds(3f);
        yield return new WaitForSeconds(1f);
        while (0 < HP)
        {
            GameMgr.GetIns._Score += 10;
            yield return new WaitForSeconds(0.5f);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!isImmuned && col.gameObject.CompareTag("Bullet"))
        {
            Destroy(col.gameObject);
            StartCoroutine(runHitEvent());
        }
    }

    public void move(float h, float v)
    {
        /*if (h < 0)
        {
            sp.flipX = true;
            pAnimator.SetBool("Move_H", true);
        }
        else if (0 < h)
        {
            sp.flipX = false;
            pAnimator.SetBool("Move_H", true);
        }
        else if (h == 0)
        {
            pAnimator.SetBool("Move_H", false);
        }*/

        /*if (v < 0)
        {
            pAnimator.SetInteger("Move_V", -1);
        }
        else if (0 < v)
        {
            pAnimator.SetInteger("Move_V", 1);
        }
        else if (v == 0)
        {
            pAnimator.SetInteger("Move_V", 0);
        }*/

        // can't escape from circle on center
        transform.position += new Vector3(h, v, 0) * speed * Time.deltaTime;
        setPosInScreen();
    }

    private IEnumerator dash(float h, float v)
    {
        audioSource.clip = dashSound;
        audioSource.Play();
        if (!isMoveKeyDown()) yield break;
        isDash = true;
        Vector3 unit = new Vector3(h, v, 0).normalized;
        float dashSpeed = 30f;

        while (speed < dashSpeed)
        {
            transform.position += unit * dashSpeed * Time.deltaTime;
            setPosInScreen();
            dashSpeed -= 180f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        isDash = false;
    }

    private void rotate(float h, float v)
    {
        if (h == 0 && v == 0) return;
        if (!isMoveKeyDown()) return;
        Vector3 dir = new Vector3(v, h * -1, 0).normalized;
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    }

    private bool isMoveKeyDown()
    {
        return Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
    }

    private IEnumerator runHitEvent()
    {
        audioSource.clip = hitSound;
        audioSource.Play();
        uiManager.minusLife();
        HP--;
        if (HP <= 0)
        {
            gameOver();
            yield break;
        }

        isImmuned = true;
        for (int i = 0; i < 20; i++)
        {
            setAlpha(0.5f);
            yield return new WaitForSeconds(0.1f);

            setAlpha(1f);
            yield return new WaitForSeconds(0.1f);
        }
        isImmuned = false;
    }

    private void gameOver()
    {
        audioSource.clip = gameOverSound;
        audioSource.Play();
        setAlpha(0f);
        isImmuned = true;
        uiManager.showGameOverScreen();
    }

    private void setAlpha(float a)
    {
        SpriteRenderer sp = GetComponent<SpriteRenderer>();
        Color c = sp.color;
        c.a = a;
        sp.color = c;
    }

    private void setPosInScreen()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        if (pos.x < 0f) pos.x = 0f;
        if (pos.x > 1f) pos.x = 1f;
        if (pos.y < 0f) pos.y = 0f;
        if (pos.y > 1f) pos.y = 1f;

        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}
