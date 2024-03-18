using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AmmoManager : MonoBehaviour
{
    public Text turnDisplay;


    public Text TISHI;

    private AudioSource audioSource;
    public AudioSource trueammo;
    public AudioSource falseammo;


    public int rond;
    public Text rondtext;


    public int ammo;

    public GameObject enadgame;
    public Text WINGAME;
    // add by w
    public Text endGameBtnText;
    public Button endGameBtn;



    public Image enemyImage; // ����ͼ�������

    //public Text myHp;
    //public Text enemyHp;

    public GameObject hpUIItemPf;
    public Transform playerHPItemRoot;
    public Transform enemyHPItemRoot;
    GameObject[] myHp;
    GameObject[] enemyHp;


    public AmmoDisplay am;

    public BulletSlotsDisplay bl;

    public Button shootSelfButton; // ���Լ���ǹ�İ�ť
    public Button shootEnemyButton; // �Ե��˿�ǹ�İ�ť
    public Button shotSkyBtn;


    public GameObject playerPanel; // ��Ҳ������
    public int playerHealth = 3; // ���Ѫ��
    int enemyHealth = 3; // ����Ѫ��

    public int bulletCount { get; private set; }
    public List<bool> bulletTypes { get; private set; } // 
    /// <summary>
    /// 用于储存行动代号，0射击敌人、1射击玩家、2射击天空
    /// </summary>
    Queue<int> actions;
    public BulletSlotsDisplay bulletSlotsDisplay;
    [Header("thief")]
    public GameObject thiefPanel;
    public Image[] thiefPanelBullets;
    Button[] thiefPanelBulletBtns;
    public RectTransform thiefArea_two;
    public RectTransform thiefArea_three;
    public GameObject totalBlock;
    int twoareaindex;
    int threeareaindex;
    List<int> changeIndexs;
    bool thiefLiveBullet;

    private void Awake()
    {
        am.UpdateDisplay(); // 
        rond = 1;
        ammo = 2;

        actions = new Queue<int>(2);
        thiefPanelBulletBtns = new Button[thiefPanelBullets.Length];
        changeIndexs = new List<int>(2);
        for (int i = 0, length = thiefPanelBullets.Length; i < length; i++)
        {
            int index = i;
            thiefPanelBulletBtns[index] = thiefPanelBullets[index].GetComponent<Button>();
            thiefPanelBulletBtns[index].onClick.AddListener(() =>
            {
                thiefPanelBulletBtns[index].interactable = false;
                changeIndexs.Add(index);
                if (changeIndexs.Count == 2)
                    StartCoroutine(ChangeandThief());
            });
        }


        enadgame.SetActive(false);
        enemyImage.sprite = GameMain.Instance.CurrentEnemySpriteConfig.idle; // idle state of the joker
        GenerateAmmo();
        shootSelfButton.onClick.AddListener(() =>
        {
            if (GameMain.Instance.CurrentLevel != 1)
                Shoot(true);
            else
            {
                shootSelfButton.interactable = false;
                shootEnemyButton.interactable = false;

                actions.Enqueue(1);
                if (actions.Count >= 2)
                    StartCoroutine(ExecuteAction());
            }
        }); // shoot self
        shootEnemyButton.onClick.AddListener(() =>
        {
            if (GameMain.Instance.CurrentLevel != 1)
                Shoot(false);
            else
            {
                shootSelfButton.interactable = false;
                shootEnemyButton.interactable = false;

                actions.Enqueue(0);
                if (actions.Count >= 2)
                    StartCoroutine(ExecuteAction());
            }
        }); // shoot enemy
        shotSkyBtn.onClick.AddListener(() =>
        {
            shotSkyBtn.interactable = false;
            actions.Enqueue(2);
            if (actions.Count >= 2)
                StartCoroutine(ExecuteAction());
        });

        // add by w
        // init hp and create item
        enemyHealth = GameMain.Instance.CurrentEnemyHP;
        myHp = new GameObject[playerHealth];
        enemyHp = new GameObject[enemyHealth];
        for (int i = 0; i < playerHealth; i++)
        {
            myHp[i] = Instantiate(hpUIItemPf, playerHPItemRoot);
        }
        for (int i = 0; i < enemyHealth; i++)
        {
            enemyHp[i] = Instantiate(hpUIItemPf, enemyHPItemRoot);
        }

        // Audio source
        audioSource = GetComponent<AudioSource>();

        turnDisplay.text = "You Turn";
        shotSkyBtn.gameObject.SetActive(GameMain.Instance.CurrentLevel == 1);
    }
    private void Update()
    {
        for (var i = 0; i < myHp.Length; i++)
        {
            if (i < playerHealth)
            {
                myHp[i].SetActive(true);
            }
            else
            {
                myHp[i].SetActive(false);
            }
        }

        for (var i = 0; i < enemyHp.Length; i++)
        {
            if (i < enemyHealth)
            {
                enemyHp[i].SetActive(true);
            }
            else
            {
                enemyHp[i].SetActive(false);
            }
        }

        if (thiefPanel.activeSelf)
        {
            am.displayImage.gameObject.SetActive(false);
            bulletSlotsDisplay.bulletSlots[0].transform.parent.gameObject.SetActive(false);
        }

        //myHp.text = "Hp"+playerHealth;
        //enemyHp.text = "JokerHp" + enemyHealth;
    }

    IEnumerator ExecuteAction()
    {
        bool jumpEnemyTurn = false;
        shootSelfButton.interactable = true;
        shootEnemyButton.interactable = true;
        shotSkyBtn.interactable = true;

        while (actions.Count > 0)
        {
            if (bulletCount <= 0)
            {
                GenerateAmmo();
                am.UpdateDisplay();
                yield return new WaitForSeconds(1);
            }

            int actionIndex = actions.Dequeue();

            if (actionIndex == 2)
            {
                Shoot(false, false, false);
            }
            else
            {
                bool isLiveBullet = Shoot(actionIndex == 1, true, false);

                if (!isLiveBullet && actionIndex == 1)
                    jumpEnemyTurn = true;
            }
            yield return new WaitForSeconds(2);
        }

        // reward
        if (jumpEnemyTurn)
            playerPanel.SetActive(true);
        else
            StartCoroutine(EnemyTurn());
    }

    bool ShowThiefPanel()
    {
        if (bulletCount <= 3)
            return false;

        thiefPanel.SetActive(true);
        totalBlock.SetActive(false);
        shootEnemyButton.interactable = false;
        shootSelfButton.interactable = false;
        am.displayImage.gameObject.SetActive(false);
        bulletSlotsDisplay.bulletSlots[0].transform.parent.gameObject.SetActive(false);

        for (int i = bulletCount, length = thiefPanelBullets.Length; i < length; i++)
        {
            thiefPanelBullets[i].gameObject.SetActive(false);
        }
        for (int i = 0, length = bulletCount; i < length; i++)
        {
            thiefPanelBullets[i].color = Color.white;
            thiefPanelBullets[i].sprite = bulletTypes[i] ? bulletSlotsDisplay.liveBulletSprite : bulletSlotsDisplay.blankBulletSprite;
            thiefPanelBulletBtns[i].interactable = true;
            thiefPanelBullets[i].gameObject.SetActive(true);

        }

        thiefArea_two.gameObject.SetActive(bulletCount == 4);
        thiefArea_three.gameObject.SetActive(bulletCount > 4);
        twoareaindex = Random.Range(0, 3);
        threeareaindex = Random.Range(0, 4);
        thiefArea_two.anchoredPosition3D = new Vector3(80 * twoareaindex, -20, 0);
        thiefArea_three.anchoredPosition3D = new Vector3(80 * threeareaindex, -20, 0);

        return true;
    }

    IEnumerator ChangeandThief()
    {
        totalBlock.SetActive(true);
        yield return null;

        changeIndexs.Sort((a, b) => a.CompareTo(b));
        int index1 = changeIndexs[0];
        int index2 = changeIndexs[1];

        bulletTypes.Insert(index2, bulletTypes[index1]);
        bulletTypes.Insert(index1, bulletTypes[index2 + 1]);
        bulletTypes.RemoveAt(index1 + 1);
        bulletTypes.RemoveAt(index2 + 1);

        thiefPanelBullets[index1].sprite = bulletTypes[index1] ? bulletSlotsDisplay.liveBulletSprite : bulletSlotsDisplay.blankBulletSprite;
        thiefPanelBullets[index2].sprite = bulletTypes[index2] ? bulletSlotsDisplay.liveBulletSprite : bulletSlotsDisplay.blankBulletSprite;

        // tou qu 
        int thiefIndex = 0;
        if (bulletCount == 4)
        {
            thiefIndex = Random.Range(twoareaindex, twoareaindex + 2);
        }
        else
        {
            thiefIndex = Random.Range(threeareaindex, thiefIndex + 3);
        }
        thiefLiveBullet = bulletTypes[thiefIndex];
        bulletTypes.RemoveAt(thiefIndex);
        bulletCount -= 1;
        yield return null;
        while (thiefPanelBullets[thiefIndex].color.a > 0)
        {
            var originColor = thiefPanelBullets[thiefIndex].color;
            thiefPanelBullets[thiefIndex].color = new Color(originColor.r, originColor.g, originColor.b, originColor.a - 0.005f);
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        am.UpdateDisplay();
        thiefPanel.SetActive(false);
        shootEnemyButton.interactable = true;
        shootSelfButton.interactable = true;
        am.displayImage.gameObject.SetActive(true);
        bulletSlotsDisplay.bulletSlots[0].transform.parent.gameObject.SetActive(true);

        changeIndexs.Clear();
    }

    public void GenerateAmmo()
    {
        am.UpdateDisplay();
        rondtext.text = "Round" + rond;
        bulletCount = ammo;
        TISHI.text = ammo / 2 + " live. " + ammo / 2 + " blank.\nRandom placement of bullets.";
        if (rond <= 3)
        {
            rond += 1;
            ammo += 2; // ȷ���ӵ����������ʵ�����
            ammo = Mathf.Clamp(ammo, 1, 6);
        }
        bulletTypes = new List<bool>();

        //
        int halfBulletCount = bulletCount / 2;

        //
        for (int i = 0; i < halfBulletCount; i++)
        {
            bulletTypes.Add(true); //
        }

        //
        for (int i = 0; i < halfBulletCount; i++)
        {
            bulletTypes.Add(false); //
        }


        for (int i = 0; i < bulletCount; i++)
        {
            int randomIndex = Random.Range(0, bulletCount);
            bool temp = bulletTypes[i];
            bulletTypes[i] = bulletTypes[randomIndex];
            bulletTypes[randomIndex] = temp;
        }

        Debug.Log($"�����ӵ�: {bulletCount} ��, ʵ��: {bulletTypes.FindAll(b => b).Count}, �հ�: {bulletCount - bulletTypes.FindAll(b => b).Count}");

        StartCoroutine(UpdateDisplays());

    }


    public GameObject[] allEnemyAnimEffect;
    public GameObject[] allSelfAnimEffect;


    public void ShowAinm(bool isSelf)
    {
        StartCoroutine(IEnShowAinm(isSelf));
    }

    public IEnumerator IEnShowAinm(bool isSelf)
    {

        if (isSelf)
        {
            //allSelfAnimEffect[0].GetComponent<ObjectShake>().enabled = true;
            //allSelfAnimEffect[1].SetActive(true);
            //yield return new WaitForSeconds(0.5f);
            allSelfAnimEffect[0].SetActive(true);
            Camera.main.GetComponent<ObjectShake>().enabled = true;
            myHp[playerHealth - 1].GetComponent<Image>().color = Color.red;
            yield return new WaitForSeconds(0.5f);
            allSelfAnimEffect[0].SetActive(false);
            playerHealth--;
            //allSelfAnimEffect[1].SetActive(false);

        }
        else
        {
            allEnemyAnimEffect[0].GetComponent<ObjectShake>().enabled = true;
            allEnemyAnimEffect[0].GetComponent<Image>().color = Color.red;
            allEnemyAnimEffect[1].SetActive(true);
            enemyHp[enemyHealth - 1].GetComponent<Image>().color = Color.red;
            yield return new WaitForSeconds(0.3f);
            allEnemyAnimEffect[0].GetComponent<Image>().color = Color.white;
            enemyHealth--;
            allEnemyAnimEffect[1].SetActive(false);

        }


        if (playerHealth <= 0 || enemyHealth <= 0)
        {
            EndGame(); // �����Ϸ��������
        }
        yield break;
    }


    private bool Shoot(bool isSelf, bool willHurt = true, bool enterNextTurn = true)
    {
        turnDisplay.text = "You Turn";
        am.UpdateDisplay();
        Debug.Log($"{(isSelf ? "���" : "Joker")} ��ǹ");
        if (bulletCount <= 0) return false; // ����Ƿ����ӵ�
        playerPanel.SetActive(false); // �������ѡ��������رղ������

        int bulletIndex = Random.Range(0, bulletCount);
        bool isLiveBullet = bulletTypes[bulletIndex]; // ���ѡ�е��ӵ�����
        bulletTypes.RemoveAt(bulletIndex); // �Ƴ�ѡ�е��ӵ�
        bulletCount--; // �ӵ�����һ

        // ��¼��ҵ�ѡ��
        Debug.Log($"���ѡ����{(isSelf ? "���Լ�" : "��Joker")}��ǹ");
        Debug.Log($"��ǹ���: {(isLiveBullet ? "ʵ��" : "�հ�")}");

        if (isLiveBullet)
        {
            trueammo.Play();
            if (willHurt)
            {
                if (isSelf)
                {
                    ShowAinm(isSelf);
                    // ��Ҷ��Լ���ǹ���������Ѫ��
                    Debug.Log($"��Ҷ��Լ���ǹ����ǰѪ��: {playerHealth}");
                }
                else
                {
                    ShowAinm(isSelf);
                    // ��ҶԵ��˿�ǹ�����ٵ���Ѫ��
                    Debug.Log($"��ҶԵ��˿�ǹ��Joker��ǰѪ��: {enemyHealth}");
                }
            }
        }

        if (bulletCount <= 0)
        {
            GenerateAmmo(); // ����ӵ������ˣ���������

            if (enterNextTurn)
                playerPanel.SetActive(true); // �������һ���ӵ�ʱ����ʹ��ʵ��Ҳ���¼���������
        }
        else if (!isLiveBullet && isSelf)
        {
            falseammo.Play();

            if (enterNextTurn)
                playerPanel.SetActive(true); // �հ�������Ҷ��Լ���ǹ����ҿ����ٴ�ѡ��
        }
        else
        {
            falseammo.Play();

            if (enterNextTurn)
                StartCoroutine(EnemyTurn()); // ʵ��������˿�ǹ���л������˵Ļغ�
        }

        am.UpdateDisplay(); // �����ӵ�������ʾ

        if (playerHealth <= 0 || enemyHealth <= 0)
        {
            EndGame(); // �����Ϸ��������
            return false;
        }
        return isLiveBullet;
    }

    IEnumerator EnemyTurn()
    {
        int executeCount = GameMain.Instance.CurrentLevel == 1 ? 2 : 1;
        for (int i = 0; i < executeCount; i++)
        {
            turnDisplay.text = GameMain.Instance.CurrentEnemyName + "Turn";
            if (bulletCount <= 0)
            {
                GenerateAmmo();
                yield break;
            }
            if (playerHealth <= 0 || enemyHealth <= 0)
            {
                EndGame();
                yield break;
            }
            Debug.Log("JokerTurn");
            yield return new WaitForSeconds(1);
            int actionMax = executeCount == 2 ? 3 : 2;
            int actionIndex = Random.Range(0, actionMax);
            if (actionIndex == 0)
            {
                enemyImage.sprite = GameMain.Instance.CurrentEnemySpriteConfig.shotSelf; // �л������Լ���ǹ��Sprite
            }
            else if (actionIndex == 1)
            {
                enemyImage.sprite = GameMain.Instance.CurrentEnemySpriteConfig.shotOther; // �л�������ҿ�ǹ��Sprite
            }
            else
            {
                enemyImage.sprite = GameMain.Instance.CurrentEnemySpriteConfig.handUpGun; // �л�������ҿ�ǹ��Sprite
            }
            yield return new WaitForSeconds(1.5f);
            if (i == executeCount - 1)
                turnDisplay.text = "You Turn";
            enemyImage.sprite = GameMain.Instance.CurrentEnemySpriteConfig.idle;
            int bulletIndex = Random.Range(0, bulletCount);
            bool isLiveBullet = bulletTypes[bulletIndex];
            bulletTypes.RemoveAt(bulletIndex);
            bulletCount--;

            //Debug.Log($"Jokerѡ����{(enemyShootsSelf ? "���Լ�" : "�����")}��ǹ");

            if (isLiveBullet)
            {
                trueammo.Play();
                if (actionIndex == 0)
                {
                    ShowAinm(false);
                    //    enemyHealth--;

                }
                else if (actionIndex == 1)
                {
                    ShowAinm(true);
                    //  playerHealth--;

                }
            }
            else
            {
                falseammo.Play();
            }

            Debug.Log($"��ǹ���: {(isLiveBullet ? "ʵ��" : "�հ�")}, ��ǰѪ�� - ���: {playerHealth}, Joker: {enemyHealth}");

            yield return new WaitForSeconds(0.5f);

            if (playerHealth <= 0 || enemyHealth <= 0)
            {
                EndGame();
            }

            if (bulletCount <= 0) GenerateAmmo(); // ����ӵ������ˣ���������

            yield return new WaitForSeconds(1f);


            am.UpdateDisplay(); // �����ӵ�������ʾ
        }
        yield return new WaitForSeconds(0.5f);
        playerPanel.SetActive(true);
        if (GameMain.Instance.CurrentLevel == 2)
            ShowThiefPanel();
    }

    private void EndGame()
    {
        bool playerWin = playerHealth > 0;

        StopAllCoroutines();
        enadgame.SetActive(true);
        WINGAME.text = "Game Over. " + (!playerWin ? (GameMain.Instance.CurrentEnemyName + " Win!") : "Player Win!");
        playerPanel.SetActive(false);
        Debug.Log("��Ϸ����");
        Debug.Log("Game Over. " + (playerHealth <= 0 ? "Joker Wins!" : "Player Wins!"));
        Debug.Log($"����Ѫ�� - ���: {playerHealth}, Joker: {enemyHealth}");

        endGameBtnText.text = playerWin ? (GameMain.Instance.IsFinalLevel ? "Back To Start" : "Next Level") : "Play Again";
        endGameBtn.onClick.AddListener(() =>
        {
            if (playerWin)
            {
                if (GameMain.Instance.IsFinalLevel)
                {

                    SceneManager.LoadScene(0);
                }
                else
                {
                    GameMain.Instance.OnEnterNextLevel();

                    SceneManager.LoadScene(1);
                }
            }
            else
            {
                SceneManager.LoadScene(1);
            }
        });
    }
    IEnumerator UpdateDisplays()
    {
        turnDisplay.text = "Your Turn";
        bl.lunpan.SetActive(false);
        bl.UpdateDisplay(); // �����ӵ�����ʾ
        am.UpdateDisplay();
        yield return new WaitForSeconds(1f);
        audioSource.Play();
        yield return new WaitForSeconds(0.5f);
        TISHI.text = " Who do you want to shoot?\n-Shooting yourself with blanks will skip the Joker's turn.";

        if (GameMain.Instance.CurrentLevel == 2)
        {
            ShowThiefPanel();
        }
    }


}
