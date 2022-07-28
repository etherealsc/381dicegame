using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Dice : MonoBehaviour
{
    public int value;
    public SpriteRenderer face;
    public List<Sprite> faces = new List<Sprite>();
    public bool locked;
    public bool finishedRolling;
    private Rigidbody2D rb;
    public BoxCollider2D boxCollider;
    public Image icon;

    public int hp;
    public int hpMax;
    public int attack;
    public int defense;

    public BattleController battleController;
    public Image hpBar;

    // Start is called before the first frame update
    void Awake()
    {
        face = gameObject.GetComponent<SpriteRenderer>();
        battleController = GameObject.Find("ArenaManager").GetComponent<BattleController>();
        lineRenderer = GetComponent<LineRenderer>();
        value = Random.Range(0, 6);
        face.sprite = faces[value];
        rb = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        StartCoroutine(Rolling());
        dead = false;
        locked = false;
        cheatDiceValue = -1;
        hpBar = gameObject.transform.GetChild(0).GetComponentInChildren<Image>();
        icon.sprite = face.sprite;

    }

    public bool dead;
    // Update is called once per frame
    void Update()
    {
        if(hp <= 0)
        {
            hpBar.fillAmount = 0f;
        }
        else
        {
            hpBar.fillAmount = ((hp*1f) / (hpMax*1f));

        }

        speed = rb.velocity.magnitude;
        if(rb.velocity.magnitude < 1)
        {
            
            if (gameObject.tag == "EnemyDice" && cheatDiceValue > -1 && icon != null)
            {
                value = cheatDiceValue;
                face.sprite = faces[value];
                icon.sprite = face.sprite;

                //locked = true;
            }
            finishedRolling = true;
        }
        else
        {
            finishedRolling = false;
        }

        if(rb.velocity.magnitude < 0.1f)
        {
            rb.velocity = new Vector2(0, 0);


        }

        if (hp <= 0 && battleController.battleState == BattleController.BattleState.BATTLING && !dead)
        {
            diceClass = Class.NONE;
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
            if(gameObject.tag == "PlayerDice")
            {
                battleController.playerDice.Remove(this.gameObject);
            }
            else
            {
                battleController.enemyDice.Remove(this.gameObject);
            }
            battleController.deadDice.Add(this.gameObject);
            face.color = new Color(face.color.r, face.color.g, face.color.b, 100);
            dead = true;
        }
        switch (diceClass)
        {
            case Class.NONE:
                canAttack = false;
                break;

            case Class.ROGUE:
                Rogue();
                break;

            case Class.CLERIC:
                Cleric();
                break;

            case Class.FIGHTER:
                Fighter();
                break;

            case Class.MONK:
                Monk();
                break;

            case Class.WIZARD:
                Wizard();
                break;

            case Class.BARBARIAN:
                Barbarian();
                break;
        }
    }

    public Class originalClass;
    public float speed;
    public IEnumerator Rolling()
    {
        while (true)
        {
            if (!finishedRolling && !locked && battleController.battleState != BattleController.BattleState.BATTLING && icon != null)
            {
                yield return new WaitForSeconds(0.2f);
                value = Random.Range(0, 6);
                face.sprite = faces[value];
                icon.sprite = face.sprite;
            }
            yield return null;
        }

    }

    public void Lock()
    {
        locked = !locked;
    }


    public int cheatDiceValue;

    public enum Class
    {
        NONE,
        ROGUE,
        CLERIC,
        FIGHTER,
        MONK,
        WIZARD,
        BARBARIAN
    }

    public Class diceClass;

    public void AssignClass()
    {
        if(value == 0)
        {
            diceClass = Class.ROGUE;
            cooldownMaxTime = 3f;
            cooldownTimer = 1f;
            moveSpeed = 5f;
        }
        else if(value == 1)
        {
            diceClass = Class.CLERIC;
            cooldownMaxTime = 2.25f;
            cooldownTimer = 2f;
            moveSpeed = 3f;
        }
        else if (value == 2)
        {
            diceClass = Class.FIGHTER;
            cooldownMaxTime = 1.5f;
            cooldownTimer = 1f;
            moveSpeed = 3f;
        }
        else if (value == 3)
        {
            diceClass = Class.MONK;
            cooldownMaxTime = 3f;
            cooldownTimer = 2f;
            moveSpeed = 4f;
        }
        else if (value == 4)
        {
            diceClass = Class.WIZARD;
            cooldownMaxTime = 2.25f;
            cooldownTimer = 1.5f;
            moveSpeed = 1f;
        }
        else if (value == 5)
        {
            diceClass = Class.BARBARIAN;
            cooldownMaxTime = 4.5f;
            cooldownTimer = 3f;
            moveSpeed = 1f;
        }
        else
        {
            diceClass = Class.NONE;
        }

        originalClass = diceClass;
    }

    public bool isfighting;
    public bool canAttack;
    public float cooldownMaxTime;
    public float cooldownTimer;
    public GameObject target;
    public float moveSpeed;
    public void Rogue()
    {
        float step = moveSpeed * Time.deltaTime;

        if (cooldownTimer <= 0)
        {
            canAttack = true;
            if(gameObject.tag == "PlayerDice" && battleController.enemyDice.Count > 0)
            {
                target = battleController.enemyDice[Random.Range(0, battleController.enemyDice.Count)];
            }
            else if(gameObject.tag == "EnemyDice" && battleController.playerDice.Count > 0)
            {
                target = battleController.playerDice[Random.Range(0, battleController.playerDice.Count)];
            }
            cooldownTimer = cooldownMaxTime;
        }
        else if (cooldownTimer > 0 && canAttack == false)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (canAttack && battleController.battleState != BattleController.BattleState.POSTCOMBAT && (target != null && target.GetComponent<Dice>().hp > 0))
        {
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, target.transform.position, step);
        }
        else if (target != null && target.GetComponent<Dice>().hp <= 0)
        {
                if (gameObject.tag == "PlayerDice" && battleController.enemyDice.Count > 0)
                {
                    target = battleController.enemyDice[Random.Range(0, battleController.enemyDice.Count)];
                }
                else if (gameObject.tag == "EnemyDice" && battleController.playerDice.Count > 0)
                {
                    target = battleController.playerDice[Random.Range(0, battleController.playerDice.Count)];
                }
            
        }
    }





    public void Cleric()
    {
        float step = moveSpeed * Time.deltaTime;

        if (cooldownTimer <= 0)
        {
            canAttack = true;
            if (gameObject.tag == "PlayerDice" && battleController.playerDice.Count > 1)
            {
                target = battleController.playerDice[Random.Range(0, battleController.playerDice.Count)];
            }
            else if (gameObject.tag == "EnemyDice" && battleController.enemyDice.Count > 0)
            {
                target = battleController.enemyDice[Random.Range(0, battleController.enemyDice.Count)];
            }
            cooldownTimer = cooldownMaxTime;
        }
        else if (cooldownTimer > 0 && canAttack == false)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (canAttack && battleController.battleState != BattleController.BattleState.POSTCOMBAT)
        {
            if (target == gameObject)
            {
                Heal(gameObject);
                canAttack = false;
            }
            else
            {
                gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, target.transform.position, step);
                if(Vector2.Distance(gameObject.transform.position, target.transform.position) < 1f)
                {
                    Heal(target);
                    canAttack = false;
                }
            }
        }
    }

    public void Heal(GameObject healTarget)
    {
        healTarget.GetComponent<Dice>().hp += attack;
        //add something here for heal effects
    }


    public void Fighter()
    {
        float step = moveSpeed * Time.deltaTime;

        if (cooldownTimer <= 0)
        {
            canAttack = true;
            if (gameObject.tag == "PlayerDice" && battleController.enemyDice.Count > 0)
            {
                target = battleController.enemyDice[Random.Range(0, battleController.enemyDice.Count)];
            }
            else if (gameObject.tag == "EnemyDice" && battleController.playerDice.Count > 0)
            {
                target = battleController.playerDice[Random.Range(0, battleController.playerDice.Count)];
            }
            cooldownTimer = cooldownMaxTime;
        }
        else if (cooldownTimer > 0 && canAttack == false)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (canAttack && battleController.battleState != BattleController.BattleState.POSTCOMBAT)
        {
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, target.transform.position, step);
        }
    }

    public void Monk()
    {
        float step = moveSpeed * Time.deltaTime;

        if (cooldownTimer <= 0)
        {
            canAttack = true;
            if (gameObject.tag == "PlayerDice" && battleController.enemyDice.Count > 0)
            {
                target = battleController.enemyDice[Random.Range(0, battleController.enemyDice.Count)];
            }
            else if (gameObject.tag == "EnemyDice" && battleController.playerDice.Count > 0)
            {
                target = battleController.playerDice[Random.Range(0, battleController.playerDice.Count)];
            }
            cooldownTimer = cooldownMaxTime;
        }
        else if (cooldownTimer > 0 && canAttack == false)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (canAttack && battleController.battleState != BattleController.BattleState.POSTCOMBAT)
        {
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, target.transform.position, step);
        }


    }

    public void MonkAOE()
    {
        {
            Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, 2f);
            foreach (var hitCollider in hitColliders)
            {
                if(gameObject.tag == "PlayerDice" && hitCollider.gameObject.tag == "EnemyDice" && hitCollider.gameObject.GetComponent<Dice>().hp > 0 ||
                    hitCollider.gameObject.tag == "PlayerDice" && gameObject.tag == "EnemyDice" && hitCollider.gameObject.GetComponent<Dice>().hp > 0)
                {
                    var target = hitCollider.gameObject.GetComponent<Dice>();

                    target.hp -= Mathf.Max((attack - target.defense), 1);
                    target.gameObject.GetComponent<Rigidbody2D>().AddForce(50 * Mathf.Max(attack - target.defense, 1) * (target.gameObject.transform.position - gameObject.transform.position).normalized);

                }
            }
        }
    }

    public LineRenderer lineRenderer;
    public void Wizard()
    {
        float step = moveSpeed * Time.deltaTime;

        if (cooldownTimer <= 0)
        {
            canAttack = true;
            if (gameObject.tag == "PlayerDice" && battleController.enemyDice.Count > 0)
            {
                target = battleController.enemyDice[Random.Range(0, battleController.enemyDice.Count)];
            }
            else if (gameObject.tag == "EnemyDice" && battleController.playerDice.Count > 0)
            {
                target = battleController.playerDice[Random.Range(0, battleController.playerDice.Count)];
            }
            cooldownTimer = cooldownMaxTime;
        }
        else if (cooldownTimer > 0 && canAttack == false)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (canAttack && battleController.battleState != BattleController.BattleState.POSTCOMBAT)
        {
            LightningBolt(target);
            canAttack = false;
            //gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, target.transform.position, step);
        }
    }

    public void LightningBolt(GameObject target)
    {
        Vector3[] positions = new Vector3[2] { gameObject.transform.position, target.transform.position };
        lineRenderer.SetWidth(0.1f, 0.1f);
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(positions);
        StartCoroutine(LightningFade());

        var hitTarget = target.gameObject.GetComponent<Dice>();

        hitTarget.hp -= Mathf.Max((attack - hitTarget.defense), 1);

    }

    public IEnumerator LightningFade()
    {
        yield return new WaitForSeconds(0.3f);
        lineRenderer.positionCount = 0;
    }

    public void Barbarian()
    {
        float step = moveSpeed * Time.deltaTime;
        canAttack = true;

        if (cooldownTimer <= 0)
        {
            if (gameObject.tag == "PlayerDice" && battleController.enemyDice.Count > 0)
            {
                
                target = battleController.enemyDice[Random.Range(0, battleController.enemyDice.Count)];
                rb.AddForce((target.gameObject.transform.position - gameObject.transform.position).normalized * 1000f);
            }
            else if (gameObject.tag == "EnemyDice" && battleController.playerDice.Count > 0)
            {
                target = battleController.playerDice[Random.Range(0, battleController.playerDice.Count)];
                rb.AddForce((target.gameObject.transform.position - gameObject.transform.position).normalized * 1000f);

            }
            cooldownTimer = cooldownMaxTime;
        }
        else if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (canAttack && battleController.battleState != BattleController.BattleState.POSTCOMBAT)
        {
            //gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, target.transform.position, step);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canAttack && gameObject.tag == "PlayerDice" && collision.gameObject.tag == "EnemyDice" && collision.gameObject.GetComponent<Dice>().hp > 0 || canAttack && (collision.gameObject.tag == "PlayerDice" && gameObject.tag == "EnemyDice" && collision.gameObject.GetComponent<Dice>().hp > 0))
        {
            var target = collision.gameObject.GetComponent<Dice>();
            GameManager.instance.sFXSource.PlayOneShot(GameManager.instance.diceNoises[Random.Range(0, GameManager.instance.diceNoises.Count)]);

            target.hp -= Mathf.Max((attack - target.defense), 1);
            target.gameObject.GetComponent<Rigidbody2D>().AddForce(40 * Mathf.Max(attack - target.defense, 1) * (target.gameObject.transform.position- gameObject.transform.position).normalized);
            gameObject.GetComponent<Rigidbody2D>().AddForce(20 * Mathf.Max(attack - target.defense, 1) * (gameObject.transform.position- target.gameObject.transform.position).normalized);
            cooldownTimer = cooldownMaxTime;
            canAttack = false;
        }
    }

}
