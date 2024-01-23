using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    // Components
    [Space(10)]
    Vector2 move;
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform checkGroundPoint;
    [SerializeField] private LayerMask groundLayer;
    
    [Space(10)]
    [SerializeField] private InventoryManager inventoryManager;
    AudioManager audioManager;
    public GameObject inventoryGroup;
    public GameObject inventory;
    public GameObject upgrade;

    // Player
    [Space(10)]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private float moveX;

    [Header("- Stats -")]
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private ManaBar manaBar;
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    [SerializeField] private int maxMana = 100;
    private int currentMana;
    
    // Booleans
    bool isGrounded;
    [HideInInspector] public bool canPlant = false;
    bool isFacingLeft = false;
    bool isFacingRight = true;
    bool isTakingDamage = false;
    [HideInInspector] public bool isDead = false;    

    [Space(10)]
    [SerializeField] private Transform toolsPoint;

    [Header("- Pickaxe components -")]
    [SerializeField] private LayerMask layersPickaxeAffect;
    bool canMine = true;
    [HideInInspector] public bool isMining = false;
    float delayMiningTime = 0;
    [SerializeField] private float pickaxePointRadius = 0.25f;

    [Header("- Sword components -")]
    [SerializeField] private LayerMask layersSwordAffect;
    bool canFight = true;
    [HideInInspector] public bool isFighting = false;
    float delayFightingTime = 0;
    [SerializeField] private float swordPointRadius = 0.2f;

    [Header("- Axe components -")]
    [SerializeField] private LayerMask layersAxeAffect;
    bool canChop = true;
    [HideInInspector] public bool isChopping = false;
    float delayChoppingTime = 0;
    [SerializeField] private float axePointRadius = 0.2f;

    [Header("- Shovel components -")]
    [SerializeField] private Tilemap grassTilemap;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private RuleTile grassTileRule;
    [SerializeField] private Transform growingPlantCheckPoint;
    [SerializeField] private LayerMask growingCropLayer;
    
    float currentAnimationTime = 0;
    float eatingTime = 0;
    bool isEating = false;

    [Header("- AudioClips -")]
    [SerializeField] private AudioClip tools_whooshSFX;
    [SerializeField] private List<AudioClip> dirtSFXList;
    [SerializeField] private AudioClip eatingSFX;
    [SerializeField] private AudioClip waterBucketSFX;

    Vector3 tempBoxPosition;
    public GameObject tempBox;

    [Header("Particles")]
    [SerializeField] ParticleSystem waterParticle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        inventoryGroup.SetActive(false);

        currentHealth = maxHealth;
        currentMana = maxMana;
        healthBar.SetMaxHealth(maxHealth);
        manaBar.SetMaxMana(maxMana);
    }

    void Update()
    {
        anim.SetBool("isDeadBool", isDead);

        if(!isDead)
        {
            FlipX();
            Jump();
            anim.SetBool("isTakingDamageBool", isTakingDamage);
            if(isTakingDamage)
            {
                currentAnimationTime = Mathf.MoveTowards(currentAnimationTime, anim.GetCurrentAnimatorStateInfo(0).length, Time.deltaTime);
                if(currentAnimationTime >= anim.GetCurrentAnimatorStateInfo(0).length)
                {
                    isTakingDamage = false;
                    currentAnimationTime = 0;
                }
            }
            if(isGrounded)
            {
                if(moveX != 0)
                    anim.SetTrigger("isRunning");
                else
                    anim.SetTrigger("isIdle");
            }
        }
        else
        {
            ResetToSpawnPoint();
        }
    }

    void FixedUpdate()
    {
        if(!isDead)
            Move();
    }

    void Move()
    {
        moveX = Input.GetAxisRaw("Horizontal");

        if(!inventoryManager.isOpeningTheInventory)
        {
            rb.position += new Vector2(moveX, 0) * speed * Time.fixedDeltaTime;
        }
    }

    void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(checkGroundPoint.position, 0.08f, groundLayer);
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
        }
    }

    void FlipX()
    {
        if((isFacingLeft && moveX > 0))
        {
            transform.Rotate(0, 180, 0);
            isFacingRight = true;
            isFacingLeft = false;
        }
        else if((isFacingRight && moveX < 0))
        {
            transform.Rotate(0, 180, 0);
            isFacingRight = false;
            isFacingLeft = true;
        }
    }

    public void PickaxeAction()
    {
        SetTempBox(false);
        if(Input.GetMouseButtonDown(0) && canMine && currentMana > 0)
        {
            anim.SetTrigger("isMining");
            canMine = false;
            isMining = true;

            Collider2D col = Physics2D.OverlapCircle(toolsPoint.position, pickaxePointRadius, layersPickaxeAffect);
            if(col != null)
            {
                if(col.CompareTag("Mineral"))
                {
                    col.GetComponent<MineralBehaviours>().TakeDamage((int)Random.Range(inventoryManager.tool.Damage - 1, inventoryManager.tool.Damage + 4));
                }
                else if(col.CompareTag("Enemy"))
                {
                    col.GetComponent<EnemyStats>().TakeDamage((int)Random.Range(inventoryManager.tool.Damage - 3, inventoryManager.tool.Damage + 4));
                }
            }

            DecreaseMana(1);
            audioManager.PlaySFX(tools_whooshSFX);
        }
        
        //Cooldown
        if(isMining)
        {
            delayMiningTime = Mathf.MoveTowards(delayMiningTime, inventoryManager.tool.Delay + 1, Time.deltaTime);
            if(delayMiningTime >= inventoryManager.tool.Delay)
            {
                canMine = true;
                isMining = false;
                delayMiningTime = 0;
            }
        }
    }

    public void SwordAction()
    {
        SetTempBox(false);
        if(Input.GetMouseButtonDown(0) && canFight && currentMana > 0)
        {
            anim.SetTrigger("isFighting");
            canFight = false;
            isFighting = true;

            Collider2D[] col = Physics2D.OverlapCircleAll(toolsPoint.position, swordPointRadius, layersSwordAffect);

            if(col != null)
            {
                for(int i = 0; i < col.Length; i++)
                {
                    col[i].GetComponent<EnemyStats>().TakeDamage((int)Random.Range(inventoryManager.tool.Damage - col.Length, inventoryManager.tool.Damage + 4 - col.Length));
                }
            }

            DecreaseMana(1);
            audioManager.PlaySFX(tools_whooshSFX);
        }

        //Cooldown
        if(isFighting)
        {
            delayFightingTime = Mathf.MoveTowards(delayFightingTime, inventoryManager.tool.Delay + 1, Time.deltaTime);
            if(delayFightingTime >= inventoryManager.tool.Delay)
            {
                canFight = true;
                isFighting = false;
                delayFightingTime = 0;
            }
        }
    }

    public void AxeAction()
    {
        SetTempBox(false);
        if(Input.GetMouseButtonDown(0) && canChop && currentMana > 0)
        {
            anim.SetTrigger("isChopping");
            canChop = false;
            isChopping = true;

            Collider2D col = Physics2D.OverlapCircle(toolsPoint.position, axePointRadius, layersAxeAffect);
            if(col != null)
            {
                if(col.CompareTag("Tree"))
                    col.GetComponent<TreeBehaviours>().TakeDamage((int)Random.Range(inventoryManager.tool.Damage - 1, inventoryManager.tool.Damage + 4));
                else if(col.CompareTag("Enemy"))
                    col.GetComponent<EnemyStats>().TakeDamage((int)Random.Range(inventoryManager.tool.Damage - 3, inventoryManager.tool.Damage + 4));
            }

            DecreaseMana(1);
            audioManager.PlaySFX(tools_whooshSFX);
        }

        //Cooldown
        if(isChopping)
        {
            delayChoppingTime = Mathf.MoveTowards(delayChoppingTime, inventoryManager.tool.Delay + 1, Time.deltaTime);
            if(delayChoppingTime >= inventoryManager.tool.Delay)
            {
                canChop = true;
                isChopping = false;
                delayChoppingTime = 0;
            }
        }
    }

    public void ShovelAction()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
        RaycastHit2D ray2 = Physics2D.Raycast(transform.position, Vector2.down, 1f, growingCropLayer);

        if(ray.collider != null && ray2.collider == null)
        {
            SetTempBox(true);
            Debug.DrawRay(transform.position, Vector3.down, Color.red);
            Vector3Int pos = grassTilemap.WorldToCell(new Vector3Int((int)(ray.point.x - 1f), (int)(ray.point.y - 1f), -1));
            TileBase grassTile = grassTilemap.GetTile(pos);

            tempBoxPosition = grassTilemap.CellToWorld(pos);
            tempBoxPosition.y += 0.5f;
            tempBoxPosition.x += 0.5f;

            if(Input.GetMouseButtonDown(0))
            {
                if(grassTile != null)
                    grassTilemap.SetTile(pos, null);
                else
                {
                    TileBase dirtTile = groundTilemap.GetTile(pos);
                    if(dirtTile != null)
                        grassTilemap.SetTile(pos, grassTileRule);
                }

                audioManager.PlaySFX(dirtSFXList[Random.Range(0, dirtSFXList.Count)]);
            }
        }
        else if(ray.collider == null && ray2.collider == null)
        {
            SetTempBox(false);
        }

        
        if(ray2.collider != null)
        {
            SetTempBox(true);
            tempBoxPosition = ray2.collider.transform.position;
            if(Input.GetMouseButtonDown(0))
                Destroy(ray2.collider.gameObject);
        }
    }

    public void BucketAction()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, growingCropLayer);

        if(ray.collider != null)
        {
            Debug.DrawRay(transform.position, Vector3.down, Color.blue);
            tempBoxPosition = ray.collider.transform.position;
            SetTempBox(true);

            if(Input.GetMouseButtonDown(0))
            {
                if(ray.collider.gameObject.TryGetComponent<GrowingCrop_ParentClass>(out GrowingCrop_ParentClass growingCrop))
                    growingCrop.WaterTheCrop();
                
                audioManager.PlaySFX(waterBucketSFX);
                ParticleSystem particle = Instantiate(waterParticle, ray.collider.transform.position, Quaternion.identity);
                particle.transform.position += new Vector3(0,-0.25f,0);
                particle.Play();
                
                Destroy(particle.gameObject, 1);
            }
        }
        else
            SetTempBox(false);
    }

    public void CropAction()
    {
        SetTempBox(false);
        if(Input.GetMouseButton(1))
        {
            eatingTime += Time.deltaTime;
            if(eatingTime >= 1)
            {
                IncreaseHealth(inventoryManager.crop.HealthIncreaseAmount);
                IncreaseMana(inventoryManager.crop.ManaIncreaseAmount);

                inventoryManager.DecreaseItem(inventoryManager.itemInSelectedSlot.item);
                isEating = false;
                eatingTime =  0;
            }
            else
            {
                if(!isEating)
                {
                    audioManager.PlaySFX(eatingSFX);
                    isEating = true;
                }
            }
        }
        else if(Input.GetMouseButtonUp(1))
        {
            eatingTime = 0;
            isEating = false;
            audioManager.StopSFX();
        }
    }

    public void CropSeedAction()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
        Debug.DrawRay(transform.position, Vector3.down, Color.green);
        if(ray.collider == null)
            return; 

        Vector3Int pos = grassTilemap.WorldToCell(new Vector3Int((int)(ray.point.x - 1f), (int)(ray.point.y - 1f), 0));
        TileBase grassTile = grassTilemap.GetTile(pos);

        if(grassTile == null)
        {
            Debug.DrawRay(transform.position, Vector3.down, Color.green);
            if(Physics2D.OverlapCircle(growingPlantCheckPoint.position, 0.25f, growingCropLayer) == null)
            {
                canPlant = true;
                TileBase dirtTile = groundTilemap.GetTile(pos);
                tempBoxPosition = groundTilemap.CellToWorld(pos);
                tempBoxPosition.x += 0.5f;
                tempBoxPosition.y += 0.5f;
                SetTempBox(true);

                if(dirtTile != null)
                {
                    if(Input.GetMouseButtonDown(1))
                    {
                        Vector3 plantPos = groundTilemap.CellToWorld(pos);

                        //The center of the growing crop takes the bottom-left position of the tile, so I change the spawn position
                        plantPos.y += 1.5f;
                        plantPos.x += 0.5f;
                        plantPos.z = 0;

                        Instantiate(inventoryManager.cropSeed.GrowingCrop, plantPos, Quaternion.identity);
                        inventoryManager.DecreaseItem(inventoryManager.cropSeed);
                        anim.SetTrigger("isPlanting");
                        
                        DecreaseMana(1);
                        audioManager.PlaySFX(dirtSFXList[Random.Range(0, dirtSFXList.Count)]);
                    }
                }
            }
            else
            {
                canPlant = false;
                SetTempBox(false);
            }
        }
        else
        {
            canPlant = false;
            SetTempBox(false);
        }
    }

    public void SetTempBox(bool check)
    {
        tempBox.SetActive(check);
        tempBox.transform.position = tempBoxPosition;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if(!isDead)
        {
            isDead = true;
            anim.SetTrigger("isDead");
        }
    }

    public void TakeDamage(int damage, Transform enemyPos, float knockbackForce)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        
        if(currentHealth > 0)
        {
            isTakingDamage = true;
            anim.SetTrigger("isTakingDamage");

            if(transform.position.x >= enemyPos.position.x)
                rb.AddForce(new Vector2(knockbackForce, 1.5f), ForceMode2D.Impulse);
            else
                rb.AddForce(new Vector2(-knockbackForce, 1.5f), ForceMode2D.Impulse);
        }
        else if(currentHealth <= 0)
        {
            if(!isDead)
            {
                isDead = true;
                anim.SetTrigger("isDead");
            }
        }
    }

    public void ResetToSpawnPoint()
    {
        ShutDownUI();
        currentAnimationTime = Mathf.MoveTowards(currentAnimationTime, anim.GetCurrentAnimatorStateInfo(0).length, Time.deltaTime);
        if(currentAnimationTime >= anim.GetCurrentAnimatorStateInfo(0).length)
        {
            transform.position = spawnPoint.position;
            currentHealth = maxHealth/3;
            healthBar.SetHealth(currentHealth);
            anim.SetTrigger("isIdle");
            isDead = false;
            currentAnimationTime = 0;
        }
    }

    public void DecreaseMana(int amount)
    {
        currentMana -= amount;
        if(currentMana < 0)
            currentMana = 0;
        manaBar.SetMana(currentMana);
    }

    public void IncreaseHealth(int amount)
    {
        currentHealth += amount;
        if(currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthBar.SetHealth(currentHealth);
    }

    public void IncreaseMana(int amount)
    {
        currentMana += amount;
        if(currentMana >= maxMana)
        {
            currentMana = maxMana;
        }

        manaBar.SetMana(currentMana);
    }

    public void ShutDownUI()
    {
        inventoryGroup.SetActive(false);
        inventoryManager.isOpeningTheInventory = false;
    }

    public int SetCurrentHealth { set { currentHealth = value; }}
    public int SetCurrentMana { set { currentMana = value; }}

    // void OnDrawGizmos()
    // {
    //     if(toolsPoint == null)
    //         return;

    //     Gizmos.DrawWireSphere(toolsPoint.position, pickaxePointRadius);
    //     Gizmos.DrawWireSphere(toolsPoint.position, swordPointRadius);
    // }
}
